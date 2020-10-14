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
    public Button Button;

    public static Cell[,] Cells;
    public static Queue<Cell> CellsActivated = new Queue<Cell>();

    private enum State { WIN, RUN, IDLE };

    private string sceneCurrent;

    private const string TUTORIAL_ROTATE = "TutorialRotate";
    private const string TUTORIAL_REACT = "TutorialReact";
    private const string EASY = "Easy";
    private const string MEDIUM = "Medium";
    private const string HARD = "Hard";
    private const string REPLAY = "Replay";

    private bool nextLevelButtonShown = false;


    void Start()
    {
        sceneCurrent = SceneManager.GetActiveScene().name;
        GenerateLevel();
        Log.Output("main initialized: ", sceneCurrent);
    }

    void Update()
    {
        switch (GetState())
        {
            case State.WIN:
                if (HARD.Equals(sceneCurrent))
                {
                    SceneManager.LoadScene(REPLAY);
                }
                else if (!nextLevelButtonShown)
                {
                    Instantiate(Button);
                    DisableCells();
                    nextLevelButtonShown = true;
                }
                break;
            case State.RUN:
                while (CellsActivated.Count > 0)
                {
                    CellsActivated.Dequeue().React();
                }
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
    }
    private State GetState()
    {
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
            case (TUTORIAL_ROTATE):
                Cells = new Cell[7, 7];
                int rotateOffsetX = 1;
                int rotateOffsetY = rotateOffsetX;
                board.transform.position = new Vector3(-3.5f, -2f, 0f);
                PopulateCell(board, 0 + rotateOffsetX, 4 + rotateOffsetY);
                PopulateCell(board, 1 + rotateOffsetX, 3 + rotateOffsetY);
                PopulateCell(board, 2 + rotateOffsetX, 2 + rotateOffsetY);
                PopulateCell(board, 3 + rotateOffsetX, 1 + rotateOffsetY);
                PopulateCell(board, 4 + rotateOffsetX, 0 + rotateOffsetY);
                PopulateRock(board, (x, y) => x + y > 4 && x + y < 8 && !(x == 6 && y == 0) && !(x == 0 && y == 6));
                break;
            case (TUTORIAL_REACT):
                Cells = new Cell[7, 7];
                int ox = 1;
                int oy = -1;
                board.transform.position = new Vector3(-1f, -3f, 0f);
                PopulateCell(board, 1 + ox, 5 + oy);
                PopulateCell(board, 1 + ox, 6 + oy);
                PopulateCell(board, 0 + ox, 5 + oy);
                PopulateCell(board, 0 + ox, 6 + oy);

                PopulateCell(board, 3, 3);
                PopulateRock(board, (x, y) => !(x == 5 || x == 6 || y == 0 || y == 1) && x + y > 3 && x + y < 9);
                break;
            case (EASY):
                Cells = new Cell[7, 7];
                board.transform.position = new Vector3(-4f, 0f, 0f);
                for (int i = 0; i < 6; ++i)
                {
                    PopulateCell(board, 6, i + 1);
                    PopulateCell(board, i, 0);
                }
                PopulateCell(board, 6, 0);
                PopulateRock(board, (x, y) => true);
                break;
            case (MEDIUM):
                Cells = new Cell[7, 7];
                board.transform.position = new Vector3(-4f, 0f, 0f);
                for (int i = 0; i < 6; ++i)
                {
                    PopulateCell(board, 0, i + 1);
                    PopulateCell(board, 6, i);
                    PopulateCell(board, i, 0);
                    PopulateCell(board, i + 1, 6);
                }
                int offsetInner = 2;
                for (int i = 0; i < 2; ++i)
                {
                    PopulateCell(board, 0 + offsetInner, i + 1 + offsetInner);
                    PopulateCell(board, 2 + offsetInner, i + offsetInner);
                    PopulateCell(board, i + offsetInner, 0 + offsetInner);
                    PopulateCell(board, i + offsetInner + 1, 2 + offsetInner);
                }
                PopulateRock(board, (x, y) => true);
                break;
            case (HARD):
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
            default:
                Debug.Log("unexpected level");
                break;
        }
    }
    private void PopulateCell(GameObject currentBoard, int x, int y)
    {
        if (!Cells[x, y])
        {
            Cell cell;
            cell = Instantiate(Cell, currentBoard.transform);
            cell.name = x + ":" + y;
            cell.X = x;
            cell.Y = y;
            Cells[x, y] = cell;
        }
        else
        {
            Log.Output("cell already populated at", x, y);
        }
    }
    private void PopulateRock(GameObject board, Func<int, int, bool> condition)
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
