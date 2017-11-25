using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item : ScriptableObject {
    [Header("Item Attributes")]
    public string Name;
    public int Value;
    public ItemRarity Rarity;
    public int Weight;
    public ItemTypes Type;
    public List<InventorySlots> ValidSlots;
    public int MaxUses = 1;

    protected CharacterData Owner;

    protected virtual void OnValidate()
    {
        if(ValidSlots == null)
        {
            ValidSlots = new List<InventorySlots>() { InventorySlots.Any };
        }
    }

    public bool CheckValidSlot(InventorySlots slot)
    {
        if(slot == InventorySlots.Any)
        {
            return true;
        }

        foreach(InventorySlots validSlot in ValidSlots)
        {
            if(validSlot == slot)
            {
                return true;
            }
        }

        return false;
    }
    public void SetOwner(CharacterData character)
    {
        Owner = character;
        OnOwnerChanged();
    }
    public virtual void OnOwnerChanged() { }
    #region ObjectOverrides
    public override bool Equals(object other)
    {
        if(other is Item)
        {
            Item otherItem = (Item)other;
            return Name == otherItem.Name;
        }
        else
        {
            return false;
        }
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        return Name;
    }
    #endregion

    protected int UsesRemaining;
}
public enum ItemTypes
{
    Weapon,
    Armor,
    Trinket,
    Consumable
}

public enum ItemRarity
{
    Improvised,
    Common,
    Professional,
    Specialized,
    Rare,
    Unique,
    Legendary
}