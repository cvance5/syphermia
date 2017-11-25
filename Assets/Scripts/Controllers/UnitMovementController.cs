using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovementController : Controller {
    public override bool HandleHoverOver(GameObject selectedObject)
    {
        bool wasConsumed = false;

        Hex selectedHex = selectedObject.GetComponent<Hex>();

        if (selectedHex != null)
        {
            if (BattleManager.controlledCharacter.canMoveToHex(selectedHex))
            {
                BattleManager.controlledCharacter.Pathfind(selectedHex);
            }
        }

        return wasConsumed;
    }
    public override bool HandleMouseDrag(GameObject selectedObject, Vector3 dragVector)
    {
        return false;
    }
    public override bool HandleSelection(GameObject selectedObject)
    {
        bool wasConsumed = false;

        Hex selectedHex = selectedObject.GetComponent<Hex>();

        if (selectedHex != null)
       {
            if (BattleManager.controlledCharacter.canMoveToHex(selectedHex))
            {
                ContextManager.LockContext(this);
                BattleManager.ConsumeAP(BattleManager.controlledCharacter.GetMovementCostToHex(selectedHex));
                BoardManager.MoveToHex(BattleManager.controlledCharacter, selectedHex);
                wasConsumed = true;
                ContextManager.UnlockContext(this);
            }
        }

        return wasConsumed;
    }
    public override bool HandleKeyPress(string key)
    {
        bool wasConsumed = false;
        if(key == "Advance")
        {
            ContextManager.LockContext(this);
            BattleManager.ConsumeAP(BattleManager.RemainingAP);
            ContextManager.UnlockContext(this);
            wasConsumed = true;
        }
        else if (key == "Debug 1")
        {            
            ContextManager.AddContext(Context.Target);
            wasConsumed = true;
        }
        return wasConsumed;
    }
    protected override Context[] InitiatlizeValidContexts()
    {
        return new Context[] { Context.Manuever };
    }
}