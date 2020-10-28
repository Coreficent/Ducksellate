namespace Coreficent.Main
{
    using Coreficent.Animation;
    using Coreficent.Display;
    using Coreficent.Utility;
    using System.Collections.Generic;
    using System;
    using UnityEngine.SceneManagement;
    using UnityEngine;

    public class Main : MonoBehaviour
    {
        public static readonly int InvalidOffset = -1024;
        public static Cell[,] Cells = new Cell[0, 0];
        public static Queue<Cell> CellsActivated = new Queue<Cell>();
        public static MonoBehaviour Handler = null;

        private static bool _gameBeat = false;

        public Cell Cell;
        public GameObject Board;
        public Obstacle LilyPad;
        public SpriteButton ButtonCredits;
        public SpriteButton ButtonSkip;

        private string _sceneCurrent;

        private enum _state { Win, Run, Idle }

        void Start()
        {
            SanityCheck.Check(this, Cell, LilyPad, Board, ButtonSkip, ButtonCredits);

            Handler = this;
            _sceneCurrent = SceneManager.GetActiveScene().name;
            if (_gameBeat || SceneType.TutorialReact == _sceneCurrent || SceneType.TutorialRotate == _sceneCurrent)
            {
                if (SceneType.Hard != _sceneCurrent && SceneType.Replay != _sceneCurrent && SceneType.Menu != _sceneCurrent && SceneType.Credits != _sceneCurrent)
                {
                    Instantiate(ButtonSkip);
                }
            }

            if (_gameBeat && SceneType.Menu == _sceneCurrent)
            {
                Instantiate(ButtonCredits);
            }

            GenerateLevel();
        }

        void Update()
        {
            switch (GetState())
            {
                case _state.Win:
                    if (SceneType.Hard == _sceneCurrent)
                    {
                        _gameBeat = true;
                    }

                    DisableCells();
                    Transitioner.TransitionOut();

                    break;
                case _state.Run:
                    while (CellsActivated.Count > 0)
                    {
                        CellsActivated.Dequeue().React();
                    }

                    break;
                case _state.Idle:
                    break;
                default:
                    Debug.Log("unexpected state");
                    break;
            }
        }

        private void DisableCells()
        {
            for (int x = 0; x < Cells.GetLength(0); ++x)
            {
                for (int y = 0; y < Cells.GetLength(1); ++y)
                {
                    if (Cells[x, y])
                    {
                        Cells[x, y].Disabled = true;
                    }
                }
            }

            enabled = false;
        }

        private _state GetState()
        {
            if (Cells.GetLength(0) == 0 && Cells.GetLength(1) == 0)
            {
                return _state.Idle;
            }

            for (int x = 0; x < Cells.GetLength(0); ++x)
            {
                for (int y = 0; y < Cells.GetLength(1); ++y)
                {
                    if (Cells[x, y] && !Cells[x, y].Tessellated())
                    {
                        return _state.Run;
                    }
                }
            }


            return _state.Win;
        }

        private void GenerateLevel()
        {
            GameObject board = Instantiate(Board);
            board.name = "Board";

            switch (_sceneCurrent)
            {
                case SceneType.TutorialRotate:
                    Cells = new Cell[7, 7];
                    int rotateOffsetX = 1;
                    int rotateOffsetY = rotateOffsetX;
                    board.transform.position = new Vector3(-5.0f, 0.0f, 0.0f);
                    PopulateCell(board, 0 + rotateOffsetX, 4 + rotateOffsetY).RotateLeft();
                    PopulateCell(board, 1 + rotateOffsetX, 3 + rotateOffsetY).RotateRight().RotateRight();
                    PopulateCell(board, 2 + rotateOffsetX, 2 + rotateOffsetY).RotateLeft();
                    PopulateCell(board, 3 + rotateOffsetX, 1 + rotateOffsetY);
                    PopulateCell(board, 4 + rotateOffsetX, 0 + rotateOffsetY).RotateLeft();

                    FillObstacles(board, (x, y) => x + y > 4 && x + y < 8 && !(x == 6 && y == 0) && !(x == 0 && y == 6));

                    int max = 5;
                    for (int i = 0; i < max; ++i)
                    {
                        PopulateObject(board, LilyPad, i, 0);
                        PopulateObject(board, LilyPad, max - 1 + 2, i + 2);
                    }

                    PopulateObject(board, LilyPad, 6, 0);

                    break;
                case SceneType.TutorialReact:
                    Cells = new Cell[7, 7];
                    int ox = 1;
                    int oy = -1;
                    board.transform.position = new Vector3(-1.0f, 0.0f, 0.0f);
                    PopulateCell(board, 1 + ox, 5 + oy).RotateRight();
                    PopulateCell(board, 1 + ox, 6 + oy);
                    PopulateCell(board, 0 + ox, 5 + oy).RotateRight().RotateRight();
                    PopulateCell(board, 0 + ox, 6 + oy).RotateLeft();

                    PopulateCell(board, 3, 3);
                    FillObstacles(board, (x, y) => !(x == 5 || x == 6 || y == 0 || y == 1) && x + y > 3 && x + y < 9);
                    break;
                case SceneType.Easy:
                    Cells = new Cell[7, 7];
                    board.transform.position = new Vector3(-4.0f, 0.0f, 0.0f);
                    for (int i = 0; i < 6; ++i)
                    {
                        PopulateCell(board, 6, i + 1).RotateRight().RotateRight();
                        PopulateCell(board, i, 0);
                    }

                    PopulateCell(board, 6, 0).RotateLeft();
                    FillObstacles(board, (x, y) => x - y > -1);
                    break;
                case SceneType.Medium:
                    Cells = new Cell[7, 7];
                    board.transform.position = new Vector3(-4.0f, 0.0f, 0.0f);
                    for (int i = 0; i < 6; ++i)
                    {
                        PopulateCell(board, 0, i + 1).Randomize();
                        PopulateCell(board, 6, i).Randomize();
                        PopulateCell(board, i, 0).Randomize();
                        PopulateCell(board, i + 1, 6).Randomize();
                    }

                    int offsetInner = 2;
                    for (int i = 0; i < 2; ++i)
                    {
                        PopulateCell(board, 0 + offsetInner, i + 1 + offsetInner).Randomize();
                        PopulateCell(board, 2 + offsetInner, i + offsetInner).Randomize();
                        PopulateCell(board, i + offsetInner, 0 + offsetInner).Randomize();
                        PopulateCell(board, i + offsetInner + 1, 2 + offsetInner).Randomize();
                    }

                    PopulateCell(board, 3, 3).Randomize();

                    FillObstacles(board, (x, y) => true);
                    break;
                case SceneType.Hard:
                    int size = 13;
                    Cells = new Cell[size, size];
                    board.transform.position = new Vector3(-4.0f, 0.0f, 0.0f);
                    for (int x = 0; x < size; ++x)
                    {
                        for (int y = 0; y < size; ++y)
                        {
                            if (x + y > 5 && x + y < 19 && y - x < 7)
                            {
                                PopulateCell(board, x, y).Randomize();
                            }
                        }
                    }

                    break;
                case SceneType.Credits:
                case SceneType.Replay:
                case SceneType.Menu:
                    Cells = new Cell[0, 0];
                    break;
                default:
                    Debug.Log("unexpected level");
                    break;
            }

            Transitioner.TransitionIn();
        }

        private Piece PopulateObject(GameObject currentBoard, Piece gameObject, int x, int y)
        {
            Piece result = Instantiate(gameObject, currentBoard.transform);

            result.X = x;
            result.Y = y;

            result.name = "decor" + " " + x + ":" + y;

            return result;
        }

        private Cell PopulateCell(GameObject currentBoard, int x, int y)
        {
            Cell cell = Instantiate(Cell, currentBoard.transform);
            if (!Cells[x, y])
            {
                cell.name = "cell:" + x + ":" + y;
                cell.X = x;
                cell.Y = y;
                Cells[x, y] = cell;
            }
            else
            {
                cell.X = cell.Y = InvalidOffset;
                cell.name = "degenerate cell:" + x + ":" + y;
                Debug.Log("cell already populated at: " + x + ":" + y);
            }

            return cell;
        }

        private void FillObstacles(GameObject board, Func<int, int, bool> condition)
        {
            for (int x = 0; x < Cells.GetLength(0); ++x)
            {
                for (int y = 0; y < Cells.GetLength(1); ++y)
                {
                    if (!Cells[x, y])
                    {
                        if (condition(x, y))
                        {
                            Obstacle currentCell = Instantiate(LilyPad, board.transform);
                            currentCell.X = x;
                            currentCell.Y = y;
                        }
                    }
                }
            }
        }
    }
}
