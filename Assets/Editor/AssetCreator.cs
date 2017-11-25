using UnityEditor;
using UnityEngine;

public class AssetCreator {

    [MenuItem("Assets/Create/Item")]
    public static void CreateItem()
    {
        ScriptableObjectUtility.CreateAsset<Item>();
    }
    [MenuItem("Assets/Create/Weapon")]
    public static void CreateWeapon()
    {
        ScriptableObjectUtility.CreateAsset<Weapon>();
    }
    [MenuItem("Assets/Create/Armor")]
    public static void CreateArmor()
    {
        ScriptableObjectUtility.CreateAsset<Armor>();
    }
}