using System;

public class TargetableNode : Node, IHighlightable
{
    public TargetableNode(Hex ownedHex, Node source, int cost) : base(ownedHex, source, cost)
    {
        VisualizationType = VisualizationTypes.Targetable;
    }

    public void Highlight()
    {
        storedType = VisualizationType;
        VisualizationType = VisualizationTypes.Highlighted;
        Visualize();
    }

    public void RemoveHighlight()
    {
        VisualizationType = storedType;
        Visualize();
    }

    VisualizationTypes storedType;
}
