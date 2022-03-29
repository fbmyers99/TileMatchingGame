/*
 * by Freya Myers 2022
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure : MonoBehaviour
{
    private Grid[,] gameGrid;

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public GridStructure(Grid[,] gameGrid, int Columns, int Rows)
    {
        this.Rows = Rows;
        this.Columns = Columns;
        this.gameGrid = gameGrid;
    }

    public Grid GetTile(Position position)
    {
        return gameGrid[position.Row, position.Column];
    }

    public void MoveDown(Position position)
    {
        gameGrid[position.Row, position.Column] = gameGrid[position.Row - 1, position.Column];
        gameGrid[position.Row, position.Column].Tile.transform.position = gameGrid[position.Row, position.Column].Tile.transform.position + new Vector3(0, -1.5f, 0);
    }

    //returns position of tile based on name
    public Position GetPosition(string tileName)
    {
        Position position = new Position(0, 0);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (gameGrid[i, j].Tile.name == tileName)
                {
                    position = new Position(i, j);
                }
            }
        }
        return position;
    }

    //checks if position is within grid
    public bool ValidTile(Position position)
    {
        return (position.Row >= 0 && position.Row < Rows &&
            position.Column >= 0 && position.Column < Columns);
    }
}
