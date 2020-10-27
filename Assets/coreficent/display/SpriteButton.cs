using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coreficent.Display
{
    public class SpriteButton : MonoBehaviour
    {
        public string scene;
        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (scene)
                {
                    case "Exit":
                        Application.Quit();
                        break;
                    case "Next":
                        Next();
                        break;
                    case "Skip":
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        break;
                    default:
                        SceneManager.LoadScene(scene);
                        break;
                }
            }
        }

        private void Next()
        {
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            Piece[] pieces = FindObjectsOfType<Piece>();

            bool shrunkenAll;
            do
            {
                shrunkenAll = true;

                foreach (Piece piece in pieces)
                {
                    if (!piece.Shrinken())
                    {
                        piece.Shrink();
                        shrunkenAll = false;
                    }
                }
                yield return null;
            } while (!shrunkenAll);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}