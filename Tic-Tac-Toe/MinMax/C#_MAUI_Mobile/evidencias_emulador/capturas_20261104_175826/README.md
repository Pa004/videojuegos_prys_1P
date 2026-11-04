# Evidencias de Tic-Tac-Toe MinMax MAUI Android

Dispositivo: emulator-5554. Capturas originales PNG, sin recortar ni modificar.

| Prueba | Resultado | Evidencia |
|---|---|---|
| Nuevo limpia tablero | PASS | Nueve casillas vacías y nuevo turno humano. |
| Turno humano e IA | PASS | X en casilla 1; Minimax responde O en el centro. |
| Casilla ocupada | PASS | Pulsar X existente no modifica el tablero. |
| Menú Archivo | PASS | Opciones originales visibles: Nuevo, Salir |
| Cancelar Archivo | PASS | Cancelar conserva la partida. |
| Opción Archivo/Nuevo | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Opción Archivo/Salir | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Menú Ver | PASS | Opciones originales visibles: Puntajes, Resetear puntajes |
| Cancelar Ver | PASS | Cancelar conserva la partida. |
| Opción Ver/Puntajes | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Opción Ver/Resetear puntajes | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Menú Ayuda | PASS | Opciones originales visibles: Acerca de..., Contenido |
| Cancelar Ayuda | PASS | Cancelar conserva la partida. |
| Opción Ayuda/Acerca de... | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Opción Ayuda/Contenido | PASS | Sin cambio de partida: el menú original carece de manejadores; Nuevo y Salir funcionan mediante los botones inferiores. |
| Nuevo limpia tablero | PASS | Nueve casillas vacías y nuevo turno humano. |
| IA bloquea amenaza | PASS | O en casilla 7 bloquea la columna de X. |
| Victoria IA | PASS | Se muestra el aviso original AI Wins!. |
| Fin de partida bloquea jugadas | PASS | Una casilla libre no acepta otra X después de la victoria. |
| Nuevo limpia tablero | PASS | Nueve casillas vacías y nuevo turno humano. |
| Empate | PASS | Se muestra el aviso original de empate. |
| Tablero de empate | PASS | Nueve casillas ocupadas sin línea ganadora. |
| Nuevo limpia tablero | PASS | Nueve casillas vacías y nuevo turno humano. |
| Salir | PASS | El botón Salir cierra el proceso Android. |
| Reapertura | PASS | La aplicación vuelve a abrir con tablero vacío. |

La victoria humana no se provoca por la interfaz: Minimax impide que X gane jugando desde un tablero válido. No se alteró código ni se inyectaron estados para fabricar esa captura.

Los elementos de los menús originales no implementan acciones; se comprobó que seleccionar cada opción conserva la partida.

## Capturas
- `01_inicio.png`: Pantalla inicial: menús, personaje original, tablero vacío y botones Nuevo y Salir.
- `02_turno_humano_ia.png`: Jugada humana X y respuesta automática O de Minimax.
- `03_casilla_ocupada.png`: El tablero se conserva tras pulsar una casilla ocupada.
- `04_menu_archivo.png`: Menú Archivo con las opciones del proyecto WinForms original.
- `05_menu_ver.png`: Menú Ver con las opciones del proyecto WinForms original.
- `06_menu_ayuda.png`: Menú Ayuda con las opciones del proyecto WinForms original.
- `07_ia_bloquea.png`: Minimax bloquea la amenaza de victoria de X en la primera columna.
- `08_victoria_ia.png`: Aviso original de victoria de la IA.
- `09_partida_finalizada.png`: Diagonal ganadora de O y tablero bloqueado al finalizar.
- `10_reinicio.png`: Nuevo borra la partida terminada y vuelve a habilitar las nueve casillas.
- `11_empate_aviso.png`: Aviso original de empate después de llenar el tablero sin ganador.
- `12_empate_tablero.png`: Tablero final de empate entre X y Minimax.
- `13_salida.png`: Pantalla de inicio Android después de cerrar la aplicación con Salir.
- `14_reapertura.png`: Aplicación reabierta en el emulador con una partida nueva.
