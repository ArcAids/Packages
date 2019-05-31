using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : WeaponBehaviour, IReloadable, IAimable
{
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected int magazineSize=6;
    [SerializeField]
    float bulletSpeed=50;
    [SerializeField]
    float bulletDamage=1;
    [SerializeField]
    protected float reloadTime=2;
    [SerializeField]
    protected Transform muzzleTransform;
    [SerializeField]
    protected float fireRate=2;
    [SerializeField]
    bool hasMagazine=true;
    [SerializeField]
    [Range(0,1)]
    protected float accuracy=.5f;
    [SerializeField]
    float recoil=0.3f;

    Light flash;
    ParticleSystem smoke;
    protected int bulletsInMagazine;
    protected float nextFire;
    protected bool isReloading=false;
    float fireDelay;

    public float FireRate { get => fireRate; private set{ fireRate = value; fireDelay = 1f / value;} }
    public int MagazineSize { get => magazineSize; }
    public float ReloadTime { get => reloadTime; }

    private void Start()
    {
        FireRate = fireRate;
        bulletsInMagazine = MagazineSize;
        flash = muzzleTransform.gameObject.GetComponent<Light>();
        smoke = muzzleTransform.gameObject.GetComponentInChildren<ParticleSystem>();
        if(flash)
            flash.enabled = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    public override void Equip()
    {
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
            BulletBehaviour bullet=Instantiate(bulletPrefab,muzzleTransform.position, muzzleTransform.rotation ,null).GetComponent<BulletBehaviour>();

            bullet.Init(bulletDamage,bulletSpeed);
            nextFire = Time.time + fireDelay;
            bulletsInMagazine--;
        }
    }

    IEnumerator ApplyRecoil()
    {

        while (transform.localRotation.eulerAngles.x !=0)
        {

            yield return null;
        }
    }

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
        
    }

    public void Reload()
    {
        Reload(MagazineSize);
    }

    IEnumerator MuzzleFlash()
    {
        if (flash!=null)
        {
            smoke.Stop();
            flash.enabled = true;
            yield return new WaitForSeconds(0.05f);
            flash.enabled = false;
            yield return new WaitForSeconds(fireDelay);
            smoke?.Play();
        }
    }

}

interface IAttack
{
    void Attack();
}

interface IReloadable : IAttack
{
    void Reload();
}

interface IAimable
{
    void Aim();
}