using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait : object {
    private Traits Name;
    private int Value;
    private string CurrentTier;
    private Trait(Traits name, int value)
    {
        Name = name;
        Value = value;
        CurrentTier = null;
    }
    public Trait(Trait master)
    {
        Name = master.Name;
        Value = master.Value;
        CurrentTier = master.CurrentTier;
    }
    public void Initialize()
    {
        Value = UnityEngine.Random.Range(0, 100);
        CurrentTier = DiscoverTier(this);
    }

    private static string DiscoverTier(Trait trait)
    {        
        return TraitTiers[trait.Name][(Range)trait.Value];
    }
    #region Getters
    public Traits GetName()
    {
        return Name;
    }
    public int GetValue()
    {
        return Value;
    }
    public string GetTier()
    {
        return CurrentTier;
    }
    #endregion
    #region StaticFunctions
    public static List<Trait> GetTraitList()
    {
        return DefaultTraits;
    }
    #endregion
    #region StaticVariables
    private static List<Trait> DefaultTraits = new List<Trait>()
    {
        new Trait(Traits.Cognition, 0),
        new Trait(Traits.Mentality, 0),
        new Trait(Traits.Expressiveness, 0)
    };
    private static Dictionary<Range, string> CognitionTiers= new Dictionary<Range, string>(new RangeComparer()) 
    {
        {new Range(0,19), "Intuitive" },
        {new Range(20,39), "Astute" },
        {new Range(40,59), "Witty" },
        {new Range(60,79), "Clever" },
        {new Range(80,100), "Intellectual" }
    };
    private static Dictionary<Range, string> MentalityTiers = new Dictionary<Range, string>(new RangeComparer())
    {
        {new Range(0,19), "Cynical" },
        {new Range(20,39), "Pessimistic" },
        {new Range(40,59), "Realistic" },
        {new Range(60,79), "Optimistic" },
        {new Range(80,100), "Hopeful" }
    };
    private static Dictionary<Range, string> ExpressivenessTiers = new Dictionary<Range, string>(new RangeComparer())
    {
        {new Range(0,19), "Stone-Faced" },
        {new Range(20,39), "Reserved" },
        {new Range(40,59), "Concise" },
        {new Range(60,79), "Outgoing" },
        {new Range(80,100), "Exuberant" }
    };
    private static Dictionary<Traits, Dictionary<Range, string>> TraitTiers = new Dictionary<Traits, Dictionary<Range, string>>()
    {
        {Traits.Cognition, CognitionTiers},
        {Traits.Mentality, MentalityTiers},
        {Traits.Expressiveness, ExpressivenessTiers}
    };
    public const int MAX_TRAIT_VALUE = 100;
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return Name.ToString();
    }
    public override bool Equals(object obj)
    {
        Trait otherTrait = obj as Trait;
        if (otherTrait != null && otherTrait.Name == Name && otherTrait.Value == Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator ==(Trait lhs, Trait rhs)
    {
        return lhs.Equals(rhs);
    }
    public static bool operator !=(Trait lhs, Trait rhs)
    {
        return !lhs.Equals(rhs);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    #endregion
}
public enum Traits
{
    Cognition,
    Mentality,
    Expressiveness
}
