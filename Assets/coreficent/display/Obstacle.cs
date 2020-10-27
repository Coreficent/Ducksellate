using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

namespace Coreficent.Display
{
    public class Obstacle : Piece 
    {
        void Start()
        {
            RandomizeDelta();
            Reposition();
        }
    }
}