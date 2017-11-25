public class NavigableNode : Node, IHighlightable
{
    public NavigableNode(Hex ownedHex, Node source, int movementCost) : base(ownedHex, source, movementCost)
    {
        VisualizationType = VisualizationTypes.Navigable;
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
