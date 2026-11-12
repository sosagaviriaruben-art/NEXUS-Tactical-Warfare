using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.World
{
    public class StaticStructure
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public Color MainColor { get; set; }
        public Color TrimColor { get; set; }
        public string Name { get; set; } = "Structure";

        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                Position - Size * 0.5f,
                Position + Size * 0.5f
            );
        }
    }

    public class MapScene
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string ReferenceImageDetail { get; set; } = "";
        public Vector3 PlayerSpawn { get; set; } = new Vector3(0, 1.8f, 25f);
        public List<Vector3> EnemySpawns { get; set; } = new();
        public List<Vector3> AllySpawns { get; set; } = new();
        public Vector3 BombSite { get; set; } = new Vector3(0, 0.5f, 0);

        public List<StaticStructure> Structures { get; set; } = new();
        public List<DestructibleObject> Destructibles { get; set; } = new();
        public Color SkyTopColor { get; set; } = new Color(15, 25, 45, 255);
        public Color SkyBottomColor { get; set; } = new Color(5, 10, 20, 255);
        public Color FloorColor { get; set; } = new Color(30, 36, 46, 255);
        public Color GridColor { get; set; } = new Color(0, 180, 230, 50);
    }

    public static class MapBuilder
    {
        public static List<MapScene> BuildAllMaps()
        {
            return new List<MapScene>
            {
                BuildDistritoHelix(),
                BuildInstalacionTecnologica(),
                BuildCampoEntrenamiento()
            };
        }

        // 1. DISTRITO HELIX (Referencia 1 y 4 de Imagen 2: Combate en ciudad futurista con destrucción y verticalidad)
        private static MapScene BuildDistritoHelix()
        {
            var map = new MapScene
            {
                Id = "distrito_helix",
                Name = "DISTRITO HELIX",
                Description = "Metrópolis futurista con rascacielos cibernéticos, avenidas tácticas y coberturas destructibles.",
                ReferenceImageDetail = "Inspirado en la Imagen #2 (Ref 1: Ciudad futurista con destrucción + Ref 4: Verticalidad)",
                PlayerSpawn = new Vector3(0, 1.8f, 32f),
                BombSite = new Vector3(0, 0.5f, 0),
                FloorColor = new Color(28, 32, 42, 255),
                SkyTopColor = new Color(10, 25, 50, 255),
                SkyBottomColor = new Color(5, 12, 25, 255)
            };

            // Spawns de bots
            map.AllySpawns.Add(new Vector3(-6f, 1.8f, 30f));
            map.AllySpawns.Add(new Vector3(6f, 1.8f, 30f));
            map.AllySpawns.Add(new Vector3(-12f, 1.8f, 32f));
            map.AllySpawns.Add(new Vector3(12f, 1.8f, 32f));

            map.EnemySpawns.Add(new Vector3(0f, 1.8f, -32f));
            map.EnemySpawns.Add(new Vector3(-8f, 1.8f, -30f));
            map.EnemySpawns.Add(new Vector3(8f, 1.8f, -30f));
            map.EnemySpawns.Add(new Vector3(-14f, 1.8f, -32f));
            map.EnemySpawns.Add(new Vector3(14f, 1.8f, -32f));

            // Rascacielos y edificios perimetrales futuristas
            map.Structures.Add(new StaticStructure { Position = new Vector3(-35f, 25f, 0f), Size = new Vector3(18f, 50f, 75f), MainColor = new Color(18, 24, 34, 255), TrimColor = new Color(0, 200, 255, 255), Name = "Torre Norte Helix" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(35f, 25f, 0f), Size = new Vector3(18f, 50f, 75f), MainColor = new Color(18, 24, 34, 255), TrimColor = new Color(0, 200, 255, 255), Name = "Torre Sur Helix" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 15f, -45f), Size = new Vector3(60f, 30f, 12f), MainColor = new Color(15, 20, 30, 255), TrimColor = new Color(0, 160, 240, 255), Name = "Complejo Defensor" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 15f, 45f), Size = new Vector3(60f, 30f, 12f), MainColor = new Color(15, 20, 30, 255), TrimColor = new Color(0, 160, 240, 255), Name = "Hangar Atacante" });

            // Plataforma Central del Núcleo Nexus (Zona de objetivo)
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 0.4f, 0f), Size = new Vector3(16f, 0.8f, 16f), MainColor = new Color(38, 48, 64, 255), TrimColor = new Color(0, 255, 220, 255), Name = "Plataforma Objetivo" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(-16f, 2.5f, 0f), Size = new Vector3(6f, 5f, 18f), MainColor = new Color(25, 32, 44, 255), TrimColor = new Color(0, 180, 255, 255), Name = "Pasarela Flanco Izquierdo" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(16f, 2.5f, 0f), Size = new Vector3(6f, 5f, 18f), MainColor = new Color(25, 32, 44, 255), TrimColor = new Color(0, 180, 255, 255), Name = "Pasarela Flanco Derecho" });

            // Elementos destructibles (Como en Referencia 1 de Imagen 2: calles con barricadas y paneles rompibles)
            map.Destructibles.Add(new DestructibleObject(new Vector3(-4f, 1.2f, 8f), new Vector3(4f, 2.4f, 1f), 80f, new Color(45, 55, 70, 255), new Color(0, 230, 255, 255), "Barricada Táctica A"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(4f, 1.2f, 8f), new Vector3(4f, 2.4f, 1f), 80f, new Color(45, 55, 70, 255), new Color(0, 230, 255, 255), "Barricada Táctica B"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(-5f, 1.2f, -8f), new Vector3(4f, 2.4f, 1f), 80f, new Color(45, 55, 70, 255), new Color(255, 100, 30, 255), "Barricada Enemiga Norte"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(5f, 1.2f, -8f), new Vector3(4f, 2.4f, 1f), 80f, new Color(45, 55, 70, 255), new Color(255, 100, 30, 255), "Barricada Enemiga Sur"));

            // Cajas energéticas destructibles
            map.Destructibles.Add(new DestructibleObject(new Vector3(-10f, 1.0f, 15f), new Vector3(2f, 2f, 2f), 50f, new Color(30, 40, 55, 255), new Color(0, 255, 150, 255), "Contenedor de Plasma 1"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(10f, 1.0f, 15f), new Vector3(2f, 2f, 2f), 50f, new Color(30, 40, 55, 255), new Color(0, 255, 150, 255), "Contenedor de Plasma 2"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(0f, 1.0f, -15f), new Vector3(2.5f, 2f, 2.5f), 65f, new Color(30, 40, 55, 255), new Color(0, 255, 150, 255), "Generador Auxiliar"));

            return map;
        }

        // 2. INSTALACIÓN TECNOLÓGICA (Referencia 2 y 3 de Imagen 2: Acción rápida en corredores y núcleo de energía)
        private static MapScene BuildInstalacionTecnologica()
        {
            var map = new MapScene
            {
                Id = "instalacion_tech",
                Name = "LABORATORIO NEXUS-X",
                Description = "Instalación subterránea de investigación militar con pasillos de combate cerrado y núcleo de fusión.",
                ReferenceImageDetail = "Inspirado en la Imagen #2 (Ref 2: Acción fluida en interiores + Ref 3: Habilidades en núcleo)",
                PlayerSpawn = new Vector3(0, 1.8f, 28f),
                BombSite = new Vector3(0, 0.5f, 0),
                FloorColor = new Color(22, 26, 34, 255),
                SkyTopColor = new Color(15, 10, 25, 255),
                SkyBottomColor = new Color(5, 5, 12, 255)
            };

            // Spawns
            map.AllySpawns.Add(new Vector3(-5f, 1.8f, 26f));
            map.AllySpawns.Add(new Vector3(5f, 1.8f, 26f));
            map.AllySpawns.Add(new Vector3(-10f, 1.8f, 28f));
            map.AllySpawns.Add(new Vector3(10f, 1.8f, 28f));

            map.EnemySpawns.Add(new Vector3(0f, 1.8f, -28f));
            map.EnemySpawns.Add(new Vector3(-6f, 1.8f, -26f));
            map.EnemySpawns.Add(new Vector3(6f, 1.8f, -26f));
            map.EnemySpawns.Add(new Vector3(-12f, 1.8f, -28f));
            map.EnemySpawns.Add(new Vector3(12f, 1.8f, -28f));

            // Estructuras de laboratorio
            map.Structures.Add(new StaticStructure { Position = new Vector3(-25f, 8f, 0f), Size = new Vector3(8f, 16f, 60f), MainColor = new Color(20, 24, 32, 255), TrimColor = new Color(180, 50, 255, 255), Name = "Muro Laboratorio A" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(25f, 8f, 0f), Size = new Vector3(8f, 16f, 60f), MainColor = new Color(20, 24, 32, 255), TrimColor = new Color(180, 50, 255, 255), Name = "Muro Laboratorio B" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 8f, -35f), Size = new Vector3(45f, 16f, 6f), MainColor = new Color(20, 24, 32, 255), TrimColor = new Color(180, 50, 255, 255), Name = "Sector Reactor" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 8f, 35f), Size = new Vector3(45f, 16f, 6f), MainColor = new Color(20, 24, 32, 255), TrimColor = new Color(180, 50, 255, 255), Name = "Acceso Principal" });

            // Columnas cuánticas centrales
            map.Structures.Add(new StaticStructure { Position = new Vector3(-8f, 5f, 0f), Size = new Vector3(3f, 10f, 3f), MainColor = new Color(30, 36, 48, 255), TrimColor = new Color(0, 240, 255, 255), Name = "Pilar Cuántico 1" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(8f, 5f, 0f), Size = new Vector3(3f, 10f, 3f), MainColor = new Color(30, 36, 48, 255), TrimColor = new Color(0, 240, 255, 255), Name = "Pilar Cuántico 2" });

            // Barricadas destructibles de contención
            map.Destructibles.Add(new DestructibleObject(new Vector3(0f, 1.2f, 10f), new Vector3(6f, 2.4f, 1.2f), 90f, new Color(40, 45, 55, 255), new Color(160, 40, 255, 255), "Mampara de Contención A"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(0f, 1.2f, -10f), new Vector3(6f, 2.4f, 1.2f), 90f, new Color(40, 45, 55, 255), new Color(160, 40, 255, 255), "Mampara de Contención B"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(-12f, 1.0f, 0f), new Vector3(2f, 2f, 2f), 55f, new Color(35, 30, 45, 255), new Color(255, 50, 180, 255), "Celda Energética Izquierda"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(12f, 1.0f, 0f), new Vector3(2f, 2f, 2f), 55f, new Color(35, 30, 45, 255), new Color(255, 50, 180, 255), "Celda Energética Derecha"));

            return map;
        }

        // 3. CAMPO DE ENTRENAMIENTO
        private static MapScene BuildCampoEntrenamiento()
        {
            var map = new MapScene
            {
                Id = "campo_entrenamiento",
                Name = "CAMPO DE ENTRENAMIENTO",
                Description = "Polígono de tiro táctico con blancos holográficos, distancias medidas y obstáculos de prueba.",
                ReferenceImageDetail = "Espacio de pruebas para calibrar las 5 armas y habilidades",
                PlayerSpawn = new Vector3(0, 1.8f, 20f),
                BombSite = new Vector3(0, 0.5f, -15f),
                FloorColor = new Color(25, 30, 40, 255),
                SkyTopColor = new Color(15, 30, 55, 255),
                SkyBottomColor = new Color(8, 15, 30, 255)
            };

            // Dummies y dianas de prueba
            map.EnemySpawns.Add(new Vector3(-8f, 1.8f, -10f));
            map.EnemySpawns.Add(new Vector3(0f, 1.8f, -15f));
            map.EnemySpawns.Add(new Vector3(8f, 1.8f, -10f));
            map.EnemySpawns.Add(new Vector3(-14f, 1.8f, -5f));
            map.EnemySpawns.Add(new Vector3(14f, 1.8f, -5f));

            map.Structures.Add(new StaticStructure { Position = new Vector3(-20f, 6f, 0f), Size = new Vector3(4f, 12f, 45f), MainColor = new Color(22, 28, 38, 255), TrimColor = new Color(0, 220, 255, 255), Name = "Límite Oeste" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(20f, 6f, 0f), Size = new Vector3(4f, 12f, 45f), MainColor = new Color(22, 28, 38, 255), TrimColor = new Color(0, 220, 255, 255), Name = "Límite Este" });
            map.Structures.Add(new StaticStructure { Position = new Vector3(0f, 6f, -25f), Size = new Vector3(44f, 12f, 4f), MainColor = new Color(22, 28, 38, 255), TrimColor = new Color(0, 220, 255, 255), Name = "Muro de Tiro Trasero" });

            // Coberturas destructibles de tiro
            map.Destructibles.Add(new DestructibleObject(new Vector3(-6f, 1.0f, 5f), new Vector3(2.5f, 2f, 1f), 60f, new Color(40, 50, 65, 255), new Color(0, 255, 200, 255), "Diana Destructible 1"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(6f, 1.0f, 5f), new Vector3(2.5f, 2f, 1f), 60f, new Color(40, 50, 65, 255), new Color(0, 255, 200, 255), "Diana Destructible 2"));
            map.Destructibles.Add(new DestructibleObject(new Vector3(0f, 1.0f, -5f), new Vector3(4f, 2f, 1f), 70f, new Color(40, 50, 65, 255), new Color(0, 255, 200, 255), "Obstáculo Central"));

            return map;
        }
    }
}
