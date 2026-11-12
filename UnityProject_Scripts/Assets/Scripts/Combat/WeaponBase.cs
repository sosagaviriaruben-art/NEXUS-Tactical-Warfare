using UnityEngine;
using System;

namespace Nexus.Combat
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Datos Generales")]
        public string WeaponName = "Nexus Weapon";
        public int MagazineSize = 30;
        public int MaxReserveAmmo = 120;
        public float FireRateRPM = 600f;
        public float Damage = 25f;
        public float HeadshotMultiplier = 2.0f;
        public float Range = 100f;
        public float ReloadTime = 2.0f;
        public bool IsAutomatic = true;

        [Header("Estado")]
        public int CurrentAmmo { get; protected set; }
        public int CurrentReserve { get; protected set; }
        public bool IsReloading { get; protected set; }
        protected float NextFireTime;

        public event Action OnFired;
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;

        protected virtual void Awake()
        {
            CurrentAmmo = MagazineSize;
            CurrentReserve = MaxReserveAmmo;
        }

        public virtual bool CanFire()
        {
            return Time.time >= NextFireTime && !IsReloading && CurrentAmmo > 0;
        }

        public abstract void Fire(Vector3 origin, Vector3 direction);

        public virtual void Reload()
        {
            if (IsReloading || CurrentAmmo >= MagazineSize || CurrentReserve <= 0) return;
            StartCoroutine(ReloadRoutine());
        }

        private System.Collections.IEnumerator ReloadRoutine()
        {
            IsReloading = true;
            OnReloadStarted?.Invoke();
            yield return new WaitForSeconds(ReloadTime);

            int needed = MagazineSize - CurrentAmmo;
            int take = Mathf.Min(needed, CurrentReserve);
            CurrentAmmo += take;
            CurrentReserve -= take;

            IsReloading = false;
            OnReloadFinished?.Invoke();
        }
    }
}
