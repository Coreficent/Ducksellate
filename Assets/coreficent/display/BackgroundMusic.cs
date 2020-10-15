using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic singleton = null;

    private void Awake()
    {
        if (!singleton)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else if (singleton == this)
        {
            return;
        }
        Destroy(gameObject);
    }
}
