using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class GunBehaviour : WeaponBehaviour, IReloadable, IAimable
    {
        [SerializeField]
        RecoilData recoilData;
        [SerializeField]
        CrossHair crossHair;
        [SerializeField]
        protected GameObject bulletPrefab;
        [SerializeField]
        protected int magazineSize = 6;
        [SerializeField]
        float bulletSpeed = 50;
        [SerializeField]
        float bulletDamage = 1;
        [SerializeField]
        protected float reloadTime = 2;
        [SerializeField]
        protected Transform muzzleTransform;
        [SerializeField]
        protected float fireRate = 2;
        [SerializeField]
        bool hasMagazine = true;
        [SerializeField]
        [Range(0, 1)]
        protected float accuracy = .5f;

        Recoil recoil;
        Camera cam;
        Light flash;
        ParticleSystem smoke;
        Vector3 defaultPosition;
        protected int bulletsInMagazine;
        protected float nextFire;
        protected bool isReloading = false;
        protected bool isAiming = true;
        float fireDelay;

        public float FireRate { get => fireRate; private set { fireRate = value; fireDelay = 1f / value; } }
        public int MagazineSize { get => magazineSize; }
        public float ReloadTime { get => reloadTime; }

        public override void Init(Camera cam)
        {
            this.cam = cam;
            FireRate = fireRate;
            bulletsInMagazine = MagazineSize;
            flash = muzzleTransform.gameObject.GetComponent<Light>();
            smoke = muzzleTransform.gameObject.GetComponentInChildren<ParticleSystem>();
            if (flash)
                flash.enabled = false;

            recoil = new Recoil(transform, recoilData);
            StopAiming();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            isReloading = false;
        }

        public override void Equip()
        {
            defaultPosition = transform.localPosition;
            gameObject.SetActive(true);
            nextFire = 0;
        }

        public void Attack()
        {
            if (isReloading)
                return;

            if (nextFire < Time.time)
            {
                if (bulletsInMagazine <= 0)
                {
                    Reload(MagazineSize);
                    return;
                }
                StartCoroutine(MuzzleFlash());
                if (bulletPrefab != null)
                    ShootBullet();
                else
                    ShootWithRayCast();
                recoil.StartRecoil();
                nextFire = Time.time + fireDelay;
                bulletsInMagazine--;
            }
        }

        private void ShootWithRayCast()
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(muzzleTransform.position, muzzleTransform.forward),out hit,500))
            {
                hit.collider.gameObject.GetComponent<ITakeDamage>()?.OnDamageTaken(bulletDamage);
            }
        }

        void ShootBullet()
        {
            BulletBehaviour bullet = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation, null).GetComponent<BulletBehaviour>();
            bullet.Init(bulletDamage, bulletSpeed);
        }

        //private void Update()
        //{
        //    recoil.Recoiling();
        //}

        public void Reload(int numberOfBullets)
        {
            isReloading = true;
            StartCoroutine(ReloadWait(numberOfBullets));
        }

        IEnumerator ReloadWait(int numberOfBullets)
        {
            yield return new WaitForSeconds(ReloadTime);
            bulletsInMagazine += numberOfBullets;
            if (bulletsInMagazine > MagazineSize)
                bulletsInMagazine = MagazineSize;
            isReloading = false;

        }

        public void Aim()
        {
            if (isAiming)
                return;

            cam.fieldOfView = 30;
            transform.position = cam.transform.position + cam.transform.forward - cam.transform.up / 3;
            isAiming = true;
            crossHair.Expand(0);
        }

        public void StopAiming()
        {
            if (!isAiming)
                return;

            transform.localPosition = defaultPosition;
            cam.fieldOfView = 60;

            isAiming = false;
            crossHair.Expand(1);
        }

        public void Reload()
        {
            Reload(MagazineSize);
        }

        IEnumerator MuzzleFlash()
        {
            if (flash != null)
            {
                smoke.Stop();
                flash.enabled = true;
                yield return new WaitForSeconds(0.05f);
                flash.enabled = false;
                smoke?.Play();
                yield return new WaitForSeconds(fireDelay);
            }
        }

        public override void Dequip()
        {
            
        }

        public override void Pick()
        {
            Picked = true;
            GetComponent<Collider>().enabled = false;
        }

        public override void Drop()
        {
            Picked = false;
            GetComponent<Collider>().enabled = true;
            gameObject.SetActive(true);
        }
    }

    public interface IAttack
    {
        void Attack();
    }

    public interface IReloadable : IAttack
    {
        void Reload();
    }

    public interface IPickable
    {
        void Pick();
        void Drop();
    }
    public interface IAimable
    {
        void Aim();
        void StopAiming();
    }
}