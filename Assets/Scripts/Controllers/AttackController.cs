using System;
using UnityEngine;

public class AttackController : Controller
{
    public override bool HandleHoverOver(GameObject selectedObject)
    {
        bool wasConsumed = false;

        Hex selectedHex = selectedObject.GetComponent<Hex>();

        if (selectedHex != null)
        {
            BattleManager.controlledCharacter.GetWeapon().Rangefinder.TargetNode(selectedHex);
            wasConsumed = true;
        }

        return wasConsumed;
    }
    public override bool HandleMouseDrag(GameObject selectedObject, Vector3 dragVector)
    {
        return false;
    }
    public override bool HandleSelection(GameObject gameObject)
    {
        bool wasConsumed = false;

        Hex selectedHex = gameObject.GetComponent<Hex>();

        if(selectedHex != null)
        {
            CharacterData selectedOpponent = selectedHex.GetOccupier();
            if (selectedOpponent != null && BattleManager.controlledCharacter.ValidateCombat(selectedOpponent, selectedHex))
            {
                ContextManager.LockContext(this);
                BattleManager.ConsumeAP(BattleManager.controlledCharacter.GetWeapon().APCost);
                CombatManager.SimulateCombat(BattleManager.controlledCharacter, selectedOpponent);
                wasConsumed = true;
                ContextManager.UnlockContext(this);
            }   
            else
            {
                wasConsumed = true;
            }

            ContextManager.EndContext();
        }
        return wasConsumed;
    }
    public override bool HandleKeyPress(string key)
    {
        bool wasConsumed = false;

        if(key == "Advance")
        {
            ContextManager.EndContext();
            wasConsumed = true;
        }
        else if(key == "Debug 1")
        {
            ContextManager.EndContext();
            wasConsumed = true;
        }

        return wasConsumed;
    }
    protected override Context[] InitiatlizeValidContexts()
    {
        return new Context[] { Context.Target };
    }
}