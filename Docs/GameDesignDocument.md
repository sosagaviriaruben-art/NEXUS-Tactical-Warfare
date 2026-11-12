# Documento de Diseño de Juego (GDD)
# NEXUS: Tactical Warfare (PENDIENTE TACTICAL SHOOTER)

## 1. Visión General del Proyecto
**NEXUS: Tactical Warfare** es un videojuego de disparos en primera persona (FPS) táctico y competitivo en 3D para PC (Windows), desarrollado completamente en **C#**. Combina la estrategia por rondas 5v5 con una estética militar futurista y de ciencia ficción, alta velocidad de respuesta, habilidades tácticas balanceadas y entornos destructibles.

---

## 2. Dirección Visual e Instrucciones Específicas del Usuario

### 2.1 Portada del Juego (Inspirada en Imagen #2 con Nombre de Imagen #1)
- **Composición Visual (de Imagen #2)**:
  - Fondo cósmico cinematográfico en órbita planetaria, crucero de guerra estelar y combate en el espacio.
  - Los 3 héroes principales (Humano, Alienígena, Pesado) en primer plano con iluminación de alto contraste azul cian y púrpura.
- **Tipografía y Nombre (de Imagen #1)**:
  - Título principal prominente: **PENDIENTE** en fuente angular futurista de alta tecnología con resplandor cian.
  - Subtítulo: **TACTICAL SHOOTER**.
  - Lema táctico: *«ESTRATEGIA · HABILIDAD · SIN LÍMITES»*.
  - Mención de universo: *Proyecto NEXUS: Tactical Warfare*.

### 2.2 Lobby del Juego (Idéntico a Imagen #1)
- **Estructura y Distribución**:
  - **Barra Superior**: Perfil del jugador, nivel (LVL 42), créditos (⬡ 12,500), cristales (✦ 150) y botón de ajustes.
  - **Menú Lateral Izquierdo (Vertical)**:
    - [1] JUGAR
    - [2] PERSONAJE
    - [3] ARMAS
    - [4] GALERÍA
    - [5] TIENDA
    - [6] EVENTOS
  - **Área Central (3D Interactivo)**:
    - Escenario de hangar futurista con plinto holográfico circular.
    - Modelo 3D del personaje seleccionado en rotación continua con arma en mano, nombre, rol y lema.
  - **Panel Derecho**:
    - Cabecera: *TEMPORADA 1*.
    - Visualización del Rango actual con su insignia.
    - Selector de modo de juego (*Modo Competitivo 5v5*, *Partida Rápida*, *Entrenamiento*).
    - Selector de mapa 3D.
    - **Botón JUGAR grande en color amarillo/dorado** característico.
  - **Sub-Tarjetas Inferiores**:
    - *Modo Lucky Royale* (Maqueta de recompensas).
    - *Galería de Armas*.
    - *Tienda de Skins*.
    - *Personalización de Personaje*.

### 2.3 Sistema de Rangos (De Imagen #1 con Modificación Esmeralda en Leyenda)
1. **Bronce**: Recluta inicial (Color Bronce/Cobre `#B4642D`).
2. **Plata**: Operador regular (Color Plata Metálico `#B9C3CD`).
3. **Oro**: Tirador experto (Color Dorado Amarillo Intenso `#FFC800`).
4. **Platino**: Estratega avanzado (Color Cian Glacial `#00DCFF`).
5. **Diamante**: Maestro táctico (Color Púrpura Profundo `#AA28FF`).
6. **Heroico**: Veterano de guerra (Color Carmesí / Rubí `#FF1E46`).
7. **Leyenda**: **COLOR ESMERALDA RADIANTE (`#00EB78` / `#78FFBE`)**.
   - *Nota de diseño obligatoria*: Se cambió el color original dorado de Leyenda a Verde Esmeralda para evitar la duplicidad estética con el rango Oro y dotarlo de una jerarquía visual radiante y exclusiva.

---

## 3. Personajes y Skins (De Imagen #3)

### 3.1 Vulcan (Soldado Humano)
- **Lema**: *«Disciplina y precisión.»*
- **Rol**: Asalto equilibrado / Vanguardia.
- **Armadura**: Placas oscuras de carbono, visor cian brillante y acentos azul noche.
- **Habilidad**: *Barrera Energética* — Despliega una cobertura 3D translúcida de energía cian frente al jugador con 300 HP de resistencia y 8s de duración.
- **Skins**: Vanguardia Élite (Navy/Cian), Sombra Cibernética (Obsidiana/Esmeralda), Tormenta de Titanio (Titanio/Cian).

### 3.2 Nyx (Alienígena)
- **Lema**: *«El poder de otro mundo.»*
- **Rol**: Infiltración y velocidad táctica.
- **Armadura**: Exoesqueleto biomecánico violeta y magenta iridiscente con crestas aerodinámicas y destellos bioluminiscentes.
- **Habilidad**: *Pulso de Interferencia (EMP)* — Onda radial que satura sensores enemigos y otorga +40% de velocidad de movimiento por 5 segundos.
- **Skins**: Bioluminiscente Xeno (Violeta), Caminante del Vacío (Negro/Magenta), Sinfonía Esmeralda (Edición Leyenda con cristales bio-resonantes).

### 3.3 Razor (Soldado Pesado)
- **Lema**: *«Resiste. Avanza. Gana.»*
- **Rol**: Tanque de choque y defensa pesada.
- **Armadura**: Blindaje colosal de contención balística con hombreras gigantes y visor ámbar dorado.
- **Habilidad**: *Sensor Táctico & Fortificación* — Escaneo de radar que revela enemigos y aplica un sobreescudo de 50 puntos de vida temporal.
- **Skins**: Blindaje Juggernaut (Gris/Ámbar), Reactor de Peligro (Negro/Amarillo Industrial), Fragua de Obsidiana (Magma/Fuego).

---

## 4. Arsenal Balístico (Las 5 Armas de Imagen #2)

| Arma | Rol de Diseño | Cadencia | Daño | Cargador | Recarga | Particularidad 3D |
| :--- | :--- | :---: | :---: | :---: | :---: | :--- |
| **Nexus-AR Valiant** | Rifle de Asalto (Versátil y equilibrado) | 620 RPM | 28 | 30 | 2.2s | Chasis grafito con acentos dorados y LED cian |
| **Vortex Rail-Sniper** | Francotirador (Alta precisión y daño) | 52 RPM | 115 | 5 | 3.2s | Cañón magnético con mira de aumento telescópica |
| **Havoc Scattergun** | Escopeta (Ideal corta distancia) | 85 RPM | 18x8 | 8 | 2.8s | Rejillas de plasma naranja ardiente y dispersión destructiva |
| **Spectre-9 SMG** | Subfusil (Alta cadencia de fuego) | 980 RPM | 20 | 35 | 1.8s | Compacta con agarre ergonómico y neón azul eléctrico |
| **Apex-11 Sidearm** | Pistola (Rápida y confiable) | 380 RPM | 34 | 12 | 1.4s | Corredera táctica bitono con mira holográfica cian |

---

## 5. Mapas y Destrucción 3D (Referencias de Imagen #2)

### 5.1 Distrito Helix (Referencia 1 y 4 de Imagen #2)
- Sector urbano futurista con rascacielos cibernéticos, avenidas de combate, pasarelas elevadas y un pedestal de objetivo central.
- **Destrucción dinámica**: Barricadas de contención táctica y contenedores de plasma que se fragmentan en piezas físicas 3D al recibir disparos.

### 5.2 Laboratorio Nexus-X (Referencia 2 y 3 de Imagen #2)
- Complejo subterráneo de reactores cuánticos con pasillos estrechos para combate de alta velocidad y choques de habilidades de escudo y EMP.

### 5.3 Campo de Entrenamiento
- Polígono de pruebas para calibración de retroceso, distancias y dianas destructibles.
