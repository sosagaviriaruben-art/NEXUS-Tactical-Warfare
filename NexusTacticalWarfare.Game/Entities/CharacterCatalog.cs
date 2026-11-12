using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.Entities
{
    public enum CharacterId
    {
        Vulcan, // Soldado Humano (Imagen 3)
        Nyx,    // Alienígena (Imagen 3)
        Razor   // Soldado Pesado (Imagen 3)
    }

    public class CharacterSkin
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Rarity { get; set; } = "Épica";
        public Color ArmorColorPrimary { get; set; }
        public Color ArmorColorSecondary { get; set; }
        public Color VisorColor { get; set; }
        public Color GlowColor { get; set; }
        public string Description { get; set; } = "";
    }

    public class CharacterDefinition
    {
        public CharacterId Id { get; set; }
        public string Name { get; set; } = "";
        public string Title { get; set; } = "";
        public string Role { get; set; } = "";
        public string Motto { get; set; } = "";
        public string Lore { get; set; } = "";
        public string AbilityName { get; set; } = "";
        public string AbilityDesc { get; set; } = "";
        public float AbilityCooldown { get; set; } = 12f;
        public float BaseHealth { get; set; } = 100f;
        public float BaseShield { get; set; } = 100f;
        public float SpeedMultiplier { get; set; } = 1.0f;
        public List<CharacterSkin> Skins { get; set; } = new();
        public int SelectedSkinIndex { get; set; } = 0;

        public CharacterSkin CurrentSkin => Skins[Math.Clamp(SelectedSkinIndex, 0, Skins.Count - 1)];

        public static List<CharacterDefinition> CreateCatalog()
        {
            return new List<CharacterDefinition>
            {
                // 1. SOLDADO HUMANO (VULCAN) - De Imagen #3: "Disciplina y precisión."
                new CharacterDefinition
                {
                    Id = CharacterId.Vulcan,
                    Name = "VULCAN",
                    Title = "SOLDADO HUMANO",
                    Role = "Asalto Táctico / Vanguardia",
                    Motto = "«Disciplina y precisión.»",
                    Lore = "Veterano de las fuerzas terrestres especiales, equipado con exoesqueleto táctico de absorción cinética y visor holográfico de combate.",
                    AbilityName = "Barrera Energética",
                    AbilityDesc = "Despliega una cobertura translúcida de energía cian que bloquea disparos frontales durante 8s (destruible).",
                    AbilityCooldown = 12f,
                    BaseHealth = 100f,
                    BaseShield = 100f,
                    SpeedMultiplier = 1.0f,
                    Skins = new List<CharacterSkin>
                    {
                        new CharacterSkin
                        {
                            Id = "vulcan_default",
                            Name = "Vanguardia Élite",
                            Rarity = "Épica",
                            ArmorColorPrimary = new Color(25, 35, 45, 255),    // Carbono oscuro
                            ArmorColorSecondary = new Color(40, 55, 75, 255),  // Azul táctico
                            VisorColor = new Color(0, 240, 255, 255),          // Visor Cian brillante
                            GlowColor = new Color(0, 210, 255, 255),
                            Description = "Diseño estándar de infantería de choque Nexus."
                        },
                        new CharacterSkin
                        {
                            Id = "vulcan_camo",
                            Name = "Sombra Cibernética",
                            Rarity = "Legendaria",
                            ArmorColorPrimary = new Color(15, 18, 22, 255),
                            ArmorColorSecondary = new Color(60, 70, 80, 255),
                            VisorColor = new Color(0, 255, 136, 255),          // Visor Esmeralda
                            GlowColor = new Color(0, 235, 120, 255),
                            Description = "Camuflaje de sigilo urbano con emisión electromagnética reducida."
                        },
                        new CharacterSkin
                        {
                            Id = "vulcan_specops",
                            Name = "Tormenta de Titanio",
                            Rarity = "Heroica",
                            ArmorColorPrimary = new Color(190, 195, 205, 255),
                            ArmorColorSecondary = new Color(230, 235, 245, 255),
                            VisorColor = new Color(0, 160, 255, 255),
                            GlowColor = new Color(100, 200, 255, 255),
                            Description = "Placas de titanio pulido para operaciones de alta visibilidad."
                        }
                    }
                },

                // 2. ALIENÍGENA (NYX) - De Imagen #3: "El poder de otro mundo."
                new CharacterDefinition
                {
                    Id = CharacterId.Nyx,
                    Name = "NYX",
                    Title = "ALIENÍGENA",
                    Role = "Infiltración y Movilidad",
                    Motto = "«El poder de otro mundo.»",
                    Lore = "Guerrera biomecánica de la civilización Xeno-Nexus. Su fisiología ágil y armadura iridiscente le otorgan velocidad sobrenatural y manipulación sensorial.",
                    AbilityName = "Pulso de Interferencia",
                    AbilityDesc = "Emite una onda electromagnética violeta que distorsiona sensores enemigos y otorga 40% de velocidad por 5s.",
                    AbilityCooldown = 10f,
                    BaseHealth = 90f,
                    BaseShield = 110f,
                    SpeedMultiplier = 1.15f,
                    Skins = new List<CharacterSkin>
                    {
                        new CharacterSkin
                        {
                            Id = "nyx_default",
                            Name = "Bioluminiscente Xeno",
                            Rarity = "Épica",
                            ArmorColorPrimary = new Color(55, 20, 75, 255),    // Violeta alienígena
                            ArmorColorSecondary = new Color(90, 35, 120, 255), // Púrpura iridiscente
                            VisorColor = new Color(0, 255, 240, 255),          // Destellos cian
                            GlowColor = new Color(200, 50, 255, 255),
                            Description = "Exoesqueleto bioluminiscente de alta vibración molecular."
                        },
                        new CharacterSkin
                        {
                            Id = "nyx_void",
                            Name = "Caminante del Vacío",
                            Rarity = "Legendaria",
                            ArmorColorPrimary = new Color(12, 10, 20, 255),
                            ArmorColorSecondary = new Color(30, 25, 45, 255),
                            VisorColor = new Color(255, 0, 180, 255),          // Magenta etéreo
                            GlowColor = new Color(230, 0, 140, 255),
                            Description = "Material absorbente de fotones forjado en los abismos estelares."
                        },
                        new CharacterSkin
                        {
                            Id = "nyx_emerald",
                            Name = "Sinfonía Esmeralda",
                            Rarity = "Leyenda",
                            ArmorColorPrimary = new Color(10, 40, 30, 255),
                            ArmorColorSecondary = new Color(20, 80, 60, 255),
                            VisorColor = new Color(0, 255, 140, 255),          // Esmeralda puro
                            GlowColor = new Color(0, 235, 120, 255),
                            Description = "Edición conmemorativa Leyenda con cristales de esmeralda bio-resonantes."
                        }
                    }
                },

                // 3. SOLDADO PESADO (RAZOR) - De Imagen #3: "Resiste. Avanza. Gana."
                new CharacterDefinition
                {
                    Id = CharacterId.Razor,
                    Name = "RAZOR",
                    Title = "SOLDADO PESADO",
                    Role = "Soporte Pesado / Defensor",
                    Motto = "«Resiste. Avanza. Gana.»",
                    Lore = "Coloso de blindaje reactivo modular. Diseñado para soportar artillería pesada y mantener posiciones estratégicas de control.",
                    AbilityName = "Sensor Táctico & Fortificación",
                    AbilityDesc = "Rastrea siluetas enemigas a través de obstáculos y genera un sobreescudo defensivo temporal de 50 pts.",
                    AbilityCooldown = 15f,
                    BaseHealth = 130f,
                    BaseShield = 100f,
                    SpeedMultiplier = 0.92f,
                    Skins = new List<CharacterSkin>
                    {
                        new CharacterSkin
                        {
                            Id = "razor_default",
                            Name = "Blindaje Juggernaut",
                            Rarity = "Épica",
                            ArmorColorPrimary = new Color(35, 40, 45, 255),    // Placas blindadas oscuras
                            ArmorColorSecondary = new Color(65, 75, 85, 255),  // Acero balístico
                            VisorColor = new Color(255, 170, 0, 255),          // Visor Ámbar dorado
                            GlowColor = new Color(255, 140, 0, 255),
                            Description = "Armadura pesada de contención con placas balísticas reforzadas."
                        },
                        new CharacterSkin
                        {
                            Id = "razor_hazard",
                            Name = "Reactor de Peligro",
                            Rarity = "Heroica",
                            ArmorColorPrimary = new Color(20, 20, 22, 255),
                            ArmorColorSecondary = new Color(220, 160, 0, 255),  // Amarillo reactivo industrial
                            VisorColor = new Color(255, 220, 0, 255),
                            GlowColor = new Color(255, 180, 0, 255),
                            Description = "Diseño de advertencia para ambientes de radiación hostiles."
                        },
                        new CharacterSkin
                        {
                            Id = "razor_magma",
                            Name = "Fragua de Obsidiana",
                            Rarity = "Legendaria",
                            ArmorColorPrimary = new Color(18, 12, 12, 255),
                            ArmorColorSecondary = new Color(180, 40, 10, 255), // Magma ardiente
                            VisorColor = new Color(255, 60, 20, 255),
                            GlowColor = new Color(255, 80, 0, 255),
                            Description = "Acero templado con núcleos de fisión de alta presión."
                        }
                    }
                }
            };
        }
    }
}
