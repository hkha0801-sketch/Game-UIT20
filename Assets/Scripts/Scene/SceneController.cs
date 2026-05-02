using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene(sceneName);});
    }

    public void PlayGame()
    {
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene("Map B");});
    }

    public void MainMenu()
    {
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene("MenuScene");});
    }

    public void EnterArea()
    {
        SceneTransition.Instance.PlayTransition(() => {SceneManager.LoadScene("GameScene");});
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        SceneTransition.Instance.PlayTransition(() => {Application.Quit();});
    } 
}