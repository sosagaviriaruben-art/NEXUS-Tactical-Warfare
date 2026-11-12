using System;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.UI
{
    public enum GameRankTier
    {
        Bronce,
        Plata,
        Oro,
        Platino,
        Diamante,
        Heroico,
        Leyenda // Con color Esmeralda radiante (#00FF88) como solicitó el usuario!
    }

    public class RankData
    {
        public GameRankTier Tier { get; set; }
        public string Name { get; set; } = "";
        public string Division { get; set; } = "I";
        public Color PrimaryColor { get; set; }
        public Color AccentColor { get; set; }
        public int RequiredPoints { get; set; }
        public string Description { get; set; } = "";

        public static RankData[] AllRanks = new RankData[]
        {
            new RankData
            {
                Tier = GameRankTier.Bronce,
                Name = "BRONCE",
                Division = "I",
                PrimaryColor = new Color(180, 100, 45, 255),
                AccentColor = new Color(225, 150, 80, 255),
                RequiredPoints = 0,
                Description = "Rango inicial de recluta táctico."
            },
            new RankData
            {
                Tier = GameRankTier.Plata,
                Name = "PLATA",
                Division = "I",
                PrimaryColor = new Color(185, 195, 205, 255),
                AccentColor = new Color(240, 245, 255, 255),
                RequiredPoints = 1000,
                Description = "Agente disciplinado en combate regular."
            },
            new RankData
            {
                Tier = GameRankTier.Oro,
                Name = "ORO",
                Division = "I",
                PrimaryColor = new Color(255, 200, 0, 255),
                AccentColor = new Color(255, 240, 100, 255),
                RequiredPoints = 2500,
                Description = "Operador de élite con gran dominio de armas."
            },
            new RankData
            {
                Tier = GameRankTier.Platino,
                Name = "PLATINO",
                Division = "I",
                PrimaryColor = new Color(0, 220, 255, 255),
                AccentColor = new Color(180, 255, 255, 255),
                RequiredPoints = 4000,
                Description = "Estratega táctico avanzado en combates 5v5."
            },
            new RankData
            {
                Tier = GameRankTier.Diamante,
                Name = "DIAMANTE",
                Division = "I",
                PrimaryColor = new Color(170, 40, 255, 255),
                AccentColor = new Color(225, 140, 255, 255),
                RequiredPoints = 6000,
                Description = "Maestría suprema en habilidades y precisión."
            },
            new RankData
            {
                Tier = GameRankTier.Heroico,
                Name = "HEROICO",
                Division = "I",
                PrimaryColor = new Color(255, 30, 70, 255),
                AccentColor = new Color(255, 120, 140, 255),
                RequiredPoints = 8500,
                Description = "Veterano de guerra legendario en el frente."
            },
            new RankData
            {
                Tier = GameRankTier.Leyenda,
                Name = "LEYENDA",
                Division = "MAX",
                // COLOR ESMERALDA RADIANTE (reemplazando el dorado repetido de imagen 1):
                PrimaryColor = new Color(0, 235, 120, 255),   // Esmeralda vibrante
                AccentColor = new Color(120, 255, 190, 255),  // Brillo esmeralda cristalino
                RequiredPoints = 12000,
                Description = "Cúspide competitiva de Nexus. Distinción Esmeralda exclusiva."
            }
        };
    }

    public static class RankRenderer
    {
        public static void DrawRankBadge(int centerX, int centerY, RankData rank, float scale = 1.0f, bool pulse = false)
        {
            float t = (float)Raylib.GetTime();
            float pulseScale = pulse ? 1.0f + 0.05f * MathF.Sin(t * 4f) : 1.0f;
            float s = scale * pulseScale;

            Color pColor = rank.PrimaryColor;
            Color aColor = rank.AccentColor;

            // Halo / Aura de resplandor
            if (rank.Tier == GameRankTier.Leyenda)
            {
                // Resplandor Esmeralda místico pulsante
                for (int i = 3; i >= 1; i--)
                {
                    Color glow = new Color(0, 235, 120, (int)(40 / i));
                    Raylib.DrawCircle(centerX, centerY, (38 + i * 8) * s, glow);
                }
            }
            else if (rank.Tier == GameRankTier.Heroico)
            {
                Raylib.DrawCircle(centerX, centerY, 42 * s, new Color(255, 30, 70, 40));
            }
            else if (rank.Tier == GameRankTier.Oro)
            {
                Raylib.DrawCircle(centerX, centerY, 40 * s, new Color(255, 200, 0, 40));
            }

            // Escudo base
            int sides = rank.Tier switch
            {
                GameRankTier.Bronce => 4,
                GameRankTier.Plata => 5,
                GameRankTier.Oro => 5,
                GameRankTier.Platino => 6,
                GameRankTier.Diamante => 4,
                GameRankTier.Heroico => 6,
                GameRankTier.Leyenda => 8, // Corona octogonal esmeralda
                _ => 6
            };

            float outerRadius = 32f * s;
            float innerRadius = 24f * s;

            // Sombra
            Raylib.DrawPoly(new Vector2(centerX + 2, centerY + 3), sides, outerRadius, 45f, new Color(10, 15, 25, 200));

            // Borde exterior
            Raylib.DrawPoly(new Vector2(centerX, centerY), sides, outerRadius, 45f, pColor);
            // Centro oscuro
            Raylib.DrawPoly(new Vector2(centerX, centerY), sides, innerRadius, 45f, new Color(20, 25, 35, 255));

            // Icono central representativo
            if (rank.Tier == GameRankTier.Leyenda)
            {
                // Corona / Joya esmeralda central
                Vector2 top = new Vector2(centerX, centerY - 14 * s);
                Vector2 left = new Vector2(centerX - 12 * s, centerY);
                Vector2 right = new Vector2(centerX + 12 * s, centerY);
                Vector2 bottom = new Vector2(centerX, centerY + 14 * s);

                Raylib.DrawTriangle(top, left, right, aColor);
                Raylib.DrawTriangle(bottom, right, left, pColor);

                // Destello esmeralda central
                Raylib.DrawCircle(centerX, centerY, 5 * s, Color.White);
            }
            else if (rank.Tier == GameRankTier.Oro)
            {
                // Estrella de oro
                Raylib.DrawPoly(new Vector2(centerX, centerY), 5, 14 * s, 18f, aColor);
                Raylib.DrawPoly(new Vector2(centerX, centerY), 5, 8 * s, 18f, pColor);
            }
            else if (rank.Tier == GameRankTier.Heroico)
            {
                // Alas de fénix
                Raylib.DrawTriangle(
                    new Vector2(centerX, centerY - 14 * s),
                    new Vector2(centerX - 14 * s, centerY + 8 * s),
                    new Vector2(centerX + 14 * s, centerY + 8 * s),
                    aColor
                );
            }
            else if (rank.Tier == GameRankTier.Diamante)
            {
                // Rombo brillante
                Raylib.DrawPoly(new Vector2(centerX, centerY), 4, 14 * s, 45f, aColor);
            }
            else
            {
                // Emblema estándar
                Raylib.DrawPoly(new Vector2(centerX, centerY), sides, 12 * s, 45f, aColor);
            }

            // Anillo brillante exterior
            Raylib.DrawPolyLinesEx(new Vector2(centerX, centerY), sides, outerRadius, 2.5f * s, 45f, aColor);
        }
    }
}
