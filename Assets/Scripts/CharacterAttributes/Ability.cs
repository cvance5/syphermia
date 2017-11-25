using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : object
{
    private Abilities Name;
    private int Value;
    private int Weight;

    private Ability(Abilities name, int value)
    {
        Name = name;
        Value = value;
        Weight = 0;
    }
    public Ability(Ability master, int CognitionValue)
    {
        Name = master.Name;
        Value = master.Value;
        Weight = FindWeight(CognitionValue);
    }
    public void IncreaseValue(int increaseAmount)
    {
        Value += increaseAmount;
    }
    public bool DecreaseValue(int decreaseAmount)
    {
        if(Value - decreaseAmount  < 0)
        {
            return false;
        }
        else
        {
            Value -= decreaseAmount;
            return true;
        }
    }
    private int FindWeight(int CognitionValue)
    {
        Range PreferredRange = PreferredCognitionRanges[Name];
        if(PreferredRange.IsInRange(CognitionValue))
        {
            return 1;
        }
        else if (PreferredRange.DistanceOutside(CognitionValue) <= CharacterGenerator.TRAIT_RANGE_FORGIVENESS)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
    #region Getters
    public Abilities GetName()
    {
        return Name;
    }
    public int GetValue()
    {
        return Value;
    }
    public int GetWeight()
    {
        return Weight;
    }
    #endregion
    #region StaticFunctions
    public static List<Ability> GetAbilityList()
    {
        return DefaultAbilities;
    }
    #endregion
    #region StaticVariables
    private static List<Ability> DefaultAbilities = new List<Ability>()
    {
        new Ability(Abilities.Constitution, 0),
        new Ability(Abilities.Reflexes, 0),
        new Ability(Abilities.Willpower, 0),
        new Ability(Abilities.Faith, 0),
        new Ability(Abilities.Luck, 0),
        new Ability(Abilities.Perception, 0)
    };
    private static Dictionary<Abilities, Range> PreferredCognitionRanges = new Dictionary<Abilities, Range>()
    {
        {Abilities.Constitution, new Range(0, 33) },
        {Abilities.Reflexes, new Range(33, 66) },
        {Abilities.Willpower, new Range(66, 100) },
        {Abilities.Faith, new Range(0, 33) },
        {Abilities.Luck, new Range(33, 66) },
        {Abilities.Perception, new Range(66, 100) }
    };
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return Name.ToString();
    }
    public override bool Equals(object obj)
    {
        Ability otherAbility = obj as Ability;
        if(otherAbility != null && otherAbility.Name == Name && otherAbility.Value == Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator ==(Ability lhs, Ability rhs)
    {
        return lhs.Equals(rhs);
    }
    public static bool operator !=(Ability lhs, Ability rhs)
    {
        return !lhs.Equals(rhs);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}

public enum Abilities
{
    Constitution,
    Reflexes,
    Willpower,
    Faith,
    Luck,
    Perception
}
