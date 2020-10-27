using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coreficent.Main;
using Coreficent.Display;
using Coreficent.Animation;

namespace Coreficent.Display
{
    public abstract class Piece : MonoBehaviour, ITransitionable
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected readonly float rotationAngle = 90f;
        protected readonly float boardAngle = 45f;

        private readonly float space = 1f;

        private Vector3 delta = new Vector3(1.0f, 1.0f, 1.0f);

        public virtual void TransitionOut()
        {
            if (!TransitionOutComplete())
            {
                transform.localScale -= delta * Time.deltaTime;
            }
            else
            {
                transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            }
        }
        public virtual void TransitionIn()
        {
            if (!TransitionInComplete())
            {
                transform.localScale += delta * Time.deltaTime;
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        public virtual bool TransitionOutComplete()
        {
            return transform.localScale.x <= 0 && transform.localScale.y <= 0 && transform.localScale.z <= 0;
        }
        public virtual bool TransitionInComplete()
        {
            return transform.localScale.x >= 1.0 && transform.localScale.y >= 1.0 && transform.localScale.z >= 1.0;
        }
        public virtual void Maximize()
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        public virtual void Minimize()
        {
            transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }
        protected void Reposition()
        {
            transform.position += new Vector3(X * space - (Main.Main.Cells.GetLength(0) - 1) / 2f, Y * space - (Main.Main.Cells.GetLength(1) - 1) / 2f, 0f);
            transform.RotateAround(transform.parent.position, Vector3.forward, boardAngle);
        }

        protected void RandomizeDelta()
        {
            delta *= Random.Range(0.5f, 2.0f);
        }
    }
}