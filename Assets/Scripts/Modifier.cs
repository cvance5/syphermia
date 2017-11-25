using System;
using UnityEngine;

public class Modifier {

    private string Name;
    private string Description;
    private ModifierType Type;
    private string StatAffected;
    private int Value;
    private CharacterData Source;
    private int TurnDuration;
    private Func<CharacterData, bool> Conditional;

    public Modifier(string name, string description, ModifierType type, string statAffected, int value, CharacterData source, int turnDuration, Func<CharacterData, bool> conditional)
    {
        Name = name;
        Description = description;
        Type = type;
        StatAffected = statAffected;
        Value = value;
        Source = source;
        TurnDuration = turnDuration;
        Conditional = conditional;
    }
    public bool CheckConditional(CharacterData character)
    {
        return Conditional(character);
    }

    #region Getters
    public string GetName()
    {
        return Name;
    }
    public string GetDescription()
    {
        return Description;
    }
    public ModifierType GetModifierType()
    {
        return Type;
    }
    public string GetStatAffected()
    {
        return StatAffected;
    }
    public int GetValue()
    {
        return Value;
    }
    public CharacterData GetSource()
    {
        return Source;
    }
    public int GetDuration()
    {
        return TurnDuration;
    }
    #endregion
}

public enum ModifierType
{
    Combat,
    Relationship,
    Tactical,
    Strategic,
    Passive
}