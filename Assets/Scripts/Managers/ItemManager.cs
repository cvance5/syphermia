using UnityEngine;

public static class ItemManager
{
    public static T GenerateItem<T>(string name) where T : Item
    {
        var source = Resources.Load("Items/" + name) as T;

        if(source == null)
        {
            Debug.LogError("Error!  Failed to load item by name: " + name);
        }
        var newItem = Object.Instantiate(source) as T;

        return newItem;
    }
}
