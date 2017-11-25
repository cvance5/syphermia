using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmyData : MonoBehaviour {

    public int ArmySize
    {
        get
        {
            return CharactersInArmy.Count;
        }
    }
    public List<CharacterData> CharactersInArmy { get; private set; }

    [SerializeField]
    private string OwnerID;

    public void Initialize(string ownerID)
    {
        OwnerID = ownerID;
        gameObject.name = OwnerID + "'s Army";
        CharactersInArmy = new List<CharacterData>();
    }

    public void AddCharacterToArmy(CharacterData character)
    {
        if(!CharactersInArmy.Contains(character))
        {
            CharactersInArmy.Add(character);
            character.JoinArmy(this);
        }
    }
    public void RemoveCharacterFromyArmy(CharacterData character)
    {
        if(CharactersInArmy.Contains(character))
        {
            CharactersInArmy.Remove(character);
            character.LeaveArmy();
        }
    }
}