namespace Coreficent.Display
{
    using Coreficent.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Cell : Piece
    {
        public bool Disabled { get; set; }

        public SpriteRenderer SpriteRenderer;
        public AudioSource AudioSource;

        private readonly Tuple<int, int>[] _reactSites = { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };
        private readonly Color _colorDefault = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private readonly Color _colorHighlight = new Color(1.0f, 1.5f, 1.0f, 1.0f);
        private readonly Color _colorActivated = new Color(1.5f, 1.0f, 1.0f, 1.0f);
        private readonly float _speedMultiplier = 2.0f;
        private float _direction = 1.0f;
        private float _angleSum = 0.0f;
        private bool _activated = false;

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
            if (_activated)
            {
                float angleCurrent = rotationAngle * _speedMultiplier * Time.deltaTime;
                transform.Rotate(Vector3.forward * _direction, angleCurrent);
                _angleSum += angleCurrent;
                if (Mathf.Abs(_angleSum) >= rotationAngle)
                {
                    _angleSum = 0.0f;
                    Deactivate();
                }
            }
        }

        void OnMouseOver()
        {
            if (!Disabled)
            {
                if (!_activated)
                {
                    SpriteRenderer.material.color = _colorHighlight;
                    if (Input.GetMouseButtonDown(0))
                    {
                        _direction = 1.0f;
                        Activate();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        _direction = -1.0f;
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
            return FindReactOffset(transform.eulerAngles.z) == 1 || (X == Main.Main.InvalidOffset && Y == Main.Main.InvalidOffset);
        }

        private void OnMouseExit()
        {
            if (!_activated)
            {
                SpriteRenderer.material.color = _colorDefault;
            }
        }

        private void ColllectReactableCells()
        {
            List<Cell> reactableCells = FindReactableCells();

            foreach (Cell cell in reactableCells)
            {
                if (!cell._activated)
                {
                    cell._direction = _direction;
                    Main.Main.CellsActivated.Enqueue(cell);
                }
            }
        }

        private void Activate()
        {
            SpriteRenderer.material.color = _colorActivated;
            AudioSource.volume = UnityEngine.Random.Range(0.75f, 1.0f);
            AudioSource.PlayDelayed(UnityEngine.Random.Range(0.0f, 0.5f / _speedMultiplier));
            _activated = true;
        }

        private void Deactivate()
        {
            CorrectAngle();
            ColllectReactableCells();
            SpriteRenderer.material.color = _colorDefault;
            _activated = false;
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
            GetCell(originCell, _reactSites[FindReactOffset(originCell.transform.eulerAngles.z)]),
            GetCell(originCell, _reactSites[FindReactOffset(originCell.transform.eulerAngles.z + rotationAngle)])
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