using System.Collections;
using UnityEngine.SceneManagement;
using Coreficent.Display;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Coreficent.Animation
{
    public class Transitioner : MonoBehaviour
    {
        public static void TransitionIn()
        {
            Main.Main.Handler.StopAllCoroutines();
            Main.Main.Handler.StartCoroutine(TransitionRoutineIn());
        }

        public static void TransitionOut(string scene = null)
        {
            Main.Main.Handler.StopAllCoroutines();
            Main.Main.Handler.StartCoroutine(TransitionRoutineOut(scene));
        }

        private static IEnumerator TransitionRoutineIn()
        {
            List<ITransitionable> transitionables = FindTransitionables();

            foreach (ITransitionable transitionable in transitionables)
            {
                transitionable.Minimize();
            }

            bool transitionInComplete;
            do
            {
                transitionInComplete = true;

                foreach (ITransitionable transformable in transitionables)
                {
                    if (!transformable.TransitionInComplete())
                    {
                        transformable.TransitionIn();
                        transitionInComplete = false;
                    }
                }
                yield return null;
            } while (!transitionInComplete);
        }

        private static IEnumerator TransitionRoutineOut(string scene = null)
        {
            List<ITransitionable> transitionables = FindTransitionables();

            bool transitionOutComplete;
            do
            {
                transitionOutComplete = true;

                foreach (Piece transitionable in transitionables)
                {
                    if (!transitionable.TransitionOutComplete())
                    {
                        transitionable.TransitionOut();
                        transitionOutComplete = false;
                    }
                }
                yield return null;
            } while (!transitionOutComplete);

            if (scene == null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }

        private static List<ITransitionable> FindTransitionables()
        {
            return new List<MonoBehaviour>(FindObjectsOfType<MonoBehaviour>()).Where(i => i is ITransitionable).Select(i => (ITransitionable)i).ToList(); ;
        }
    }
}

