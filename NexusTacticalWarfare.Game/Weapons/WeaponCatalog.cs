using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace NexusTacticalWarfare.Game.Weapons
{
    public enum WeaponType
    {
        AssaultRifle,
        SniperRifle,
        Shotgun,
        SMG,
        Pistol
    }

    public class WeaponDefinition
    {
        public WeaponType Type { get; set; }
        public string Name { get; set; } = "";
        public string RoleDescription { get; set; } = "";
        public int MagazineSize { get; set; }
        public int MaxReserveAmmo { get; set; }
        public float FireRateRPM { get; set; }
        public float BaseDamage { get; set; }
        public float HeadshotMultiplier { get; set; } = 2.0f;
        public float Range { get; set; } = 100f;
        public float SpreadAngle { get; set; } = 0.02f;
        public float RecoilKick { get; set; } = 0.03f;
        public float ReloadTimeSeconds { get; set; } = 2.2f;
        public int PelletsPerShot { get; set; } = 1;
        public bool IsAutomatic { get; set; } = true;
        public bool HasScopeZoom { get; set; } = false;
        public float ScopeZoomFov { get; set; } = 30f;

        // Estética 3D (Colores de Imagen #2)
        public Color ChassisColor { get; set; }
        public Color AccentColor { get; set; }
        public Color LightGlowColor { get; set; }
        public Vector3 ModelScale { get; set; }

        public static List<WeaponDefinition> CreateArsenal()
        {
            return new List<WeaponDefinition>
            {
                // 1. RIFLE DE ASALTO (Imagen 2: Versátil y equilibrado)
                new WeaponDefinition
                {
                    Type = WeaponType.AssaultRifle,
                    Name = "NEXUS-AR VALIANT",
                    RoleDescription = "Versátil y equilibrado. Ideal para medias y largas distancias.",
                    MagazineSize = 30,
                    MaxReserveAmmo = 120,
                    FireRateRPM = 620f,
                    BaseDamage = 28f,
                    HeadshotMultiplier = 2.2f,
                    Range = 120f,
                    SpreadAngle = 0.015f,
                    RecoilKick = 0.04f,
                    ReloadTimeSeconds = 2.2f,
                    PelletsPerShot = 1,
                    IsAutomatic = true,
                    HasScopeZoom = true,
                    ScopeZoomFov = 45f,
                    ChassisColor = new Color(28, 32, 40, 255),
                    AccentColor = new Color(180, 140, 50, 255), // Dorado/bronce táctico como en Imagen 2
                    LightGlowColor = new Color(0, 220, 255, 255), // LED cian
                    ModelScale = new Vector3(0.08f, 0.12f, 0.60f)
                },

                // 2. FRANCOTIRADOR (Imagen 2: Alta precisión y daño)
                new WeaponDefinition
                {
                    Type = WeaponType.SniperRifle,
                    Name = "VORTEX RAIL-SNIPER",
                    RoleDescription = "Alta precisión y daño extremo. Cañón de aceleración magnética.",
                    MagazineSize = 5,
                    MaxReserveAmmo = 25,
                    FireRateRPM = 52f,
                    BaseDamage = 115f,
                    HeadshotMultiplier = 2.5f,
                    Range = 250f,
                    SpreadAngle = 0.001f,
                    RecoilKick = 0.14f,
                    ReloadTimeSeconds = 3.2f,
                    PelletsPerShot = 1,
                    IsAutomatic = false,
                    HasScopeZoom = true,
                    ScopeZoomFov = 20f, // Gran aumento óptico
                    ChassisColor = new Color(20, 22, 28, 255),
                    AccentColor = new Color(140, 40, 180, 255), // Púrpura cósmico como en Imagen 2
                    LightGlowColor = new Color(230, 80, 255, 255),
                    ModelScale = new Vector3(0.07f, 0.13f, 0.85f)
                },

                // 3. ESCOPETA (Imagen 2: Ideal para corta distancia)
                new WeaponDefinition
                {
                    Type = WeaponType.Shotgun,
                    Name = "HAVOC SCATTERGUN",
                    RoleDescription = "Especialista de corta distancia. Dispersión destructiva masiva.",
                    MagazineSize = 8,
                    MaxReserveAmmo = 32,
                    FireRateRPM = 85f,
                    BaseDamage = 18f, // 18 x 8 pellets = 144 dmg a quemarropa
                    HeadshotMultiplier = 1.6f,
                    Range = 40f,
                    SpreadAngle = 0.075f,
                    RecoilKick = 0.12f,
                    ReloadTimeSeconds = 2.8f,
                    PelletsPerShot = 8,
                    IsAutomatic = false,
                    HasScopeZoom = false,
                    ScopeZoomFov = 55f,
                    ChassisColor = new Color(35, 30, 30, 255),
                    AccentColor = new Color(230, 80, 20, 255), // Naranja ardiente reactivo como en Imagen 2
                    LightGlowColor = new Color(255, 120, 30, 255),
                    ModelScale = new Vector3(0.10f, 0.14f, 0.55f)
                },

                // 4. SUBFUSIL (Imagen 2: Alta cadencia de fuego)
                new WeaponDefinition
                {
                    Type = WeaponType.SMG,
                    Name = "SPECTRE-9 SMG",
                    RoleDescription = "Cadencia supersónica y máxima agilidad en espacios cerrados.",
                    MagazineSize = 35,
                    MaxReserveAmmo = 140,
                    FireRateRPM = 980f,
                    BaseDamage = 20f,
                    HeadshotMultiplier = 1.9f,
                    Range = 65f,
                    SpreadAngle = 0.028f,
                    RecoilKick = 0.025f,
                    ReloadTimeSeconds = 1.8f,
                    PelletsPerShot = 1,
                    IsAutomatic = true,
                    HasScopeZoom = false,
                    ScopeZoomFov = 50f,
                    ChassisColor = new Color(22, 28, 38, 255),
                    AccentColor = new Color(0, 160, 255, 255), // Azul eléctrico como en Imagen 2
                    LightGlowColor = new Color(80, 210, 255, 255),
                    ModelScale = new Vector3(0.06f, 0.11f, 0.42f)
                },

                // 5. PISTOLA (Imagen 2: Rápida y confiable)
                new WeaponDefinition
                {
                    Type = WeaponType.Pistol,
                    Name = "APEX-11 SIDEARM",
                    RoleDescription = "Rápida, precisa y confiable. Arma secundaria táctica instantánea.",
                    MagazineSize = 12,
                    MaxReserveAmmo = 60,
                    FireRateRPM = 380f,
                    BaseDamage = 34f,
                    HeadshotMultiplier = 2.0f,
                    Range = 70f,
                    SpreadAngle = 0.018f,
                    RecoilKick = 0.035f,
                    ReloadTimeSeconds = 1.4f,
                    PelletsPerShot = 1,
                    IsAutomatic = false,
                    HasScopeZoom = false,
                    ScopeZoomFov = 55f,
                    ChassisColor = new Color(30, 35, 42, 255),
                    AccentColor = new Color(0, 230, 210, 255), // Cian turquesa como en Imagen 2
                    LightGlowColor = new Color(0, 255, 220, 255),
                    ModelScale = new Vector3(0.05f, 0.09f, 0.28f)
                }
            };
        }
    }

    public class ActiveWeaponInstance
    {
        public WeaponDefinition Definition { get; }
        public int CurrentAmmo { get; set; }
        public int ReserveAmmo { get; set; }
        public float FireTimer { get; set; } = 0f;
        public float ReloadTimer { get; set; } = 0f;
        public bool IsReloading => ReloadTimer > 0f;
        public float RecoilOffset { get; set; } = 0f;

        public ActiveWeaponInstance(WeaponDefinition definition)
        {
            Definition = definition;
            CurrentAmmo = definition.MagazineSize;
            ReserveAmmo = definition.MaxReserveAmmo;
        }

        public void Update(float dt)
        {
            if (FireTimer > 0f) FireTimer -= dt;

            if (IsReloading)
            {
                ReloadTimer -= dt;
                if (ReloadTimer <= 0f)
                {
                    int needed = Definition.MagazineSize - CurrentAmmo;
                    int take = Math.Min(needed, ReserveAmmo);
                    CurrentAmmo += take;
                    ReserveAmmo -= take;
                    ReloadTimer = 0f;
                }
            }

            RecoilOffset = Math.Max(0f, RecoilOffset - dt * 0.8f);
        }

        public bool CanShoot()
        {
            return FireTimer <= 0f && !IsReloading && CurrentAmmo > 0;
        }

        public void StartReload()
        {
            if (IsReloading || CurrentAmmo >= Definition.MagazineSize || ReserveAmmo <= 0) return;
            ReloadTimer = Definition.ReloadTimeSeconds;
        }
    }
}
