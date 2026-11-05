import subprocess, time, re, json, hashlib
from pathlib import Path
import xml.etree.ElementTree as ET

ADB = r'C:\Users\usuario01\AppData\Local\Android\Sdk\platform-tools\adb.exe'
PACKAGE = 'com.companyname.tictactoeminmax'
ACTIVITY = PACKAGE + '/crc64a31815018c98c801.MainActivity'
OUT = Path(__file__).parent / time.strftime('capturas_%Y%m%d_%H%M%S')
OUT.mkdir()
results, captures = [], []

def adb(*args):
    return subprocess.run([ADB, '-s', 'emulator-5554', *args], capture_output=True, check=True).stdout

def tree():
    for _ in range(8):
        adb('shell', 'uiautomator', 'dump', '/sdcard/minmax_report.xml')
        data = adb('shell', 'cat', '/sdcard/minmax_report.xml')
        root = ET.fromstring(data)
        if root.findall('.//node'):
            return root
        time.sleep(1)
    raise RuntimeError('No hay jerarquía de interfaz disponible')

def find(text=None, cell=None):
    for n in tree().iter('node'):
        if (text is not None and n.get('text') == text) or (cell is not None and n.get('resource-id', '').endswith('/btnTicTacToe' + str(cell))):
            return n
    raise RuntimeError(f'Control ausente: {text or cell}')

def tap_node(n):
    x1,y1,x2,y2 = map(int, re.findall(r'\d+', n.get('bounds')))
    adb('shell', 'input', 'tap', str((x1+x2)//2), str((y1+y2)//2))
    time.sleep(.25)

def tap(text=None, cell=None):
    tap_node(find(text, cell))

def board():
    nodes = {n.get('resource-id'): n for n in tree().iter('node')}
    return [next(n.get('text','') for k,n in nodes.items() if k and k.endswith('/btnTicTacToe'+str(i))) for i in range(1,10)]

def check(name, condition, detail):
    results.append(dict(prueba=name, resultado='PASS' if condition else 'FAIL', detalle=detail))
    if not condition:
        raise AssertionError(name + ': ' + detail)

def shot(name, caption):
    path = OUT / (name+'.png')
    path.write_bytes(adb('exec-out','screencap','-p'))
    captures.append(dict(archivo=path.name, descripcion=caption, sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    print('CAPTURA '+path.name, flush=True)

def reset():
    tap(text='Nuevo')
    check('Nuevo limpia tablero', board()==['']*9, 'Nueve casillas vacías y nuevo turno humano.')

try:
    adb('shell','input','keyevent','3')
    adb('shell','am','start','-n',ACTIVITY)
    time.sleep(10)
    reset()
    shot('01_inicio','Pantalla inicial: menús, personaje original, tablero vacío y botones Nuevo y Salir.')
    tap(cell=1)
    check('Turno humano e IA', board()==['X','','','','O','','','',''], 'X en casilla 1; Minimax responde O en el centro.')
    shot('02_turno_humano_ia','Jugada humana X y respuesta automática O de Minimax.')
    before=board()
    tap(cell=1)
    check('Casilla ocupada', board()==before, 'Pulsar X existente no modifica el tablero.')
    shot('03_casilla_ocupada','El tablero se conserva tras pulsar una casilla ocupada.')
    for name,items,num in [('Archivo',['Nuevo','Salir'],4),('Ver',['Puntajes','Resetear puntajes'],5),('Ayuda',['Acerca de...','Contenido'],6)]:
        tap(text=name)
        texts=[n.get('text') for n in tree().iter('node')]
        check('Menú '+name, all(i in texts for i in items), 'Opciones originales visibles: '+', '.join(items))
        shot(f'{num:02d}_menu_'+name.lower(),'Menú '+name+' con las opciones del proyecto WinForms original.')
        tap(text='Cancelar')
        check('Cancelar '+name, board()==before, 'Cancelar conserva la partida.')
        for item in items:
            tap(text=name)
            tap(text=item)
            check('Opción '+name+'/'+item, board()==before, 'Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores.')
    reset()
    tap(cell=1)
    tap(cell=4)
    check('IA bloquea amenaza', board()==['X','','','X','O','','O','',''], 'O en casilla 7 bloquea la columna de X.')
    shot('07_ia_bloquea','Minimax bloquea la amenaza de victoria de X en la primera columna.')
    tap(cell=8)
    check('Victoria IA', find(text='AI Wins!') is not None, 'Se muestra el aviso original AI Wins!.')
    shot('08_victoria_ia','Aviso original de victoria de la IA.')
    tap(text='OK')
    before=board()
    tap(cell=9)
    check('Fin de partida bloquea jugadas', board()==before, 'Una casilla libre no acepta otra X después de la victoria.')
    shot('09_partida_finalizada','Diagonal ganadora de O y tablero bloqueado al finalizar.')
    reset()
    shot('10_reinicio','Nuevo borra la partida terminada y vuelve a habilitar las nueve casillas.')
    for i in [1,2,7,6,9]:
        tap(cell=i)
    check('Empate', find(text="It's a Draw!") is not None, 'Se muestra el aviso original de empate.')
    shot('11_empate_aviso','Aviso original de empate después de llenar el tablero sin ganador.')
    tap(text='OK')
    check('Tablero de empate', board()==['X','X','O','O','O','X','X','O','X'], 'Nueve casillas ocupadas sin línea ganadora.')
    shot('12_empate_tablero','Tablero final de empate entre X y Minimax.')
    reset()
    tap(text='Salir')
    time.sleep(1)
    proc=subprocess.run([ADB,'-s','emulator-5554','shell','pidof',PACKAGE],capture_output=True)
    check('Salir', not proc.stdout.strip(), 'El botón Salir cierra el proceso Android.')
    adb('shell','input','keyevent','3')
    shot('13_salida','Pantalla de inicio Android después de cerrar la aplicación con Salir.')
    adb('shell','am','start','-n',ACTIVITY)
    time.sleep(10)
    check('Reapertura', board()==['']*9, 'La aplicación vuelve a abrir con tablero vacío.')
    shot('14_reapertura','Aplicación reabierta en el emulador con una partida nueva.')
except BaseException as exc:
    results.append(dict(prueba='Ejecución completa', resultado='FAIL', detalle=type(exc).__name__ + ': ' + str(exc)))
    raise
finally:
    (OUT/'resultados.json').write_text(json.dumps(dict(pruebas=results,capturas=captures),ensure_ascii=False,indent=2),encoding='utf-8')
    report=['# Evidencias de Tic-Tac-Toe MinMax MAUI Android','', 'Dispositivo: emulator-5554. Capturas originales PNG, sin recortar ni modificar.','', '| Prueba | Resultado | Evidencia |','|---|---|---|']
    report += [f"| {r['prueba']} | {r['resultado']} | {r['detalle']} |" for r in results]
    report += ['', 'La victoria humana no se provoca por la interfaz: Minimax impide que X gane jugando desde un tablero válido. No se alteró código ni se inyectaron estados para fabricar esa captura.', '', 'Los elementos de los menús originales no implementan acciones; se comprobó que seleccionar cada opción conserva la partida.', '', '## Capturas']
    report += [f"- `{c['archivo']}`: {c['descripcion']}" for c in captures]
    (OUT/'README.md').write_text('\n'.join(report)+'\n',encoding='utf-8')
    tex=['% Requiere \\usepackage{graphicx}. Copiar este directorio junto al documento .tex.','% Ajustar la ruta si las capturas se ubican en otra carpeta.']
    for c in captures:
        tex += ['\\begin{figure}[htbp]','  \\centering',f"  \\includegraphics[height=0.65\\textheight,keepaspectratio]{{{OUT.name}/{c['archivo']}}}",f"  \\caption{{{c['descripcion']}}}",f"  \\label{{fig:minmax-{c['archivo'][:2]}}}",'\\end{figure}','']
    (OUT/'figuras.tex').write_text('\n'.join(tex),encoding='utf-8')
    print('DIRECTORIO '+str(OUT.resolve()),flush=True)
    print('RESULTADOS '+str(len(results))+'; FAIL='+str(sum(r['resultado']=='FAIL' for r in results)),flush=True)
