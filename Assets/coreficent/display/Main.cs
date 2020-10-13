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

    private enum State { WIN, RUN };

    private string sceneCurrent;
    private const string MAIN = "Main";
    private const string TUTORIAL_ROTATE = "TutorialRotate";
    private const string TUTORIAL_REACT = "TutorialReact";
    private const string REPLAY = "Replay";


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
                Debug.Log("won");
                Button buttonCurrent = Instantiate(Button); ;
                switch (sceneCurrent)
                {
                    case TUTORIAL_ROTATE:
                        buttonCurrent.scene = TUTORIAL_REACT;
                        DisableCells();
                        break;
                    case TUTORIAL_REACT:
                        buttonCurrent.scene = MAIN;
                        DisableCells();
                        break;
                    case MAIN:
                        buttonCurrent.scene = REPLAY;
                        DisableCells();
                        break;
                    default:
                        Log.Output("unexpected scene");
                        break;
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
        GameObject currentBoard = Instantiate(Board);
        currentBoard.name = "Board";

        switch (sceneCurrent)
        {
            case (MAIN):
                int size = 13;
                Cells = new Cell[size, size];
                currentBoard.transform.position = new Vector3(-4f, 0f, 0f);
                for (int x = 0; x < size; ++x)
                {
                    for (int y = 0; y < size; ++y)
                    {
                        if (x + y > 5 && x + y < 19 && y - x < 7)
                        {
                            Cell currentCell = Instantiate(Cell, currentBoard.transform);
                            currentCell.X = x;
                            currentCell.Y = y;
                            currentCell.Randomize();
                            Cells[x, y] = currentCell;
                        }

                    }
                }
                break;
            case (TUTORIAL_ROTATE):
                Cells = new Cell[7, 7];
                int rotateOffsetX = 1;
                int rotateOffsetY = rotateOffsetX;
                currentBoard.transform.position = new Vector3(-3.5f, -2f, 0f);
                PopulateCell(currentBoard, 0 + rotateOffsetX, 4 + rotateOffsetY);
                PopulateCell(currentBoard, 1 + rotateOffsetX, 3 + rotateOffsetY);
                PopulateCell(currentBoard, 2 + rotateOffsetX, 2 + rotateOffsetY);
                PopulateCell(currentBoard, 3 + rotateOffsetX, 1 + rotateOffsetY);
                PopulateCell(currentBoard, 4 + rotateOffsetX, 0 + rotateOffsetY);
                PopulateRock(currentBoard, (x, y) => x + y > 4 && x + y < 8 && !(x == 6 && y == 0) && !(x == 0 && y == 6));
                break;
            case (TUTORIAL_REACT):
                Cells = new Cell[7, 7];
                int ox = 1;
                int oy = -1;
                currentBoard.transform.position = new Vector3(-1f, -3f, 0f);
                PopulateCell(currentBoard, 1 + ox, 5 + oy);
                PopulateCell(currentBoard, 1 + ox, 6 + oy);
                PopulateCell(currentBoard, 0 + ox, 5 + oy);
                PopulateCell(currentBoard, 0 + ox, 6 + oy);

                PopulateCell(currentBoard, 3, 3);
                PopulateRock(currentBoard, (x, y) => !(x == 5 || x == 6 || y == 0 || y == 1) && x + y > 3 && x + y < 9);
                break;
            default:
                Debug.Log("unexpected level");
                break;
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

    private void PopulateCell(GameObject currentBoard, int x, int y)
    {
        Cell cellRotate;
        cellRotate = Instantiate(Cell, currentBoard.transform);
        cellRotate.X = x;
        cellRotate.Y = y;
        Cells[x, y] = cellRotate;
    }
}
