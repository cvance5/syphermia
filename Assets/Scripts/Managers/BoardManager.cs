using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager> {

    [SerializeField]
    GameObject hex = null;
    void Start()
    {
        GenerateBoard(10, 3, 10, BoardTopology.Jagged);
    }
    public static void GenerateBoard(int maxColumn, int maxHeight, int maxRow, BoardTopology topologyType = BoardTopology.Rolling)
    {
        Instance.gameObject.name = "Board";

        Rows = maxRow;
        Columns = maxColumn;
        Height = maxHeight;

        float[][] topology = GenerateTopologicalNoise(maxColumn, maxRow, topologyType);

        for(int column = 0; column < maxColumn; column++)
        {
            for(int row = 0; row < maxRow; row++)
            {
                int height = Mathf.RoundToInt(topology[column][row] * maxHeight);
                board.Add(new Vector3(column, height, row), GenerateHex(column, height, row));
                while(height > 0)
                {
                    height--;
                    board.Add(new Vector3(column, height, row), GenerateHex(column, height, row));
                }
            }
        }

        BoardCenter = new Vector3(Rows / 2, Height / 2, Columns / 2);
    }
    private static float[][] GenerateTopologicalNoise(int width, int height, BoardTopology topologyType)
    {
        float[][] noise = PerlinNoise.GenerateWhiteNoise(width, height);

        switch (topologyType)
        {
            case BoardTopology.Rolling:
                {
                    noise = PerlinNoise.GenerateSmoothNoise(noise, 1);
                }
                break;
            case BoardTopology.Jagged:
                break;
        }

        return noise;
    }
    public static void MoveToHex(CharacterData character, Hex hex)
    {
        if(character.OccupiedHex != null)
        {
            character.OccupiedHex.Vacate();
        }

        character.transform.position = hex.GetWorldPosition() + Vector3.up;
        hex.Occupy(character);
    }
    #region HexManagement
    public static Hex GetHex(int column, int height, int row)
    {
        return GetHex(new Vector3(column, height, row));
    }
    public static Hex GetHex(Vector3 boardPos)
    {
        if(IsValidBoardPosition(boardPos))
        {
            if (board.ContainsKey(boardPos))
            {
                return board[boardPos];
            }
        }
        return null;
    }
    public static Hex GetHex(Vector4 cubePos)
    {
        return GetHex(ConvertFromCubicToBoardCoordinates(cubePos));
    }
    public static Hex[] GetNeighborsOfHex(Hex origin, bool guaranteeSurfaceHexes = true)
    {
        return GetNeighborsOfHex(ConvertFromBoardToCubicCoordinates(origin.GetBoardPosition()), guaranteeSurfaceHexes);
    }
    public static Hex[] GetNeighborsOfHex(Vector3 boardPos, bool guaranteeSurfaceHexes = true)
    { 
        return GetNeighborsOfHex(ConvertFromBoardToCubicCoordinates(boardPos), guaranteeSurfaceHexes);
    }
    public static Hex[] GetNeighborsOfHex(Vector4 cubePos, bool guaranteeSurfaceHexes = true)
    {
        Hex[] neighbors = new Hex[6];

        for(int i = 0; i < neighborOrientations.Length; i++)
        {
            neighbors[i] = GetHex(cubePos + neighborOrientations[i]);
            if (guaranteeSurfaceHexes)
            {
                if(neighbors[i] != null)
                {
                    neighbors[i] = FindSummit(neighbors[i]);
                }
                else
                {
                    neighbors[i] = FindValley(cubePos + neighborOrientations[i]);
                }
            }
        }

        return neighbors;
    }
    private static Hex GenerateHex(int columnPos, int height, int rowPos)
    {
        Hex newHex;
        newHex = Instantiate(Instance.hex, ConvertFromBoardToWorldCoordinates(columnPos, height, rowPos), Quaternion.identity).GetComponent<Hex>();
        newHex.SetPosition(columnPos, height, rowPos);
        newHex.transform.Rotate(0, Random.Range(0, 6) * 60, 0);
        newHex.transform.SetParent(Instance.gameObject.transform);
        return newHex;
    }
    #endregion
    #region Tools
    public static bool IsValidBoardPosition(Vector3 boardPos)
    {
        bool isValid = true;

        if (ReferenceEquals(boardPos, null))
        {
            isValid = false;
        }
        else if(boardPos.x < 0 || boardPos.x >= Columns)
        {
            isValid = false;
        }
        else if(boardPos.z < 0 || boardPos.z >= Rows)
        {
            isValid = false;
        }
        else if(boardPos.y < 0 || boardPos.y > Height)
        {
            isValid = false;
        }

        return isValid;
    }
    public static int CalculateHexDistance(Vector3 hex1Pos, Vector3 hex2Pos)
    {
        Vector4 hex1Cubic = ConvertFromBoardToCubicCoordinates(hex1Pos);
        Vector4 hex2Cubic = ConvertFromBoardToCubicCoordinates(hex2Pos);

        float xDistance = Mathf.Abs(hex1Cubic.x - hex2Cubic.x);
        float yDistance = Mathf.Abs(hex1Cubic.y - hex2Cubic.y);
        float zDistance = Mathf.Abs(hex1Cubic.z - hex2Cubic.z);
        float heightDistance = (int)Mathf.Abs(hex1Cubic.w - hex2Cubic.w);

        return Mathf.RoundToInt((xDistance + yDistance + zDistance) / 2);
    }
    public static int CalculateHexDistance(Hex hex1, Hex hex2)
    {
        return CalculateHexDistance(ConvertFromWorldToBoardCoordinates(hex1.transform.position), ConvertFromWorldToBoardCoordinates(hex2.transform.position));
    }
    public static Hex FindSummit(Hex seekingHex)
    {
        if(seekingHex != null)
        {
            while (seekingHex.GetIsBuried())
            {
                seekingHex = GetHex(seekingHex.GetBoardPosition() + Vector3.up);
            }
        }
        return seekingHex;
    }
    public static Hex FindValley(Vector3 seekingPosition)
    {
        Hex seekingHex = null;

        while(seekingHex == null && seekingPosition.y > 0)
        {
            seekingPosition += Vector3.down;
            seekingHex = GetHex(seekingPosition);
        }

        return seekingHex;
    }
    public static Hex FindValley(Vector4 seekingPosition)
    {
        return FindValley(ConvertFromCubicToBoardCoordinates(seekingPosition));
    }
    #endregion
    #region Converters
    public static Vector3 ConvertFromBoardToWorldCoordinates(int column, int height, int row)
    {
        if(column % 2 == 0)
        {
            return new Vector3(column * HEX_WIDTH_OFFSET, height, row * HEX_DIAMETER);
        }
        else
        {
            return new Vector3(column * HEX_WIDTH_OFFSET, height, (row * HEX_DIAMETER) + HEX_RADIUS);
        }
    }
    public static Vector3 ConvertFromBoardToWorldCoordinates(Vector3 boardPos)
    {
        return ConvertFromBoardToWorldCoordinates((int)boardPos.x, (int)boardPos.y, (int)boardPos.z);
    }
    public static Vector3 ConvertFromWorldToBoardCoordinates(float x, float y, float z)
    {
        if (z % 2 == 0)
        {
            return new Vector3((x / HEX_WIDTH_OFFSET), y, z / HEX_DIAMETER);
        }
        else
        {
            return new Vector3((x / HEX_WIDTH_OFFSET), y, (z - HEX_RADIUS) / HEX_DIAMETER);
        }
    }
    public static Vector3 ConvertFromWorldToBoardCoordinates(Vector3 worldPos)
    {
        return ConvertFromWorldToBoardCoordinates(worldPos.x, worldPos.y, worldPos.z);
    }
    public static Vector4 ConvertFromBoardToCubicCoordinates(Vector3 boardPos)
    {
        float x = boardPos.x;
        float z = boardPos.z - (boardPos.x - (Mathf.RoundToInt(boardPos.x) & 1)) / 2;
        float y = -x - z;
        float w = boardPos.y;

        return new Vector4(x, y, z, w);
    }
    public static Vector3 ConvertFromCubicToBoardCoordinates(Vector4 cubicPos)
    {
        float col = cubicPos.x;
        float row = cubicPos.z + (cubicPos.x - (Mathf.RoundToInt(cubicPos.x) & 1)) / 2;
        float height = cubicPos.w;

        return new Vector3(col, height, row);
    }
    #endregion

    private static int Rows = 0;
    private static int Columns = 0;
    private static int Height = 1;

    private static Dictionary<Vector3, Hex> board = new Dictionary<Vector3, Hex>();

    protected static Vector4[] neighborOrientations = new Vector4[]
    { new Vector4(1, -1, 0, 0),
      new Vector4(1, 0, -1, 0),
      new Vector4(0, 1, -1, 0),
      new Vector4(-1, 1, 0, 0),
      new Vector4(-1, 0, 1, 0),
      new Vector4(0, -1, 1, 0)
    };

    public static Vector3 BoardCenter = Vector3.zero;

    private const float HEX_WIDTH_OFFSET = 1.8f;
    private const float HEX_RADIUS = 1f;
    private const float HEX_DIAMETER = HEX_RADIUS * 2;
    private const float HEX_HEIGHT = 1f;
}

public enum BoardTopology
{
    Rolling,
    Jagged
}