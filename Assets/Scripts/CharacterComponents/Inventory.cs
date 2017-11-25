using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<InventorySlots, Item> Slots = new Dictionary<InventorySlots, Item>()
    {
        {InventorySlots.Weapon, null},
        {InventorySlots.Offhand, null},
        {InventorySlots.Armor, null},
        {InventorySlots.Trinket, null},
        {InventorySlots.Pocket, null},
        {InventorySlots.Pocket2, null},
        {InventorySlots.Pocket3, null}
    };
    public void SetOwner(CharacterData owner)
    {
        if(Owner == null)
        {
            Owner = owner;
        }
        else
        {
            Debug.LogError("Error!  An Inventory has been assigned multiple owners!");
        }
    }

    public bool EquipItem(Item item, InventorySlots slot = InventorySlots.Any, bool replaceItem = false)
    {
        bool wasSlotted;

        if (!item.CheckValidSlot(slot))
        {
            wasSlotted = false;
        }
        else if (slot == InventorySlots.Any)
        {
            foreach (InventorySlots inventorySlot in Slots.Keys)
            {
                if (item.CheckValidSlot(slot) && (replaceItem || Slots[slot] == null))
                {
                    Slots[slot] = item;
                    wasSlotted = true;
                }
            }
            wasSlotted = false;
        }
        else if (slot == InventorySlots.Pocket)
        {
            InventorySlots firstEmpty = InventorySlots.Any;

            if (Slots[InventorySlots.Pocket] == null)
            {
                firstEmpty = InventorySlots.Pocket;
            }
            else if (Slots[InventorySlots.Pocket2] == null)
            {
                firstEmpty = InventorySlots.Pocket2;
            }
            else if (Slots[InventorySlots.Pocket3] == null)
            {
                firstEmpty = InventorySlots.Pocket3;
            }

            if (firstEmpty == InventorySlots.Any)
            {
                if (replaceItem)
                {
                    Slots[InventorySlots.Pocket] = item;
                    wasSlotted = true;
                }
                else
                {
                    wasSlotted = false;
                }
            }
            else
            {
                Slots[firstEmpty] = item;
                wasSlotted = true;
            }
        }
        else
        {
            if (Slots[slot] == null || replaceItem)
            {
                Slots[slot] = item;
                wasSlotted = true;
            }
            else
            {
                wasSlotted = false;
            }
        }

        if(wasSlotted)
        {
            item.SetOwner(Owner);
        }

        return wasSlotted;
    }
    public bool UnequipItem(Item item)
    {
        foreach (InventorySlots possibleSlot in item.ValidSlots)
        {
            if (Slots[possibleSlot] == item)
            {
                EmptySlot(possibleSlot);          
                return true;
            }
        }

        return false;
    }
    public bool UnequipItem(InventorySlots slot)
    {
        bool foundItem = false;

        if (slot == InventorySlots.Any)
        {
            foreach (InventorySlots selectedSlot in Slots.Keys)
            {
                if (Slots[selectedSlot] != null)
                {
                    EmptySlot(selectedSlot);
                    foundItem = true;
                }
            }          
        }
        else if (Slots[slot] != null)
        {
            EmptySlot(slot);
            foundItem = true;
        }

        return foundItem;
    }
    private void EmptySlot(InventorySlots slot)
    {
        Slots[slot] = null;
    }
    #region Getters
    public Item GetItemInSlot(InventorySlots slot)
    {
        return Slots[slot];
    }
    public int GetCombinedWeight()
    {
        int combinedWeight = 0;

        foreach(InventorySlots slot in Slots.Keys)
        {
            if(Slots[slot] != null)
            {
                combinedWeight += Slots[slot].Weight;
            }
        }

        return combinedWeight;
    }
    #endregion
    #region ObjectOverrides
    public Item this[InventorySlots slot]
    {
        get
        {
            return Slots[slot];
        }
        set
        {
            if(value is Item)
            {
                Item valueItem = value; 
                foreach(InventorySlots validSlot in valueItem.ValidSlots)
                {
                    if(EquipItem(valueItem, slot))
                    {
                        return;
                    }
                }
                EquipItem(valueItem, valueItem.ValidSlots[0], true);            }

        }
    }
    #endregion

    private CharacterData Owner;
}

public enum InventorySlots
{
    Weapon,
    Offhand,
    Armor,
    Trinket,
    Pocket,
    Pocket2,
    Pocket3,
    Any
}