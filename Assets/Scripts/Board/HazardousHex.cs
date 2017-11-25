using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardousHex : Hex
{
    [SerializeField]
    private int HazardDamage = 0;

    #region Getters
    public int GetHazardDamage()
    {
        return HazardDamage;
    }
    #endregion
}