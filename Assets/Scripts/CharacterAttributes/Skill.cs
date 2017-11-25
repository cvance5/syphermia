using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Skill
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected SkillTypes Type;
    [SerializeField]
    protected Sprite Icon = null;

    public abstract void ActivateSkill(params object[] args);

    #region Getters
    public string GetName()
    {
        return Name;
    }
    public string GetDescription()
    {
        return Description;
    }
    public SkillTypes GetSkillType()
    {
        return Type;
    }
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return GetName();
    }
    #endregion
}

public enum SkillTypes
{
    Combat,
    None
}
