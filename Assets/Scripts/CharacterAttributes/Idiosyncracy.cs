using System;
using UnityEngine;

public abstract class Idiosyncracy {
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected Severities Severity;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected Sprite Icon = null;
    [SerializeField]
    protected IdiosyncracyTypes Type;
    [SerializeField]
    protected Kind Kind;

    public abstract bool CheckActivationConditions(params object[] args);
    public abstract void ActivateIdiosyncracy();
    #region Getters
    public Severities GetSeverity()
    {
        return Severity;
    }
    public string GetName()
    {
        return Name;
    }
    public string GetDescription()
    {
        return Description;
    }
    public IdiosyncracyTypes GetIdiosyncracyType()
    {
        return Type;
    }
    public Kind GetKind()
    {
        return Kind;
    }
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return GetName();
    }
    #endregion
}

public enum Severities
{
    Trivial,
    Moderate,
    Severe,
    Critical,
    Overwhelming
}

public enum IdiosyncracyTypes
{
    Combat,
    None
}

public enum Kind
{
    Quirk,
    Flaw
}
