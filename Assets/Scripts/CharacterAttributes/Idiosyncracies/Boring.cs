using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boring : Idiosyncracy
{
    public Boring()
    {
        Name = "Boring";
        Severity = Severities.Trivial;
        Description = "Joy in complacency.";
        Type = IdiosyncracyTypes.None;
        Kind = Kind.Flaw;
    }
    public override void ActivateIdiosyncracy() { }
    public override bool CheckActivationConditions(object[] args) { return false; }
}
