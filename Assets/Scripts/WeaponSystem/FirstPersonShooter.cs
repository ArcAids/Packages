using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonShooter : MonoBehaviour
{
    [SerializeField]
    protected Transform gunHolder;
    [SerializeField]
    bool followCursor=false;
    [SerializeField]
    WeaponBehaviour[] weapons;

    IAttack shootable;
    IAttack melee;
    IReloadable reloadable;
    IAimable aimable;
    [SerializeField]
    protected Camera cam;
    int gunEquippedIndex=-1;
    protected RaycastHit hit;

    // Start is called before the first frame update
    protected void Awake()
    {
        foreach (var gun in weapons)
        {
            Dequip(gun);
            if (gun is MeleeBehaviour)
                melee = gun.GetComponent<IAttack>();
        }
        if(weapons.Length>0)
            EquipWeapon(0);
        if(cam==null)
            cam = GetComponentInChildren<Camera>();
    }

    protected void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length || index == gunEquippedIndex)
            return;

        if (gunEquippedIndex >= 0)
            Dequip(weapons[gunEquippedIndex]);

        if (weapons[index] is GunBehaviour)
        {
            weapons[index].gameObject.transform.parent = gunHolder;
            weapons[index].transform.localPosition = Vector3.zero;
            weapons[index].transform.localRotation = Quaternion.identity;
        }

        weapons[index].Equip();

        reloadable =weapons[index].GetComponent<IReloadable>();
        if (reloadable != null)
            shootable = reloadable;
        else
            shootable=weapons[index].GetComponent<IAttack>();
        aimable=weapons[index].GetComponent<IAimable>();
        gunEquippedIndex = index;
    }

    protected void Dequip(WeaponBehaviour weapon)
    {
        if (weapon is GunBehaviour)
        {
            weapon.gameObject.transform.parent = null;
            weapon.gameObject.SetActive(false);
        }
        gunEquippedIndex = -1;
    }

    protected virtual void PointGunAtTarget()
    {
        Ray ray;
        if (followCursor)
            ray = cam.ScreenPointToRay(Input.mousePosition);
        else
            ray = new Ray(cam.transform.position + cam.transform.forward, cam.transform.forward);

        if (Physics.Raycast(ray, out hit, 500))
        {
            //gunHolder.LookAt(hit.point,Vector3.up);

            Vector3 gunPointDirection = hit.point;
            gunPointDirection =( gunPointDirection - gunHolder.position).normalized;
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.LookRotation(gunPointDirection, Vector3.up), Time.deltaTime * 6);
        }
    }
    // Update is called once per frame
    protected void Update()   
    {
        if(cam!=null)
            PointGunAtTarget();
        if (Input.GetButton("Fire1"))
            shootable?.Attack();
        if (Input.GetKey(KeyCode.Space))
            melee?.Attack();

        
        if (Input.GetKey(KeyCode.Alpha1))
            EquipWeapon(0);
        if (Input.GetKey(KeyCode.Alpha2))
            EquipWeapon(1);
        if (Input.GetKey(KeyCode.Alpha3))
            EquipWeapon(2);
        if (Input.GetKey(KeyCode.Alpha4))
            EquipWeapon(3);
        if (Input.GetKey(KeyCode.R))
            reloadable?.Reload();
    }
}
