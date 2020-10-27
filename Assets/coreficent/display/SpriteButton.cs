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
                        StartCoroutine(Transitioner.TransitionOut());
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
    }
}