using Coreficent.Animation;
using Coreficent.Display;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace Coreficent.Display
{
    public class Fader : Piece
    {
        private SpriteRenderer spriteRenderer;
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void TransitionOut()
        {
            if (!TransitionOutComplete())
            {
                spriteRenderer.material.color -= new Color(0.0f, 0.0f, 0.0f, 0.75f * Time.deltaTime);
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
                spriteRenderer.material.color += new Color(0.0f, 0.0f, 0.0f, 0.75f * Time.deltaTime);
            }
            else
            {
                Maximize();
            }
        }
        public override bool TransitionOutComplete()
        {
            return spriteRenderer.material.color.a <= 0.0f;
        }
        public override bool TransitionInComplete()
        {
            return spriteRenderer.material.color.a >= 1.0f;
        }
        public override void Maximize()
        {
            spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        public override void Minimize()
        {
            spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
}