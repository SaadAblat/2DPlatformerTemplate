using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] internal int howMuchAmmoContains;
    [SerializeField] Player player;

    internal enum AmmoType
    {
        Shotgun,
        Pistol
    }
    [SerializeField] internal AmmoType ammoType;

}
