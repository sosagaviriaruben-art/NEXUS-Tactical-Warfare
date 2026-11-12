using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NexusTacticalWarfare.Game.Entities;
using NexusTacticalWarfare.Game.Weapons;
using NexusTacticalWarfare.Game.Audio;
using NexusTacticalWarfare.Game.World;

namespace NexusTacticalWarfare.Game.UI
{
    public enum LobbyTab
    {
        Main,
        Characters,
        Arsenal,
        Gallery,
        Shop,
        Events,
        RankInspect,
        Settings
    }

    public class LobbyAndCoverUI
    {
        public LobbyTab CurrentTab = LobbyTab.Main;
        public int SelectedRankIndex = 6; // Por defecto: LEYENDA (Esmeralda)
        public int SelectedMapIndex = 0;
        public int SelectedGameMode = 0; // 0: 5v5 Competitivo, 1: Partida Rapida, 2: Entrenamiento

        public float CharacterRotation = 0f;

        // Texturas Realistas en Alta Definicion
        private Texture2D _texCover;
        private Texture2D _texLobbyHangar;
        private Texture2D _texVulcan;
        private Texture2D _texNyx;
        private Texture2D _texRazor;
        private Texture2D _texWeaponAR;
        private Texture2D _texWeaponSniper;
        private Texture2D _texWeaponShotgun;
        private Texture2D _texWeaponSMG;
        private Texture2D _texWeaponPistol;
        private Texture2D _texMap1;
        private Texture2D _texMap2;
        private Texture2D _texMap3;
        private Texture2D _texMap4;
        private Texture2D _texEmerald;

        public void LoadAssets()
        {
            try
            {
                // Portada compuesta: fondo ciudad+luna + 3 personajes + logo NEXUS Tactical Warfare
                _texCover = LoadTextureSafe("cover_nexus.png");
                if (_texCover.Id == 0) _texCover = LoadTextureSafe("cover_art.png");
                if (_texCover.Id == 0) _texCover = LoadTextureSafe("imagen2.png");

                _texLobbyHangar = LoadTextureSafe("lobby_hangar.png");
                if (_texLobbyHangar.Id == 0) _texLobbyHangar = LoadTextureSafe("lobby_bg.png");

                _texVulcan = LoadTextureSafe("char_vulcan.png");
                _texNyx = LoadTextureSafe("char_nyx.png");
                _texRazor = LoadTextureSafe("char_razor.png");

                _texWeaponAR = LoadTextureSafe("weapon_ar.png");
                _texWeaponSniper = LoadTextureSafe("weapon_sniper.png");
                _texWeaponShotgun = LoadTextureSafe("weapon_shotgun.png");
                _texWeaponSMG = LoadTextureSafe("weapon_smg.png");
                _texWeaponPistol = LoadTextureSafe("weapon_pistol.png");

                _texMap1 = LoadTextureSafe("map1.png");
                _texMap2 = LoadTextureSafe("map2.png");
                _texMap3 = LoadTextureSafe("map3.png");
                _texMap4 = LoadTextureSafe("map4.png");

                _texEmerald = LoadTextureSafe("rank_emerald.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LobbyAndCoverUI] Error cargando texturas: " + ex.Message);
            }
        }

        private static Texture2D LoadTextureSafe(string fileName)
        {
            string[] searchPaths = new string[]
            {
                System.IO.Path.Combine("Assets", fileName),
                System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", fileName),
                System.IO.Path.Combine(AppContext.BaseDirectory, fileName),
                fileName,
                System.IO.Path.Combine("..", "Assets", fileName),
                System.IO.Path.Combine("..", fileName)
            };

            foreach (var path in searchPaths)
            {
                if (System.IO.File.Exists(path))
                {
                    var tex = Raylib.LoadTexture(path);
                    if (tex.Id > 0) return tex;
                }
            }
            return new Texture2D();
        }

        public void UnloadAssets()
        {
            if (_texCover.Id > 0) Raylib.UnloadTexture(_texCover);
            if (_texLobbyHangar.Id > 0) Raylib.UnloadTexture(_texLobbyHangar);
            if (_texVulcan.Id > 0) Raylib.UnloadTexture(_texVulcan);
            if (_texNyx.Id > 0) Raylib.UnloadTexture(_texNyx);
            if (_texRazor.Id > 0) Raylib.UnloadTexture(_texRazor);
            if (_texWeaponAR.Id > 0) Raylib.UnloadTexture(_texWeaponAR);
            if (_texWeaponSniper.Id > 0) Raylib.UnloadTexture(_texWeaponSniper);
            if (_texWeaponShotgun.Id > 0) Raylib.UnloadTexture(_texWeaponShotgun);
            if (_texWeaponSMG.Id > 0) Raylib.UnloadTexture(_texWeaponSMG);
            if (_texWeaponPistol.Id > 0) Raylib.UnloadTexture(_texWeaponPistol);
            if (_texMap1.Id > 0) Raylib.UnloadTexture(_texMap1);
            if (_texMap2.Id > 0) Raylib.UnloadTexture(_texMap2);
            if (_texMap3.Id > 0) Raylib.UnloadTexture(_texMap3);
            if (_texMap4.Id > 0) Raylib.UnloadTexture(_texMap4);
            if (_texEmerald.Id > 0) Raylib.UnloadTexture(_texEmerald);
        }

        // =========================================================================
        // PANTALLA DE PORTADA — NEXUS TACTICAL WARFARE
        // Composicion: fondo ciudad/luna (imagen2) + 3 personajes (imagen3) + logo
        // =========================================================================
        public void DrawCoverScreen(int screenW, int screenH, Action onEnterLobby)
        {
            // --- FONDO: cover_nexus.png (ya contiene fondo+personajes+logo integrados) ---
            if (_texCover.Id > 0)
            {
                // Mostrar la portada compuesta a pantalla completa sin deformacion de aspecto
                float scaleX = (float)screenW / _texCover.Width;
                float scaleY = (float)screenH / _texCover.Height;
                float scale  = Math.Max(scaleX, scaleY);
                int dstW = (int)(_texCover.Width  * scale);
                int dstH = (int)(_texCover.Height * scale);
                int offX = (screenW - dstW) / 2;
                int offY = (screenH - dstH) / 2;

                Raylib.DrawTexturePro(
                    _texCover,
                    new Rectangle(0, 0, _texCover.Width, _texCover.Height),
                    new Rectangle(offX, offY, dstW, dstH),
                    Vector2.Zero,
                    0f,
                    Color.White
                );
            }
            else
            {
                // Fallback: fondo oscuro degradado si no carga la portada
                Raylib.DrawRectangleGradientV(0, 0, screenW, screenH,
                    new Color(8, 14, 28, 255), new Color(2, 5, 14, 255));

                // Dibujar logo de texto como respaldo
                string nexusText = "NEXUS";
                int nw = Raylib.MeasureText(nexusText, 90);
                Raylib.DrawText(nexusText, screenW/2 - nw/2 + 3, 153, 90, new Color(5,10,20,200));
                Raylib.DrawText(nexusText, screenW/2 - nw/2, 150, 90, new Color(215,228,248,255));

                string twText = "TACTICAL WARFARE";
                int tw = Raylib.MeasureText(twText, 28);
                Raylib.DrawLine(screenW/2 - tw/2 - 90, 253, screenW/2 - tw/2 - 10, 253, new Color(140,170,210,200));
                Raylib.DrawText(twText, screenW/2 - tw/2, 242, 28, new Color(155,185,220,255));
                Raylib.DrawLine(screenW/2 + tw/2 + 10, 253, screenW/2 + tw/2 + 90, 253, new Color(140,170,210,200));

                string tag = "HUMANOS  *  ALIENS  *  UNA SOLA BATALLA";
                int taw = Raylib.MeasureText(tag, 18);
                Raylib.DrawText(tag, screenW/2 - taw/2, 285, 18, new Color(185,200,220,230));
            }

            // --- PANEL INFERIOR SEMITRANSPARENTE para los botones de acceso ---
            // Solo aparece en la franja inferior para no tapar la portada
            Raylib.DrawRectangleGradientV(0, screenH - 200, screenW, 200,
                new Color(4, 8, 18, 0), new Color(4, 8, 18, 220));

            // Botones de Acceso
            int btnY = screenH - 152;
            Rectangle guestBtn  = new Rectangle(screenW / 2 - 190, btnY, 380, 50);
            Rectangle googleBtn = new Rectangle(screenW / 2 - 190, btnY + 62, 182, 40);
            Rectangle fbBtn     = new Rectangle(screenW / 2 + 8,   btnY + 62, 182, 40);

            Vector2 mouse = Raylib.GetMousePosition();
            bool hoverGuest  = Raylib.CheckCollisionPointRec(mouse, guestBtn);
            bool hoverGoogle = Raylib.CheckCollisionPointRec(mouse, googleBtn);
            bool hoverFb     = Raylib.CheckCollisionPointRec(mouse, fbBtn);

            // Boton principal: INGRESAR
            Color guestBg = hoverGuest ? new Color(0, 235, 255, 255) : new Color(0, 170, 235, 245);
            Raylib.DrawRectangleRec(guestBtn, guestBg);
            Raylib.DrawRectangleLinesEx(guestBtn, 2, Color.White);
            string enterText = "INGRESAR AL JUEGO";
            int enterW = Raylib.MeasureText(enterText, 19);
            Raylib.DrawText(enterText,
                (int)(guestBtn.X + guestBtn.Width / 2 - enterW / 2),
                (int)(guestBtn.Y + 15), 19, Color.Black);

            // Boton Google
            Color gBg = hoverGoogle ? new Color(245, 245, 245, 255) : new Color(190, 190, 190, 215);
            Raylib.DrawRectangleRec(googleBtn, gBg);
            Raylib.DrawRectangleLinesEx(googleBtn, 1, new Color(180,180,180,200));
            Raylib.DrawText("G  Google Login",
                (int)googleBtn.X + 26, (int)googleBtn.Y + 12, 15, Color.Black);

            // Boton Facebook
            Color fbBg = hoverFb ? new Color(80, 120, 200, 255) : new Color(50, 82, 150, 215);
            Raylib.DrawRectangleRec(fbBtn, fbBg);
            Raylib.DrawRectangleLinesEx(fbBtn, 1, new Color(60,90,180,200));
            Raylib.DrawText("f  Facebook Login",
                (int)fbBtn.X + 22, (int)fbBtn.Y + 12, 15, Color.White);

            // Activar al hacer clic o presionar Espacio/Enter
            if (Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsKeyPressed(KeyboardKey.Enter) ||
                (hoverGuest  && Raylib.IsMouseButtonPressed(MouseButton.Left)) ||
                (hoverGoogle && Raylib.IsMouseButtonPressed(MouseButton.Left)) ||
                (hoverFb     && Raylib.IsMouseButtonPressed(MouseButton.Left)))
            {
                SoundSynthesizer.PlayClick();
                onEnterLobby();
            }

            // Hint inferior
            string info = "NEXUS TACTICAL WARFARE  --  Presiona ESPACIO o haz clic para continuar";
            int infoW = Raylib.MeasureText(info, 12);
            Raylib.DrawText(info, screenW / 2 - infoW / 2, screenH - 22, 12,
                new Color(155, 180, 210, 200));
        }

        // =========================================================================
        // LOBBY PRINCIPAL (Fidelidad Realista a la Imagen #1)
        // =========================================================================
        public void DrawLobby(
            int screenW,
            int screenH,
            CharacterDefinition currentCharacter,
            List<CharacterDefinition> allCharacters,
            WeaponDefinition currentWeapon,
            List<WeaponDefinition> allWeapons,
            List<MapScene> allMaps,
            Action onStartMatch)
        {
            float dt = Raylib.GetFrameTime();
            CharacterRotation += dt * 25f;

            // 1. FONDO FOTORREALISTA DEL HANGAR MILITAR (Imagen #1)
            if (_texLobbyHangar.Id > 0)
            {
                Raylib.DrawTexturePro(
                    _texLobbyHangar,
                    new Rectangle(0, 0, _texLobbyHangar.Width, _texLobbyHangar.Height),
                    new Rectangle(0, 0, screenW, screenH),
                    Vector2.Zero,
                    0f,
                    Color.White
                );
                // Velo suave para armonizar con la interfaz
                Raylib.DrawRectangle(0, 0, screenW, screenH, new Color(4, 8, 16, 60));
            }
            else
            {
                Raylib.DrawRectangleGradientV(0, 0, screenW, screenH, new Color(12, 16, 26, 255), new Color(4, 7, 14, 255));
            }

            // 2. BARRA SUPERIOR (Limpia, sin errores de texto '?')
            DrawTopBar(screenW, screenH);

            // 3. MENU LATERAL IZQUIERDO (Limpio y elegante)
            DrawLeftSidebar(screenW, screenH);

            // 4. PERSONAJE REALISTA DE ALTA DEFINICION EN EL CENTRO (Imagen #3)
            DrawCenterRealisticCharacter(screenW, screenH, currentCharacter, currentWeapon);

            // 5. PANEL DERECHO: TEMPORADA 1, RANGO ESMERALDA Y BOTON JUGAR
            DrawRightPanel(screenW, screenH, allMaps, onStartMatch);

            // 6. SUB-TARJETAS INFERIORES DE LA IMAGEN #1
            DrawBottomCards(screenW, screenH, currentCharacter);

            // Modales desplegables con graficos de alta fidelidad
            if (CurrentTab == LobbyTab.Characters) DrawCharactersModal(screenW, screenH, allCharacters);
            else if (CurrentTab == LobbyTab.Arsenal) DrawArsenalModal(screenW, screenH, allWeapons);
            else if (CurrentTab == LobbyTab.Gallery) DrawGalleryModal(screenW, screenH, allMaps);
            else if (CurrentTab == LobbyTab.RankInspect) DrawRankTreeModal(screenW, screenH);
            else if (CurrentTab == LobbyTab.Settings) DrawSettingsModal(screenW, screenH);
        }

        private void DrawTopBar(int screenW, int screenH)
        {
            Rectangle topBarRec = new Rectangle(0, 0, screenW, 54);
            Raylib.DrawRectangleRec(topBarRec, new Color(8, 14, 22, 230));
            Raylib.DrawLine(0, 54, screenW, 54, new Color(0, 190, 255, 90));

            // Perfil de jugador
            Raylib.DrawCircle(32, 27, 17, new Color(0, 190, 255, 255));
            Raylib.DrawCircle(32, 27, 14, new Color(18, 25, 38, 255));
            Raylib.DrawText("NX", 23, 20, 13, Color.White);

            Raylib.DrawText("OPERADOR_NEXUS", 58, 12, 15, Color.White);
            Raylib.DrawText("NIVEL 42 -- PASE DE BATALLA ACTIVO", 58, 30, 11, new Color(0, 220, 255, 220));

            string titleText = "NEXUS: PENDIENTE TACTICAL SHOOTER";
            int tw = Raylib.MeasureText(titleText, 15);
            Raylib.DrawText(titleText, screenW / 2 - tw / 2, 18, 15, new Color(0, 235, 120, 255)); // Toque esmeralda

            // Divisas limpias (sin caracteres corruptos '?')
            int rightX = screenW - 330;

            Raylib.DrawRectangle(rightX, 12, 100, 30, new Color(15, 22, 34, 220));
            Raylib.DrawRectangleLines(rightX, 12, 100, 30, new Color(255, 200, 0, 100));
            Raylib.DrawText("ORO: 12,500", rightX + 8, 20, 12, new Color(255, 215, 0, 255));

            Raylib.DrawRectangle(rightX + 108, 12, 105, 30, new Color(15, 22, 34, 220));
            Raylib.DrawRectangleLines(rightX + 108, 12, 105, 30, new Color(0, 235, 120, 140));
            Raylib.DrawText("CRISTAL: 150", rightX + 108 + 8, 20, 12, new Color(0, 235, 120, 255));

            Rectangle settBtn = new Rectangle(rightX + 222, 12, 90, 30);
            bool hoverSett = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), settBtn);
            Raylib.DrawRectangleRec(settBtn, hoverSett ? new Color(0, 180, 240, 220) : new Color(20, 28, 44, 220));
            Raylib.DrawRectangleLinesEx(settBtn, 1, Color.White);
            Raylib.DrawText("AJUSTES", (int)settBtn.X + 14, (int)settBtn.Y + 8, 13, Color.White);

            if (hoverSett && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                SoundSynthesizer.PlayClick();
                CurrentTab = (CurrentTab == LobbyTab.Settings) ? LobbyTab.Main : LobbyTab.Settings;
            }
        }

        private void DrawLeftSidebar(int screenW, int screenH)
        {
            int startY = 75;
            int itemH = 46;
            int menuW = 165;

            // Etiquetas limpias en ASCII para maxima legibilidad
            string[] items = new string[]
            {
                "[1] JUGAR",
                "[2] PERSONAJE",
                "[3] ARMAS",
                "[4] GALERIA",
                "[5] TIENDA",
                "[6] EVENTOS"
            };

            LobbyTab[] tabs = new LobbyTab[]
            {
                LobbyTab.Main,
                LobbyTab.Characters,
                LobbyTab.Arsenal,
                LobbyTab.Gallery,
                LobbyTab.Shop,
                LobbyTab.Events
            };

            Vector2 mouse = Raylib.GetMousePosition();

            for (int i = 0; i < items.Length; i++)
            {
                Rectangle rec = new Rectangle(18, startY + i * (itemH + 8), menuW, itemH);
                bool isSelected = CurrentTab == tabs[i];
                bool hover = Raylib.CheckCollisionPointRec(mouse, rec);

                Color bg = isSelected ? new Color(0, 180, 240, 230) : (hover ? new Color(22, 35, 54, 220) : new Color(12, 18, 30, 200));
                Color border = isSelected ? Color.White : (hover ? new Color(0, 220, 255, 220) : new Color(0, 150, 220, 90));

                Raylib.DrawRectangleRec(rec, bg);
                Raylib.DrawRectangleLinesEx(rec, 1.5f, border);

                Color textCol = isSelected ? Color.Black : (hover ? Color.White : new Color(195, 220, 245, 255));
                Raylib.DrawText(items[i], (int)rec.X + 16, (int)rec.Y + 14, 15, textCol);

                if (hover && Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    SoundSynthesizer.PlayClick();
                    CurrentTab = tabs[i];
                }
            }
        }

        // =========================================================================
        // PERSONAJE REALISTA DE ALTA CALIDAD EN EL CENTRO (De Imagen #3)
        // =========================================================================
        private void DrawCenterRealisticCharacter(int screenW, int screenH, CharacterDefinition character, WeaponDefinition weapon)
        {
            float cx = screenW / 2;
            float cy = screenH / 2 + 10;
            float time = (float)Raylib.GetTime();

            // Animacion sutil de respiracion
            float breathe = MathF.Sin(time * 2.5f) * 3f;

            // Plinto holografico circular brillante en el suelo
            int floorY = (int)(cy + 175);
            Raylib.DrawEllipse((int)cx, floorY, 175, 44, new Color(0, 220, 255, 35));
            Raylib.DrawEllipseLines((int)cx, floorY, 175, 44, new Color(0, 255, 220, 200));
            Raylib.DrawEllipseLines((int)cx, floorY, 140, 34, new Color(0, 200, 255, 140));
            Raylib.DrawEllipseLines((int)cx, floorY, 95, 22, new Color(0, 240, 255, 100));

            // Particulas holograficas ascendentes
            for (int p = 0; p < 8; p++)
            {
                float pTime = time * 0.8f + p * 0.78f;
                float px = cx + MathF.Sin(pTime * 3f) * 70f;
                float py = floorY - ((pTime * 80f) % 260f);
                Raylib.DrawCircle((int)px, (int)py, 2f, new Color(0, 240, 255, 180));
            }

            // Seleccionar textura del personaje segun eleccion del jugador
            Texture2D charTex = _texVulcan;
            if (character.Id == CharacterId.Nyx) charTex = _texNyx;
            else if (character.Id == CharacterId.Razor) charTex = _texRazor;

            // RENDERIZADO DEL PERSONAJE REALISTA DE IMAGEN #3
            if (charTex.Id > 0)
            {
                int drawW = 280;
                int drawH = 370;
                int drawX = (int)(cx - drawW / 2);
                int drawY = (int)(floorY - drawH + breathe);

                // Halo de iluminacion detras del personaje
                Color glow = character.Id == CharacterId.Nyx ? new Color(180, 50, 255, 30) :
                             (character.Id == CharacterId.Razor ? new Color(255, 150, 0, 30) : new Color(0, 200, 255, 30));
                Raylib.DrawCircle((int)cx, drawY + drawH / 2, 140, glow);

                // Dibujar el modelo realista
                Raylib.DrawTexturePro(
                    charTex,
                    new Rectangle(0, 0, charTex.Width, charTex.Height),
                    new Rectangle(drawX, drawY, drawW, drawH),
                    Vector2.Zero,
                    0f,
                    Color.White
                );

                // Marco holografico de seleccion
                Raylib.DrawRectangleLinesEx(new Rectangle(drawX - 2, drawY - 2, drawW + 4, drawH + 4), 1.5f, character.CurrentSkin.VisorColor);
            }

            // Placa informativa holografica inferior
            int infoY = floorY + 22;
            string heroName = character.Name;
            int hnw = Raylib.MeasureText(heroName, 26);

            // Fondo de placa
            Raylib.DrawRectangle((int)cx - 150, infoY - 6, 300, 48, new Color(10, 16, 26, 220));
            Raylib.DrawRectangleLinesEx(new Rectangle((int)cx - 150, infoY - 6, 300, 48), 1, character.CurrentSkin.VisorColor);

            Raylib.DrawText(heroName, (int)cx - hnw / 2, infoY, 26, Color.White);

            string heroTitle = $"{character.Title} -- {character.Role}";
            int htw = Raylib.MeasureText(heroTitle, 12);
            Raylib.DrawText(heroTitle, (int)cx - htw / 2, infoY + 28, 12, new Color(0, 230, 255, 230));

            string motto = character.Motto;
            int mw = Raylib.MeasureText(motto, 13);
            Raylib.DrawText(motto, (int)cx - mw / 2, infoY - 26, 13, new Color(255, 215, 0, 240));

            // Indicador de cambio con flechas
            Raylib.DrawText("< Flecha Izq", (int)cx - 140, floorY + 70, 11, new Color(160, 180, 210, 200));
            Raylib.DrawText("Flecha Der >", (int)cx + 70, floorY + 70, 11, new Color(160, 180, 210, 200));
        }

        // =========================================================================
        // PANEL DERECHO (Con Rango Esmeralda y Boton Jugar)
        // =========================================================================
        private void DrawRightPanel(int screenW, int screenH, List<MapScene> allMaps, Action onStartMatch)
        {
            int panelW = 275;
            int panelX = screenW - panelW - 20;
            int panelY = 70;

            Rectangle rightPanelRec = new Rectangle(panelX, panelY, panelW, screenH - panelY - 135);
            Raylib.DrawRectangleRec(rightPanelRec, new Color(10, 16, 28, 225));
            Raylib.DrawRectangleLinesEx(rightPanelRec, 1.5f, new Color(0, 180, 240, 110));

            // TEMPORADA 1
            Raylib.DrawRectangle(panelX + 15, panelY + 14, panelW - 30, 30, new Color(18, 26, 44, 255));
            Raylib.DrawText("TEMPORADA 1", panelX + 35, panelY + 20, 15, new Color(0, 240, 255, 255));

            // RANGO ESMERALDA (Cumpliendo el cambio de color solicitado para Leyenda)
            var currentRank = RankData.AllRanks[SelectedRankIndex];
            int badgeY = panelY + 110;

            // Insignia Esmeralda
            if (currentRank.Tier == GameRankTier.Leyenda && _texEmerald.Id > 0)
            {
                // Dibujar textura de insignia esmeralda con resplandor
                int bSize = 64;
                Raylib.DrawCircle(panelX + panelW / 2, badgeY, 36, new Color(0, 235, 120, 50));
                Raylib.DrawTexturePro(
                    _texEmerald,
                    new Rectangle(0, 0, _texEmerald.Width, _texEmerald.Height),
                    new Rectangle(panelX + panelW / 2 - bSize / 2, badgeY - bSize / 2, bSize, bSize),
                    Vector2.Zero,
                    0f,
                    Color.White
                );
            }
            else
            {
                RankRenderer.DrawRankBadge(panelX + panelW / 2, badgeY, currentRank, 1.15f, true);
            }

            string rankTitle = $"RANGO: {currentRank.Name} {currentRank.Division}";
            int rtw = Raylib.MeasureText(rankTitle, 16);
            Raylib.DrawText(rankTitle, panelX + panelW / 2 - rtw / 2, badgeY + 44, 16, currentRank.PrimaryColor);

            string pts = $"{currentRank.RequiredPoints} PTS COMPETITIVOS";
            int pw = Raylib.MeasureText(pts, 12);
            Raylib.DrawText(pts, panelX + panelW / 2 - pw / 2, badgeY + 66, 12, new Color(180, 200, 220, 200));

            // Boton Arbol de Rangos
            Rectangle inspectRankBtn = new Rectangle(panelX + 25, badgeY + 86, panelW - 50, 26);
            bool hoverInsp = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), inspectRankBtn);
            Raylib.DrawRectangleRec(inspectRankBtn, hoverInsp ? new Color(0, 235, 120, 220) : new Color(16, 28, 42, 220));
            Raylib.DrawRectangleLinesEx(inspectRankBtn, 1, hoverInsp ? Color.White : new Color(0, 235, 120, 160));
            Raylib.DrawText("Ver Arbol de Rangos", (int)inspectRankBtn.X + 24, (int)inspectRankBtn.Y + 6, 12, hoverInsp ? Color.Black : Color.White);

            if (hoverInsp && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                SoundSynthesizer.PlayClick();
                CurrentTab = (CurrentTab == LobbyTab.RankInspect) ? LobbyTab.Main : LobbyTab.RankInspect;
            }

            // Modo Seleccionado
            int modeY = badgeY + 124;
            Raylib.DrawText("MODO SELECCIONADO:", panelX + 20, modeY, 12, new Color(0, 200, 255, 255));

            string[] modes = new string[] { "Modo Competitivo 5v5", "Partida Rapida", "Entrenamiento" };
            for (int m = 0; m < modes.Length; m++)
            {
                Rectangle mRec = new Rectangle(panelX + 15, modeY + 18 + m * 28, panelW - 30, 24);
                bool sel = SelectedGameMode == m;
                bool hov = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), mRec);

                Raylib.DrawRectangleRec(mRec, sel ? new Color(0, 160, 240, 210) : (hov ? new Color(28, 40, 62, 200) : new Color(16, 22, 34, 180)));
                Raylib.DrawRectangleLinesEx(mRec, 1, sel ? Color.White : new Color(0, 140, 200, 60));
                Raylib.DrawText(modes[m], (int)mRec.X + 10, (int)mRec.Y + 5, 12, sel ? Color.White : new Color(175, 195, 215, 255));

                if (hov && Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    SoundSynthesizer.PlayClick();
                    SelectedGameMode = m;
                }
            }

            // Selector de Mapa
            int mapY = modeY + 112;
            Raylib.DrawText("MAPA DE COMBATE:", panelX + 20, mapY, 12, new Color(0, 200, 255, 255));
            Rectangle mapBtn = new Rectangle(panelX + 15, mapY + 18, panelW - 30, 30);
            bool hovMap = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), mapBtn);
            Raylib.DrawRectangleRec(mapBtn, hovMap ? new Color(0, 180, 240, 200) : new Color(18, 28, 44, 220));
            Raylib.DrawRectangleLinesEx(mapBtn, 1, Color.White);

            string mapName = allMaps[SelectedMapIndex].Name;
            Raylib.DrawText($"MAPA: {mapName}", (int)mapBtn.X + 12, (int)mapBtn.Y + 8, 12, Color.White);

            if (hovMap && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                SoundSynthesizer.PlayClick();
                SelectedMapIndex = (SelectedMapIndex + 1) % allMaps.Count;
            }

            // BOTON JUGAR GRANDE Y DORADO/AMARILLO (Imagen #1)
            int playBtnH = 54;
            int playBtnY = (int)(rightPanelRec.Y + rightPanelRec.Height - playBtnH - 14);
            Rectangle playBtnRec = new Rectangle(panelX + 15, playBtnY, panelW - 30, playBtnH);

            Vector2 mouse = Raylib.GetMousePosition();
            bool hoverPlay = Raylib.CheckCollisionPointRec(mouse, playBtnRec);

            Color playBg = hoverPlay ? new Color(255, 235, 45, 255) : new Color(245, 195, 0, 255);
            Raylib.DrawRectangleRec(playBtnRec, playBg);
            Raylib.DrawRectangleLinesEx(playBtnRec, 2, Color.White);

            string playText = "JUGAR";
            int ptw = Raylib.MeasureText(playText, 28);
            Raylib.DrawText(playText, (int)(playBtnRec.X + playBtnRec.Width / 2 - ptw / 2), (int)(playBtnRec.Y + 13), 28, Color.Black);

            if (hoverPlay && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                SoundSynthesizer.PlayClick();
                onStartMatch();
            }
        }

        // =========================================================================
        // SUB-TARJETAS INFERIORES (Imagen #1)
        // =========================================================================
        private void DrawBottomCards(int screenW, int screenH, CharacterDefinition currentCharacter)
        {
            int cardY = screenH - 105;
            int cardH = 88;
            int cardW = 165;
            int startX = 205;
            int spacing = 14;

            string[] titles = new string[]
            {
                "Modo Lucky Royal",
                "Galeria de Armas",
                "Tienda de Skins",
                "Personalizacion"
            };

            string[] sub = new string[]
            {
                "Ruleta Tactica (Mock)",
                "5 Armas de Imagen 2",
                "Skins de Imagen 3",
                $"{currentCharacter.Name}"
            };

            LobbyTab[] targets = new LobbyTab[]
            {
                LobbyTab.Shop,
                LobbyTab.Arsenal,
                LobbyTab.Shop,
                LobbyTab.Characters
            };

            Vector2 mouse = Raylib.GetMousePosition();

            for (int i = 0; i < titles.Length; i++)
            {
                Rectangle rec = new Rectangle(startX + i * (cardW + spacing), cardY, cardW, cardH);
                bool hover = Raylib.CheckCollisionPointRec(mouse, rec);

                Color bg = hover ? new Color(22, 35, 54, 230) : new Color(12, 18, 30, 210);
                Raylib.DrawRectangleRec(rec, bg);
                Raylib.DrawRectangleLinesEx(rec, 1.5f, hover ? new Color(0, 240, 255, 255) : new Color(0, 140, 200, 90));

                Raylib.DrawText(titles[i], (int)rec.X + 10, (int)rec.Y + 12, 13, Color.White);
                Raylib.DrawText(sub[i], (int)rec.X + 10, (int)rec.Y + 32, 11, new Color(0, 220, 255, 220));

                Raylib.DrawRectangle((int)rec.X + 10, (int)rec.Y + 56, cardW - 20, 20, new Color(18, 26, 40, 255));
                Raylib.DrawText("VER DETALLES", (int)rec.X + 16, (int)rec.Y + 60, 10, new Color(255, 215, 0, 220));

                if (hover && Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    SoundSynthesizer.PlayClick();
                    CurrentTab = targets[i];
                }
            }
        }

        // =========================================================================
        // MODALES CON ACTIVOS FOTORREALISTAS
        // =========================================================================
        private void DrawCharactersModal(int screenW, int screenH, List<CharacterDefinition> characters)
        {
            Rectangle modalRec = new Rectangle(100, 60, screenW - 200, screenH - 120);
            Raylib.DrawRectangleRec(modalRec, new Color(8, 12, 22, 250));
            Raylib.DrawRectangleLinesEx(modalRec, 2, new Color(0, 220, 255, 255));

            Raylib.DrawText("PERSONAJES Y SKINS REALISTAS (IMAGEN #3)", (int)modalRec.X + 25, (int)modalRec.Y + 18, 22, Color.White);
            Raylib.DrawText("Modelos 3D de alta definicion, habilidades unicas y biografias.", (int)modalRec.X + 25, (int)modalRec.Y + 46, 13, new Color(0, 220, 255, 220));

            Rectangle closeBtn = new Rectangle(modalRec.X + modalRec.Width - 42, modalRec.Y + 14, 28, 28);
            Raylib.DrawRectangleRec(closeBtn, Color.Red);
            Raylib.DrawText("X", (int)closeBtn.X + 9, (int)closeBtn.Y + 6, 16, Color.White);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeBtn) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                CurrentTab = LobbyTab.Main;
            }

            int cardW = ((int)modalRec.Width - 80) / 3;
            Texture2D[] texs = new Texture2D[] { _texVulcan, _texNyx, _texRazor };

            for (int i = 0; i < characters.Count; i++)
            {
                var ch = characters[i];
                Rectangle chRec = new Rectangle(modalRec.X + 25 + i * (cardW + 15), modalRec.Y + 75, cardW, modalRec.Height - 95);
                Raylib.DrawRectangleRec(chRec, new Color(16, 22, 34, 230));
                Raylib.DrawRectangleLinesEx(chRec, 1.5f, ch.CurrentSkin.VisorColor);

                // Retrato realista en alta resolucion de Imagen #3
                if (i < texs.Length && texs[i].Id > 0)
                {
                    int imgH = 175;
                    Raylib.DrawTexturePro(
                        texs[i],
                        new Rectangle(0, 0, texs[i].Width, texs[i].Height),
                        new Rectangle(chRec.X + 10, chRec.Y + 10, chRec.Width - 20, imgH),
                        Vector2.Zero,
                        0f,
                        Color.White
                    );
                    Raylib.DrawRectangleLinesEx(new Rectangle(chRec.X + 10, chRec.Y + 10, chRec.Width - 20, imgH), 1, ch.CurrentSkin.VisorColor);
                }

                int textStartY = (int)chRec.Y + 195;
                Raylib.DrawText(ch.Name, (int)chRec.X + 15, textStartY, 20, Color.White);
                Raylib.DrawText(ch.Title, (int)chRec.X + 15, textStartY + 24, 12, ch.CurrentSkin.VisorColor);
                Raylib.DrawText(ch.Motto, (int)chRec.X + 15, textStartY + 42, 11, new Color(255, 215, 0, 220));

                Raylib.DrawText($"Habilidad: {ch.AbilityName}", (int)chRec.X + 15, textStartY + 64, 12, new Color(0, 240, 255, 255));

                // Skins
                Raylib.DrawText("SKINS DISPONIBLES:", (int)chRec.X + 15, textStartY + 90, 11, Color.Gold);
                for (int s = 0; s < ch.Skins.Count; s++)
                {
                    var skin = ch.Skins[s];
                    Rectangle skinBtn = new Rectangle(chRec.X + 15, textStartY + 110 + s * 34, chRec.Width - 30, 28);
                    bool isCurrent = ch.SelectedSkinIndex == s;
                    bool hov = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), skinBtn);

                    Raylib.DrawRectangleRec(skinBtn, isCurrent ? skin.ArmorColorSecondary : (hov ? new Color(35, 45, 65, 255) : new Color(22, 28, 40, 255)));
                    Raylib.DrawRectangleLinesEx(skinBtn, 1, isCurrent ? Color.White : skin.VisorColor);
                    Raylib.DrawText($"* {skin.Name}", (int)skinBtn.X + 8, (int)skinBtn.Y + 7, 11, isCurrent ? Color.White : new Color(190, 210, 235, 255));

                    if (hov && Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        SoundSynthesizer.PlayClick();
                        ch.SelectedSkinIndex = s;
                    }
                }
            }
        }

        private void DrawArsenalModal(int screenW, int screenH, List<WeaponDefinition> weapons)
        {
            Rectangle modalRec = new Rectangle(90, 50, screenW - 180, screenH - 100);
            Raylib.DrawRectangleRec(modalRec, new Color(8, 12, 22, 252));
            Raylib.DrawRectangleLinesEx(modalRec, 2, new Color(0, 220, 255, 255));

            Raylib.DrawText("GALERIA DE ARMAS REALISTAS (LAS 5 ARMAS DE LA IMAGEN #2)", (int)modalRec.X + 25, (int)modalRec.Y + 18, 22, Color.White);
            Raylib.DrawText("Modelos de ciencia ficcion militar de alta fidelidad con miras holograficas y retroceso.", (int)modalRec.X + 25, (int)modalRec.Y + 44, 13, new Color(0, 220, 255, 220));

            Rectangle closeBtn = new Rectangle(modalRec.X + modalRec.Width - 42, modalRec.Y + 14, 28, 28);
            Raylib.DrawRectangleRec(closeBtn, Color.Red);
            Raylib.DrawText("X", (int)closeBtn.X + 9, (int)closeBtn.Y + 6, 16, Color.White);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeBtn) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                CurrentTab = LobbyTab.Main;
            }

            Texture2D[] wTexs = new Texture2D[] { _texWeaponAR, _texWeaponSniper, _texWeaponShotgun, _texWeaponSMG, _texWeaponPistol };

            for (int i = 0; i < weapons.Count; i++)
            {
                var w = weapons[i];
                Rectangle wRec = new Rectangle(modalRec.X + 25, modalRec.Y + 72 + i * 86, modalRec.Width - 50, 78);
                Raylib.DrawRectangleRec(wRec, new Color(16, 22, 34, 230));
                Raylib.DrawRectangleLinesEx(wRec, 1.5f, w.AccentColor);

                // Renderizado de la imagen real de la Imagen #2
                if (i < wTexs.Length && wTexs[i].Id > 0)
                {
                    int imgW = 180;
                    int imgH = 60;
                    Raylib.DrawTexturePro(
                        wTexs[i],
                        new Rectangle(0, 0, wTexs[i].Width, wTexs[i].Height),
                        new Rectangle(wRec.X + 15, wRec.Y + 9, imgW, imgH),
                        Vector2.Zero,
                        0f,
                        Color.White
                    );
                    Raylib.DrawRectangleLinesEx(new Rectangle(wRec.X + 15, wRec.Y + 9, imgW, imgH), 1, w.AccentColor);
                }

                Raylib.DrawText(w.Name, (int)wRec.X + 215, (int)wRec.Y + 14, 17, Color.White);
                Raylib.DrawText(w.RoleDescription, (int)wRec.X + 215, (int)wRec.Y + 40, 12, new Color(0, 220, 255, 200));

                Raylib.DrawText($"DANO: {w.BaseDamage}", (int)wRec.X + 570, (int)wRec.Y + 18, 13, Color.Gold);
                Raylib.DrawText($"CADENCIA: {w.FireRateRPM} RPM", (int)wRec.X + 570, (int)wRec.Y + 42, 12, Color.LightGray);

                Raylib.DrawText($"CARGADOR: {w.MagazineSize}", (int)wRec.X + 750, (int)wRec.Y + 18, 13, Color.White);
                Raylib.DrawText($"RECARGA: {w.ReloadTimeSeconds}s", (int)wRec.X + 750, (int)wRec.Y + 42, 12, Color.LightGray);
            }
        }

        private void DrawGalleryModal(int screenW, int screenH, List<MapScene> maps)
        {
            Rectangle modalRec = new Rectangle(90, 50, screenW - 180, screenH - 100);
            Raylib.DrawRectangleRec(modalRec, new Color(8, 12, 22, 252));
            Raylib.DrawRectangleLinesEx(modalRec, 2, new Color(0, 220, 255, 255));

            Raylib.DrawText("IMAGENES DE REFERENCIA DE MAPAS Y COMBATE (IMAGEN #2)", (int)modalRec.X + 25, (int)modalRec.Y + 18, 22, Color.White);

            Rectangle closeBtn = new Rectangle(modalRec.X + modalRec.Width - 42, modalRec.Y + 14, 28, 28);
            Raylib.DrawRectangleRec(closeBtn, Color.Red);
            Raylib.DrawText("X", (int)closeBtn.X + 9, (int)closeBtn.Y + 6, 16, Color.White);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeBtn) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                CurrentTab = LobbyTab.Main;
            }

            Texture2D[] mapTexs = new Texture2D[] { _texMap1, _texMap2, _texMap3, _texMap4 };
            string[] mapDescs = new string[]
            {
                "Ref 1: Combate en ciudad futurista con destruccion y rascacielos ciberneticos.",
                "Ref 2: Accion rapida y fluida en modo competitivo 5v5 con pasillos tacticos.",
                "Ref 3: Habilidades unicas por personaje (Cupula de barrera de Vulcan y pulsos).",
                "Ref 4: Mapas con verticalidad extrema, pasarelas y estructuras rompibles."
            };

            int colW = ((int)modalRec.Width - 60) / 2;
            int rowH = ((int)modalRec.Height - 100) / 2;

            for (int i = 0; i < 4; i++)
            {
                int col = i % 2;
                int row = i / 2;
                Rectangle r = new Rectangle(modalRec.X + 20 + col * (colW + 20), modalRec.Y + 70 + row * (rowH + 15), colW, rowH);

                Raylib.DrawRectangleRec(r, new Color(14, 20, 32, 230));
                Raylib.DrawRectangleLinesEx(r, 1, new Color(0, 200, 255, 120));

                if (i < mapTexs.Length && mapTexs[i].Id > 0)
                {
                    int imgH = (int)r.Height - 45;
                    Raylib.DrawTexturePro(
                        mapTexs[i],
                        new Rectangle(0, 0, mapTexs[i].Width, mapTexs[i].Height),
                        new Rectangle(r.X + 8, r.Y + 8, r.Width - 16, imgH),
                        Vector2.Zero,
                        0f,
                        Color.White
                    );
                }

                Raylib.DrawText(mapDescs[i], (int)r.X + 10, (int)(r.Y + r.Height - 28), 11, new Color(0, 235, 120, 255));
            }
        }

        private void DrawRankTreeModal(int screenW, int screenH)
        {
            Rectangle modalRec = new Rectangle(80, 45, screenW - 160, screenH - 90);
            Raylib.DrawRectangleRec(modalRec, new Color(6, 10, 18, 252));
            Raylib.DrawRectangleLinesEx(modalRec, 2, new Color(0, 235, 120, 255)); // Borde Esmeralda

            Raylib.DrawText("SISTEMA DE RANGOS COMPETITIVOS (IMAGEN #1)", (int)modalRec.X + 25, (int)modalRec.Y + 18, 22, Color.White);
            Raylib.DrawText("* NOTA DE DISENO: Rango LEYENDA con COLOR ESMERALDA radiante para contrastar con ORO.", (int)modalRec.X + 25, (int)modalRec.Y + 44, 13, new Color(0, 235, 120, 255));

            Rectangle closeBtn = new Rectangle(modalRec.X + modalRec.Width - 42, modalRec.Y + 14, 28, 28);
            Raylib.DrawRectangleRec(closeBtn, Color.Red);
            Raylib.DrawText("X", (int)closeBtn.X + 9, (int)closeBtn.Y + 6, 16, Color.White);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeBtn) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                CurrentTab = LobbyTab.Main;
            }

            int rankCount = RankData.AllRanks.Length;
            int cardW = ((int)modalRec.Width - 40) / rankCount;

            for (int r = 0; r < rankCount; r++)
            {
                var rk = RankData.AllRanks[r];
                Rectangle rRec = new Rectangle(modalRec.X + 20 + r * cardW, modalRec.Y + 75, cardW - 8, modalRec.Height - 95);
                bool isSelected = SelectedRankIndex == r;
                bool hover = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rRec);

                Raylib.DrawRectangleRec(rRec, isSelected ? new Color(18, 32, 48, 240) : new Color(12, 16, 26, 210));
                Raylib.DrawRectangleLinesEx(rRec, isSelected ? 2.5f : 1f, rk.PrimaryColor);

                // Dibujar emblema
                RankRenderer.DrawRankBadge((int)(rRec.X + rRec.Width / 2), (int)(rRec.Y + 75), rk, 1.0f, isSelected);

                Raylib.DrawText(rk.Name, (int)(rRec.X + 12), (int)(rRec.Y + 150), 16, rk.PrimaryColor);
                Raylib.DrawText($"{rk.RequiredPoints} PTS", (int)(rRec.X + 12), (int)(rRec.Y + 175), 13, Color.Gold);

                Raylib.DrawText(rk.Description, (int)(rRec.X + 8), (int)(rRec.Y + 208), 10, new Color(180, 200, 220, 200));

                if (rk.Tier == GameRankTier.Leyenda)
                {
                    Raylib.DrawText("COLOR ESMERALDA", (int)(rRec.X + 8), (int)(rRec.Y + 270), 10, new Color(0, 235, 120, 255));
                }
                else if (rk.Tier == GameRankTier.Oro)
                {
                    Raylib.DrawText("COLOR DORADO", (int)(rRec.X + 8), (int)(rRec.Y + 270), 10, Color.Gold);
                }

                if (hover && Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    SoundSynthesizer.PlayClick();
                    SelectedRankIndex = r;
                }
            }
        }

        private void DrawSettingsModal(int screenW, int screenH)
        {
            Rectangle modalRec = new Rectangle(180, 90, screenW - 360, screenH - 180);
            Raylib.DrawRectangleRec(modalRec, new Color(8, 12, 22, 250));
            Raylib.DrawRectangleLinesEx(modalRec, 2, new Color(0, 220, 255, 255));

            Raylib.DrawText("AJUSTES DE JUEGO & RENDIMIENTO (C# 3D)", (int)modalRec.X + 25, (int)modalRec.Y + 20, 20, Color.White);

            Rectangle closeBtn = new Rectangle(modalRec.X + modalRec.Width - 42, modalRec.Y + 14, 28, 28);
            Raylib.DrawRectangleRec(closeBtn, Color.Red);
            Raylib.DrawText("X", (int)closeBtn.X + 9, (int)closeBtn.Y + 6, 16, Color.White);
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), closeBtn) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                CurrentTab = LobbyTab.Main;
            }

            int sy = (int)modalRec.Y + 70;
            Raylib.DrawText("- Calidad Grafica 3D: ALTA (Iluminacion dinamica + Sombras)", (int)modalRec.X + 25, sy, 14, Color.White);
            Raylib.DrawText("- Resolucion: Dinamica de Ventana con Aceleracion GPU", (int)modalRec.X + 25, sy + 30, 14, Color.White);
            Raylib.DrawText("- Sensibilidad del Raton: 0.0022 (Optimizada para FPS)", (int)modalRec.X + 25, sy + 60, 14, Color.White);
            Raylib.DrawText("- Audio: Sintetizador Tactico Procedural Activo (Sonido limpio)", (int)modalRec.X + 25, sy + 90, 14, new Color(0, 235, 120, 255));
            Raylib.DrawText("- Controles: WASD (Mover), Raton (Mirar), Clic Izq (Disparar), Clic Der (Apuntar)", (int)modalRec.X + 25, sy + 130, 13, new Color(0, 220, 255, 255));
            Raylib.DrawText("- Teclas: R (Recargar), E (Habilidad especial de personaje), 1-5 (Cambiar arma)", (int)modalRec.X + 25, sy + 155, 13, new Color(0, 220, 255, 255));
        }
    }
}
