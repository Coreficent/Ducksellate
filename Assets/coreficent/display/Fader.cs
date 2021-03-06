﻿namespace Coreficent.Display
{
    using Coreficent.Utility;
    using UnityEngine;

    public class Fader : Piece
    {
        public SpriteRenderer SpriteRenderer;

        private Color colorMax = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private Color colorMin = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        void Start()
        {
            SanityCheck.Check(this, SpriteRenderer);
        }

        public override void TransitionOut()
        {
            if (!TransitionOutComplete())
            {
                SpriteRenderer.material.color -= new Color(0.0f, 0.0f, 0.0f, 0.75f * Time.deltaTime);
            }
            else
            {
                Minimize();
            }
        }

        public override void TransitionIn()
        {
            if (!TransitionInComplete())
            {
                SpriteRenderer.material.color += new Color(0.0f, 0.0f, 0.0f, 0.75f * Time.deltaTime);
            }
            else
            {
                Maximize();
            }
        }

        public override bool TransitionOutComplete()
        {
            return SpriteRenderer.material.color.a <= 0.01f;
        }

        public override bool TransitionInComplete()
        {
            return SpriteRenderer.material.color.a >= 0.99f;
        }

        public override void Maximize()
        {
            SpriteRenderer.material.color = colorMax;
        }

        public override void Minimize()
        {
            SpriteRenderer.material.color = colorMin;
        }
    }
}