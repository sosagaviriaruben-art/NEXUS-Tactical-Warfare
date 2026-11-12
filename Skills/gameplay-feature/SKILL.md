---
name: gameplay-feature
description: Convierte una mecánica o regla de combate en especificación de componentes C#, eventos y pruebas.
---

# Skill: Implementación de Gameplay Feature

## Procedimiento
1. **Definición de Requisitos**:
   - Identificar entradas de usuario requeridas (WASD, Mouse, Teclas numéricas, E, R).
   - Definir variables de configuración (velocidades, daños, tiempos de enfriamiento).
2. **Arquitectura C#**:
   - Crear componente desacoplado con responsabilidad única.
   - Utilizar eventos de C# o delegados para comunicar cambios de estado a la UI y al audio.
3. **Validación de Criterios de Aceptación**:
   - Comprobar que la mecánica responde en tiempo real sin latencia perceptible.
