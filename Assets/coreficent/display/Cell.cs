﻿using Coreficent.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coreficent.Display
{

    public class Cell : Piece
    {
        public bool Disabled { get; set; }

        public SpriteRenderer SpriteRenderer;
        public AudioSource AudioSource;

        private readonly Tuple<int, int>[] reactSites = { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };
        private readonly Color colorDefault = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private readonly Color colorHighlight = new Color(1.0f, 1.5f, 1.0f, 1.0f);
        private readonly Color colorActivated = new Color(1.5f, 1.0f, 1.0f, 1.0f);
        private readonly float speedMultiplier = 2.0f;
        private float direction = 1.0f;
        private float angleSum = 0.0f;
        private bool activated = false;

        void Start()
        {
            SanityCheck.Check(this, SpriteRenderer, AudioSource);

            RandomizeDelta();
            AudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.25f);
            Reposition();
            transform.eulerAngles += new Vector3(0.0f, 0.0f, -boardAngle);
            CorrectAngle();
        }
        void Update()
        {
            if (activated)
            {
                float angleCurrent = rotationAngle * speedMultiplier * Time.deltaTime;
                transform.Rotate(Vector3.forward * direction, angleCurrent);
                angleSum += angleCurrent;
                if (Mathf.Abs(angleSum) >= rotationAngle)
                {
                    angleSum = 0.0f;
                    Deactivate();
                }
            }
        }
        void OnMouseOver()
        {
            if (!Disabled)
            {
                if (!activated)
                {
                    SpriteRenderer.material.color = colorHighlight;
                    if (Input.GetMouseButtonDown(0))
                    {
                        direction = 1.0f;
                        Activate();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        direction = -1.0f;
                        Activate();
                    }
                }
            }
        }
        public void Randomize()
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, UnityEngine.Random.Range(0, 4) * rotationAngle);
        }
        public Cell RotateLeft()
        {
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationAngle);
            return this;
        }
        public Cell RotateRight()
        {
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -rotationAngle);
            return this;
        }
        public void React()
        {
            Activate();
        }
        public bool Tessellated()
        {
            return FindReactOffset(transform.eulerAngles.z) == 1 || (X == Main.Main.INVALID_OFFSET && Y == Main.Main.INVALID_OFFSET);
        }
        private void OnMouseExit()
        {
            if (!activated)
            {
                SpriteRenderer.material.color = colorDefault;
            }
        }
        private void ColllectReactableCells()
        {
            List<Cell> reactableCells = FindReactableCells();

            foreach (Cell cell in reactableCells)
            {
                cell.direction = direction;
                Main.Main.CellsActivated.Enqueue(cell);
            }
        }
        private void Activate()
        {
            SpriteRenderer.material.color = colorActivated;
            AudioSource.volume = UnityEngine.Random.Range(0.75f, 1.0f);
            AudioSource.PlayDelayed(UnityEngine.Random.Range(0.0f, 0.5f / speedMultiplier));
            activated = true;
        }
        private void Deactivate()
        {
            CorrectAngle();
            ColllectReactableCells();
            SpriteRenderer.material.color = colorDefault;
            activated = false;
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
            GetCell(originCell, reactSites[FindReactOffset(originCell.transform.eulerAngles.z + rotationAngle)])
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
            int offset = (int)Mathf.Round(angle / rotationAngle);
            offset = offset < 0 ? offset + 4 : offset;
            offset = offset > 3 ? offset - 4 : offset;
            return offset;
        }
        private Cell GetCell(Cell originCell, Tuple<int, int> coordinate)
        {
            int x = originCell.X + coordinate.Item1;
            int y = originCell.Y + coordinate.Item2;
            if (x >= 0 && x < Main.Main.Cells.GetLength(0))
            {
                if (y >= 0 && y < Main.Main.Cells.GetLength(1))
                {
                    return Main.Main.Cells[x, y];
                }
            }
            return null;
        }
    }
}