using System;
using System.Collections.Generic;
using UnityEngine;

public class RangefinderMap : NodeMap
{
    public RangefinderMap(Weapon weapon, CharacterData owner)
    {
        Owner = owner;
        associatedWeapon = weapon;
    }
    public override void CreateMap(Hex origin)
    {
        EndVisualization();
        MapNodes.Clear();

        List<Hex> hexesToCheck = new List<Hex>();
        hexesToCheck.AddRange(BoardManager.GetNeighborsOfHex(origin));

        int counter = 0;

        while (counter < hexesToCheck.Count)
        {
            Hex testingHex = hexesToCheck[counter];

            if(testingHex != null)
            {
                int distance = BoardManager.CalculateHexDistance(origin, testingHex);

                if (distance < associatedWeapon.WeaponRange)
                {
                    foreach (Hex hex in BoardManager.GetNeighborsOfHex(testingHex))
                    {
                        if (!hexesToCheck.Contains(hex) && !Contains((hex)))
                        {
                            hexesToCheck.Add(hex);
                        }
                    }
                }
                if (associatedWeapon.WeaponRange.IsInRange(distance))
                {
                    MapNodes.Add(GenerateNode(null, testingHex));
                }
            }
            counter++;
        }
    }
    public void TargetNode(Node node)
    {
        if(node != MarkedNode)
        {
            if (MarkedNode != null)
            {
                MarkedNode.RemoveHighlight();
                MarkedNode = null;
            }
            if (Contains(node))
            {
                MarkedNode = node as TargetableNode;
                MarkedNode.Highlight();
            }
        }
    }
    public void TargetNode(Hex hex)
    {
        TargetNode(GetNodeForHex(hex));
    }

    protected override Node GenerateNode(Node sourceNode, Hex generationHex)
    {
        return new TargetableNode(generationHex, null, 0);
    }

    private Weapon associatedWeapon;
    private TargetableNode MarkedNode = null;

    private static int count = 1;
}
