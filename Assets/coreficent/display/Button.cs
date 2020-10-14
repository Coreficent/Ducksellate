using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public string scene;
    void OnMouseOver()
    {
        Log.Output("scene to load", scene);
        if (Input.GetMouseButtonDown(0))
        {
            if ("Exit".Equals(scene))
            {
                Application.Quit();
            }
            else if ("Next".Equals(scene) || "Skip".Equals(scene))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }
    }
}
