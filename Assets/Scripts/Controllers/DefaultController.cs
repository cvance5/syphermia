using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultController : Controller
{
    public override bool HandleHoverOver(GameObject selectedObject)
    {
        return true;
    }
    public override bool HandleSelection(GameObject gameObject)
    {
        return true;
    }
    public override bool HandleMouseDrag(GameObject selectedObject, Vector3 dragVector)
    {
        return true;
    }
    public override bool HandleKeyPress(string key)
    {
        return true;
    }
    protected override Context[] InitiatlizeValidContexts()
    {
        return new Context[] { Context.None, Context.Locked };
    }
}
