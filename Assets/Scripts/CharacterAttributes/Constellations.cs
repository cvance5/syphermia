using System;
using System.Collections.Generic;
using UnityEngine;

public class Constellations : object
{
    #region StaticFunctions
    public static Constellation FindConstellation(int month)
    {
        return (Constellation)month - 1;
    }
    public static void GainBoon(CharacterData character, Constellation constellation)
    {
        switch(constellation)
        {
            case Constellation.Olag:
                character.ApplyModifier(new Modifier("Blessing of the Winter Winds", "Those who grow in the frigid winds grow strong.", ModifierType.Passive, "MaxHealth", 3, character, 0, null));
                break;
            case Constellation.VrembomeresBow:
                character.ApplyModifier(new Modifier("Legends of Vrembomere", "Hearing the tales of your patron cause many to aspire to mimicry.", ModifierType.Combat, "HitChance", 15, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.JuliepTheSorrowful:
                character.ApplyModifier(new Modifier("Beautiful Tears", "He was not known as the sorrowful for his own sorrows, but those he stole away.", ModifierType.Relationship, "Relationship", 20, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.DarkHorseman:
                character.ApplyModifier(new Modifier("A Fell Wind", "Those born under the sign of the Dark Horseman often feel his dreaded, lingering curse.", ModifierType.Tactical, "Movement", 1, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.TheFreeMason:
                character.ApplyModifier(new Modifier("Freedom of the Worthy", "A strange misnomer, considering the legendary Free Mason's many chains.", ModifierType.Tactical, "Damage", 3, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.Ululunden:
                character.ApplyModifier(new Modifier("Devotion", "The truth in faith comes to the devoted.", ModifierType.Passive, "Faith", 1, character, 0, null));
                break;
            case Constellation.HammerAndTheForge:
                character.ApplyModifier(new Modifier("Forged in Fire", "A rumor of a temperamental sign causes more fights than the actual temper.", ModifierType.Combat, "Damage", 5, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.TheHangedSon:
                character.ApplyModifier(new Modifier("Mysterious Omens", "The man haunted by his son never outlived his guilt.", ModifierType.Relationship, "Morale", -10, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.ThreeStags:
                character.ApplyModifier(new Modifier("Wild in Nature", "Nature nurtures.", ModifierType.Passive, "CarryWeight", 30, character, 0, null));
                break;
            case Constellation.DarkmorthTheLost:
                character.ApplyModifier(new Modifier("Poet of the Silence", "They say not all who wander are lost, but those who are lost wander.", ModifierType.Strategic, "Leadership", 1, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.Dammeron:
                character.ApplyModifier(new Modifier("He Who Calls Winds", "Harken, for his song is subtle.", ModifierType.Strategic, "WinChance", 5, character, 0, (CharacterData caller) => true));
                break;
            case Constellation.GiftsOfTheMagi:
                character.ApplyModifier(new Modifier("The Truest Gift", "A blessed time of year, filled with blessed babes.", ModifierType.Passive, "Experience", 10, character, 0, null));
                break;
            default:
                Debug.LogError("Error!  Requested boon from non-existent constellation! Caller: " + character + " Constellation: " + constellation);
                break;
        }
    }
    #endregion
}

public enum Constellation
{
    Olag,
    VrembomeresBow,
    JuliepTheSorrowful,
    DarkHorseman,
    TheFreeMason,
    Ululunden,
    HammerAndTheForge,
    TheHangedSon,
    ThreeStags,
    DarkmorthTheLost,
    Dammeron,
    GiftsOfTheMagi
}

public static class ConstellationTranslator
{
    public static string TranslateConstellation(this Constellation constellation)
    {
        switch(constellation)
        {
            case Constellation.Olag:
            case Constellation.Ululunden:
            case Constellation.Dammeron:
                return constellation.ToString();

            case Constellation.VrembomeresBow:
                return "Vrembomere's Bow";

            case Constellation.JuliepTheSorrowful:
                return "Juliep the Sorrowful";

            case Constellation.DarkHorseman:
                return "Dark Horseman";

            case Constellation.HammerAndTheForge:
                return "Hammer and the Forge";

            case Constellation.TheHangedSon:
                return "The Hanged Son";

            case Constellation.TheFreeMason:
                return "The Free Mason";

            case Constellation.ThreeStags:
                return "Three Stags";

            case Constellation.DarkmorthTheLost:
                return "Darkmorth the Lost";

            case Constellation.GiftsOfTheMagi:
                return "Gifts of the Magi";

            default:
                return "Not a constellation!";
        }
    }
}