using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData : MonoBehaviour
{
    #region Attributes
    public string Name;
    public int Age;
    public int MaxHealth;
    public int CurrentHealth;
    public int ActionPoints = 3;
    public Date Birthday;
    public Profession CharacterProfession;
    public List<Trait> CharacterTraits;
    public List<Ability> CharacterAbilities;
    public int SkillSlots;
    public List<Skill> CharacterSkills;
    public Idiosyncracy CharacterQuirk;
    public Idiosyncracy CharacterFlaw;
    public Constellation PatronConstellation;
    public Dictionary<string, List<Modifier>> ModifiersMap = new Dictionary<string, List<Modifier>>();
    #endregion
    #region Components
    public Hex OccupiedHex { get; private set; }
    public ArmyData CurrentArmy { get; private set; }
    private Inventory CharacterInventory;
    private SpriteRenderer CharacterRenderer;
    private Animator CharacterAnimator;
    private PathfinderMap Pathfinder;
    #endregion

    public void Initialize()
    {
        InitializeEquipment();
        InitializeGraphics();
        InitializeHealth();
        Pathfinder = new PathfinderMap(this);
    }
    public void JoinArmy(ArmyData newArmy)
    {
        Debug.Assert(CurrentArmy != newArmy, "The character " + name + " has tried to join army " + newArmy + " multiple times.");
        CurrentArmy = newArmy;
        transform.parent = newArmy.transform;
    }
    public void LeaveArmy()
    {
        CurrentArmy = null;
        transform.parent = null;
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
    #region MovementHandlers
    public void Occupy(Hex hex)
    {
        OccupiedHex = hex;
        ChangeHealth(-Pathfinder.GetDamageOfHex(hex));
        transform.LookAt(Camera.main.transform.position);
    }
    public void UpdateOptions()
    {
        switch(ContextManager.GetCurrentContext())
        {
            case Context.Manuever:
                GetWeapon().EndVisualization();
                if (Pathfinder.IsOutdated())
                {
                    Pathfinder.CreateMap(OccupiedHex);
                }
                Pathfinder.VisualizeMap();
                break;
            case Context.Target:
                Pathfinder.EndVisualization();
                GetWeapon().VisualizeAttack();
                break;
            default:
                print(ContextManager.GetCurrentContext());
                break;
        }
    }
    public void ClearOptions()
    {
        Pathfinder.EndVisualization();
        GetWeapon().EndVisualization();
        Pathfinder.ClearPath();
    }
    public void HidePath()
    {
        Pathfinder.EndVisualization();
    }
    public void Pathfind(Hex hex)
    {
        Pathfinder.ShowPath(hex);
    }
    public void Vacate()
    {
        OccupiedHex = null;
    }
    #endregion
    #region CombatHandlers
    public bool ValidateCombat(CharacterData target, Hex targetLocation)
    {
        if(target != null)
        {
            if (target.CurrentArmy != CurrentArmy)
            {
                if (GetWeapon().WeaponRange == BoardManager.CalculateHexDistance(targetLocation, OccupiedHex))
                {
                    return true;
                }
            }
        }
        return false;               
    }
    public int RollInitiative()
    {
        int initiative;
        do
        {
            initiative = Random.Range(0, 101);
        } while (initiative < CharacterAbilities[(int)Abilities.Reflexes].GetValue());

        initiative += RetrieveModifiersForStat("Initiative");

        return initiative;
    }
    public float CalculateHitChance()
    {
        return GetWeapon().HitRate + RetrieveModifiersForStat("Hit Rate");
    }
    public int CalculateDamageDealt()
    {
        return GetWeapon().Damage + RetrieveModifiersForStat("Damage") + Random.Range(-GetWeapon().Weight, GetWeapon().Weight + 1);
    }
    public bool AttemptDodge(float hitChance)
    {
        //Does character have sufficient CON to carry armor
        float armorWeightEffect = CharacterAbilities[(int)Abilities.Constitution].GetValue() - GetArmor().Weight;

        //Exacerbate or reduce dodge modifier accordingly
        armorWeightEffect *= GetArmor().DodgeModifier;

        //Calculate REF effectiveness in this armor
        float dodgeChance = CharacterAbilities[(int)Abilities.Reflexes].GetValue() + (1 * armorWeightEffect);

        //Combine with raw possibility of missing
        hitChance -= dodgeChance;

        for(int i = 0; i < CombatManager.MAX_DODGE_ATTEMPTS; i++)
        {
            if (Random.Range(0, 1) > hitChance)
            {
                return true;
            }
            else if (Random.Range(0, 1) < CharacterAbilities[(int)Abilities.Reflexes].GetValue() + CharacterAbilities[(int)Abilities.Luck].GetValue())
            {
                continue;
            }
            else
            {
                break;
            }
        }
        return false;
    }
    public void CalculateDamageRecieved(int incomingDamage)
    {
        //Calculate base resistance from deflection
        int damageResistance = GetArmor().Deflection + RetrieveModifiersForStat("Deflection");

        //Figure out what the actual damage is
        int appliedDamage = incomingDamage - damageResistance;

        //And reduce that by the % dampened
        appliedDamage -= (int)Mathf.Floor((appliedDamage * (GetArmor().Dampening) + RetrieveModifiersForStat("Dampening")));

        //Cannot deal 0 damage
        if(appliedDamage <= 1)
        {
            appliedDamage = 1;
        }

        ChangeHealth(-appliedDamage);

        Debug.Log(Name + " has received " + appliedDamage + " damage.");        
    }
    public void ChangeHealth(int amount)
    {
        if(CurrentHealth + amount >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else if (CurrentHealth + amount <= 0)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth += amount;
        }
    }
    #endregion
    #region ModifiersHandlers
    public void ApplyModifier(Modifier newModifier)
    {
        if(ModifiersMap.ContainsKey(newModifier.GetName()))
        {
            ModifiersMap[newModifier.GetName()].Add(newModifier);
        }
        else
        {
            ModifiersMap.Add(newModifier.GetName(), new List<Modifier>() { newModifier });
        }
    }
    private int RetrieveModifiersForStat(string statName)
    {
        if(ModifiersMap.ContainsKey(statName))
        {
            int cumulativeValue = 0;
            foreach(Modifier modifier in ModifiersMap[statName])
            {
                cumulativeValue += modifier.GetValue();
            }
            return cumulativeValue;
        }
        else
        {
            return 0;
        }
    }
    #endregion
    #region Initializers
    private void InitializeHealth()
    {
        MaxHealth = CharacterGenerator.MINIMUM_HEALTH + (CharacterAbilities[(int)Abilities.Constitution].GetValue() * 2) + (Age / 10) + RetrieveModifiersForStat("MaxHealth");
        CurrentHealth = MaxHealth;
    }
    private void InitializeGraphics()
    {
        CharacterRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        CharacterRenderer.sprite = SpriteBuilder.BuildSlimeSprite()[0];

        CharacterAnimator = gameObject.GetOrAddComponent<Animator>();
    }
    private void InitializeEquipment()
    {
        CharacterInventory = gameObject.GetOrAddComponent<Inventory>();
        CharacterInventory.SetOwner(this);

        CharacterInventory[InventorySlots.Weapon] = ItemManager.GenerateItem<Weapon>("Old Bow");
        CharacterInventory[InventorySlots.Armor] = ItemManager.GenerateItem<Armor>("Clothes");
    }

    #endregion
    #region GrowthFunctions
    public void IncreaseAbilityPoints(int pointsGained)
    {
        while (pointsGained > 0)
        {
            for (int gainCutoff = 1; ; gainCutoff--)
            {
                int abilityToCheck = Random.Range(0, CharacterAbilities.Count);
                if (CharacterAbilities[abilityToCheck].GetWeight() >= gainCutoff)
                {
                    CharacterAbilities[abilityToCheck].IncreaseValue(1);
                    break;
                }
            }
            pointsGained--;
        }        
    }
    public void ModifyMaxHealth(int modificationAmount)
    {
        MaxHealth += modificationAmount;
    }
    public Skill GainSkill(Skill skill)
    {
        for(int i = 0; i < SkillSlots; i++)
        {
            if(CharacterSkills[i].GetName() == "None")
            {
                CharacterSkills[i] = skill;
                return skill;
            }
        }
        return null;
    }
    public void GainSkillSlots(int gainedSlots)
    {
        if(CharacterSkills == null)
        {
            CharacterSkills = new List<Skill>();
        }

        SkillSlots += gainedSlots;
        for (int i = 0; i < gainedSlots; i++)
        {
            CharacterSkills.Add(CharacterGenerator.GenerateSkill("None"));
        }
    }
    #endregion
    #region Getters
    public Weapon GetWeapon()
    {
        return (Weapon)CharacterInventory[InventorySlots.Weapon];
    }
    public Armor GetArmor()
    {
        return (Armor)CharacterInventory[InventorySlots.Armor];
    }
    public float GetAbilityValue(Abilities ability)
    {
        return CharacterAbilities[(int)ability].GetValue();
    }
    public bool canMoveToHex(Hex hex)
    {
        return Pathfinder.Contains(hex);
    }
    public int GetMovementCostToHex(Hex hex)
    {
        return Pathfinder.GetCostOfHex(hex);
    }
    #endregion
    #region ObjectOverrides
    public override string ToString()
    {
        string output = null;
        output = "Name: " + Name + "\n";
        output += "Age: " + Age + "\n";
        output += "Birthdate: " + Birthday + "\n";
        output += "Profession " + CharacterProfession + "\n";
        foreach (Trait trait in CharacterTraits)
        {
            output += trait.GetName() + ": " + trait.GetTier() + " at " + trait.GetValue() + "\n";
        }
        foreach (Ability ability in CharacterAbilities)
        {
            output += ability.GetName() + ": " + ability.GetValue() + "\n";
        }
        output += "Skills: ";
        foreach (Skill skill in CharacterSkills)
        {
            output += skill + " ";
        }
        output += CharacterQuirk + " | " + CharacterFlaw;
        return output;
    }
    public string ToString(bool verbose)
    {
        if (verbose)
        {
            string output = null;
            output = "Name: " + Name + "\n";
            output += "Age: " + Age + "\n";
            output += "Birthdate: " + Birthday.ToString(true) + " under " + PatronConstellation.TranslateConstellation() + "\n";
            output += CharacterProfession.ToString(true) + "\n";
            foreach (Trait trait in CharacterTraits)
            {
                output += trait.GetName() + ": " + trait.GetTier() + " at " + trait.GetValue() + "\n";
            }
            foreach (Ability ability in CharacterAbilities)
            {
                output += ability.GetName() + ": " + ability.GetValue() + "\n";
            }
            output += string.Format("Skills[{0}]: ", SkillSlots);
            foreach (Skill skill in CharacterSkills)
            {
                output += skill + ": " + skill.GetDescription() + "   ";
            }
            output += string.Format("Idiosyncracy Severity: {0} \n Quirk: {1} : {2} | Flaw: {3} : {4}", CharacterFlaw.GetSeverity(), CharacterQuirk, CharacterQuirk.GetDescription(), CharacterFlaw, CharacterFlaw.GetDescription());
            return output;
        }
        else
        {
            return ToString();
        }
    }
    #endregion  
}   