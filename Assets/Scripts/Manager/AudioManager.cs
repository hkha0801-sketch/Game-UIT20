using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource backgroundAudioSource;

    [Header("Music List")]
    public AudioClip[] bgmList;

    private int currentMusicIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(currentMusicIndex);
    }

    public void PlayMusic(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;

        currentMusicIndex = index;

        backgroundAudioSource.clip = bgmList[index];
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void NextMusic()
    {
        currentMusicIndex++;

        if (currentMusicIndex >= bgmList.Length)
        {
            currentMusicIndex = 0;
        }

        PlayMusic(currentMusicIndex);
    }

    public string GetCurrentMusicName()
    {
        if (bgmList.Length == 0)
        {
            return "No Music";
        }

        return bgmList[currentMusicIndex].name;
    }
}