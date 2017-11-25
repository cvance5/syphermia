using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitiativeOrder
{
    public List<CharacterData> Characters;
    public List<int> Initiatives;
    private Dictionary<CharacterData, int> InitiativeModifiers = new Dictionary<CharacterData, int>();
    public int Count
    {
        get
        {
            return Characters.Count;
        }
    }

    public InitiativeOrder()
    {
        Characters = new List<CharacterData>();
        Initiatives = new List<int>();
    }
    public void Advance()
    {
        for (int i = 0; i < Count; i++)
        {
            int initiativeGrowth = 1;

            if (InitiativeModifiers.ContainsKey(Characters[i]))
            {
                initiativeGrowth += InitiativeModifiers[Characters[i]];
            }
            Initiatives[i] += initiativeGrowth;

            Bubble(i);
        }
    }
    #region CollectionTools
    public void Clear()
    {
        Characters.Clear();
        Initiatives.Clear();

        ValidateDataCount();
    }
    public void Add(CharacterData newCharacter, int newInitiative)
    {
        Characters.Add(newCharacter);
        Initiatives.Add(newInitiative);

        ValidateDataCount();
    }
    public void Insert(int index, CharacterData newCharacter, int newInitiative)
    {
        Characters.Insert(index, newCharacter);
        Initiatives.Insert(index, newInitiative);

        ValidateDataCount();
    }
    public void Swap(int index1, int index2)
    {
        CharacterData tempChar = Characters[index1];
        int tempInit = Initiatives[index1];

        Characters[index1] = Characters[index2];
        Initiatives[index1] = Initiatives[index2];

        Characters[index2] = tempChar;
        Initiatives[index2] = tempInit;

        ValidateDataCount();
        ValidateUniqueData();
    }
    public void Remove(CharacterData removedCharacter)
    {
        Remove(Characters.IndexOf(removedCharacter));
    }
    public void Remove(int index)
    {
        if(InitiativeModifiers.ContainsKey(Characters[index]))
        {
            InitiativeModifiers.Remove(Characters[index]);
        }

        Characters.RemoveAt(index);
        Initiatives.RemoveAt(index);

        ValidateDataCount();
        ValidateUniqueData();
    }
    public CharacterData Pop()
    {
        CharacterData nextCharacter = Characters[0];

        Characters.Add(Characters[0]);
        Initiatives.Add(Initiatives[0] - TurnManager.InitativeCost);

        Characters.RemoveAt(0);
        Initiatives.RemoveAt(0);

        ValidateDataCount();

        return nextCharacter;
    }
    public CharacterData PeekCharacter()
    {
        return Characters[0];
    }
    public int PeekInitiative()
    {
        return Initiatives[0];
    }
    public void Bubble(int index)
    {
        while(index - 1 >= 0 && Initiatives[index] > Initiatives[index - 1])
        {
            Swap(index, index - 1);
            index--;
        }

        ValidateDataCount();
        ValidateUniqueData();
    }
    public void Bubble(CharacterData character)
    {
        Bubble(Characters.IndexOf(character));
    }
    #endregion
    #region Validation
    private void ValidateDataCount()
    {
        Debug.Assert(Characters.Count == Initiatives.Count, "Error!  Desynchronized initiatives in initiative order!");
    }
    private void ValidateUniqueData()
    {
        Debug.Assert(Characters.Distinct().Count() == Characters.Count, "Error!  Duplicate characters in initiative order!");
    }
    #endregion
    #region ObjectOverrides
    public int this[int index]
    {
        get
        {
            return Initiatives[index];
        }
        set
        {
            Initiatives[index] = value;
        }
    }
    public int this[CharacterData characterKey]
    {
        get
        {
            int index = Characters.IndexOf(characterKey);
            return Initiatives[index];
        }
        set
        {
            int index = Characters.IndexOf(characterKey);
            Initiatives[index] = value;
        }
    }
    #endregion
}
