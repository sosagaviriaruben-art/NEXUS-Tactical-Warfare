using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NexusTacticalWarfare.Game.Audio;
using NexusTacticalWarfare.Game.Entities;
using NexusTacticalWarfare.Game.UI;
using NexusTacticalWarfare.Game.Weapons;
using NexusTacticalWarfare.Game.World;

namespace NexusTacticalWarfare.Game
{
    public enum GameState
    {
        CoverScreen,
        Lobby,
        Playing,
        RoundFinished
    }

    public static class Program
    {
        private static int _screenWidth = 1280;
        private static int _screenHeight = 720;
        private static GameState _state = GameState.CoverScreen;

        private static List<CharacterDefinition> _characterCatalog = new();
        private static List<WeaponDefinition> _arsenal = new();
        private static List<MapScene> _mapScenes = new();

        private static int _selectedCharacterIndex = 0;
        private static CharacterDefinition CurrentCharacter => _characterCatalog[_selectedCharacterIndex];

        private static PlayerController? _player;
        private static List<AIBot> _bots = new();
        private static List<EnergyShieldEntity> _activeShields = new();
        private static List<DebrisChunk> _activeDebris = new();
        private static ParticleManager _particles = new();

        private static LobbyAndCoverUI _lobbyUI = new();
        private static GameHUD _hud = new();

        private static int _roundScoreAllies = 0;
        private static int _roundScoreEnemies = 0;
        private static float _roundTimeRemaining = 120.0f;
        private static bool _isPaused = false;
        private static string _roundEndMessage = "";
        private static float _roundEndTimer = 0f;

        private static Texture2D _texBotVulcan;
        private static Texture2D _texBotNyx;
        private static Texture2D _texBotRazor;
        private static Texture2D[] _texWeaponArsenal = new Texture2D[5];

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

        public static void Main(string[] args)
        {
            // Inicializar Raylib con aceleración por hardware y MSAA
            Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.ResizableWindow);
            Raylib.InitWindow(_screenWidth, _screenHeight, "NEXUS: Tactical Warfare (PENDIENTE TACTICAL SHOOTER) [3D C#]");
            Raylib.SetTargetFPS(60);

            // Inicializar audio procedural táctico
            SoundSynthesizer.Initialize();

            // Cargar datos
            _characterCatalog = CharacterDefinition.CreateCatalog();
            _arsenal = WeaponDefinition.CreateArsenal();
            _mapScenes = MapBuilder.BuildAllMaps();

            _lobbyUI.LoadAssets();

            _texBotVulcan = LoadTextureSafe("char_vulcan.png");
            _texBotNyx = LoadTextureSafe("char_nyx.png");
            _texBotRazor = LoadTextureSafe("char_razor.png");

            _texWeaponArsenal[0] = LoadTextureSafe("weapon_ar.png");
            _texWeaponArsenal[1] = LoadTextureSafe("weapon_sniper.png");
            _texWeaponArsenal[2] = LoadTextureSafe("weapon_shotgun.png");
            _texWeaponArsenal[3] = LoadTextureSafe("weapon_smg.png");
            _texWeaponArsenal[4] = LoadTextureSafe("weapon_pistol.png");

            // Ciclo principal del juego
            while (!Raylib.WindowShouldClose())
            {
                _screenWidth = Raylib.GetScreenWidth();
                _screenHeight = Raylib.GetScreenHeight();
                float dt = Raylib.GetFrameTime();

                // Lógica según estado
                switch (_state)
                {
                    case GameState.CoverScreen:
                        UpdateCoverScreen();
                        break;
                    case GameState.Lobby:
                        UpdateLobby(dt);
                        break;
                    case GameState.Playing:
                        UpdateGameplay(dt);
                        break;
                    case GameState.RoundFinished:
                        UpdateRoundFinished(dt);
                        break;
                }

                // Renderizado
                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(6, 10, 18, 255));

                switch (_state)
                {
                    case GameState.CoverScreen:
                        _lobbyUI.DrawCoverScreen(_screenWidth, _screenHeight, () =>
                        {
                            _state = GameState.Lobby;
                        });
                        break;

                    case GameState.Lobby:
                        _lobbyUI.DrawLobby(
                            _screenWidth,
                            _screenHeight,
                            CurrentCharacter,
                            _characterCatalog,
                            _arsenal[0],
                            _arsenal,
                            _mapScenes,
                            StartMatch
                        );
                        break;

                    case GameState.Playing:
                    case GameState.RoundFinished:
                        Draw3DGameScene();
                        DrawGameHUD();
                        break;
                }

                Raylib.EndDrawing();
            }

            // Liberación de recursos al cerrar
            _lobbyUI.UnloadAssets();
            if (_texBotVulcan.Id > 0) Raylib.UnloadTexture(_texBotVulcan);
            if (_texBotNyx.Id > 0) Raylib.UnloadTexture(_texBotNyx);
            if (_texBotRazor.Id > 0) Raylib.UnloadTexture(_texBotRazor);
            foreach (var w in _texWeaponArsenal)
            {
                if (w.Id > 0) Raylib.UnloadTexture(w);
            }
            SoundSynthesizer.Shutdown();
            Raylib.CloseWindow();
        }

        private static void UpdateCoverScreen()
        {
            // Manejado en LobbyAndCoverUI
        }

        private static void UpdateLobby(float dt)
        {
            // Atajos de cambio de personaje rápido
            if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                _selectedCharacterIndex = (_selectedCharacterIndex - 1 + _characterCatalog.Count) % _characterCatalog.Count;
                SoundSynthesizer.PlayClick();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                _selectedCharacterIndex = (_selectedCharacterIndex + 1) % _characterCatalog.Count;
                SoundSynthesizer.PlayClick();
            }
        }

        private static void StartMatch()
        {
            var map = _mapScenes[_lobbyUI.SelectedMapIndex];

            // Instanciar jugador
            _player = new PlayerController(CurrentCharacter, _arsenal);
            _player.ResetPosition(map.PlayerSpawn);

            // Reconstruir mapa limpio
            _mapScenes = MapBuilder.BuildAllMaps();
            map = _mapScenes[_lobbyUI.SelectedMapIndex];

            // Instanciar bots para partida táctica 5v5
            _bots.Clear();
            _activeShields.Clear();
            _activeDebris.Clear();
            _particles.Clear();

            // 4 Aliados
            for (int i = 0; i < map.AllySpawns.Count; i++)
            {
                var ch = _characterCatalog[(i + 1) % _characterCatalog.Count];
                var wp = _arsenal[(i + 1) % _arsenal.Count];
                _bots.Add(new AIBot($"ALIADO_{i + 1}", BotTeam.Allied, ch, wp, map.AllySpawns[i]));
            }

            // 5 Enemigos
            for (int i = 0; i < map.EnemySpawns.Count; i++)
            {
                var ch = _characterCatalog[i % _characterCatalog.Count];
                var wp = _arsenal[i % _arsenal.Count];
                _bots.Add(new AIBot($"ENEMIGO_{i + 1}", BotTeam.Enemy, ch, wp, map.EnemySpawns[i]));
            }

            _roundScoreAllies = 0;
            _roundScoreEnemies = 0;
            _roundTimeRemaining = 120f;
            _isPaused = false;
            _state = GameState.Playing;

            Raylib.DisableCursor(); // Bloquear cursor para control FPS en 3D
        }

        private static void UpdateGameplay(float dt)
        {
            if (_player == null) return;

            // Pausa con ESC
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                _isPaused = !_isPaused;
                if (_isPaused) Raylib.EnableCursor();
                else Raylib.DisableCursor();
            }

            if (_isPaused) return;

            var map = _mapScenes[_lobbyUI.SelectedMapIndex];

            // Tiempo de ronda
            _roundTimeRemaining -= dt;
            if (_roundTimeRemaining <= 0)
            {
                EndRound(false, "¡TIEMPO AGOTADO! LOS DEFENSORES GANAN LA RONDA.");
                return;
            }

            // Actualizar jugador
            _player.Update(dt, map, _bots, _particles, _activeShields, (k, v, w) =>
            {
                _hud.AddKillFeed(k, v, w);
            });

            // Actualizar bots
            foreach (var bot in _bots)
            {
                bot.Update(
                    dt,
                    _player.Position,
                    !_player.IsDead,
                    _bots,
                    map,
                    _particles,
                    (k, v, w) => _hud.AddKillFeed(k, v, w),
                    (dmg) => _player.TakeDamage(dmg)
                );
            }

            // Actualizar escudos
            for (int i = _activeShields.Count - 1; i >= 0; i--)
            {
                _activeShields[i].Update(dt);
                if (!_activeShields[i].IsActive) _activeShields.RemoveAt(i);
            }

            // Actualizar partículas
            _particles.Update(dt);

            // Actualizar fragmentos de destrucción
            for (int i = _activeDebris.Count - 1; i >= 0; i--)
            {
                _activeDebris[i].Update(dt);
                if (_activeDebris[i].IsDead) _activeDebris.RemoveAt(i);
            }

            // Actualizar HUD
            _hud.Update(dt);

            // Verificar condiciones de victoria de ronda (5v5)
            int alliesAlive = (_player.IsDead ? 0 : 1);
            int enemiesAlive = 0;

            foreach (var b in _bots)
            {
                if (b.IsDead) continue;
                if (b.Team == BotTeam.Allied) alliesAlive++;
                else enemiesAlive++;
            }

            if (enemiesAlive == 0)
            {
                _roundScoreAllies++;
                EndRound(true, "¡VICTORIA DE RONDA! TODOS LOS ENEMIGOS FUERON ELIMINADOS.");
            }
            else if (alliesAlive == 0)
            {
                _roundScoreEnemies++;
                EndRound(false, "¡DERROTA DE RONDA! EL EQUIPO FUE ELIMINADO.");
            }
        }

        private static void EndRound(bool victory, string message)
        {
            _roundEndMessage = message;
            _roundEndTimer = 4.0f;
            _state = GameState.RoundFinished;
            Raylib.EnableCursor();

            if (victory) SoundSynthesizer.PlayHeadshot();
            else SoundSynthesizer.PlayExplosion();
        }

        private static void UpdateRoundFinished(float dt)
        {
            _roundEndTimer -= dt;
            _particles.Update(dt);

            if (_roundEndTimer <= 0f)
            {
                // Siguiente ronda o volver a jugar
                if (_roundScoreAllies >= 5 || _roundScoreEnemies >= 5)
                {
                    _state = GameState.Lobby;
                }
                else
                {
                    // Reiniciar ronda 5v5
                    var map = _mapScenes[_lobbyUI.SelectedMapIndex];
                    _player?.ResetPosition(map.PlayerSpawn);

                    for (int i = 0; i < _bots.Count; i++)
                    {
                        var b = _bots[i];
                        Vector3 spawn = b.Team == BotTeam.Allied ? map.AllySpawns[i % map.AllySpawns.Count] : map.EnemySpawns[i % map.EnemySpawns.Count];
                        b.Position = spawn;
                        b.CurrentHealth = b.MaxHealth;
                        b.CurrentShield = b.MaxShield;
                    }

                    _roundTimeRemaining = 120f;
                    _state = GameState.Playing;
                    Raylib.DisableCursor();
                }
            }
        }

        private static void Draw3DGameScene()
        {
            if (_player == null) return;

            var map = _mapScenes[_lobbyUI.SelectedMapIndex];

            Raylib.BeginMode3D(_player.Camera);

            // Piso y cuadrícula táctica
            Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(90, 90), map.FloorColor);
            Raylib.DrawGrid(45, 2.0f);

            // Estructuras estáticas del mapa 3D (Inspiradas en Referencias de Imagen #2)
            foreach (var str in map.Structures)
            {
                Raylib.DrawCube(str.Position, str.Size.X, str.Size.Y, str.Size.Z, str.MainColor);
                Raylib.DrawCubeWires(str.Position, str.Size.X, str.Size.Y, str.Size.Z, str.TrimColor);
            }

            // Elementos destructibles (Referencia 1 y 4 de Imagen #2)
            foreach (var dest in map.Destructibles)
            {
                if (dest.IsDestroyed) continue;

                // Color según estado de salud
                float hpRatio = dest.CurrentHealth / dest.MaxHealth;
                Color col = new Color(
                    (int)(dest.BaseColor.R * hpRatio),
                    (int)(dest.BaseColor.G * hpRatio),
                    (int)(dest.BaseColor.B * hpRatio),
                    255
                );

                Raylib.DrawCube(dest.Position, dest.Size.X, dest.Size.Y, dest.Size.Z, col);
                Raylib.DrawCubeWires(dest.Position, dest.Size.X, dest.Size.Y, dest.Size.Z, dest.AccentColor);
            }

            // Fragmentos de escombros físicos de destrucción
            foreach (var chunk in _activeDebris)
            {
                Raylib.DrawCube(chunk.Position, chunk.Size.X, chunk.Size.Y, chunk.Size.Z, chunk.Color);
            }

            // Escudos de energía tácticos (Habilidad de Vulcan / Imagen #3)
            foreach (var sh in _activeShields)
            {
                sh.Draw();
            }

            // Bots 3D Fotorrealistas (Aliados en Azul / Enemigos en Rojo)
            foreach (var bot in _bots)
            {
                bot.Draw3D(_player.Camera, _texBotVulcan, _texBotNyx, _texBotRazor);
            }

            // Trazadoras y partículas 3D
            _particles.Draw();

            Raylib.EndMode3D();
        }

        private static void DrawGameHUD()
        {
            if (_player == null) return;
            var map = _mapScenes[_lobbyUI.SelectedMapIndex];

            int currentWpIdx = Math.Clamp(_player.CurrentWeaponIndex, 0, _texWeaponArsenal.Length - 1);
            Texture2D wpTex = _texWeaponArsenal[currentWpIdx];

            _hud.DrawHUD(
                _screenWidth,
                _screenHeight,
                _player,
                _bots,
                map,
                _roundScoreAllies,
                _roundScoreEnemies,
                _roundTimeRemaining,
                _isPaused,
                wpTex,
                () =>
                {
                    _isPaused = false;
                    Raylib.DisableCursor();
                },
                () =>
                {
                    _isPaused = false;
                    _state = GameState.Lobby;
                    Raylib.EnableCursor();
                }
            );

            // Mensaje de fin de ronda
            if (_state == GameState.RoundFinished)
            {
                int mw = Raylib.MeasureText(_roundEndMessage, 24);
                Raylib.DrawRectangle(_screenWidth / 2 - mw / 2 - 20, _screenHeight / 2 - 35, mw + 40, 70, new Color(10, 15, 25, 220));
                Raylib.DrawRectangleLines(_screenWidth / 2 - mw / 2 - 20, _screenHeight / 2 - 35, mw + 40, 70, Color.Gold);
                Raylib.DrawText(_roundEndMessage, _screenWidth / 2 - mw / 2, _screenHeight / 2 - 12, 24, Color.White);
            }
        }
    }
}
