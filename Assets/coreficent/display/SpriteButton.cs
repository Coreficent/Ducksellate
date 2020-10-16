using UnityEngine;
using UnityEngine.SceneManagement;

public class SpriteButton : MonoBehaviour
{
    public string scene;
    void OnMouseOver()
    {
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
