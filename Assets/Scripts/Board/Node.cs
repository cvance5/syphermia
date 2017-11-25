using UnityEngine;
public abstract class Node {

    public Hex OwnedHex { get; set; }
    public Node Source { get; set; }
    public int CumulativeCost { get; set; }
    public bool isVisualized = false;
    public VisualizationTypes VisualizationType;

    public Node(Hex ownedHex, Node source, int cost)
    {
        OwnedHex = ownedHex;
        Source = source;
        CumulativeCost = cost;
    }
    public void Visualize()
    {
        switch (VisualizationType)
        {
            case VisualizationTypes.Navigable:
                OwnedHex.Visualize(Color.green);
                break;
            case VisualizationTypes.Hazardous:
                OwnedHex.Visualize(Color.red);
                break;
            case VisualizationTypes.Highlighted:
                OwnedHex.Visualize(Color.blue);
                break;
            case VisualizationTypes.Targetable:
                OwnedHex.Visualize(new Color(1, 1, 0));
                break;
        }
        isVisualized = true;
    }
    public void RemoveVisualization()
    {
        OwnedHex.RemoveVisualization();
        isVisualized = false;
    }
    #region ObjectOverrides
    public static bool operator ==(Node node, Hex hex)
    {
        if(ReferenceEquals(node, hex))
        {
            return true;
        }
        else if((object)node == null || (object)hex == null)
        {
            return false;
        }
        else
        {
            return node.OwnedHex == hex;
        }
    }
    public static bool operator !=(Node node, Hex hex)
    {
        return !(node == hex);
    }
    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }
        else if (obj is Node)
        {
            return this == obj as Node;
        }
        else if (obj is Hex)
        {
            return this == obj as Hex;
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
    #endregion
}
public enum VisualizationTypes
{
    Navigable,
    Hazardous,
    Highlighted,
    Targetable,
}