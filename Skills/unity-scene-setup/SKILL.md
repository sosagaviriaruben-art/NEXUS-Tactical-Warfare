---
name: unity-scene-setup
description: Estructuración y configuración estándar de escenas 3D en Unity/C#.
---

# Skill: Unity Scene Setup

## Procedimiento
1. **Jerarquía Limpia**:
   - [CORE]: GameManager, EventBus, InputManager.
   - [ENVIRONMENT]: Luces direccionales, Skybox, Geometría estática del mapa, Coberturas destructibles.
   - [SPAWNS]: Puntos de reaparición de equipo Atacante y Defensor.
   - [UI]: Canvas de HUD, Mira táctica, Minimapa.
2. **Configuración de Iluminación y Capas**:
   - Asignar capas específicas: Default, Player, Enemy, Destructible, EnergyShield.
