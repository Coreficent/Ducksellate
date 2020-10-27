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
        public string Scene;
        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (Scene)
                {
                    case "Quit":
                        Application.Quit();
                        break;
                    case "Start":
                    case "Next":
                    case "Skip":
                        Transitioner.TransitionOut();
                        break;
                    default:
                        SceneManager.LoadScene(Scene);
                        break;
                }
            }
        }
    }
}