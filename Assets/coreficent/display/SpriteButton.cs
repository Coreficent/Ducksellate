namespace Coreficent.Display
{
    using Coreficent.Animation;
    using Coreficent.Utility;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SpriteButton : MonoBehaviour
    {
        public string Scene;

        void Start()
        {
            SanityCheck.Check(this, Scene);
        }

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