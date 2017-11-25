using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardousNode : NavigableNode
{
    public int MoveToDamage { get; set; }
    public bool isInnatelyHazardous;

    public HazardousNode(Hex ownedHex, Node source, int movementCost, int movementDamage = 0) : base(ownedHex, source, movementCost)
    {
        MoveToDamage = movementDamage;

        HazardousHex ownedHazardousHex = ownedHex as HazardousHex;

        if (ownedHazardousHex != null)
        {
            MoveToDamage += ownedHazardousHex.GetHazardDamage();
            isInnatelyHazardous = true;
        }
        else
        {
            isInnatelyHazardous = false;
        }

        VisualizationType = VisualizationTypes.Hazardous;
    }

    #region Getters
    public bool GetInnatelyHazardous()
    {
        return isInnatelyHazardous;
    }
    #endregion
}
