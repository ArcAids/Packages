using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonShooter : MonoBehaviour
{
    [SerializeField]
    Transform gunHolder;
    [SerializeField]
    WeaponBehaviour[] weapons;
    IShootable

    Camera cam;
    int gunEquippedIndex=-1;
    RaycastHit hit;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var gun in weapons)
        {
            gun.gameObject.transform.parent = null;
            gun.gameObject.SetActive(false);
        }
        if(weapons.Length>0)
            EquipWeapon(0);
        cam = GetComponentInChildren<Camera>();
    }

    void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length || index == gunEquippedIndex)
            return;
        if (gunEquippedIndex >= 0)
        {
            weapons[gunEquippedIndex].gameObject.transform.parent = null; 
            weapons[gunEquippedIndex].gameObject.SetActive(false);
        }
        weapons[index].gameObject.transform.parent = gunHolder;
        weapons[index].gameObject.SetActive(true);
        weapons[index].transform.localPosition = Vector3.zero;
        weapons[index].transform.localRotation = Quaternion.identity;
        gunEquippedIndex = index;
    }

    void PointGunAtTarget()
    {
        if (Physics.Raycast(cam.transform.position + cam.transform.forward, cam.transform.forward, out hit, 500))
        {
            //gunHolder.LookAt(hit.point,Vector3.up);
            Vector3 gunPointDirection = (hit.point - gunHolder.position).normalized;
            gunHolder.rotation = Quaternion.Lerp(gunHolder.rotation, Quaternion.LookRotation(gunPointDirection, Vector3.up), Time.deltaTime * 6);
        }
    }
    // Update is called once per frame
    void Update()
    {
        PointGunAtTarget();
        if (Input.GetButton("Fire1"))
        {
            weapons[gunEquippedIndex].Shoot();
        }
        if (Input.GetKey(KeyCode.Alpha1) && weapons.Length>=1)
            EquipWeapon(0);
        if (Input.GetKey(KeyCode.Alpha2) && weapons.Length>=2)
            EquipWeapon(1);
        if (Input.GetKey(KeyCode.Alpha3) && weapons.Length>=3)
            EquipWeapon(2);
    }
}
