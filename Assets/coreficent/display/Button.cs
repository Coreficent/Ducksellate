using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public string scene;
    void OnMouseOver()
    {
        Log.Output("Replay Over");
        if (Input.GetMouseButtonDown(0))
        {
            Log.Output("exit debug", scene);
            Log.Output("exit debug", "Exit");
            Log.Output("exit debug", "Exit".Equals(scene));
            if ("Exit".Equals(scene))
            {
                Application.Quit();
            }
            else
            {
                Log.Output("Replay Scene Loaded");
                SceneManager.LoadScene(scene);
            }
        }
    }
}
