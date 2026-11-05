# Informe de Minimax en .NET MAUI

[Abrir el informe PDF](main.pdf).

## Compilación

Requiere LuaLaTeX, Times New Roman y Consolas. El preámbulo utiliza las fuentes de `C:/Windows/Fonts/`.

Desde este directorio:

```powershell
lualatex -interaction=nonstopmode -halt-on-error main.tex
lualatex -interaction=nonstopmode -halt-on-error main.tex
```

También se puede ejecutar `latexmk`, que usa LuaLaTeX mediante `.latexmkrc`.

## Organización

- `main.tex`: documento principal con las secciones incluidas mediante `\input`.
- `preambulo.tex`: Times New Roman, tablas, enlaces del índice y bloques de código Dracula.
- `secciones/`: portada, desarrollo y referencias.
- `anexos/`: evidencias del juego.
- `diagramas_mermaid/`: fuentes editables de los diagramas.
- `figuras/`: diagramas renderizados, logo y capturas del emulador.
- `configuraciones_visual_studio/`: capturas de configuración del proyecto.

Los nombres de estudiantes y el grupo se completan en `secciones/00_portada.tex`.
El informe distingue las evidencias existentes de las capturas de configuración; no registra una nueva ejecución de las pruebas de la aplicación.

Los auxiliares de LaTeX, los renders temporales `qa-*` y los metadatos de Visual Studio están excluidos mediante el `.gitignore` del repositorio. El PDF final se conserva como entregable.
