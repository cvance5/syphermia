using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Idiosyncracy
{
    public Normal()
    {
        Name = "Normal";
        Severity = Severities.Trivial;
        Description = "Well, at least you're not weird.";
        Type = IdiosyncracyTypes.None;
        Kind = Kind.Quirk;
    }
    public override void ActivateIdiosyncracy() { }
    public override bool CheckActivationConditions(object[] args) { return false; }
}