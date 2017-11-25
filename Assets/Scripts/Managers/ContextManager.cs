using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContextManager
{
    public delegate void ContextChangedHandler(Context oldContext, Context newContext);
    public static event ContextChangedHandler ContextChanged;

    public static void AddContext(Context newcontext)
    {
        if (GetCurrentContext() != Context.Locked || LockingObject == null)
        {
            if (activeContexts.Count > 0)
            {
                Debug.Assert(activeContexts.Peek() != newcontext, "Error!  Context " + newcontext + " has been set multiple times!");
            }

            Context oldContext = GetCurrentContext();
            activeContexts.Push(newcontext);

            if (ContextChanged != null)
            {
                ContextChanged(oldContext, newcontext);
            }
        }
        else
        {
            Debug.LogWarning("Warning!  Context is locked, but someone tried to change it!");
        }
    }
    public static void EndContext()
    {
        if (GetCurrentContext() != Context.Locked || LockingObject == null)
        {
            Context oldContext = GetCurrentContext();
            activeContexts.Pop();
            Context newContext = GetCurrentContext();

            if (ContextChanged != null)
            {
                ContextChanged(oldContext, newContext);
            }
        }
        else
        {
            Debug.LogWarning("Warning!  Context is locked, but someone tried to change it!");
        }
    }
    public static void LockContext(object lockingObject)
    {
        AddContext(Context.Locked);
        LockingObject = lockingObject;
    }
    public static void UnlockContext(object unlockingObject)
    {
        if(GetCurrentContext() == Context.Locked)
        {
            if (unlockingObject == LockingObject)
            {
                LockingObject = null;
                EndContext();
            }
            else
            {
                Debug.LogError("Error! " + unlockingObject + " tried to unlock context, but failed!");
            }
        }
        else
        {
            Debug.Log("Note: " + unlockingObject + " has tried to unlock context, but it was not locked.");
        }
            
    }
    public static Context GetCurrentContext()
    {
        if(activeContexts.Count > 0)
        {
            return activeContexts.Peek();
        }
        else
        {
            return Context.None;
        }
    }
    public static Stack<Context> GetActiveContexts()
    {
        if(activeContexts.Count > 0)
        {
            return activeContexts;
        }
        else
        {
            return null;
        }
    }

    static object LockingObject = null;
    static Stack<Context> activeContexts = new Stack<Context>();
}

public enum Context
{
    None,
    Locked,
    Combat,
    Manuever,
    Target
}