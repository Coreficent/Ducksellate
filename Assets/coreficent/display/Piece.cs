using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }

    protected readonly float rotationAngle = 90f;
    protected readonly float boardAngle = 45f;

    private readonly float space = 1f;

    private Vector3 delta = new Vector3(0.75f, 0.75f, 0.75f);

    public void Shrink()
    {
        if (!Shrinken())
        {
            transform.localScale -= delta * Time.deltaTime;
        }
        else
        {
            transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
    public void Expand()
    {
        if (!Expanded())
        {
            transform.localScale += delta * Time.deltaTime;
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    public bool Shrinken()
    {
        return transform.localScale.x <= 0 && transform.localScale.y <= 0 && transform.localScale.z <= 0;
    }
    public bool Expanded()
    {
        return transform.localScale.x >= 1.0 && transform.localScale.y >= 1.0 && transform.localScale.z >= 1.0;
    }
    protected void Reposition()
    {
        transform.position += new Vector3(X * space - (Main.Cells.GetLength(0) - 1) / 2f, Y * space - (Main.Cells.GetLength(1) - 1) / 2f, 0f);
        transform.RotateAround(transform.parent.position, Vector3.forward, boardAngle);
    }

    protected void RandomizeDelta()
    {
        delta *= Random.Range(0.5f, 2.0f);
    }
}
