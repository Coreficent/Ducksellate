using Coreficent.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Coreficent.Audio
{
    public class BackgroundMusic : MonoBehaviour
    {
        public AudioSource CCCP;
        public AudioSource FluffingADuck;

        private static BackgroundMusic backgroundMusic = null;

        void Start()
        {
            SanityCheck.Check(this, CCCP, FluffingADuck);

            PlayTrack(SceneManager.GetActiveScene());
            SceneManager.activeSceneChanged += UpdateMusic;
            FluffingADuck.Play();
        }
        private void Awake()
        {
            if (!backgroundMusic)
            {
                backgroundMusic = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            else if (backgroundMusic == this)
            {
                return;
            }
            Destroy(gameObject);
        }
        private void UpdateMusic(Scene current, Scene next)
        {
            PlayTrack(next);
        }
        private void PlayTrack(Scene scene)
        {
            if (Display.SceneType.CREDITS == scene.name)
            {
                if (FluffingADuck.isPlaying)
                {
                    FluffingADuck.Stop();
                    CCCP.Play();
                }
            }
            else
            {
                if (CCCP.isPlaying)
                {
                    CCCP.Stop();
                    FluffingADuck.Play();
                }
            }
        }
    }
}