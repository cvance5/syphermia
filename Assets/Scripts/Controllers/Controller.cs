using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Error!  Too many " + GetType().ToString() + " in play!");
        }
    }
    protected void OnEnable()
    {
        validContexts = InitiatlizeValidContexts();
        ControllerManager.RegisterController(this);
    }
    public abstract bool HandleSelection(GameObject selectedObject);
    public abstract bool HandleHoverOver(GameObject selectedObject);
    public abstract bool HandleMouseDrag(GameObject selectedObject, Vector3 dragVector);
    public abstract bool HandleKeyPress(string key);
    protected abstract Context[] InitiatlizeValidContexts();
    public Context[] GetValidContexts()
    {
        return validContexts;
    }

    Context[] validContexts;

    protected Controller instance;
}