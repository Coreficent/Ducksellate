using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Cell Cell;
    public Obstacle Obstacle;
    public GameObject Board;
    public Button ButtonNext;
    public Button ButtonSkip;
    public Button ButtonCredits;

    public static Cell[,] Cells = new Cell[0, 0];
    public static Queue<Cell> CellsActivated = new Queue<Cell>();

    private static bool gameBeat = false;
    private enum State { WIN, RUN, IDLE };

    private string sceneCurrent;

    void Start()
    {
        sceneCurrent = SceneManager.GetActiveScene().name;
        GenerateLevel();
        if (!SceneType.HARD.Equals(sceneCurrent) && gameBeat || SceneType.TUTORIAL_REACT.Equals(sceneCurrent) || SceneType.TUTORIAL_ROTATE.Equals(sceneCurrent))
        {
            Instantiate(ButtonSkip);
        }
        if (gameBeat && SceneType.MENU.Equals(sceneCurrent))
        {
            Instantiate(ButtonCredits);
        }
        Log.Output("main initialized: ", sceneCurrent);
    }

    void Update()
    {
        switch (GetState())
        {
            case State.WIN:
                if (SceneType.HARD.Equals(sceneCurrent))
                {
                    gameBeat = true;
                    SceneManager.LoadScene(SceneType.REPLAY);
                }
                else
                {
                    Instantiate(ButtonNext);
                    DisableCells();
                }
                break;
            case State.RUN:
                while (CellsActivated.Count > 0)
                {
                    CellsActivated.Dequeue().React();
                }
                break;
            case State.IDLE:
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
    private State GetState()
    {
        if (Cells.GetLength(0) == 0 && Cells.GetLength(1) == 0)
        {
            return State.IDLE;
        }

        for (int x = 0; x < Cells.GetLength(0); ++x)
        {
            for (int y = 0; y < Cells.GetLength(1); ++y)
            {
                if (Cells[x, y] && !Cells[x, y].Tessellated())
                {
                    return State.RUN;
                }
            }
        }


        return State.WIN;
    }

    private void GenerateLevel()
    {
        GameObject board = Instantiate(Board);
        board.name = "Board";

        switch (sceneCurrent)
        {
            case (SceneType.TUTORIAL_ROTATE):
                Cells = new Cell[7, 7];
                int rotateOffsetX = 1;
                int rotateOffsetY = rotateOffsetX;
                board.transform.position = new Vector3(-2f, 0f, 0f);
                PopulateCell(board, 0 + rotateOffsetX, 4 + rotateOffsetY).RotateLeft();
                PopulateCell(board, 1 + rotateOffsetX, 3 + rotateOffsetY).RotateRight().RotateRight();
                PopulateCell(board, 2 + rotateOffsetX, 2 + rotateOffsetY).RotateLeft();
                PopulateCell(board, 3 + rotateOffsetX, 1 + rotateOffsetY);
                PopulateCell(board, 4 + rotateOffsetX, 0 + rotateOffsetY).RotateLeft();

                if (Log.DEBUG)
                {
                    PopulateCell(board, 1, 5);
                    PopulateCell(board, 1, 5);
                }

                PopulateObstacle(board, (x, y) => x + y > 4 && x + y < 8 && !(x == 6 && y == 0) && !(x == 0 && y == 6));
                break;
            case (SceneType.TUTORIAL_REACT):
                Cells = new Cell[7, 7];
                int ox = 1;
                int oy = -1;
                board.transform.position = new Vector3(-1f, 0f, 0f);
                PopulateCell(board, 1 + ox, 5 + oy).RotateRight();
                PopulateCell(board, 1 + ox, 6 + oy);
                PopulateCell(board, 0 + ox, 5 + oy).RotateRight().RotateRight();
                PopulateCell(board, 0 + ox, 6 + oy).RotateLeft();

                PopulateCell(board, 3, 3);
                PopulateObstacle(board, (x, y) => !(x == 5 || x == 6 || y == 0 || y == 1) && x + y > 3 && x + y < 9);
                break;
            case (SceneType.EASY):
                Cells = new Cell[7, 7];
                board.transform.position = new Vector3(-4f, 0f, 0f);
                for (int i = 0; i < 6; ++i)
                {
                    PopulateCell(board, 6, i + 1).RotateRight().RotateRight();
                    PopulateCell(board, i, 0);
                }
                PopulateCell(board, 6, 0).RotateLeft();
                PopulateObstacle(board, (x, y) => x - y > -1);
                break;
            case (SceneType.MEDIUM):
                Cells = new Cell[7, 7];
                board.transform.position = new Vector3(-4f, 0f, 0f);
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

                PopulateObstacle(board, (x, y) => true);
                break;
            case (SceneType.HARD):
                int size = 13;
                Cells = new Cell[size, size];
                board.transform.position = new Vector3(-4f, 0f, 0f);
                for (int x = 0; x < size; ++x)
                {
                    for (int y = 0; y < size; ++y)
                    {
                        if (x + y > 5 && x + y < 19 && y - x < 7)
                        {
                            Cell currentCell = Instantiate(Cell, board.transform);
                            currentCell.X = x;
                            currentCell.Y = y;
                            currentCell.Randomize();
                            Cells[x, y] = currentCell;
                        }

                    }
                }
                break;
            case (SceneType.CREDITS):
                Cells = new Cell[0, 0];
                break;
            case (SceneType.MENU):
                Cells = new Cell[0, 0];
                break;
            default:
                Debug.Log("unexpected level");
                break;
        }
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
            cell.X = cell.Y = -1024;
            cell.name = "degenerate cell:" + x + ":" + y;
            Log.Output("cell already populated at", x, y);
        }
        return cell;
    }
    private void PopulateObstacle(GameObject board, Func<int, int, bool> condition)
    {
        for (int x = 0; x < Cells.GetLength(0); ++x)
        {
            for (int y = 0; y < Cells.GetLength(1); ++y)
            {
                if (!Cells[x, y])
                {
                    if (condition(x, y))
                    {
                        Obstacle currentCell = Instantiate(Obstacle, board.transform);
                        currentCell.X = x;
                        currentCell.Y = y;
                    }
                }
            }
        }
    }
}
