using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Obstacle : Piece
{
    void Start()
    {
        RandomizeDelta();
        Reposition();
    }
}
