using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using NexusTacticalWarfare.Game.Weapons;
using NexusTacticalWarfare.Game.World;
using NexusTacticalWarfare.Game.Audio;

namespace NexusTacticalWarfare.Game.Entities
{
    public class PlayerController
    {
        public Camera3D Camera;
        public Vector3 Position;
        public Vector3 Velocity;
        public float Pitch = 0f;
        public float Yaw = 0f;

        public float MouseSensitivity = 0.0022f;
        public bool InvertY = false;

        public CharacterDefinition Character { get; set; }
        public List<ActiveWeaponInstance> Weapons { get; } = new();
        public int CurrentWeaponIndex = 0;
        public ActiveWeaponInstance CurrentWeapon => Weapons[CurrentWeaponIndex];

        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public float MaxShield = 100f;
        public float CurrentShield = 100f;
        public bool IsDead => CurrentHealth <= 0f;

        public float AbilityCooldownTimer = 0f;
        public bool AbilityReady => AbilityCooldownTimer <= 0f;

        public bool IsAiming = false;
        public bool IsSprinting = false;
        public bool IsCrouching = false;
        public bool IsGrounded = true;

        public float HitmarkerTimer = 0f;
        public bool WasHeadshot = false;

        public float FootstepTimer = 0f;

        public PlayerController(CharacterDefinition character, List<WeaponDefinition> arsenal)
        {
            Character = character;
            MaxHealth = character.BaseHealth;
            CurrentHealth = character.BaseHealth;
            MaxShield = character.BaseShield;
            CurrentShield = character.BaseShield;

            foreach (var w in arsenal)
            {
                Weapons.Add(new ActiveWeaponInstance(w));
            }

            Position = new Vector3(0, 1.8f, 25f);
            Camera = new Camera3D
            {
                Position = Position,
                Target = Position + new Vector3(0, 0, -1),
                Up = new Vector3(0, 1, 0),
                FovY = 70.0f,
                Projection = CameraProjection.Perspective
            };
        }

        public void ResetPosition(Vector3 spawn)
        {
            Position = spawn;
            Velocity = Vector3.Zero;
            CurrentHealth = MaxHealth;
            CurrentShield = MaxShield;
            AbilityCooldownTimer = 0f;
        }

        public void Update(
            float dt,
            MapScene map,
            List<AIBot> bots,
            ParticleManager particles,
            List<EnergyShieldEntity> shields,
            Action<string, string, string> onKillFeed)
        {
            if (IsDead) return;

            // Enfriamientos
            if (AbilityCooldownTimer > 0f) AbilityCooldownTimer -= dt;
            if (HitmarkerTimer > 0f) HitmarkerTimer -= dt;

            // Actualizar armas
            foreach (var w in Weapons) w.Update(dt);

            // 1. Control del Mouse (Cámara)
            Vector2 mouseDelta = Raylib.GetMouseDelta();
            Yaw -= mouseDelta.X * MouseSensitivity;
            float pitchDir = InvertY ? 1f : -1f;
            Pitch += mouseDelta.Y * MouseSensitivity * pitchDir;
            Pitch = Math.Clamp(Pitch, -1.45f, 1.45f);

            // Vector de visión
            Vector3 forward = new Vector3(
                MathF.Sin(Yaw) * MathF.Cos(Pitch),
                MathF.Sin(Pitch),
                MathF.Cos(Yaw) * MathF.Cos(Pitch)
            );
            forward = Vector3.Normalize(forward);

            Vector3 flatForward = Vector3.Normalize(new Vector3(forward.X, 0, forward.Z));
            Vector3 flatRight = Vector3.Normalize(Vector3.Cross(flatForward, new Vector3(0, 1, 0)));

            // 2. Apuntar (ADS)
            IsAiming = Raylib.IsMouseButtonDown(MouseButton.Right);
            float targetFov = IsAiming ? CurrentWeapon.Definition.ScopeZoomFov : 70.0f;
            Camera.FovY += (targetFov - Camera.FovY) * dt * 12.0f;

            // 3. Movimiento (WASD)
            Vector3 moveInput = Vector3.Zero;
            if (Raylib.IsKeyDown(KeyboardKey.W)) moveInput += flatForward;
            if (Raylib.IsKeyDown(KeyboardKey.S)) moveInput -= flatForward;
            if (Raylib.IsKeyDown(KeyboardKey.D)) moveInput -= flatRight;
            if (Raylib.IsKeyDown(KeyboardKey.A)) moveInput += flatRight;

            IsSprinting = Raylib.IsKeyDown(KeyboardKey.LeftShift) && !IsAiming && moveInput.LengthSquared() > 0;
            IsCrouching = Raylib.IsKeyDown(KeyboardKey.C);

            float speed = 7.0f * Character.SpeedMultiplier;
            if (IsSprinting) speed *= 1.45f;
            if (IsCrouching) speed *= 0.55f;
            if (IsAiming) speed *= 0.70f;

            if (moveInput.LengthSquared() > 0)
            {
                moveInput = Vector3.Normalize(moveInput);
                Velocity.X = moveInput.X * speed;
                Velocity.Z = moveInput.Z * speed;

                FootstepTimer += dt * (IsSprinting ? 1.6f : 1.0f);
                if (FootstepTimer > 0.45f)
                {
                    FootstepTimer = 0f;
                    // Paso táctico sutil
                }
            }
            else
            {
                Velocity.X *= 0.85f;
                Velocity.Z *= 0.85f;
            }

            // Salto y Gravedad
            if (IsGrounded && Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                Velocity.Y = 6.2f;
                IsGrounded = false;
            }

            if (!IsGrounded)
            {
                Velocity.Y -= 16.0f * dt;
            }

            // Aplicar velocidad con colisiones básicas de escenario
            Vector3 nextPos = Position + Velocity * dt;
            float eyeHeight = IsCrouching ? 1.1f : 1.8f;

            if (nextPos.Y < eyeHeight)
            {
                nextPos.Y = eyeHeight;
                Velocity.Y = 0f;
                IsGrounded = true;
            }

            // Colisión con límites del mapa
            nextPos.X = Math.Clamp(nextPos.X, -38f, 38f);
            nextPos.Z = Math.Clamp(nextPos.Z, -42f, 42f);

            Position = nextPos;
            Camera.Position = Position;
            Camera.Target = Position + forward;

            // 4. Cambio de arma (Teclas 1 a 5 o rueda)
            if (Raylib.IsKeyPressed(KeyboardKey.One)) SwitchWeapon(0);
            if (Raylib.IsKeyPressed(KeyboardKey.Two)) SwitchWeapon(1);
            if (Raylib.IsKeyPressed(KeyboardKey.Three)) SwitchWeapon(2);
            if (Raylib.IsKeyPressed(KeyboardKey.Four)) SwitchWeapon(3);
            if (Raylib.IsKeyPressed(KeyboardKey.Five)) SwitchWeapon(4);

            float wheel = Raylib.GetMouseWheelMove();
            if (wheel > 0) SwitchWeapon((CurrentWeaponIndex - 1 + Weapons.Count) % Weapons.Count);
            else if (wheel < 0) SwitchWeapon((CurrentWeaponIndex + 1) % Weapons.Count);

            // 5. Recarga (R)
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                if (!CurrentWeapon.IsReloading && CurrentWeapon.CurrentAmmo < CurrentWeapon.Definition.MagazineSize)
                {
                    CurrentWeapon.StartReload();
                    SoundSynthesizer.PlayReload();
                }
            }

            // 6. Disparo (Click Izquierdo)
            bool fireTriggered = CurrentWeapon.Definition.IsAutomatic ?
                Raylib.IsMouseButtonDown(MouseButton.Left) :
                Raylib.IsMouseButtonPressed(MouseButton.Left);

            if (fireTriggered && CurrentWeapon.CanShoot())
            {
                ShootWeapon(forward, map, bots, particles, shields, onKillFeed);
            }

            // 7. Habilidad de Personaje (E)
            if (Raylib.IsKeyPressed(KeyboardKey.E) && AbilityReady)
            {
                ActivateAbility(flatForward, particles, shields);
            }
        }

        private void SwitchWeapon(int index)
        {
            if (index >= 0 && index < Weapons.Count && index != CurrentWeaponIndex)
            {
                CurrentWeaponIndex = index;
                SoundSynthesizer.PlayClick();
            }
        }

        private void ShootWeapon(
            Vector3 forward,
            MapScene map,
            List<AIBot> bots,
            ParticleManager particles,
            List<EnergyShieldEntity> shields,
            Action<string, string, string> onKillFeed)
        {
            var def = CurrentWeapon.Definition;
            CurrentWeapon.CurrentAmmo--;
            CurrentWeapon.FireTimer = 60f / def.FireRateRPM;
            CurrentWeapon.RecoilOffset = def.RecoilKick;

            // Sonido de disparo
            if (def.Type == WeaponType.AssaultRifle) SoundSynthesizer.PlayRifle();
            else if (def.Type == WeaponType.SniperRifle) SoundSynthesizer.PlaySniper();
            else if (def.Type == WeaponType.Shotgun) SoundSynthesizer.PlayShotgun();
            else if (def.Type == WeaponType.SMG) SoundSynthesizer.PlaySmg();
            else SoundSynthesizer.PlayPistol();

            // Retroceso en la mira
            Pitch += def.RecoilKick * (IsAiming ? 0.35f : 0.75f);

            // Disparar proyectil o perdigones
            int pellets = def.PelletsPerShot;
            for (int p = 0; p < pellets; p++)
            {
                Vector3 shootDir = forward;
                if (p > 0 || !IsAiming)
                {
                    float spread = def.SpreadAngle * (IsAiming ? 0.3f : 1.0f);
                    shootDir += new Vector3(
                        (float)(Random.Shared.NextDouble() - 0.5) * spread,
                        (float)(Random.Shared.NextDouble() - 0.5) * spread,
                        (float)(Random.Shared.NextDouble() - 0.5) * spread
                    );
                    shootDir = Vector3.Normalize(shootDir);
                }

                Ray ray = new Ray(Camera.Position, shootDir);
                float closestHitDist = def.Range;
                Vector3 hitPoint = Camera.Position + shootDir * def.Range;
                Vector3 hitNormal = -shootDir;
                bool hitSomething = false;

                // Impacto en escudos de energía
                foreach (var sh in shields)
                {
                    if (!sh.IsActive) continue;
                    if (sh.CheckHit(ray, out RayCollision sc) && sc.Distance < closestHitDist)
                    {
                        closestHitDist = sc.Distance;
                        hitPoint = sc.Point;
                        hitNormal = sc.Normal;
                        sh.TakeDamage(def.BaseDamage);
                        hitSomething = true;
                        particles.SpawnHitSparks(hitPoint, hitNormal, new Color(0, 240, 255, 255), 8);
                    }
                }

                // Impacto en coberturas destructibles
                foreach (var dest in map.Destructibles)
                {
                    if (dest.IsDestroyed) continue;
                    var col = Raylib.GetRayCollisionBox(ray, dest.GetBoundingBox());
                    if (col.Hit && col.Distance < closestHitDist)
                    {
                        closestHitDist = col.Distance;
                        hitPoint = col.Point;
                        hitNormal = col.Normal;
                        hitSomething = true;

                        if (dest.TakeDamage(def.BaseDamage, out var debris))
                        {
                            SoundSynthesizer.PlayExplosion();
                        }
                        particles.SpawnHitSparks(hitPoint, hitNormal, dest.AccentColor, 6);
                    }
                }

                // Impacto en estructuras sólidas
                foreach (var str in map.Structures)
                {
                    var col = Raylib.GetRayCollisionBox(ray, str.GetBoundingBox());
                    if (col.Hit && col.Distance < closestHitDist)
                    {
                        closestHitDist = col.Distance;
                        hitPoint = col.Point;
                        hitNormal = col.Normal;
                        hitSomething = true;
                        particles.SpawnHitSparks(hitPoint, hitNormal, str.TrimColor, 4);
                    }
                }

                // Impacto en enemigos (Bots)
                foreach (var bot in bots)
                {
                    if (bot.IsDead || bot.Team == BotTeam.Allied) continue;

                    var col = Raylib.GetRayCollisionBox(ray, bot.BoundingBox);
                    if (col.Hit && col.Distance < closestHitDist)
                    {
                        closestHitDist = col.Distance;
                        hitPoint = col.Point;
                        hitNormal = col.Normal;
                        hitSomething = true;

                        // Verificar Headshot (impacto en la zona superior de la cabeza)
                        bool isHead = (hitPoint.Y - bot.Position.Y) > 1.45f;
                        float dmg = def.BaseDamage * (isHead ? def.HeadshotMultiplier : 1.0f);

                        bot.TakeDamage(dmg, Character.Name, onKillFeed);

                        HitmarkerTimer = 0.15f;
                        WasHeadshot = isHead;

                        if (isHead) SoundSynthesizer.PlayHeadshot();
                        else SoundSynthesizer.PlayHit();

                        particles.SpawnHitSparks(hitPoint, hitNormal, isHead ? Color.Red : Color.Gold, 8);
                        break;
                    }
                }

                if (!hitSomething)
                {
                    hitPoint = Camera.Position + shootDir * def.Range;
                }

                // Trazadora de bala
                Color tracerColor = def.LightGlowColor;
                particles.SpawnBulletTracer(Camera.Position + shootDir * 0.4f, hitPoint, tracerColor);
            }
        }

        private void ActivateAbility(Vector3 flatForward, ParticleManager particles, List<EnergyShieldEntity> shields)
        {
            AbilityCooldownTimer = Character.AbilityCooldown;

            if (Character.Id == CharacterId.Vulcan)
            {
                // VULCAN: Despliega Barrera Energética Translúcida 3D
                Vector3 shieldPos = Position + flatForward * 2.8f + new Vector3(0, 1.0f, 0);
                shields.Add(new EnergyShieldEntity
                {
                    Position = shieldPos,
                    Forward = flatForward,
                    Duration = 8.0f,
                    CurrentHealth = 300f
                });
                SoundSynthesizer.PlayShield();
                particles.SpawnEnergyPulse(shieldPos, new Color(0, 240, 255, 255), 30);
            }
            else if (Character.Id == CharacterId.Nyx)
            {
                // NYX: Pulso de Interferencia (EMP)
                SoundSynthesizer.PlayShield();
                particles.SpawnEnergyPulse(Position, new Color(200, 50, 255, 255), 45);
                // Aumento de velocidad de fase
                CurrentShield = Math.Min(MaxShield, CurrentShield + 30f);
            }
            else if (Character.Id == CharacterId.Razor)
            {
                // RAZOR: Sensor Táctico & Fortificación (+50 escudo)
                SoundSynthesizer.PlayShield();
                CurrentShield = Math.Min(MaxShield + 50f, CurrentShield + 50f);
                particles.SpawnEnergyPulse(Position, new Color(255, 180, 0, 255), 35);
            }
        }

        public void TakeDamage(float dmg)
        {
            if (IsDead) return;

            if (CurrentShield > 0f)
            {
                if (CurrentShield >= dmg)
                {
                    CurrentShield -= dmg;
                    dmg = 0;
                }
                else
                {
                    dmg -= CurrentShield;
                    CurrentShield = 0;
                }
            }

            if (dmg > 0)
            {
                CurrentHealth -= dmg;
                if (CurrentHealth < 0) CurrentHealth = 0;
            }
        }

        public void DrawFirstPersonWeapon(float recoilOffset)
        {
            // El renderizado de alta definicion ahora se realiza mediante el overlay fotorrealista de Imagen #2 en GameHUD
        }
    }
}
