using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MapPointData
{
    public string sceneName;
    public Image pointImage;
}

public class MinimapPoint : MonoBehaviour
{
    public Sprite currentSceneSprite;
    public Sprite otherSceneSprite;
    public List<MapPointData> mapPoints;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateMapPoints(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMapPoints(scene.name);
    }

    public void UpdateMapPoints(string currentSceneName)
    {
        foreach (MapPointData data in mapPoints)
        {
            if (data.pointImage != null)
            {
                if (data.sceneName == currentSceneName)
                {
                    data.pointImage.sprite = currentSceneSprite;
                }
                else
                {
                    data.pointImage.sprite = otherSceneSprite;
                }
            }
        }
    }
}