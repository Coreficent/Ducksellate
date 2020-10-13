using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Piece
{
    public SpriteRenderer SpriteRenderer;

    private readonly Tuple<int, int>[] reactSites = { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };
    private readonly float rotationAngle = 90f;
    private readonly float boardAngle = 45f;
    private readonly Color colorDefault = new Color(1f, 1f, 1f, 1f);
    private readonly Color colorHighlight = new Color(1f, 1.5f, 1f, 1f);
    private readonly Color colorActivated = new Color(1.5f, 1f, 1f, 1f);

    private float targetAngle = 0f;
    private float direction = 1f;
    private bool activated = false;

    void Start()
    {
        Reposition();
        transform.RotateAround(transform.parent.position, Vector3.forward, boardAngle);
        transform.eulerAngles += new Vector3(0f, 0f, -boardAngle);

        CorrectAngle();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            transform.Rotate(Vector3.forward * direction, rotationAngle * Time.deltaTime);
            if (Mathf.Abs(transform.eulerAngles.z - targetAngle) < 5f)
            {
                CorrectAngle();
                ColllectReactableCells();
                SpriteRenderer.material.color = colorDefault;
                activated = false;
            }
        }
    }

    void OnMouseOver()
    {
        if (!activated)
        {
            SpriteRenderer.material.color = colorHighlight;
            if (Input.GetMouseButtonDown(0))
            {
                direction = 1f;
                Activate();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                direction = -1f;
                Activate();
            }
        }
    }

    private void OnMouseExit()
    {
        SpriteRenderer.material.color = colorDefault;
    }

    private void ColllectReactableCells()
    {
        List<Cell> reactableCells = FindReactableCells();

        foreach (Cell cell in reactableCells)
        {
            cell.direction = direction;
            Main.CellsActivated.Enqueue(cell);
        }
    }
    public void Randomize()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, UnityEngine.Random.Range(0, 4) * rotationAngle);
    }
    public void React()
    {
        Activate();
    }

    private void Activate()
    {
        targetAngle = CalculateTargetAngle();
        SpriteRenderer.material.color = colorActivated;
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
        // prevents inaccuracy over many iterations due to floating point mathematics
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Round(transform.eulerAngles.z / rotationAngle) * rotationAngle);
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
        if (x >= 0 && x < Main.Cells.GetLength(0))
        {
            if (y >= 0 && y < Main.Cells.GetLength(1))
            {
                return Main.Cells[x, y];
            }
        }

        return null;
    }

    public bool Tessellated()
    {
        return FindReactOffset(transform.eulerAngles.z) == 1;
    }
}
