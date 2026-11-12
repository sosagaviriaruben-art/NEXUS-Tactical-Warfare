using UnityEngine;

namespace Nexus.Combat
{
    // 1. RIFLE DE ASALTO (Imagen 2: Versátil y equilibrado)
    public class AssaultRifle : WeaponBase
    {
        private void Reset()
        {
            WeaponName = "Nexus-AR Valiant";
            MagazineSize = 30;
            MaxReserveAmmo = 120;
            FireRateRPM = 620f;
            Damage = 28f;
            Range = 120f;
            ReloadTime = 2.2f;
            IsAutomatic = true;
        }

        public override void Fire(Vector3 origin, Vector3 direction)
        {
            if (!CanFire()) return;
            CurrentAmmo--;
            NextFireTime = Time.time + (60f / FireRateRPM);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, Range))
            {
                // Aplicar daño
            }
        }
    }

    // 2. FRANCOTIRADOR (Imagen 2: Alta precisión y daño)
    public class SniperRifle : WeaponBase
    {
        public float ZoomFov = 20f;

        private void Reset()
        {
            WeaponName = "Vortex Rail-Sniper";
            MagazineSize = 5;
            MaxReserveAmmo = 25;
            FireRateRPM = 50f;
            Damage = 115f;
            HeadshotMultiplier = 2.5f;
            Range = 250f;
            ReloadTime = 3.2f;
            IsAutomatic = false;
        }

        public override void Fire(Vector3 origin, Vector3 direction)
        {
            if (!CanFire()) return;
            CurrentAmmo--;
            NextFireTime = Time.time + (60f / FireRateRPM);
        }
    }

    // 3. ESCOPETA (Imagen 2: Ideal para corta distancia)
    public class Shotgun : WeaponBase
    {
        public int PelletCount = 8;
        public float SpreadAngle = 4.5f;

        private void Reset()
        {
            WeaponName = "Havoc Scattergun";
            MagazineSize = 8;
            MaxReserveAmmo = 32;
            FireRateRPM = 85f;
            Damage = 18f;
            Range = 40f;
            ReloadTime = 2.8f;
            IsAutomatic = false;
        }

        public override void Fire(Vector3 origin, Vector3 direction)
        {
            if (!CanFire()) return;
            CurrentAmmo--;
            NextFireTime = Time.time + (60f / FireRateRPM);
        }
    }

    // 4. SUBFUSIL (Imagen 2: Alta cadencia de fuego)
    public class SMG : WeaponBase
    {
        private void Reset()
        {
            WeaponName = "Spectre-9 SMG";
            MagazineSize = 35;
            MaxReserveAmmo = 140;
            FireRateRPM = 980f;
            Damage = 20f;
            Range = 65f;
            ReloadTime = 1.8f;
            IsAutomatic = true;
        }

        public override void Fire(Vector3 origin, Vector3 direction)
        {
            if (!CanFire()) return;
            CurrentAmmo--;
            NextFireTime = Time.time + (60f / FireRateRPM);
        }
    }

    // 5. PISTOLA (Imagen 2: Rápida y confiable)
    public class Pistol : WeaponBase
    {
        private void Reset()
        {
            WeaponName = "Apex-11 Sidearm";
            MagazineSize = 12;
            MaxReserveAmmo = 60;
            FireRateRPM = 380f;
            Damage = 34f;
            Range = 70f;
            ReloadTime = 1.4f;
            IsAutomatic = false;
        }

        public override void Fire(Vector3 origin, Vector3 direction)
        {
            if (!CanFire()) return;
            CurrentAmmo--;
            NextFireTime = Time.time + (60f / FireRateRPM);
        }
    }
}
