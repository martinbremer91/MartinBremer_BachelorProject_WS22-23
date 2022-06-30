using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public bool isHex;
    
    #region ----- SQUARE FIELDS -----
    
    public Square[,] squares;

    public Vector2Int sqGridSizes;
    [SerializeField] private Square squarePrefab;

    [SerializeField] [Range(1, 10)] private float sqSpacing;
    #endregion

    #region ----- HEX FIELDS -----

    

    #endregion

    #region ----- INIT -----
    private void Start() => GenerateGrid();
    private void GenerateGrid()
    {
        if (isHex)
            return;
        else
            GenerateSqGrid();
    }
    #endregion
    
    #region ----- SQUARE FUNCTIONS -----
    private void GenerateSqGrid()
    {
        if (sqGridSizes.x < 1 || sqGridSizes.y < 1)
            return;
        if (squares != null)
            DestroySqGrid();

        squares = new Square[sqGridSizes.x, sqGridSizes.y];
        
        float xOffset = -(sqGridSizes.x * .5f * sqSpacing);
        float yOffset = -(sqGridSizes.y * .5f * sqSpacing);

        for (int y = 0; y < sqGridSizes.y; y++)
        {
            for (int x = 0; x < sqGridSizes.x; x++)
            {
                Square sq = Instantiate(squarePrefab, 
                    new Vector3(xOffset + x * sqSpacing, yOffset + y * sqSpacing, 0), 
                    Quaternion.identity, transform);

                sq.name = "Square_" + x + "/" + y;
                sq.coords = new Vector2Int(x, y);
                
                squares[x,y] = sq;
            }
        }
        
        ScrambleSqGrid();
        ReferenceCollection.Instance.cursor.SetInitialCoords(Vector2Int.zero);
    }

    private void ScrambleSqGrid()
    {
        foreach (Square sq in squares)
        {
            sq.SetRandomType();
            sq.SetRandomOrientation();
            sq.SetRandomColors();
        }
    }

    private void DestroySqGrid()
    {
        for (int y = 0; y < sqGridSizes.y; y++)
        {
            for (int x = 0; x < sqGridSizes.x; x++)
                Destroy(squares[x, y].gameObject);
        }

        Array.Clear(squares, 0, squares.Length);
    }
    #endregion

    #region ----- HEX FUNCTIONS -----

    

    #endregion
}
