using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GameObjectExtensions
{
    /// <summary>
    /// Gets or add a component. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T result = gameObject.GetComponent<T>();
        if (result == null)
        {
            result = gameObject.gameObject.AddComponent<T>();
        }
        return result;
    }
}
