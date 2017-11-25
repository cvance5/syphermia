using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

    public CharacterData occupier = null;

    void Awake()
    {
        Visualizer = GetComponent<MeshRenderer>().materials[1];
        defaultColor = Visualizer.GetColor("_Color");
        defaultEmission = Visualizer.GetColor("_EmissionColor");
    }
    public void SetPosition(int column, int height, int row)
    {
        Column = column;
        Row = row;
        Height = height;

        if(BoardManager.GetHex(new Vector3(column, height+1, row)) != null)
        {
            isBuried = true;
        }
    }
    public void Occupy(CharacterData character)
    {
        occupier = character;
        occupier.Occupy(this);
    }
    public void Vacate()
    {
        if(occupier != null)
        {
            occupier.Vacate();
            occupier = null;
        }
    }
    public void Visualize(Color visualizationColor)
    {
        Visualizer.SetColor("_Color", visualizationColor);
        Visualizer.SetColor("_EmissionColor", visualizationColor / 2);
    }
    public void RemoveVisualization()
    {
        Visualizer.SetColor("_Color", defaultColor);
        Visualizer.SetColor("_EmissionColor", defaultEmission);
    }
    #region Getters
    public int GetMovementCost()
    {
        return MovementCost;
    }
    public int GetHeight()
    {
        return Height;
    }
    public Vector3 GetBoardPosition()
    {
        return new Vector3(Column, Height, Row);
    }
    public Vector3 GetWorldPosition()
    {
        return BoardManager.ConvertFromBoardToWorldCoordinates(Column, Height, Row);
    }
    public CharacterData GetOccupier()
    {
        return occupier;
    }
    public bool isOccupied()
    {
        return occupier != null;
    }
    public bool GetIsBuried()
    {
        return isBuried;
    }
    #endregion

    protected bool isBuried = false;

    protected int Column;
    protected int Height;
    protected int Row;

    protected int MovementCost = 1;

    protected Color defaultColor;
    protected Color defaultEmission;
    protected Material Visualizer;
}