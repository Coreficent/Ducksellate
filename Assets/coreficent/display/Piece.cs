using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }
    private readonly float space = 1f;

    protected void Reposition()
    {
        transform.position += new Vector3(X * space - (Main.Cells.GetLength(0) - 1) / 2f, Y * space - (Main.Cells.GetLength(1) - 1) / 2f, 0f);
    }
}
