using UnityEngine;

namespace Coreficent.Display
{
    public class Fader : Piece
    {
        public SpriteRenderer spriteRenderer;

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
            return spriteRenderer.material.color.a <= 0.01f;
        }
        public override bool TransitionInComplete()
        {
            return spriteRenderer.material.color.a >= 0.99f;
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