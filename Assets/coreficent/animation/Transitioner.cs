﻿using System.Collections;
using UnityEngine.SceneManagement;
using Coreficent.Display;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Coreficent.Animation
{
    public class Transitioner
    {
        public static bool TransitionReady = true;
        public static IEnumerator TransitionIn()
        {
            TransitionReady = false;
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
            TransitionReady = true;
        }

        public static IEnumerator TransitionOut()
        {
            TransitionReady = false;
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

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            TransitionReady = true;
        }

        private static List<ITransitionable> FindTransitionables()
        {
            return new List<MonoBehaviour>(Object.FindObjectsOfType<MonoBehaviour>()).Where(i => i is ITransitionable).Select(i => (ITransitionable)i).ToList(); ;
        }
    }
}

