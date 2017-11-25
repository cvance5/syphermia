using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item {

    [Header("Armor Attributes")]
    [Tooltip("The amount of damage negated per hit.")]
    public int Deflection = 0;
    [Range(0, 2)]
    public float DodgeModifier = 1;
    [Range(-1, 1)]
    public float Dampening = 0;

    protected override void OnValidate()
    {
        base.OnValidate();
        Type = ItemTypes.Armor;
    }
}
