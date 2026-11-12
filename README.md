# NEXUS: Tactical Warfare (PENDIENTE TACTICAL SHOOTER)
### Shooter Táctico Competitivo en 3D desarrollado en C# para PC (Windows)

Este proyecto implementa de manera completa y funcional todas las directrices, diseño de juego, interfaz y arquitectura solicitadas en los documentos e imágenes de referencia:

---

## 🎮 ¿Cómo Iniciar y Jugar Inmediatamente?

1. Haz doble clic en el archivo:
   ```
   JUGAR_NEXUS_3D.bat
   ```
2. O desde la consola de comandos de Windows (PowerShell / CMD):
   ```bash
   cd NexusTacticalWarfare.Game
   dotnet run -c Release
   ```
   *(También puedes ejecutar directamente `NexusTacticalWarfare.Game\bin\Release\net10.0\NexusTacticalWarfare.exe`)*

---

## 🕹️ Controles del Juego

- **W, A, S, D**: Movimiento del personaje (Adelante, Izquierda, Atrás, Derecha).
- **Ratón (Mouse)**: Mirar alrededor y apuntar con la cámara FPS en 3D.
- **Clic Izquierdo**: Disparar arma actual.
- **Clic Derecho**: Apuntar por la mira (ADS) / Zoom telescópico de Francotirador.
- **1, 2, 3, 4, 5 o Rueda del Ratón**: Alternar entre las 5 armas de la Imagen #2.
- **R**: Recargar munición.
- **Espacio**: Saltar.
- **Shift Izquierdo**: Correr (Sprint táctico).
- **C**: Agacharse.
- **E**: Activar habilidad especial de personaje (Barrera 3D de Vulcan, Pulso EMP de Nyx, etc.).
- **ESC**: Menú de pausa táctica y opciones.

---

## 🌟 Cumplimiento de Instrucciones Específicas

1. **Gráficos en 3D**:
   - Todo el motor visual (cámara, personajes, armas, escenarios, trazadoras láser, escombros de destrucción y lobby) es renderizado en **3D con aceleración de hardware** en tiempo real a 60+ FPS.

2. **Imagen #1 - Rangos y Lobby**:
   - **Rangos**: Bronce, Plata, Oro, Platino, Diamante, Heroico y **Leyenda**.
   - **Rango LEYENDA en Color Esmeralda**: Diseñado con un brillante verde esmeralda radiante (`#00FF88`), creando un fuerte contraste con el tono dorado del rango Oro.
   - **Lobby**: Diseñado idéntico a la Imagen #1:
     - Barra superior con perfil y divisas (Créditos, Cristales).
     - Menú lateral izquierdo vertical (Jugar, Personaje, Armas, Galería, Tienda, Eventos).
     - Pedestal central con el personaje 3D rotando interactivamente.
     - Panel derecho con Temporada 1, insignia de rango esmeralda, selectores y el gran botón amarillo **JUGAR**.
     - Sub-tarjetas inferiores de acceso rápido.

3. **Imagen #2 - Portada, Mapas y Armas**:
   - **Portada**: Portada cinematográfica espacial de la Imagen #2, con el título angular y futurista **PENDIENTE TACTICAL SHOOTER** de la Imagen #1 (*«ESTRATEGIA · HABILIDAD · SIN LÍMITES»*).
   - **Mapas 3D**:
     - *Distrito Helix*: Ciudad futurista con calles y barricadas destructibles (Ref 1 y 4).
     - *Laboratorio Nexus-X*: Pasillos interiores y reactor central (Ref 2 y 3).
     - *Campo de Entrenamiento*: Polígono de tiro táctico con blancos de prueba.
   - **Las 5 Armas**:
     1. Rifle de Asalto (Nexus-AR Valiant).
     2. Francotirador (Vortex Rail-Sniper con zoom).
     3. Escopeta (Havoc Scattergun con dispersión múltiple).
     4. Subfusil (Spectre-9 SMG de alta cadencia).
     5. Pistola (Apex-11 Sidearm de precisión).

4. **Imagen #3 - Skins de Personajes y Habilidades**:
   - **Vulcan (Soldado Humano)**: *«Disciplina y precisión.»* Armadura táctica azul/marina con visor cian. Habilidad: Barrera Energética 3D translúcida desplegable.
   - **Nyx (Alienígena)**: *«El poder de otro mundo.»* Armadura biomecánica violeta iridiscente con bioluminiscencia. Habilidad: Pulso de Interferencia EMP.
   - **Razor (Soldado Pesado)**: *«Resiste. Avanza. Gana.»* Armadura colosal ámbar y acero balístico. Habilidad: Sensor Táctico y Sobreescudo.

5. **Instrucciones de Skills y Agentes**:
   - Carpeta `AIAgents/`: Los 9 perfiles y responsabilidades de Agentes de IA.
   - Carpeta `Skills/`: Los 13 manuales y procedimientos estandarizados en formato `SKILL.md`.
   - Carpeta `UnityProject_Scripts/`: Arquitectura modular en C# para Unity.
   - Carpeta `Docs/`: GDD, Decisiones Técnicas, Plan de Pruebas y Changelog.
