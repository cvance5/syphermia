using System.Collections.Generic;
using System.Linq;

public abstract class NodeMap
{
    public abstract void CreateMap(Hex origin);
    public virtual void VisualizeMap()
    {
        if(MapNodes.Count > 0)
        {
            foreach(Node node in MapNodes)
            {
                node.Visualize();
            }
        }
    }
    public virtual void EndVisualization()
    {
        if (MapNodes.Count > 0)
        {
            foreach (Node node in MapNodes)
            {
                node.OwnedHex.RemoveVisualization();
            }
        }
    }
    protected abstract Node GenerateNode(Node sourceNode, Hex generationHex);
    protected void ReplaceNode(Node oldNode, Node newNode)
    {
        int index = MapNodes.IndexOf(oldNode);
        if (index >= 0)
        {
            MapNodes[index] = newNode;
        }
        oldNode = null;
    }
    #region Getters
    public bool IsOutdated()
    {
        if(MapNodes.Count == 0 || MapNodes[0] != Owner.OccupiedHex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Contains(Node node)
    {
        return MapNodes.Contains(node);
    }
    public bool Contains(Hex hex)
    {
        return MapNodes.Any(node => node == hex);
    }
    public Node GetNodeForHex(Hex hex)
    {
        return MapNodes.Find(node => node == hex);
    }
    public int GetCostOfHex(Hex hex)
    {
        return GetNodeForHex(hex).CumulativeCost;
    }
    public int GetDamageOfHex(Hex hex)
    {
        HazardousNode soughtNode = GetNodeForHex(hex) as HazardousNode;

        if (soughtNode != null)
        {
            return soughtNode.MoveToDamage;                                 
        }
        return 0;        
    }
    #endregion
    protected List<Node> MapNodes = new List<Node>();
    protected CharacterData Owner = null;
}