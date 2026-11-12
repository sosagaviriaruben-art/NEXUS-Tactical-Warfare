# 🎮 NEXUS Tactical Warfare — Guía para continuar en Unity

> Esta guía fue generada por Antigravity AI para continuar el desarrollo del juego
> desde cualquier computador. El código fuente completo ya está en esta carpeta.

---

## ¿Por qué Unity y no el juego actual en C# + Raylib?

El juego que ya existe (`Juego_NEXUS_ListoParaJugar/`) funciona y se puede jugar,
pero se ve con gráficos básicos (cubos, primitivos) porque Raylib solo puede dibujar
formas simples. Para que el juego se vea **100% como las imágenes de referencia**
(personajes realistas, ciudad futurista, efectos de luz), necesitas Unity.

| Motor actual (C# + Raylib) | Unity |
|---|---|
| Solo primitivos: cubos, esferas | Modelos 3D completos (.fbx, .obj) |
| Sin shaders realistas | Shader Graph, URP, HDRP |
| Sin iluminación dinámica | Iluminación PBR en tiempo real |
| Sin físicas avanzadas | PhysX integrado |
| Básico / prototipo | Calidad AAA posible |

---

## PASO 1 — Descargar e instalar Unity

1. Ve a: **https://unity.com/download**
2. Descarga **Unity Hub** (es el gestor de instalación, gratis)
3. Dentro de Unity Hub, instala **Unity 6 LTS** (la versión más estable y moderna)
4. Durante la instalación marca estas opciones:
   - ✅ **Windows Build Support**
   - ✅ **Visual Studio Community** (editor de código)
   - ✅ **Android Build Support** (opcional, si quieres móvil en el futuro)

---

## PASO 2 — Crear el proyecto NEXUS en Unity

1. Abre **Unity Hub**
2. Clic en **"New Project"**
3. Selecciona plantilla: **"3D (URP)"** ← MUY IMPORTANTE, no uses "3D" básico
   - URP = Universal Render Pipeline = gráficos modernos estilo AAA
4. Nombre del proyecto: `NEXUS_Tactical_Warfare`
5. Ubicación: pon la carpeta donde quieras guardarlo
6. Clic en **"Create Project"**

---

## PASO 3 — Importar el código que ya existe

Todo el código C# del juego ya está escrito y listo en esta carpeta:

```
ProyectoNEXUS.md/
└── UnityProject_Scripts/
    └── Assets/
        └── Scripts/
            ├── Core/           ← Gestión de partida y lógica principal
            ├── Player/         ← Control del jugador FPS
            ├── Combat/         ← Sistema de disparo, daño, headshots
            ├── Characters/     ← Vulcan, Nyx, Razor y sus habilidades
            ├── Abilities/      ← Cúpula de barrera, pulso, dash
            ├── Destruction/    ← Entorno destructible
            ├── AI/             ← Bots con IA táctica
            ├── UI/             ← HUD, lobby, killfeed, radar
            └── Match/          ← Sistema de rondas, ranking, puntuación
```

**Para importar a Unity:**
1. En Unity, abre la ventana **Project** (abajo)
2. Navega a `Assets/Scripts/`
3. Arrastra la carpeta `UnityProject_Scripts/Assets/Scripts/` de tu explorador
   de archivos directamente a la ventana Project de Unity

---

## PASO 4 — Assets gratuitos para que se vea como las imágenes

Estos assets son **GRATUITOS** en la Unity Asset Store y harán que el juego
se vea exactamente como las imágenes de referencia:

### Personajes
- Busca en Asset Store: **"Sci-Fi Soldier"** o **"Military Character Pack"**
- Recomendado: **"KayKit - Spaceship Pack"** o **"Synty Studios - POLYGON Sci-Fi"**

### Entorno / Mapas
- Busca: **"Futuristic City Environment"** o **"Sci-Fi Industrial Pack"**
- Recomendado: **"City Builder Pack"** (tiene rascacielos y edificios futuristas)

### Armas FPS
- Busca: **"FPS Weapon Bundle"** o **"Sci-Fi Gun Pack"**
- Recomendado: **"Modern Weapons Pack"**

### Efectos (disparos, explosiones, partículas)
- Busca: **"Sci-Fi Effects"** o **"VFX Particle Pack"**
- Recomendado: **"AllSky Free"** (cielos y atmósferas para el mapa)

**Cómo entrar a la Asset Store desde Unity:**
`Window → Asset Store` o ve a **https://assetstore.unity.com**

---

## PASO 5 — Configurar el juego como FPS táctico 5v5

Una vez que tienes Unity con URP y los assets, configura esto:

### Cámara FPS
```csharp
// Ya está en: UnityProject_Scripts/Assets/Scripts/Player/
// Archivo: PlayerController.cs
// Solo arrastra el script al objeto Camera en Unity
```

### Sistema de Rangos (Bronce → Leyenda Esmeralda)
```csharp
// Ya está en: UnityProject_Scripts/Assets/Scripts/UI/
// El rango "Leyenda" tiene color Esmeralda (#00FF88) como pediste
```

### Personajes con habilidades
| Personaje | Habilidad | Archivo |
|---|---|---|
| Vulcan | Cúpula de barrera táctica | `Characters/Vulcan.cs` |
| Nyx | Pulso de disrupción + invisibilidad | `Characters/Nyx.cs` |
| Razor | Dash supersónico + lanzacuchillas | `Characters/Razor.cs` |

---

## PASO 6 — Imágenes de referencia del juego

Las imágenes con el diseño visual del juego están en:
```
ProyectoNEXUS.md/
├── imagen1.png  ← Sistema de rangos + lobby + nombre del juego
├── imagen2.png  ← Portada + armas + mapas de referencia
└── imagen3.png  ← Personajes Vulcan, Nyx y Razor
```

Los assets extraídos de esas imágenes están en:
```
ProyectoNEXUS.md/
└── NexusTacticalWarfare.Game/
    └── Assets/
        ├── cover_nexus.png      ← Portada del juego
        ├── char_vulcan.png      ← Personaje Vulcan
        ├── char_nyx.png         ← Personaje Nyx
        ├── char_razor.png       ← Personaje Razor
        ├── weapon_ar.png        ← Rifle de asalto
        ├── weapon_sniper.png    ← Francotirador
        ├── weapon_shotgun.png   ← Escopeta
        ├── weapon_smg.png       ← Subfusil
        └── weapon_pistol.png    ← Pistola
```

---

## PASO 7 — Continuar con Antigravity AI

Cuando estés en el otro computador con Unity instalado, abre **Antigravity**
y dile exactamente esto para que el asistente retome el contexto:

> "Estoy desarrollando NEXUS Tactical Warfare, un shooter táctico 5v5 en Unity URP.
> Tengo el código en la carpeta UnityProject_Scripts. Los personajes son Vulcan, Nyx
> y Razor. El rango más alto se llama Leyenda y es de color esmeralda (#00FF88).
> Quiero continuar configurando el proyecto en Unity. ¿Puedes ayudarme?"

---

## Resumen de lo que ya está listo

| Componente | Estado |
|---|---|
| Código C# completo (Unity) | ✅ En `UnityProject_Scripts/` |
| Juego básico jugable (Raylib) | ✅ En `Juego_NEXUS_ListoParaJugar/` |
| Portada NEXUS Tactical Warfare | ✅ `Assets/cover_nexus.png` |
| 9 Agentes de IA documentados | ✅ En `AIAgents/` |
| 13 Skills del proyecto | ✅ En `Skills/` |
| GDD (Game Design Document) | ✅ En `Docs/GameDesignDocument.md` |
| Assets de imágenes extraídos | ✅ En `NexusTacticalWarfare.Game/Assets/` |

---

*Generado por Antigravity AI — Proyecto NEXUS Tactical Warfare*
*Para continuar: abre Antigravity en cualquier computador y pega el texto del Paso 7*
