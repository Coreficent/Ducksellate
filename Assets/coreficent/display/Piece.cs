using UnityEngine;
using Coreficent.Animation;

namespace Coreficent.Display
{
    public abstract class Piece : MonoBehaviour, ITransitionable
    {
        public int X { get; set; }
        public int Y { get; set; }

        protected readonly float rotationAngle = 90.0f;
        protected readonly float boardAngle = 45.0f;

        private readonly float space = 1.0f;
        private Vector3 delta = new Vector3(1.0f, 1.0f, 1.0f);

        public virtual void TransitionOut()
        {
            if (!TransitionOutComplete())
            {
                transform.localScale -= delta * Time.deltaTime;
            }
            else
            {
                Minimize();
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
                Maximize();
            }
        }
        public virtual bool TransitionOutComplete()
        {
            return transform.localScale.x <= 0.01 && transform.localScale.y <= 0 && transform.localScale.z <= 0.01;
        }
        public virtual bool TransitionInComplete()
        {
            return transform.localScale.x >= 0.99 && transform.localScale.y >= 1.0 && transform.localScale.z >= 0.99;
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
            transform.position += new Vector3(X * space - (Main.Main.Cells.GetLength(0) - 1) / 2.0f, Y * space - (Main.Main.Cells.GetLength(1) - 1) / 2.0f, 0.0f);
            transform.RotateAround(transform.parent.position, Vector3.forward, boardAngle);
        }
        protected void RandomizeDelta()
        {
            delta *= Random.Range(0.5f, 2.0f);
        }
    }
}