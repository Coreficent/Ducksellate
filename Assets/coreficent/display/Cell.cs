﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Cell : MonoBehaviour
{
    private readonly Tuple<int, int>[] reactSites = { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };
    private readonly float rotationAngle = 90f;
    private readonly float space = 1f;

    private float targetAngle = 0f;
    private float direction = 1f;
    private bool activated = false;
    public int X { get; set; }
    public int Y { get; set; }
    

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
        List<Cell> reactableCells = FindReactableCells();

        foreach (Cell cell in reactableCells)
        {
            cell.direction = direction;
            Main.activatedCells.Enqueue(cell);
        }
    }

    void OnMouseOver()
    {
        if (!activated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                direction = 1f;
                targetAngle = CalculateTargetAngle();
                activated = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
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

        foreach (Cell cell in FindCandidateCells(this))
        {
            if (CanReact(cell))
            {
                cells.Add(cell);
            }
        }

        return cells;
    }

    private List<Cell> FindCandidateCells(Cell originCell)
    {
        return new List<Cell>
        {
            GetCell(originCell, reactSites[FindReactOffset(originCell.transform.eulerAngles.z)]),
            GetCell(originCell, reactSites[FindReactOffset(originCell.transform.eulerAngles.z + 90f)])
        };
    }

    private bool CanReact(Cell candidate)
    {
        if (candidate)
        {
            foreach (Cell cell in FindCandidateCells(candidate))
            {
                if (cell == this)
                {
                    return true;
                }
            }
        }

        return false;
    }
    private int FindReactOffset(float angle)
    {
        int offset = (int)Mathf.Round(angle / 90f);
        offset = offset < 0 ? offset + 4 : offset;
        offset = offset > 3 ? offset - 4 : offset;
        return offset;
    }

    private Cell GetCell(Cell originCell, Tuple<int, int> coordinate)
    {
        int x = originCell.X + coordinate.Item1;
        int y = originCell.Y + coordinate.Item2;
        if (x >= 0 && x < Main.cells.GetLength(0))
        {
            if (y >= 0 && y < Main.cells.GetLength(1))
            {
                return Main.cells[x, y];
            }
        }

        return null;
    }
}
