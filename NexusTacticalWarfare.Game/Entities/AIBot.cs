using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NexusTacticalWarfare.Game.Weapons;
using NexusTacticalWarfare.Game.World;
using NexusTacticalWarfare.Game.Audio;

namespace NexusTacticalWarfare.Game.Entities
{
    public enum BotTeam
    {
        Allied, // Azul
        Enemy   // Rojo
    }

    public class AIBot
    {
        public string Name { get; set; } = "Bot";
        public BotTeam Team { get; set; }
        public CharacterDefinition Character { get; set; }
        public WeaponDefinition WeaponDef { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; } = 100f;
        public float MaxShield { get; set; } = 100f;
        public float CurrentShield { get; set; } = 100f;
        public bool IsDead => CurrentHealth <= 0f;

        public float ShootTimer { get; set; } = 0f;
        public float HitFlashTimer { get; set; } = 0f;
        public Vector3 TargetPosition { get; set; }
        public float MoveTimer { get; set; } = 0f;

        public BoundingBox BoundingBox => new BoundingBox(
            Position + new Vector3(-0.4f, 0f, -0.4f),
            Position + new Vector3(0.4f, 1.8f, 0.4f)
        );

        public Vector3 EyePosition => Position + new Vector3(0, 1.6f, 0);

        public AIBot(string name, BotTeam team, CharacterDefinition character, WeaponDefinition weapon, Vector3 spawnPos)
        {
            Name = name;
            Team = team;
            Character = character;
            WeaponDef = weapon;
            Position = spawnPos;
            TargetPosition = spawnPos;
            MaxHealth = character.BaseHealth;
            CurrentHealth = character.BaseHealth;
            MaxShield = character.BaseShield;
            CurrentShield = character.BaseShield;
        }

        public void Update(
            float dt,
            Vector3 playerPos,
            bool playerAlive,
            List<AIBot> allBots,
            MapScene map,
            ParticleManager particles,
            Action<string, string, string> onKillFeed,
            Action<float> onDamagePlayer)
        {
            if (IsDead) return;

            if (HitFlashTimer > 0f) HitFlashTimer -= dt;
            if (ShootTimer > 0f) ShootTimer -= dt;
            MoveTimer -= dt;

            // IA Táctica de patrulla y combate
            Vector3? targetEnemy = null;
            float closestDist = float.MaxValue;

            // Si es enemigo, puede apuntar al jugador
            if (Team == BotTeam.Enemy && playerAlive)
            {
                float d = Vector3.Distance(Position, playerPos);
                if (d < 50f && HasLineOfSight(EyePosition, playerPos + new Vector3(0, 1.5f, 0), map))
                {
                    targetEnemy = playerPos + new Vector3(0, 1.5f, 0);
                    closestDist = d;
                }
            }

            // O a los bots del equipo contrario
            foreach (var other in allBots)
            {
                if (other == this || other.IsDead || other.Team == this.Team) continue;

                float d = Vector3.Distance(Position, other.Position);
                if (d < closestDist && HasLineOfSight(EyePosition, other.EyePosition, map))
                {
                    targetEnemy = other.EyePosition;
                    closestDist = d;
                }
            }

            // Movimiento hacia objetivo o cobertura
            if (targetEnemy.HasValue)
            {
                // Mirar al enemigo
                Vector3 toEnemy = targetEnemy.Value - EyePosition;
                Yaw = MathF.Atan2(toEnemy.X, toEnemy.Z);

                // Disparo táctico con ráfagas
                if (ShootTimer <= 0f)
                {
                    ShootTimer = 60f / WeaponDef.FireRateRPM + (float)Random.Shared.NextDouble() * 0.15f;
                    FireAtTarget(targetEnemy.Value, playerPos, playerAlive, allBots, particles, onDamagePlayer, onKillFeed);
                }

                // Mantener distancia o buscar cobertura
                if (closestDist < 10f)
                {
                    // Retroceder
                    Vector3 moveDir = -Vector3.Normalize(new Vector3(toEnemy.X, 0, toEnemy.Z));
                    Position += moveDir * 2.8f * dt;
                }
                else if (closestDist > 25f)
                {
                    // Acercarse
                    Vector3 moveDir = Vector3.Normalize(new Vector3(toEnemy.X, 0, toEnemy.Z));
                    Position += moveDir * 3.5f * dt;
                }
            }
            else
            {
                // Patrulla hacia el punto de objetivo del mapa
                if (MoveTimer <= 0f)
                {
                    MoveTimer = 2.0f + (float)Random.Shared.NextDouble() * 3.0f;
                    Vector3 offset = new Vector3(
                        (float)(Random.Shared.NextDouble() - 0.5) * 16f,
                        0,
                        (float)(Random.Shared.NextDouble() - 0.5) * 16f
                    );
                    TargetPosition = map.BombSite + offset;
                }

                Vector3 toTarget = TargetPosition - Position;
                toTarget.Y = 0;
                if (toTarget.Length() > 1.0f)
                {
                    Vector3 dir = Vector3.Normalize(toTarget);
                    Yaw = MathF.Atan2(dir.X, dir.Z);
                    Position += dir * 2.5f * dt;
                }
            }

            // Mantener en el suelo
            Position = new Vector3(Position.X, 0f, Position.Z);
        }

        private void FireAtTarget(
            Vector3 target,
            Vector3 playerPos,
            bool playerAlive,
            List<AIBot> allBots,
            ParticleManager particles,
            Action<float> onDamagePlayer,
            Action<string, string, string> onKillFeed)
        {
            // Pequeña imprecisión según distancia
            Vector3 aimDir = Vector3.Normalize(target - EyePosition);
            aimDir += new Vector3(
                (float)(Random.Shared.NextDouble() - 0.5) * 0.04f,
                (float)(Random.Shared.NextDouble() - 0.5) * 0.04f,
                (float)(Random.Shared.NextDouble() - 0.5) * 0.04f
            );
            aimDir = Vector3.Normalize(aimDir);

            Vector3 endTracer = EyePosition + aimDir * WeaponDef.Range;
            Color tracerCol = Team == BotTeam.Enemy ? new Color(255, 60, 60, 255) : new Color(0, 200, 255, 255);
            particles.SpawnBulletTracer(EyePosition, endTracer, tracerCol);

            // Audio distante
            if (WeaponDef.Type == WeaponType.AssaultRifle) SoundSynthesizer.PlayRifle();
            else if (WeaponDef.Type == WeaponType.SMG) SoundSynthesizer.PlaySmg();
            else if (WeaponDef.Type == WeaponType.Shotgun) SoundSynthesizer.PlayShotgun();
            else if (WeaponDef.Type == WeaponType.SniperRifle) SoundSynthesizer.PlaySniper();
            else SoundSynthesizer.PlayPistol();

            // Daño al jugador si es enemigo y le impacta
            if (Team == BotTeam.Enemy && playerAlive)
            {
                BoundingBox playerBox = new BoundingBox(
                    playerPos + new Vector3(-0.4f, 0, -0.4f),
                    playerPos + new Vector3(0.4f, 1.8f, 0.4f)
                );
                Ray ray = new Ray(EyePosition, aimDir);
                var col = Raylib.GetRayCollisionBox(ray, playerBox);
                if (col.Hit && col.Distance < WeaponDef.Range)
                {
                    float dmg = WeaponDef.BaseDamage * (0.8f + (float)Random.Shared.NextDouble() * 0.4f);
                    onDamagePlayer(dmg);
                    particles.SpawnHitSparks(col.Point, col.Normal, new Color(255, 100, 100, 255), 6);
                    return;
                }
            }

            // Daño a otros bots
            foreach (var bot in allBots)
            {
                if (bot == this || bot.IsDead || bot.Team == this.Team) continue;

                Ray ray = new Ray(EyePosition, aimDir);
                var col = Raylib.GetRayCollisionBox(ray, bot.BoundingBox);
                if (col.Hit && col.Distance < WeaponDef.Range)
                {
                    float dmg = WeaponDef.BaseDamage * (0.85f + (float)Random.Shared.NextDouble() * 0.3f);
                    bot.TakeDamage(dmg, this.Name, onKillFeed);
                    particles.SpawnHitSparks(col.Point, col.Normal, new Color(255, 220, 100, 255), 6);
                    break;
                }
            }
        }

        public void TakeDamage(float damage, string attackerName, Action<string, string, string> onKillFeed)
        {
            if (IsDead) return;

            HitFlashTimer = 0.15f;

            // Daño primero al escudo, luego a la vida
            if (CurrentShield > 0)
            {
                if (CurrentShield >= damage)
                {
                    CurrentShield -= damage;
                    damage = 0;
                }
                else
                {
                    damage -= CurrentShield;
                    CurrentShield = 0;
                }
            }

            if (damage > 0)
            {
                CurrentHealth -= damage;
                if (CurrentHealth <= 0)
                {
                    CurrentHealth = 0;
                    onKillFeed(attackerName, this.Name, WeaponDef.Name);
                }
            }
        }

        private bool HasLineOfSight(Vector3 from, Vector3 to, MapScene map)
        {
            Ray ray = new Ray(from, Vector3.Normalize(to - from));
            float maxDist = Vector3.Distance(from, to);

            foreach (var str in map.Structures)
            {
                var col = Raylib.GetRayCollisionBox(ray, str.GetBoundingBox());
                if (col.Hit && col.Distance < maxDist - 0.5f) return false;
            }

            foreach (var d in map.Destructibles)
            {
                if (d.IsDestroyed) continue;
                var col = Raylib.GetRayCollisionBox(ray, d.GetBoundingBox());
                if (col.Hit && col.Distance < maxDist - 0.5f) return false;
            }

            return true;
        }

        public void Draw3D(Camera3D camera, Texture2D texVulcan, Texture2D texNyx, Texture2D texRazor)
        {
            if (IsDead) return;

            Texture2D tex = texVulcan;
            if (Character.Id == CharacterId.Nyx) tex = texNyx;
            else if (Character.Id == CharacterId.Razor) tex = texRazor;

            Color tint = HitFlashTimer > 0f ? Color.White : (Team == BotTeam.Enemy ? new Color(255, 160, 160, 255) : new Color(180, 230, 255, 255));

            if (tex.Id > 0)
            {
                // Renderizado 3D fotorrealista con Billboard
                Raylib.DrawBillboard(camera, tex, Position + new Vector3(0, 1.1f, 0), 2.2f, tint);
            }
            else
            {
                // Fallback
                var skin = Character.CurrentSkin;
                Color bodyColor = HitFlashTimer > 0f ? Color.White : (Team == BotTeam.Enemy ? new Color(180, 45, 45, 255) : skin.ArmorColorPrimary);
                Raylib.DrawCube(Position + new Vector3(0, 1.15f, 0), 0.55f, 1.5f, 0.35f, bodyColor);
            }

            // Barra de vida y nombre sobre la cabeza en 3D
            Vector3 barPos = Position + new Vector3(0, 2.25f, 0);
            Color teamTagCol = Team == BotTeam.Enemy ? Color.Red : new Color(0, 200, 255, 255);
            Raylib.DrawCube(barPos, 0.8f * (CurrentHealth / MaxHealth), 0.08f, 0.08f, teamTagCol);
            Raylib.DrawCubeWires(barPos, 0.82f, 0.09f, 0.09f, Color.White);
        }
    }
}
