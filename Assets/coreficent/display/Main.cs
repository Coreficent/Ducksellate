using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Cell Cell;
    public GameObject Board;

    public static Cell[,] Cells;
    public static Queue<Cell> CellsActivated = new Queue<Cell>();

    private enum State { win, run };

    private string sceneCurrent;
    private const string MAIN = "Main";
    private const string TUTORIAL_ROTATE = "TutorialRotate";
    private const string TUTORIAL_REACT = "TutorialReact";

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
            case State.win:
                Debug.Log("won");
                switch (sceneCurrent)
                {
                    case TUTORIAL_ROTATE:
                        SceneManager.LoadScene(TUTORIAL_REACT);
                        break;
                    case TUTORIAL_REACT:
                        SceneManager.LoadScene(MAIN);
                        break;
                    case MAIN:
                        SceneManager.LoadScene("Replay");
                        break;
                    default:
                        Log.Output("unexpected scene");
                        break;
                }

                break;
            case State.run:
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

    private State GetState()
    {
        for (int x = 0; x < Cells.GetLength(0); ++x)
        {
            for (int y = 0; y < Cells.GetLength(1); ++y)
            {
                if (Cells[x, y] && !Cells[x, y].Tessellated())
                {
                    return State.run;
                }
            }
        }
        return State.win;
    }

    private void GenerateLevel()
    {
        GameObject currentBoard = Instantiate(Board);
        currentBoard.name = "Board";


        int size = 7;
        Cells = new Cell[size, size];
        switch (sceneCurrent)
        {
            case (MAIN):
                currentBoard.transform.position = new Vector3(-2f, 0f, 0f);
                for (int x = 0; x < size; ++x)
                {
                    for (int y = 0; y < size; ++y)
                    {
                        Cell currentCell = Instantiate(Cell, currentBoard.transform);
                        currentCell.X = x;
                        currentCell.Y = y;
                        currentCell.Randomize();
                        Cells[x, y] = currentCell;
                    }
                }
                break;
            case (TUTORIAL_ROTATE):
                currentBoard.transform.position = new Vector3(-4f, -1f, 0f);
                PopulateCell(currentBoard, 0, 4);
                PopulateCell(currentBoard, 1, 3);
                PopulateCell(currentBoard, 2, 2);
                PopulateCell(currentBoard, 3, 1);
                PopulateCell(currentBoard, 4, 0);
                break;
            case (TUTORIAL_REACT):
                currentBoard.transform.position = new Vector3(-1f, -3f, 0f);
                PopulateCell(currentBoard, 1, 5);
                PopulateCell(currentBoard, 1, 6);
                PopulateCell(currentBoard, 0, 5);
                PopulateCell(currentBoard, 0, 6);

                PopulateCell(currentBoard, 3, 3);
                break;
            default:
                Debug.Log("unexpected level");
                break;
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
