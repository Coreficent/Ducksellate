using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Cell : MonoBehaviour
{
    private readonly float rotationAngle = 90f;
    private float targetAngle = 0f;
    private readonly float space = 1f;
    private float direction = 1f;
    private bool activated = false;
    //private readonly int[,] reactSites = { { -1, 0 }, { 0, -1 }, { 1, 0 }, { 0, 1 } };
    public int X { get; set; }
    public int Y { get; set; }
    private readonly Tuple<int, int>[] reactSites = { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };

    void Start()
    {
        transform.position = new Vector3(X * space - (Main.cells.GetLength(0) - 1) / 2f, Y * space - (Main.cells.GetLength(1) - 1) / 2f, 0f);
        CorrectAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.Rotate(Vector3.forward * direction, rotationAngle * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - targetAngle) < 10f)
            {
                CorrectAngle();
                ColllectReactableCells();
                activated = false;
            }
        }
    }

    private void ColllectReactableCells()
    {
        /*
        if (X < Main.cells.GetLength(0) - 1)
        {
            Main.activatedCells.Enqueue(Main.cells[X + 1, Y]);
        }
        if (Y < Main.cells.GetLength(1) - 1)
        {
            Main.activatedCells.Enqueue(Main.cells[X, Y + 1]);
        }
        */
        List<Cell> reactableCells = FindReactableCells();

        Log.Output("capacity ", reactableCells.Count);
        foreach (Cell cell in reactableCells)
        {
            Main.activatedCells.Enqueue(cell);
        }
    }

    void OnMouseOver()
    {
        if (!activated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Log.Output("left clicked on::" + X + "::" + Y);
                direction = 1f;
                targetAngle = CalculateTargetAngle();
                activated = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Log.Output("right clicked on::" + X + "::" + Y);
                direction = -1f;
                targetAngle = CalculateTargetAngle();
                activated = true;
            }
        }
    }

    public void React()
    {
        targetAngle = CalculateTargetAngle();
        activated = true;
    }

    private float CalculateTargetAngle()
    {
        float target = transform.eulerAngles.z + rotationAngle * direction;
        target = target > 360f ? target - 360f : target;
        target = target < 0f ? target + 360f : target;
        // Log.Output("target " + target);

        return target;
    }

    private void CorrectAngle()
    {
        // prevents inaccuracy over many iterations
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Round(transform.eulerAngles.z / 90f) * 90f);
    }

    private List<Cell> FindReactableCells()
    {
        List<Cell> cells = new List<Cell>();

        Cell current;

        current = GetCell(reactSites[FindReactOffset(transform.eulerAngles.z)]);
        Log.Output("current==null", current == null);
        if (current)
        {
            cells.Add(current);
        }
        current = GetCell(reactSites[FindReactOffset(transform.eulerAngles.z + 90f)]);
        Log.Output("current==null", current == null);
        if (current)
        {
            cells.Add(current);
        }

        return cells;
    }

    private int FindReactOffset(float angle)
    {
        int offset = (int)Mathf.Round(angle / 90f);
        offset = offset < 0 ? offset + 4 : offset;
        offset = offset > 3 ? offset - 4 : offset;
        return offset;
    }

    private Cell GetCell(Tuple<int, int> coordinate)
    {
        Log.Output("coordinate", coordinate);
        int x = X + coordinate.Item1;
        int y = Y + coordinate.Item2;
        Log.Output("coordinate x", x);
        Log.Output("coordinate y", y);
        if (x >= 0 && x < Main.cells.GetLength(0))
        {
            if (y >= 0 && y < Main.cells.GetLength(1))
            {
                return Main.cells[x, y];
            }
        }

        return null;
    }

    private bool CanReact(Cell pairCell)
    {
        return false;
    }
}
