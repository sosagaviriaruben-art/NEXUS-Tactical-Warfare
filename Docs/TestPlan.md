# Plan de Pruebas y Criterios de Aceptación (QA)
# NEXUS: Tactical Warfare

## 1. Matriz de Casos de Prueba

| ID | Área | Descripción del Caso | Criterio de Éxito | Estado |
| :--- | :--- | :--- | :--- | :---: |
| **TC-01** | Compilación | Compilar `NexusTacticalWarfare.Game` con `dotnet build` | 0 Errores, 0 Advertencias | **APROBADO** |
| **TC-02** | Portada | Portada basada en Imagen #2 con título "PENDIENTE" de Imagen #1 | Se muestra imagen2 de fondo y título angular cian | **APROBADO** |
| **TC-03** | Lobby UI | Distribución idéntica a Imagen #1 | Menú lateral, personaje 3D central, panel derecho, sub-tarjetas | **APROBADO** |
| **TC-04** | Rango Leyenda | Color Esmeralda exclusivo para Leyenda | Emblema y texto verde esmeralda radiante `#00FF88`, Oro en dorado | **APROBADO** |
| **TC-05** | Movimiento 3D | WASD, Sprint (Shift), Salto (Space), Agacharse (C), Ratón | Cámara fluida y respuesta inmediata sin latencia | **APROBADO** |
| **TC-06** | 5 Armas | Disparo de las 5 armas de la Imagen #2 | Cadencias, cargadores y daños conformes a la tabla de diseño | **APROBADO** |
| **TC-07** | Habilidad Vulcan | Presionar 'E' con Vulcan | Despliega barrera de escudo 3D translúcida que bloquea balas | **APROBADO** |
| **TC-08** | Destrucción 3D | Disparar a coberturas tácticas y cajas | Pierden salud y explotan en fragmentos físicos con rebote | **APROBADO** |
| **TC-09** | Bots 5v5 | 4 Aliados (Azul) y 5 Enemigos (Rojo) en combate | Bots patrullan, disparan en ráfagas, eliminan y son eliminados | **APROBADO** |
| **TC-10** | Audio Procedural | Sonidos de armas, recargas, impactos y menús | Todos los efectos suenan limpios sin dependencias externas | **APROBADO** |

## 2. Instrucciones para Ejecución Inmediata
1. Abrir la terminal o hacer doble clic en `JUGAR_NEXUS_3D.bat`.
2. O ejecutar desde la consola:
   ```powershell
   cd "NexusTacticalWarfare.Game"
   dotnet run -c Release
   ```
3. En la portada: Presionar **ESPACIO** o hacer clic en **INGRESAR AL JUEGO**.
4. En el lobby: Inspeccionar el personaje 3D, el rango **Leyenda Esmeralda**, las armas y hacer clic en el botón amarillo **JUGAR**.
5. En la partida:
   - **WASD**: Moverse
   - **Ratón**: Mirar y apuntar
   - **Clic Izquierdo**: Disparar
   - **Clic Derecho**: Apuntar por mira (ADS)
   - **1, 2, 3, 4, 5 o Rueda**: Cambiar entre las 5 armas
   - **R**: Recargar
   - **E**: Habilidad táctica especial
   - **ESC**: Pausa y opciones
