---
name: account-auth-flow
description: Gestión de flujos de autenticación para Invitado, Google y Facebook.
---

# Skill: Account Auth Flow

## Procedimiento
1. **Acceso de Invitado**:
   - Asignar UUID local persistido de forma segura en almacenamiento de usuario.
2. **Inicio Social (Google / Facebook)**:
   - Utilizar flujos OAuth 2.0 / OpenID Connect certificados.
   - Nunca solicitar contraseñas en el cliente.
