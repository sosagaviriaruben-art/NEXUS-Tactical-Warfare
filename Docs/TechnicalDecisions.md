# Decisiones Técnicas y Arquitectura del Proyecto

## 1. Lenguaje y Ecosistema
- **Lenguaje**: C# (.NET 10.0 SDK).
- **Prototipo Jugable Standalone**: Implementado en C# puro utilizando `Raylib-cs` con aceleración por hardware (OpenGL 3.3 / Core), garantizando compatibilidad nativa en Windows sin dependencias externas pesadas.
- **Estructura Unity**: Todos los módulos de lógica están paralelamente estructurados en `UnityProject_Scripts/Assets/Scripts` con clases `MonoBehaviour`, `ScriptableObject` y separación estricta por ensamblados para futura apertura en el motor Unity LTS.

## 2. Decisiones de Renderizado y Gráficos 3D
- **Perspectiva y Cámara**: Cámara FPS con desacoplamiento entre el pitch (cabeceo vertical clamped a ±85°) y el yaw (guiñada horizontal de 360°).
- **Modelado Procedural 3D**:
  - Armas en primera persona renderizadas en el espacio de vista de la cámara con retroceso angular, desplazamiento de mira ADS y materiales emisivos LED.
  - Personajes en 3D con modelado anatómico por bloques segmentados (piernas, torso, placas balísticas, hombreras, casco y visor luminoso).
  - Geometría de mapas 3D con iluminación direccional y sombras sólidas.
- **Sistema de Partículas y Debris**:
  - Simulación física balística con gravedad para los fragmentos de cobertura destruida.
  - Trazadoras luminosas con decaimiento de opacidad alfa para feedback de disparo instantáneo.

## 3. Síntesis de Audio Táctico en Tiempo Real
- Para eliminar la dependencia de ficheros externos de audio `.wav` o `.mp3`, se implementó un sintetizador procedural de audio PCM en C# (`SoundSynthesizer.cs`) que genera dinámicamente ondas de sonido para:
  - Disparos de cada una de las 5 armas (frecuencias graves para francotirador, ráfagas cortas para SMG, etc.).
  - Recarga y clicks de interfaz.
  - Impactos (hitmarker) y sonido de tiro a la cabeza (headshot chime).
  - Activación de barreras energéticas y explosión de escombros.

## 4. Arquitectura de Red y Autenticación (Plan Online)
- **Autoridad del Servidor**: El servidor calcula los impactos y los estados de salud; el cliente ejecuta predicción local con interpolación.
- **Acceso de Usuarios**:
  - Modo Invitado implementado con almacenamiento local de perfil.
  - Flujo planificado para Google OAuth 2.0 y Facebook Login según directrices oficiales de seguridad.
