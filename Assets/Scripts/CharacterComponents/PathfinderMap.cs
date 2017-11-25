using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderMap : NodeMap
{
    public PathfinderMap(CharacterData owner)
    {
        Owner = owner;
    }
    public void ShowPath(Hex hex)
    {
        NavigableNode markingNode = GetNodeForHex(hex) as NavigableNode;

        if (markingNode != null && MarkedNode != markingNode)
        {
            if (MarkedNode != null)
            {
                RemovePath();
            }

            MarkedNode = markingNode;

            while (markingNode.Source != null)
            {
                markingNode.Highlight();
                markingNode = markingNode.Source as NavigableNode;
            }
        }
    }
    public void RemovePath()
    {
        while (MarkedNode.Source != null)
        {
            MarkedNode.RemoveHighlight();
            MarkedNode = MarkedNode.Source as NavigableNode;
        }
        MarkedNode = null;
    }
    public void ClearPath()
    {
        MapNodes.Clear();
    }
    public override void EndVisualization()
    {
        base.EndVisualization();
        if (MarkedNode != null)
        {
            RemovePath();
        }
    }
    public override void CreateMap(Hex origin)
    {
        EndVisualization();
        MapNodes.Clear();
        MapNodes.Add(new NavigableNode(origin, null, 0));

        int nodesExplored = 0;

        if (BattleManager.controlledCharacter == Owner)
        {
            AP = BattleManager.RemainingAP;
        }
        else
        {
            AP = Owner.ActionPoints;
        }

        while (nodesExplored < MapNodes.Count)
        {
            ExploreNode(MapNodes[nodesExplored]);
            nodesExplored++;
        }
    }
    private void ExploreNode(Node currentNode)
    {
        if (currentNode.CumulativeCost < AP)
        {
            foreach (Hex neighbor in BoardManager.GetNeighborsOfHex(currentNode.OwnedHex))
            {
                if (neighbor != null)
                {
                    if (currentNode.Source != neighbor && !neighbor.isOccupied())
                    {
                        Node neighborNode = MapNodes.Find(node => node == neighbor);

                        if (neighborNode == null)
                        {
                            Node NodeToAdd = GenerateNode(currentNode, neighbor);
                            if (NodeToAdd != null)
                            {
                                MapNodes.Add(NodeToAdd);
                            }
                        }
                        else
                        {
                            currentNode.Source = CompareSourceNodes(currentNode, currentNode.Source, neighborNode);
                            neighborNode.Source = CompareSourceNodes(neighborNode, neighborNode.Source, currentNode);
                        }
                    }
                }
            }
        }
    }
    protected override Node GenerateNode(Node sourceNode, Hex generationHex)
    {
        Node NodeToReturn = null;

        int heightDifference = generationHex.GetHeight() - sourceNode.OwnedHex.GetHeight();
        if (isHazardousHeight(generationHex, sourceNode))
        {
            NodeToReturn = new HazardousNode(generationHex, sourceNode, sourceNode.CumulativeCost + generationHex.GetMovementCost(), -heightDifference);
        }
        else if (heightDifference < 0)
        {
            NodeToReturn = new NavigableNode(generationHex, sourceNode, sourceNode.CumulativeCost + generationHex.GetMovementCost());
        }
        else if (isReachableFromNode(generationHex, sourceNode))
        {
            NodeToReturn = new NavigableNode(generationHex, sourceNode, sourceNode.CumulativeCost + generationHex.GetMovementCost() + Mathf.Abs(heightDifference));
        }

        return NodeToReturn;
    }
    private Node CompareSourceNodes(Node currentNode, Node currentSource, Node comparisonSource)
    {
        Node betterSource = currentSource;

        //If it's not reachable from comparison node, skip.
        if (isReachableFromNode(currentNode.OwnedHex, comparisonSource))
        {
            if (currentSource is HazardousNode)
            {
                //If current source is hazardous, and other source isn't...
                if (!(comparisonSource is HazardousNode))
                {
                    //...safer is better.
                    betterSource = comparisonSource;
                }
                else
                {
                    HazardousNode currentHazardousSource = currentSource as HazardousNode;
                    HazardousNode comparisonHazardousSource = comparisonSource as HazardousNode;

                    if (comparisonHazardousSource.MoveToDamage < currentHazardousSource.MoveToDamage && comparisonSource.CumulativeCost < AP)
                    {
                        //...else, compare damage and pick safer.
                        betterSource = comparisonSource;
                    }
                    else if (comparisonHazardousSource.MoveToDamage == currentHazardousSource.MoveToDamage && comparisonSource.CumulativeCost < currentSource.CumulativeCost)
                    {
                        betterSource = comparisonSource;
                    }
                }
            }
            else if (currentNode is HazardousNode)
            {
                HazardousNode currentHazardousNode = currentNode as HazardousNode;

                if (!currentHazardousNode.isInnatelyHazardous)
                {
                    //If current node is hazardous because of source, see if other source is safer.
                    if (comparisonSource.OwnedHex.GetHeight() < currentSource.OwnedHex.GetHeight())
                    {
                        betterSource = comparisonSource;
                        ReplaceNode(currentNode, new NavigableNode(currentNode.OwnedHex, betterSource, betterSource.CumulativeCost + currentNode.OwnedHex.GetMovementCost()));
                    }
                }
            }
            else if (!(comparisonSource is HazardousNode))
            {
                if (!isHazardousHeight(currentNode.OwnedHex, comparisonSource))
                {
                    //If the comparison is cheaper and safe, use it...
                    if (comparisonSource.CumulativeCost < currentNode.Source.CumulativeCost)
                    {
                        betterSource = comparisonSource;
                    }
                }
            }
        }
        return betterSource;
    }
    #region Getters
    public bool isHazardousHeight(Hex target, Node source)
    {
        return target.GetHeight() - source.OwnedHex.GetHeight() < -(AP - source.CumulativeCost);
    }
    public bool isReachableFromNode(Hex target, Node source)
    {
        bool isReachable = true;

        if (target.GetHeight() - source.OwnedHex.GetHeight() > AP - source.CumulativeCost)
        {
            isReachable = false;
        }

        return isReachable;
    }
    #endregion

    int AP;
    NavigableNode MarkedNode = null;
}
