﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour, IEquipableAndDequipable
{
    public abstract void Init(Camera cam);
    public abstract void Equip();
}

interface IEquipableAndDequipable
{
    void Equip();

}