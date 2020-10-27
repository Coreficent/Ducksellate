using System.Collections;
using UnityEngine.SceneManagement;
using Coreficent.Display;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Coreficent.Animation
{
    public class Transitioner
    {
        public static IEnumerator TransitionIn()
        {
            Piece[] pieces = Object.FindObjectsOfType<Piece>();

            // new List<MonoBehaviour>(Object.FindObjectsOfType<MonoBehaviour>()).Where(i => i is ITransitionable).ToList();

            foreach (Piece piece in pieces)
            {
                if (!piece.TransitionOutComplete())
                {
                    piece.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }

            bool expandedAll;
            do
            {
                expandedAll = true;

                foreach (Piece piece in pieces)
                {
                    if (!piece.TransitionInComplete())
                    {
                        piece.TransitionIn();
                        expandedAll = false;
                    }
                }
                yield return null;
            } while (!expandedAll);
        }

        public static IEnumerator TransitionOut()
        {
            Piece[] pieces = UnityEngine.Object.FindObjectsOfType<Piece>();

            bool shrunkenAll;
            do
            {
                shrunkenAll = true;

                foreach (Piece piece in pieces)
                {
                    if (!piece.TransitionOutComplete())
                    {
                        piece.TransitionOut();
                        shrunkenAll = false;
                    }
                }
                yield return null;
            } while (!shrunkenAll);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

