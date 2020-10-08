using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Cell cell;

    public static Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        Log.Output("main initialized");

        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int size = 10;
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
