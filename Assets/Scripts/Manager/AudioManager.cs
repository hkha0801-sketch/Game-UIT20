using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource backgroundAudioSource;
    public AudioClip[] bgmList;
    private int currentMusicIndex = 0;

    private const string BGM_VOLUME_KEY = "BGM_Volume_Save";
    private float currentBgmVolume = 1f;

    [Header("SFX Pooling")]
    [SerializeField] private GameObject sfxPrefab;

    private void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject);

            sfxPrefab = new GameObject("SFX_Pool_Prefab");
            sfxPrefab.AddComponent<AudioSource>();
            sfxPrefab.SetActive(false);
            DontDestroyOnLoad(sfxPrefab);
        }
        else Destroy(gameObject);

        LoadVolume();
    }

    private void OnEnable() { SceneManager.sceneLoaded += OnSceneChange; }
    private void OnDisable() { SceneManager.sceneLoaded -= OnSceneChange; }

    private void Start()
    {
        AudioListener.volume = 1f; 
        PlayMusic(currentMusicIndex);
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.ToLower() == "endingscene")
        {
            StopMusic();
        }
        else
        {
            if (!backgroundAudioSource.isPlaying)
            {
                PlayMusic(currentMusicIndex);
            }
        }
    }

    public void PlayMusic(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;
        currentMusicIndex = index;
        backgroundAudioSource.clip = bgmList[index];
        backgroundAudioSource.loop = true;
        backgroundAudioSource.volume = currentBgmVolume;
        backgroundAudioSource.Play();
    }

    public void StopMusic()
    {
        backgroundAudioSource.Stop();
    }

    public void IncreaseVolume()
    {
        currentBgmVolume = Mathf.Clamp01(currentBgmVolume + 0.1f);
        UpdateSourceVolume();
    }

    public void DecreaseVolume()
    {
        currentBgmVolume = Mathf.Clamp01(currentBgmVolume - 0.1f);
        UpdateSourceVolume();
    }

    private void UpdateSourceVolume()
    {
        backgroundAudioSource.volume = currentBgmVolume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, currentBgmVolume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        currentBgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        backgroundAudioSource.volume = currentBgmVolume;
    }

    public void NextMusic()
    {
        currentMusicIndex++;
        if (currentMusicIndex >= bgmList.Length) currentMusicIndex = 0;
        PlayMusic(currentMusicIndex);
    }

    public string GetCurrentMusicName()
    {
        if (bgmList.Length == 0) return "No Music";
        return bgmList[currentMusicIndex].name;
    }

    public void PlaySFX(AudioClip clip, float volume, float pitch)
    {
        if (ObjectPoolManager.Instance == null || clip == null) return;

        GameObject sfxObj = ObjectPoolManager.Instance.Spawn(sfxPrefab, Vector3.zero, Quaternion.identity);
        AudioSource source = sfxObj.GetComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = 0f; 

        source.Play();
        
        StartCoroutine(ReturnSFXToPool(sfxObj, clip.length + 0.1f));
    }

    private IEnumerator ReturnSFXToPool(GameObject sfxObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (sfxObj != null)
        {
            PooledObject pooledObj = sfxObj.GetComponent<PooledObject>();
            if (pooledObj != null)
            {
                pooledObj.DestroyOrReturn();
            }
        }
    }
}