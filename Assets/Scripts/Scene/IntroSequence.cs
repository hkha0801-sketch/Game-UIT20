using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [Header("Intro Settings")]
    public NPCController LanController;
    public MedalSO IntroCompleteFlag;

    private void Start()
    {
        if (MedalManager.Instance != null && IntroCompleteFlag != null)
        {
            if (!MedalManager.Instance.ownedMedals.Contains(IntroCompleteFlag))
            {
                StartCoroutine(AutoStartDialogue());
            }
            else
            {
                LanController.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator AutoStartDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (LanController != null)
        {
            LanController.Interact(); 
        }
    }

    public void FinishIntro()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.PlayTransition(() => 
            {
                LanController.gameObject.SetActive(false);

                if (MedalManager.Instance != null && IntroCompleteFlag != null)
                {
                    MedalManager.Instance.AddMedal(IntroCompleteFlag);
                }
                
                if (SaveManager.Instance != null)
                {
                    SaveManager.Instance.SaveGame();
                }
            });
        }
    }
}