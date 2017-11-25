using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Profession : object
{
    private string Name;
    private Range AgeLimitations;
    private string Description;
    private ProficiencyTiers Proficiency;
    private Dictionary<Abilities, int> AbilityModifiers = new Dictionary<Abilities, int>();
    private ProfessionalCategories JobType;
    private Skill JobSkill;
    public Profession(CharacterData character)
    {
        string[] allProfessions = File.ReadAllLines("Assets/Resources/Professions.txt");
        string[] professionData = allProfessions[UnityEngine.Random.Range(0, allProfessions.Length)].Split('|');

        Name = professionData[0];
        AgeLimitations = new Range(professionData[1]);
        Description = professionData[2];

        string[] abilityModifiers = professionData[3].Split('&');
        for(int i = 0; i < abilityModifiers.Length; i++)
        {
            string[] abilityModifier = abilityModifiers[i].Split('=');
            AbilityModifiers.Add((Abilities)Enum.Parse(typeof(Abilities), abilityModifier[0]), int.Parse(abilityModifier[1]));
        }

        JobType = (ProfessionalCategories)Enum.Parse(typeof(ProfessionalCategories), professionData[4]);
        JobSkill = character.GainSkill(CharacterGenerator.GenerateSkill(professionData[5]));
        Proficiency = DetermineProficiency(character);
    }

    public bool CheckAgeLimitations(int age)
    {
        return AgeLimitations.IsInRange(age);
    }
    public int GetAbilityModifier(Abilities ability)
    {
        return AbilityModifiers.ContainsKey(ability) ? AbilityModifiers[ability] : 0;
    }
    private ProficiencyTiers DetermineProficiency(CharacterData character)
    {
        int proficiency = 0;
        foreach (Ability ability in character.CharacterAbilities)
        {
            if(AbilityModifiers.ContainsKey(ability.GetName()))
            {
                if(AbilityModifiers[ability.GetName()] > 0)
                {
                    proficiency = proficiency + Mathf.Clamp(ability.GetValue(), 0, AbilityModifiers[ability.GetName()]);
                }
                else
                {
                    proficiency = proficiency - Mathf.Clamp(ability.GetValue(), 0, AbilityModifiers[ability.GetName()]);
                }
            }
        }
        return (ProficiencyTiers)proficiency;
    }
    #region Getters
    public Skill GetProfessionalSkill()
    {
        return JobSkill;
    }
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        return Name;
    }
    public string ToString(bool verbose)
    {
        if(verbose)
        {
            return "Name: " + Proficiency + " " + Name + " Age Limitations: " + AgeLimitations + " Description: " + Description + " Ability Modifiers: " + AbilityModifiers.Keys + " Job Type: " + JobType + " Job Skill: " + JobType;
        }
        else
        {
            return ToString();
        }
    }
    #endregion
}

public enum ProfessionalCategories
{
    Combatant,
    Craftsman,
    Artisan,
    Scholar,
    Peasant,
    Rogue,
    Noble,
    Other
}
public enum ProficiencyTiers
{
    Failing,
    Apprentice,
    Novice,
    Journeyman,
    Master,
    Legendary
}
