using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BattleManager : Singleton<BattleManager> {

    public static CharacterData controlledCharacter;
    public static int RemainingAP;
    void Start()
    {
        ContextManager.ContextChanged += HandleContextChangedEvent;

        GameObject army1 = new GameObject();
        army1.AddComponent<ArmyData>();
        army1.GetComponent<ArmyData>().Initialize("Player");
        army1.GetComponent<ArmyData>().AddCharacterToArmy(CharacterGenerator.GenerateCharacter().GetComponent<CharacterData>());

        GameObject army2 = new GameObject();
        army2.AddComponent<ArmyData>();
        army2.GetComponent<ArmyData>().Initialize("Enemy");
        army2.GetComponent<ArmyData>().AddCharacterToArmy(CharacterGenerator.GenerateCharacter().GetComponent<CharacterData>());

        army1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        army2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        BoardManager.MoveToHex(army1.GetComponentInChildren<CharacterData>(), BoardManager.FindSummit(BoardManager.GetHex(0,0,0)));
        BoardManager.MoveToHex(army2.GetComponentInChildren<CharacterData>(), BoardManager.FindSummit(BoardManager.GetHex(0,0,2)));

        TurnManager.GenerateInitiativeOrder(new List<ArmyData>() { army1.GetComponent<ArmyData>(), army2.GetComponent<ArmyData>() });

        BeginBattle();
    }
    void Update()
    {
        if(controlledCharacter == null)
        {
            TurnManager.TickTurns();
        }
    }
    public static void ControlCharacter(CharacterData character)
    {
        if(controlledCharacter == null)
        {
            controlledCharacter = character;
            RemainingAP = controlledCharacter.ActionPoints;
            ContextManager.AddContext(Context.Manuever);
        }
        else
        {
            Debug.LogWarning("Warning!  A controlled character was unsafely overwritten!  Was this intended?");
        }
    }
    public static void BeginBattle()
    {
        ContextManager.AddContext(Context.Combat);
        TurnManager.BeginBattle();
    }
    public static void EndBattle()
    {
        TurnManager.EndBattle();
    }
    public static bool ConsumeAP(int amount)
    {
        bool wasSuccesful = false;

        if(RemainingAP - amount >= 0)
        {
            RemainingAP -= amount;
            wasSuccesful = true;
        }
        else
        {
            RemainingAP = 0;
            wasSuccesful = false;
            Debug.LogWarning("More AP consumed than was remaining by " + controlledCharacter + "!");
        }

        return wasSuccesful;
    }
    public static void HandleContextChangedEvent(Context oldContext, Context newContext)
    {
        if(newContext == Context.Manuever)
        {
            if(RemainingAP == 0)
            {
                ReleaseCharacter();
            }
        }

        if (oldContext == Context.Manuever)
        {
            if(controlledCharacter != null)
            {
                controlledCharacter.HidePath();
            }
        }

        if(controlledCharacter != null)
        {
            controlledCharacter.UpdateOptions();            
        }
    }
    private static void ReleaseCharacter()
    {
        controlledCharacter.ClearOptions();
        controlledCharacter = null;
        ContextManager.EndContext();
    }
}