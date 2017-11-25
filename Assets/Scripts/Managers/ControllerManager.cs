using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : Singleton<ControllerManager>
{
    void Start()
    {
        ContextManager.ContextChanged += HandleContextChangedEvent;
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                DistributeSelection(hit.collider.gameObject);
            }
            else if (Input.mousePosition != lastMousePosition)
            {
                DistributeHoverOver(hit.collider.gameObject);
            }

            if (Input.GetMouseButtonDown(1))
            {
                rightClickSelection = hit.collider.gameObject;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                rightClickSelection = null;
            }
        }
        if (Input.GetMouseButton(1))
        {
            DistributeMouseDrag(rightClickSelection, Input.mousePosition - lastMousePosition);
        }

        foreach(string key in virtualKeys)
        {
            if(Input.GetButtonDown(key))
            {
                DistributeKeyPress(key);
            }
        }

        lastMousePosition = Input.mousePosition;
    }
    private void DistributeSelection(GameObject gameObject)
    {
        bool wasConsumed = false;

        foreach (Context context in ContextManager.GetActiveContexts())
        {
            if (controllerMap.ContainsKey(context))
            {
                foreach (Controller controller in controllerMap[context])
                {
                    if (controller.HandleSelection(gameObject))
                    {
                        wasConsumed = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            if(wasConsumed)
            {
                break;
            }
        }
    }
    private void DistributeHoverOver(GameObject gameObject)
    {
        bool wasConsumed = false;

        foreach (Context context in ContextManager.GetActiveContexts())
        {
            if (controllerMap.ContainsKey(context))
            {
                foreach (Controller controller in controllerMap[context])
                {
                    if (controller.HandleHoverOver(gameObject))
                    {
                        wasConsumed = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            if (wasConsumed)
            {
                break;
            }
        }
    }
    private void DistributeMouseDrag(GameObject selectedObject, Vector3 dragVector)
    {
        bool wasConsumed = false;

        foreach (Context context in ContextManager.GetActiveContexts())
        {
            if (controllerMap.ContainsKey(context))
            {
                foreach (Controller controller in controllerMap[context])
                {
                    if (controller.HandleMouseDrag(selectedObject, dragVector))
                    {
                        wasConsumed = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            if (wasConsumed)
            {
                break;
            }
        }
    }
    private void DistributeKeyPress(string key)
    {
        bool wasConsumed = false;

        foreach (Context context in ContextManager.GetActiveContexts())
        {
            if (controllerMap.ContainsKey(context))
            {
                foreach (Controller controller in controllerMap[context])
                {
                    if (controller.HandleKeyPress(key))
                    {
                        wasConsumed = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            if (wasConsumed)
            {
                break;
            }
        }
    }
    public static void RegisterController(Controller controller)
    {
        foreach(Context validContext in controller.GetValidContexts())
        {
            if(controllerMap.ContainsKey(validContext))
            {
                Debug.Assert(!controllerMap[validContext].Contains(controller), "Error!  Controller " + controller + " has been registered multiple times!");
                controllerMap[validContext].Add(controller);
            }
            else
            {
                controllerMap.Add(validContext, new List<Controller>() { controller });
            }
        }
    }
    public static void HandleContextChangedEvent(Context oldContext, Context newContext)
    {
        print(oldContext + " has become " + newContext);
    }

    static string[] virtualKeys = new string[]
    {
        "Advance",
        "Debug 1"
    };

    static GameObject rightClickSelection = null;
    static Vector3 lastMousePosition = new Vector3();
    static Dictionary<Context, List<Controller>> controllerMap = new Dictionary<Context, List<Controller>>();

    public const int MOUSE_DEADZONE = 60;
}