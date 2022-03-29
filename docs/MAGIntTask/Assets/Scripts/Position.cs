/*
 * by Freya Myers 2022
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public int Row { get; private set; }
    public int Column { get; private set; }

    public Position(int Row, int Column)
    {
        this.Row = Row;
        this.Column = Column;
    }

    public Position NextPosition(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Position(Row - 1, Column);
            case Direction.Down:
                return new Position(Row + 1, Column);
            case Direction.Left:
                return new Position(Row, Column - 1);
            case Direction.Right:
                return new Position(Row, Column + 1);
            default:
                return null;
        }
    }
}
