using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    void OnMouseOver()
    {
        Log.Output("Replay Over");
        if (Input.GetMouseButtonDown(0))
        {
            Log.Output("Replay Scene Loaded");
            SceneManager.LoadScene("Main");
        }
    }
}
