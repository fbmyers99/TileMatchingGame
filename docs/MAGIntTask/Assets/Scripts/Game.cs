/*
 * by Freya Myers 2022
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Rows = 9;
    public int Columns = 9;

    public GameObject Tile;
    public GameObject RedTile;
    public GameObject BlueTile;
    public GameObject GreenTile;
    public GameObject YellowTile;

    private int tileCount = 0;

    private Grid[,] gameGrid;
    private GridStructure structure;

    void Start()
    {

        structure = GenerateGrid();

    }

    GameObject SetTileColour(TileColour tileColour)
    {

        switch (tileColour)
        {
            case TileColour.Red:
                return RedTile;
            case TileColour.Blue:
                return BlueTile;
            case TileColour.Green:
                return GreenTile;
            case TileColour.Yellow:
                return YellowTile;
            default:
                return null;
        }
    }

    GridStructure GenerateGrid()
    {
        gameGrid = new Grid[Rows, Columns];


        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                tileCount++;
                gameGrid[i, j] = new Grid();

                TileColour tileColour = (TileColour)Random.Range(0, 4);

                GameObject tile = Instantiate(SetTileColour(tileColour), new Vector3(j * 1.5f, -i * 1.5f, 0), Quaternion.identity);
                tile.name = tileColour + "Tile" + tileCount;

                gameGrid[i, j].Tile = tile;
                gameGrid[i, j].Colour = tileColour;
                tile.transform.parent = transform;

            }
        }

        return new GridStructure(gameGrid, Rows, Columns);

    }

    //returns true if there is an empty space with a tile above
    bool tilesCanMoveDown()
    {
        for (int i = structure.Rows - 1; i > 0; i--)
        {
            for (int j = 0; j < structure.Columns; j++)
            {
                Position position = new Position(i, j);

                if (i != structure.Rows - 1 && !structure.GetTile(position.NextPosition(Direction.Up)).Empty && structure.GetTile(position).Empty)
                {
                    return true;
                }

            }
        }
        return false;
    }

    //returns a list of all empty tile positions
    List<Position> EmptyTiles()
    {
        var AllEmptyTiles = new List<Position>();

        for (int i = 0; i < structure.Rows; i++)
        {
            for (int j = 0; j < structure.Columns; j++)
            {

                Position position = new Position(i, j);

                if (structure.GetTile(position).Empty)
                {
                    AllEmptyTiles.Add(position);
                }

            }
        }
        return AllEmptyTiles;
    }

    //for each empty position create a new tile
    void FillEmpty()
    {
        var AllEmptyTiles = EmptyTiles();

        foreach (Position tilePosition in AllEmptyTiles)
        {
            tileCount++;
            TileColour tileColour = (TileColour)Random.Range(0, 4);

            GameObject tile = Instantiate(SetTileColour(tileColour), new Vector3(tilePosition.Column * 1.5f, -tilePosition.Row * 1.5f, 0), Quaternion.identity);
            tile.name = tileColour + "Tile" + tileCount;

            gameGrid[tilePosition.Row, tilePosition.Column].Tile = tile;
            gameGrid[tilePosition.Row, tilePosition.Column].Colour = tileColour;

            tile.transform.parent = transform;

            structure.GetTile(tilePosition).Empty = false;
        }
 
    }

    GridStructure MoveTilesDown()
    {
        while (tilesCanMoveDown())
        {
            for (int i = structure.Rows - 1; i > 0; i--)
            {
                for (int j = 0; j < structure.Columns; j++)
                {
                    Position position = new Position(i, j);

                    if (structure.GetTile(position).Empty)
                    {

                        //if it's not the top row AND the tile above isn't empty
                        if (i != 0 && !structure.GetTile(position.NextPosition(Direction.Up)).Empty)
                        {
         
                            structure.MoveDown(position);

                            /*
                             * There is currently a problem with two tile positions referencing the same object once it has
                             * been moved (in GridStructure) -- at the moment trying to dereference once causes a nullPointerException.
                             * Therefore with the two lines below when trying to reassign  which tile is empty or not it sets both tiles
                             * at once, leaving gaps in the grid when moving tiles and refilling
                             */

                            structure.GetTile(position).Empty = false;
                            structure.GetTile(position.NextPosition(Direction.Up)).Empty = true;
                        }

                    }
                    
                    //resets selected tiles
                    if (structure.GetTile(position).Selected)
                    {
                        structure.GetTile(position).Selected = false;
                    }
                }
            }
            
        }

        FillEmpty();

        return structure;
    }

    Position GetTileClicked()
    {
        string tileClicked = "";
        Position position = new Position(0, 0);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null)
        {
            tileClicked = hit.collider.gameObject.name;
            Debug.Log($"{hit.collider.gameObject.name} clicked");
        }

        return structure.GetPosition(tileClicked);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Position position = GetTileClicked();
            new Algorithm().SelectAllMatchingTiles(structure, position);
            structure = MoveTilesDown();
        }
    }
}

