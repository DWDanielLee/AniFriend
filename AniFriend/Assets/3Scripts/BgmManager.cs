using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance;
    
    AudioSource audioSource;
    
    [SerializeField] private AudioClip menuSceneMusic;
    [SerializeField] private AudioClip playSceneMusic;

    private BgmManager[] bgms;

    private void Awake()
    {
        Instance = this;
        bgms = FindObjectsOfType<BgmManager>();
         if(bgms.Length > 1)
             Destroy(bgms[1].gameObject);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuSceneMusic;
        audioSource.Play();
        
        DontDestroyOnLoad(gameObject);

    }

    public void StartPlaySceneMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = playSceneMusic;
            audioSource.Play();
        }
        
    }

    public void StartMenuSceneMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = menuSceneMusic;
            audioSource.Play();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
