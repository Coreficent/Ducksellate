using Coreficent.Animation;
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
                    case "Skip":
                        if (Transitioner.TransitionReady)
                        {
                            StartCoroutine(Transitioner.TransitionOut());
                        }
                        break;
                    default:
                        SceneManager.LoadScene(scene);
                        break;
                }
            }
        }
    }
}