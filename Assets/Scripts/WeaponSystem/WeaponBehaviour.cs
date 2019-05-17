using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour, IEquipableAndDequipable
{
    public abstract WeaponBehaviour Equip();
}

interface IEquipableAndDequipable
{
    WeaponBehaviour Equip();

}