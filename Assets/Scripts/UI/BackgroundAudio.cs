using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    private static BackgroundAudio instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
            
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}