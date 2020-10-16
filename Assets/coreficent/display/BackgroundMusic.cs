using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource CCCP;
    public AudioSource FluffingADuck;

    private static BackgroundMusic backgroundMusic = null;
    private string sceneCurrent;

    void Start()
    {
        sceneCurrent = SceneManager.GetActiveScene().name;
        if (SceneType.CREDIT.Equals(sceneCurrent))
        {
            CCCP.Play();
        }
        else
        {
            FluffingADuck.Play();
        }
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

    void Update()
    {
        if (SceneType.CREDIT.Equals(sceneCurrent))
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
