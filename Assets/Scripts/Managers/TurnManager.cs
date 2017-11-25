using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class TurnManager {
    public static int InitativeCost = 100;
    public static void GenerateInitiativeOrder(List<ArmyData> armiesInPlay)
    {
        Initiatives.Clear();

        foreach(ArmyData army in armiesInPlay)
        {
            foreach(CharacterData character in army.CharactersInArmy)
            {
                int initative = character.RollInitiative();

                if(Initiatives.Characters.Count == 0)
                {
                    Initiatives.Add(character, initative);
                }
                else
                {
                    bool wasPlaced = false;
                    for (int i = 0; i < Initiatives.Characters.Count; i++)
                    {
                        if (Initiatives[i] < initative)
                        {
                            Initiatives.Insert(i, character, initative);
                            wasPlaced = true;
                            break;
                        }
                    }
                    if(wasPlaced == false)
                    {
                        Initiatives.Add(character, initative);
                    }
                }
            }
        }
    }
    public static InitiativeOrder GetInitiativeOrder()
    {
        return Initiatives;
    }
    public static void TickTurns()
    {
        Debug.Assert(Initiatives.Count > 0, "Error!  Battle initiated with 0 armies and troops!");
        {
            if (Initiatives.PeekInitiative() < 100)
            {
                Initiatives.Advance();
            }
            else
            {
                CharacterData currentCharacter = Initiatives.Pop();
                BattleManager.ControlCharacter(currentCharacter);
            }
        }
    }
    public static void RemoveCharacter(CharacterData removedCharacter)
    {
        Initiatives.Remove(removedCharacter);
    }
    public static void BeginBattle()
    {
        if (!isBattling)
        {
            isBattling = true;
        }
        else
        {
            Debug.LogError("Begin Battle called inside of battle!");
        }
    }
    public static void EndBattle()
    {
        if (isBattling)
        {
            isBattling = false;
        }
        else
        {
            Debug.LogError("End Battle called outside of battle!");
        }
    }

    private static bool isBattling = false; 
    private static InitiativeOrder Initiatives = new InitiativeOrder();
}