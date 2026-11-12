---
name: test-and-build
description: Ejecución de compilación y validación de entregables ejecutables.
---

# Skill: Test and Build

## Procedimiento
1. **Compilación Limpia**:
   - dotnet build verificando cero errores y cero advertencias.
2. **Prueba de Humo (Smoke Test)**:
   - Validar que el ejecutable inicia, navega por el lobby y entra a partida 3D sin caídas.
