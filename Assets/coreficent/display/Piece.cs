namespace Coreficent.Display
{
    using Coreficent.Animation;
    using UnityEngine;

    public abstract class Piece : MonoBehaviour, ITransitionable
    {
        public int X { get; set; }

        public int Y { get; set; }

        protected readonly float rotationAngle = 90.0f;
        protected readonly float boardAngle = 45.0f;

        private readonly float space = 1.0f;
        private readonly float scaleMaxThreshold = 0.99f;
        private readonly float scaleMinThreshold = 0.01f;
        private Vector3 delta = new Vector3(1.0f, 1.0f, 1.0f);
        private Vector3 scaleMax = new Vector3(1.0f, 1.0f, 1.0f);
        private Vector3 scaleMin = new Vector3(0.0f, 0.0f, 0.0f);


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
            return transform.localScale.x <= scaleMinThreshold && transform.localScale.y <= scaleMinThreshold && transform.localScale.z <= scaleMinThreshold;
        }

        public virtual bool TransitionInComplete()
        {
            return transform.localScale.x >= scaleMaxThreshold && transform.localScale.y >= scaleMaxThreshold && transform.localScale.z >= scaleMaxThreshold;
        }

        public virtual void Maximize()
        {
            transform.localScale = scaleMax;
        }

        public virtual void Minimize()
        {
            transform.localScale = scaleMin;
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