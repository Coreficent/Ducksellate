using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Cell cell;

    public static Cell[,] cells;

    public static Queue<Cell> activatedCells = new Queue<Cell>();

    // Start is called before the first frame update
    void Start()
    {
        Log.Output("main initialized");

        GenerateLevel();
    }
    // Update is called once per frame
    void Update()
    {
        if (activatedCells.Count > 0)
        {
            while (activatedCells.Count > 0)
            {
                activatedCells.Dequeue().React();
            }
        }
    }
    private void GenerateLevel()
    {
        int size = 6;
        cells = new Cell[size, size];
        for (int x = 0; x < size; ++x)
        {
            for (int y = 0; y < size; ++y)
            {
                Cell current = Instantiate(cell);
                current.X = x;
                current.Y = y;
                cells[x, y] = current;
            }
        }
    }

}
