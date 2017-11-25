using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterGenerator : Singleton<CharacterGenerator> {

	void Awake()
    {
        PrepareIdiosyncracies();
    }
    #region Retrievers
    public static Skill GenerateSkill(string skill)
    {
        //Runs a Func<Skill> that returns the appropriate new skill
        Skill newSkill = SkillSpawner[skill]();

        if (newSkill == null)
        {
            Debug.LogError("Error!  Skill \"" + skill + "\" not found!");
        }

        return newSkill as Skill;
    }
    public static Idiosyncracy RetrieveIdiosyncracy(Kind kind, Severities severity)
    {
        return IdiosyncracyMap[kind][severity][UnityEngine.Random.Range(0, IdiosyncracyMap[kind][severity].Count)];
    }
    #endregion
    #region GenerationFunctions
    public static GameObject GenerateCharacter()
    {
        GameObject newCharacter = new GameObject();
        CharacterData character = newCharacter.AddComponent<CharacterData>();

        GenerateName(character, NameType.Syllabic);
        GenerateTraits(character);
        GenerateAbilities(character);
        GenerateAge(character);
        GenerateProfession(character);
        GenerateIdiosyncracies(character);
        GenerateConstellation(character);
        character.Initialize();

        return newCharacter;
    }
    public static void GenerateName(CharacterData character, NameType nameType)
    {
        switch (nameType)
        {
            case NameType.Syllabic:
                {
                    //Determine name length, weighted slightly toward shorter
                    int numSyllables = UnityEngine.Random.Range(1, 3);
                    numSyllables += UnityEngine.Random.Range(0, 2);

                    //Determine if the name has special separators, weighted by length of name
                    int separatorLocation = int.MaxValue;
                    if (10 * (numSyllables - 1) > UnityEngine.Random.Range(0, 100))
                    {
                        separatorLocation = UnityEngine.Random.Range(0, numSyllables);
                    }

                    //Read in all possible names
                    string[] allLines = File.ReadAllLines("Assets/Resources/NameData/SyllabicName.txt");

                    for (int i = 1; i <= numSyllables; i++)
                    {
                        //Pick a syllable at random and clean up any white space
                        string syllable = allLines[UnityEngine.Random.Range(0, allLines.Length)];
                        syllable.Trim();

                        //If the last addition was a separator, capitalize this syllable
                        if (i == separatorLocation + 1)
                        {
                            syllable = char.ToUpper(syllable[0]) + syllable.Substring(1);
                        }
                        character.Name += syllable;

                        //Double the ending letter of this syllable some of the time
                        if (UnityEngine.Random.value > .85f)
                        {
                            character.Name += character.Name[character.Name.Length - 1];
                        }

                        //Add a random vowel to the end of this syllable, with weights
                        float vowelSelector = UnityEngine.Random.value;
                        if (vowelSelector < .2f)
                        {
                            character.Name += "a";
                        }
                        else if (vowelSelector >= .2f && vowelSelector < .4f)
                        {
                            character.Name += "e";
                        }
                        else if (vowelSelector >= .4f && vowelSelector < .6f)
                        {
                            character.Name += "i";
                        }
                        else if (vowelSelector >= .6f && vowelSelector < .7f)
                        {
                            character.Name += "o";
                        }
                        else if (vowelSelector >= .7f && vowelSelector < .8f)
                        {
                            character.Name += "u";
                        }

                        //If a separator was randomly chosen to go here, pick from that list and add it
                        if (i == separatorLocation)
                        {
                            string[] allSeparators = File.ReadAllLines("Assets/Resources/NameData/Separators.txt");
                            character.Name += allSeparators[UnityEngine.Random.Range(0, allSeparators.Length)];
                        }
                    }
                    character.Name = char.ToUpper(character.Name[0]) + character.Name.Substring(1);

                    character.gameObject.name = character.Name;
                }
                break;
        }

    }
    public static void GenerateTraits(CharacterData character)
    {
        character.CharacterTraits = new List<Trait>();

        foreach (Trait trait in Trait.GetTraitList())
        {
            character.CharacterTraits.Add(new Trait(trait));
        }
        foreach (Trait trait in character.CharacterTraits)
        {
            trait.Initialize();
        }
    }
    public static void GenerateAbilities(CharacterData character)
    {
        character.CharacterAbilities = new List<Ability>();

        foreach (Ability ability in Ability.GetAbilityList())
        {
            character.CharacterAbilities.Add(new Ability(ability, character.CharacterTraits[(int)Traits.Cognition].GetValue()));
        }
        character.IncreaseAbilityPoints(INITIAL_ABILITY_POINTS);
    }
    public static void GenerateAge(CharacterData character)
    {
        for (int i = 0; i < MIN_AGE; i++)
        {
            character.Age += UnityEngine.Random.Range(0, MAX_AGE / MIN_AGE);
        }
        character.Age += MIN_AGE;
        character.Age -= UnityEngine.Random.Range(0, character.Age % MIN_AGE);

        character.GainSkillSlots(INITIAL_SKILL_SLOTS - (character.Age / 20));

        character.Birthday = new Date(UnityEngine.Random.Range(0, 365), UnityEngine.Random.Range(0, 12), CURRENT_YEAR - character.Age);
    }
    public static void GenerateProfession(CharacterData character)
    {
        do
        {
            character.CharacterProfession = new Profession(character);
        } while (!character.CharacterProfession.CheckAgeLimitations(character.Age));

        DistributeProfessionAbilityBonuses(character);
    }
    private static void DistributeProfessionAbilityBonuses(CharacterData character)
    {        
        foreach (Ability ability in character.CharacterAbilities)
        {
            ability.IncreaseValue(Mathf.RoundToInt(character.CharacterProfession.GetAbilityModifier(ability.GetName()) * character.Age / MAX_AGE));
            while (!character.CharacterAbilities[UnityEngine.Random.Range(0, character.CharacterAbilities.Count)].DecreaseValue(Mathf.RoundToInt(character.CharacterProfession.GetAbilityModifier(ability.GetName()) * character.Age / MAX_AGE))) ;
        }
    }
    public static void GenerateIdiosyncracies(CharacterData character)
    {
        int strangeness = 0;
        character.CharacterQuirk = RetrieveIdiosyncracy(Kind.Quirk, (Severities)strangeness);
        character.CharacterFlaw = RetrieveIdiosyncracy(Kind.Flaw, (Severities)strangeness);
    }
    public static void GenerateConstellation(CharacterData character)
    {
        character.PatronConstellation = Constellations.FindConstellation(character.Birthday.GetMonth());
        Constellations.GainBoon(character, character.PatronConstellation);
    }
    #endregion
    private void PrepareIdiosyncracies()

    {
        Dictionary<Severities, List<Idiosyncracy>> QuirkMap = new Dictionary<Severities, List<Idiosyncracy>>();
        Dictionary<Severities, List<Idiosyncracy>> FlawMap = new Dictionary<Severities, List<Idiosyncracy>>();

        for (int i = 0; i <= (int)Severities.Overwhelming; i++)
        {
            QuirkMap.Add((Severities)i, new List<Idiosyncracy>());
            FlawMap.Add((Severities)i, new List<Idiosyncracy>());
        }

        IdiosyncracyMap.Add(Kind.Quirk, QuirkMap);
        IdiosyncracyMap.Add(Kind.Flaw, FlawMap);

        foreach (Idiosyncracy idiosyncracy in IdiosyncracyPool)
        {
            IdiosyncracyMap[idiosyncracy.GetKind()][idiosyncracy.GetSeverity()].Add(idiosyncracy);
        }
    }

    private static List<Idiosyncracy> IdiosyncracyPool = new List<Idiosyncracy>()
    {
        new Boring(),
        new Normal()
    };

    private static Dictionary<Kind, Dictionary<Severities, List<Idiosyncracy>>> IdiosyncracyMap = new Dictionary<Kind, Dictionary<Severities, List<Idiosyncracy>>>();

    private static Dictionary<string, Func<Skill>> SkillSpawner = new Dictionary<string, Func<Skill>>()
    {
        {"None", () => new None() }
    };

    public const int INITIAL_ABILITY_POINTS = 6;
    public const int TRAIT_RANGE_FORGIVENESS = 17;
    public const int MIN_AGE = 10;
    public const int MAX_AGE = 60;
    public const int INITIAL_SKILL_SLOTS = 3;
    public const int MINIMUM_HEALTH = 1;
    public const int CURRENT_YEAR = 350;
}
public enum NameType
{
    Syllabic,
    Descriptive,
    Multigenerational
}