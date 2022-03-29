/*
 * by Freya Myers 2022
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{

    Position currentPosition = new Position(0, 0);

    GridStructure structure;

    //checks if the tile in the direction is within the grid
    bool ValidDirection(Direction direction)
    {
        return (structure.ValidTile(currentPosition.NextPosition(direction)));
    }

    //returns true while the current tile has one next to it that is not yet selected AND matches in colour
    bool HasUnselectedMatchingNeighbour()
    {
        foreach (Direction direction in System.Enum.GetValues(typeof(Direction)))
        {
            if (TileUnselected(currentPosition.NextPosition(direction)) &&
                        TilesMatch(currentPosition, direction))
            { 
                return true;
            }
        }
        return false;
    }

    //checks the colours match
    bool TilesMatch(Position position, Direction direction)
    {
        if (structure.ValidTile(position) && structure.ValidTile(position.NextPosition(direction)))
        {
            return (structure.GetTile(currentPosition).Colour == structure.GetTile(currentPosition.NextPosition(direction)).Colour);
        }

        return false;
    }

    bool TileUnselected(Position position)
    {
        return (structure.ValidTile(position) && !structure.GetTile(position).Selected);
    }
    

    public void SelectAllMatchingTiles(GridStructure structure, Position startPosition)
    {
        this.structure = structure;
        currentPosition = startPosition;

        bool isCompleted = false;
        var selectedTiles = new List<Position>();
        structure.GetTile(currentPosition).Selected = true;

        //adds clicked tile to selected
        selectedTiles.Add(currentPosition);

        while (!isCompleted)
        {
            while (HasUnselectedMatchingNeighbour())
            {
                foreach (Direction direction in System.Enum.GetValues(typeof(Direction)))
                {

                    //if its a viable direction AND the tile in that direction isn't selected AND matches the colour of the current tile
                    if (ValidDirection(direction) &&
                        TileUnselected(currentPosition.NextPosition(direction)) && 
                        structure.GetTile(currentPosition).Colour == structure.GetTile(currentPosition.NextPosition(direction)).Colour)
                    {
                        //hide tile, mark as empty, mark as selected and repeat for the tile in the direction
                        structure.GetTile(currentPosition).Tile.SetActive(false);
                        structure.GetTile(currentPosition).Empty = true;

                        structure.GetTile(currentPosition.NextPosition(direction)).Selected = true;
                        selectedTiles.Add(currentPosition.NextPosition(direction));

                        structure.GetTile(currentPosition.NextPosition(direction)).Tile.SetActive(false);
                        structure.GetTile(currentPosition.NextPosition(direction)).Empty = true;
                        
                    }
                }     
            }

            //if there are no tiles left to select, it is finished
            if (selectedTiles.Count == 1 && !HasUnselectedMatchingNeighbour())
            {
                isCompleted = true;
            }

            else
            {
                selectedTiles.RemoveAt(0);
                currentPosition = selectedTiles[0];
            }
        }
    }
}
