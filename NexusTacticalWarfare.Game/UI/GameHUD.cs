using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NexusTacticalWarfare.Game.Entities;
using NexusTacticalWarfare.Game.Weapons;
using NexusTacticalWarfare.Game.World;

namespace NexusTacticalWarfare.Game.UI
{
    public class KillFeedEntry
    {
        public string Killer = "";
        public string Victim = "";
        public string WeaponName = "";
        public float Timer = 4.0f;
    }

    public class GameHUD
    {
        private readonly List<KillFeedEntry> _killFeed = new();

        public void AddKillFeed(string killer, string victim, string weapon)
        {
            _killFeed.Insert(0, new KillFeedEntry
            {
                Killer = killer,
                Victim = victim,
                WeaponName = weapon,
                Timer = 4.5f
            });

            if (_killFeed.Count > 5) _killFeed.RemoveAt(_killFeed.Count - 1);
        }

        public void Update(float dt)
        {
            for (int i = _killFeed.Count - 1; i >= 0; i--)
            {
                _killFeed[i].Timer -= dt;
                if (_killFeed[i].Timer <= 0f) _killFeed.RemoveAt(i);
            }
        }

        public void DrawHUD(
            int screenW,
            int screenH,
            PlayerController player,
            List<AIBot> bots,
            MapScene map,
            int roundScoreAllies,
            int roundScoreEnemies,
            float roundTimeLeft,
            bool isPaused,
            Texture2D weaponTex,
            Action onResume,
            Action onReturnLobby)
        {
            // 0. Arma en Primera Persona Realista (De Imagen #2)
            DrawFirstPersonWeaponOverlay(screenW, screenH, player, weaponTex);

            // 1. Mira Táctica / Crosshair
            DrawCrosshair(screenW, screenH, player);

            // 2. Barra de Vida y Escudo (Esquina inferior izquierda)
            DrawHealthAndShield(screenW, screenH, player);

            // 3. Indicador de Munición y Arma (Esquina inferior derecha)
            DrawAmmoDisplay(screenW, screenH, player);

            // 4. Habilidad de Personaje (Centro inferior)
            DrawAbilityIcon(screenW, screenH, player);

            // 5. Encabezado de Ronda 5v5 (Centro superior)
            DrawRoundHeader(screenW, screenH, bots, player, roundScoreAllies, roundScoreEnemies, roundTimeLeft);

            // 6. Minimapa / Radar Táctico (Esquina superior izquierda)
            DrawMinimap(screenW, screenH, player, bots, map);

            // 7. Kill Feed (Esquina superior derecha)
            DrawKillFeed(screenW, screenH);

            // 8. Mira de Francotirador (Scope Overlay)
            if (player.IsAiming && player.CurrentWeapon.Definition.Type == WeaponType.SniperRifle)
            {
                DrawSniperScope(screenW, screenH);
            }

            // 9. Menú de Pausa
            if (isPaused)
            {
                DrawPauseMenu(screenW, screenH, onResume, onReturnLobby);
            }
        }

        private void DrawFirstPersonWeaponOverlay(int screenW, int screenH, PlayerController player, Texture2D weaponTex)
        {
            if (player.IsDead || weaponTex.Id == 0) return;
            if (player.IsAiming && player.CurrentWeapon.Definition.Type == WeaponType.SniperRifle) return;

            float walkTime = (float)Raylib.GetTime();
            float isMoving = player.Velocity.Length() > 0.5f ? 1f : 0f;
            float bobX = MathF.Cos(walkTime * 8f) * 6f * isMoving;
            float bobY = MathF.Sin(walkTime * 16f) * 4f * isMoving;
            float kick = player.CurrentWeapon.RecoilOffset * 50f;

            int wpW = 340;
            int wpH = 125;
            int wpX = player.IsAiming ? (screenW / 2 - wpW / 2) : (screenW - wpW - 40 + (int)bobX);
            int wpY = screenH - wpH - 15 + (int)bobY + (int)kick;

            // Guante / Mano tactica
            Raylib.DrawRectangle(wpX + 45, wpY + 75, 75, 45, player.Character.CurrentSkin.ArmorColorPrimary);
            Raylib.DrawRectangleLines(wpX + 45, wpY + 75, 75, 45, player.Character.CurrentSkin.VisorColor);

            // Textura del arma de Imagen #2
            Raylib.DrawTexturePro(
                weaponTex,
                new Rectangle(0, 0, weaponTex.Width, weaponTex.Height),
                new Rectangle(wpX, wpY, wpW, wpH),
                Vector2.Zero,
                0f,
                Color.White
            );

            // Muzzle flash al disparar
            if (player.CurrentWeapon.RecoilOffset > 0.015f)
            {
                Raylib.DrawCircle(wpX + 20, wpY + 28, 22, Color.Yellow);
                Raylib.DrawCircle(wpX + 20, wpY + 28, 12, Color.White);
            }
        }

        private void DrawCrosshair(int screenW, int screenH, PlayerController player)
        {
            int cx = screenW / 2;
            int cy = screenH / 2;

            if (player.IsAiming && player.CurrentWeapon.Definition.Type == WeaponType.SniperRifle)
            {
                return; // Ocultar crosshair normal si usa scope de francotirador
            }

            float spread = player.CurrentWeapon.Definition.SpreadAngle * 400f * (player.IsAiming ? 0.4f : 1.0f);
            int gap = (int)(6 + spread + player.CurrentWeapon.RecoilOffset * 25f);
            int len = 7;

            Color crossCol = new Color(0, 240, 255, 230);

            // Cuatro líneas de mira
            Raylib.DrawLine(cx - gap - len, cy, cx - gap, cy, crossCol);
            Raylib.DrawLine(cx + gap, cy, cx + gap + len, cy, crossCol);
            Raylib.DrawLine(cx, cy - gap - len, cx, cy - gap, crossCol);
            Raylib.DrawLine(cx, cy + gap, cx, cy + gap + len, crossCol);
            Raylib.DrawCircle(cx, cy, 1.5f, crossCol);

            // Hitmarker indicador de impacto
            if (player.HitmarkerTimer > 0f)
            {
                Color hitCol = player.WasHeadshot ? Color.Red : Color.White;
                int hSize = 8;
                Raylib.DrawLine(cx - hSize, cy - hSize, cx - 2, cy - 2, hitCol);
                Raylib.DrawLine(cx + hSize, cy - hSize, cx + 2, cy - 2, hitCol);
                Raylib.DrawLine(cx - hSize, cy + hSize, cx - 2, cy + 2, hitCol);
                Raylib.DrawLine(cx + hSize, cy + hSize, cx + 2, cy + 2, hitCol);
            }
        }

        private void DrawHealthAndShield(int screenW, int screenH, PlayerController player)
        {
            int x = 30;
            int y = screenH - 85;

            // Barra de Escudo (Cian)
            Raylib.DrawRectangle(x, y, 220, 18, new Color(15, 22, 35, 200));
            float shieldRatio = player.CurrentShield / player.MaxShield;
            Raylib.DrawRectangle(x, y, (int)(220 * Math.Clamp(shieldRatio, 0f, 1f)), 18, new Color(0, 200, 255, 240));
            Raylib.DrawRectangleLines(x, y, 220, 18, new Color(0, 240, 255, 120));
            Raylib.DrawText($"ESCUDO  {(int)player.CurrentShield}", x + 8, y + 2, 13, Color.White);

            // Barra de Salud (Verde / Rojo cuando bajo)
            Raylib.DrawRectangle(x, y + 24, 220, 22, new Color(15, 22, 35, 200));
            float hpRatio = player.CurrentHealth / player.MaxHealth;
            Color hpColor = hpRatio > 0.35f ? new Color(0, 235, 120, 240) : new Color(255, 40, 40, 240);
            Raylib.DrawRectangle(x, y + 24, (int)(220 * Math.Clamp(hpRatio, 0f, 1f)), 22, hpColor);
            Raylib.DrawRectangleLines(x, y + 24, 220, 22, Color.White);
            Raylib.DrawText($"SALUD  {(int)player.CurrentHealth}", x + 8, y + 27, 15, Color.White);
        }

        private void DrawAmmoDisplay(int screenW, int screenH, PlayerController player)
        {
            var weapon = player.CurrentWeapon;
            int w = 210;
            int x = screenW - w - 30;
            int y = screenH - 85;

            Raylib.DrawRectangle(x, y, w, 50, new Color(12, 18, 30, 200));
            Raylib.DrawRectangleLinesEx(new Rectangle(x, y, w, 50), 1.5f, weapon.Definition.AccentColor);

            string ammoStr = $"{weapon.CurrentAmmo} / {weapon.ReserveAmmo}";
            Raylib.DrawText(ammoStr, x + 15, y + 10, 24, weapon.CurrentAmmo == 0 ? Color.Red : Color.White);

            Raylib.DrawText(weapon.Definition.Name, x + 15, y + 34, 11, weapon.Definition.LightGlowColor);

            if (weapon.IsReloading)
            {
                Raylib.DrawText("RECARGANDO...", x + 100, y + 14, 13, Color.Gold);
            }
        }

        private void DrawAbilityIcon(int screenW, int screenH, PlayerController player)
        {
            int cx = screenW / 2;
            int y = screenH - 75;
            int size = 48;

            Rectangle rec = new Rectangle(cx - size / 2, y, size, size);
            bool ready = player.AbilityReady;

            Raylib.DrawRectangleRec(rec, ready ? new Color(0, 160, 220, 180) : new Color(25, 30, 40, 180));
            Raylib.DrawRectangleLinesEx(rec, 2, ready ? new Color(0, 240, 255, 255) : Color.DarkGray);

            Raylib.DrawText("[E]", cx - 11, y + 6, 14, Color.Gold);

            if (!ready)
            {
                string cdStr = $"{player.AbilityCooldownTimer:0.0}s";
                Raylib.DrawText(cdStr, cx - 14, y + 26, 12, Color.LightGray);
            }
            else
            {
                Raylib.DrawText("LISTO", cx - 16, y + 26, 11, Color.White);
            }

            Raylib.DrawText(player.Character.AbilityName, cx - Raylib.MeasureText(player.Character.AbilityName, 12) / 2, y - 18, 12, new Color(0, 220, 255, 230));
        }

        private void DrawRoundHeader(
            int screenW,
            int screenH,
            List<AIBot> bots,
            PlayerController player,
            int scoreAllies,
            int scoreEnemies,
            float timeLeft)
        {
            int cx = screenW / 2;
            int y = 12;

            int alliesAlive = (player.IsDead ? 0 : 1);
            int enemiesAlive = 0;

            foreach (var b in bots)
            {
                if (b.IsDead) continue;
                if (b.Team == BotTeam.Allied) alliesAlive++;
                else enemiesAlive++;
            }

            // Marcador
            Raylib.DrawRectangle(cx - 150, y, 300, 42, new Color(10, 15, 25, 220));
            Raylib.DrawRectangleLinesEx(new Rectangle(cx - 150, y, 300, 42), 1.5f, new Color(0, 180, 240, 150));

            // Equipo Azul
            Raylib.DrawText($"{alliesAlive} VIVOS", cx - 140, y + 12, 14, new Color(0, 200, 255, 255));
            Raylib.DrawText($"{scoreAllies}", cx - 50, y + 8, 24, new Color(0, 220, 255, 255));

            // Tiempo restante
            int minutes = (int)(timeLeft / 60);
            int seconds = (int)(timeLeft % 60);
            string timeStr = $"{minutes:00}:{seconds:00}";
            Raylib.DrawText(timeStr, cx - 22, y + 12, 16, Color.Gold);

            // Equipo Rojo
            Raylib.DrawText($"{scoreEnemies}", cx + 38, y + 8, 24, new Color(255, 60, 60, 255));
            Raylib.DrawText($"{enemiesAlive} VIVOS", cx + 70, y + 12, 14, new Color(255, 60, 60, 255));
        }

        private void DrawMinimap(int screenW, int screenH, PlayerController player, List<AIBot> bots, MapScene map)
        {
            int size = 110;
            int x = 20;
            int y = 20;

            Raylib.DrawRectangle(x, y, size, size, new Color(10, 15, 22, 210));
            Raylib.DrawRectangleLines(x, y, size, size, new Color(0, 180, 255, 120));

            int mcx = x + size / 2;
            int mcy = y + size / 2;
            float mapScale = 1.15f;

            // Punto de objetivo
            Vector2 bombOffset = new Vector2(map.BombSite.X - player.Position.X, map.BombSite.Z - player.Position.Z) * mapScale;
            Raylib.DrawCircle(mcx + (int)bombOffset.X, mcy + (int)bombOffset.Y, 3, Color.Gold);

            // Bots aliados y enemigos en radar
            foreach (var b in bots)
            {
                if (b.IsDead) continue;
                Vector2 diff = new Vector2(b.Position.X - player.Position.X, b.Position.Z - player.Position.Z) * mapScale;
                if (Math.Abs(diff.X) < size / 2 - 4 && Math.Abs(diff.Y) < size / 2 - 4)
                {
                    Color col = b.Team == BotTeam.Allied ? new Color(0, 200, 255, 255) : Color.Red;
                    Raylib.DrawCircle(mcx + (int)diff.X, mcy + (int)diff.Y, 3, col);
                }
            }

            // Jugador (Flecha verde/cian central)
            Raylib.DrawCircle(mcx, mcy, 3.5f, new Color(0, 255, 120, 255));
            Vector2 fwd = new Vector2(MathF.Sin(player.Yaw), MathF.Cos(player.Yaw)) * 7f;
            Raylib.DrawLine(mcx, mcy, mcx + (int)fwd.X, mcy + (int)fwd.Y, Color.White);
        }

        private void DrawKillFeed(int screenW, int screenH)
        {
            int x = screenW - 270;
            int y = 20;

            for (int i = 0; i < _killFeed.Count; i++)
            {
                var k = _killFeed[i];
                float alpha = Math.Clamp(k.Timer / 0.5f, 0f, 1f);
                Color bg = new Color(15, 20, 30, (int)(190 * alpha));
                Raylib.DrawRectangle(x, y + i * 26, 250, 22, bg);

                string msg = $"{k.Killer} ➔ {k.Victim}";
                Raylib.DrawText(msg, x + 8, y + i * 26 + 4, 12, new Color(240, 240, 240, (int)(255 * alpha)));
            }
        }

        private void DrawSniperScope(int screenW, int screenH)
        {
            int cx = screenW / 2;
            int cy = screenH / 2;
            int radius = screenH / 2 - 40;

            // Viñeta negra alrededor de la mira
            Raylib.DrawCircleLines(cx, cy, radius, new Color(180, 50, 255, 255));
            Raylib.DrawCircleLines(cx, cy, radius + 2, new Color(180, 50, 255, 200));

            // Retícula militar de francotirador
            Raylib.DrawLine(cx - radius, cy, cx + radius, cy, new Color(200, 80, 255, 180));
            Raylib.DrawLine(cx, cy - radius, cx, cy + radius, new Color(200, 80, 255, 180));

            for (int i = -4; i <= 4; i++)
            {
                if (i == 0) continue;
                Raylib.DrawLine(cx + i * 35, cy - 6, cx + i * 35, cy + 6, new Color(200, 80, 255, 150));
                Raylib.DrawLine(cx - 6, cy + i * 35, cx + 6, cy + i * 35, new Color(200, 80, 255, 150));
            }
        }

        private void DrawPauseMenu(int screenW, int screenH, Action onResume, Action onReturnLobby)
        {
            Raylib.DrawRectangle(0, 0, screenW, screenH, new Color(5, 10, 18, 190));

            int w = 340;
            int h = 260;
            Rectangle rec = new Rectangle(screenW / 2 - w / 2, screenH / 2 - h / 2, w, h);
            Raylib.DrawRectangleRec(rec, new Color(15, 22, 35, 255));
            Raylib.DrawRectangleLinesEx(rec, 2, new Color(0, 220, 255, 255));

            Raylib.DrawText("PAUSA TÁCTICA", (int)rec.X + 85, (int)rec.Y + 25, 22, Color.White);

            Rectangle resumeBtn = new Rectangle(rec.X + 40, rec.Y + 80, w - 80, 42);
            bool hovResume = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), resumeBtn);
            Raylib.DrawRectangleRec(resumeBtn, hovResume ? new Color(0, 180, 240, 255) : new Color(25, 38, 58, 255));
            Raylib.DrawRectangleLinesEx(resumeBtn, 1, Color.White);
            Raylib.DrawText("REANUDAR COMBATE", (int)resumeBtn.X + 40, (int)resumeBtn.Y + 12, 15, Color.White);

            if (hovResume && Raylib.IsMouseButtonPressed(MouseButton.Left)) onResume();

            Rectangle lobbyBtn = new Rectangle(rec.X + 40, rec.Y + 145, w - 80, 42);
            bool hovLobby = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), lobbyBtn);
            Raylib.DrawRectangleRec(lobbyBtn, hovLobby ? new Color(220, 50, 50, 255) : new Color(45, 25, 30, 255));
            Raylib.DrawRectangleLinesEx(lobbyBtn, 1, Color.White);
            Raylib.DrawText("REGRESAR AL LOBBY", (int)lobbyBtn.X + 40, (int)lobbyBtn.Y + 12, 15, Color.White);

            if (hovLobby && Raylib.IsMouseButtonPressed(MouseButton.Left)) onReturnLobby();
        }
    }
}
