using System.Collections.Generic;
using UnityEngine;

public class AppearanceCondition : MonoBehaviour
{
    public enum ActionType { Show, Hide }

    public ActionType actionIfMet = ActionType.Show;
    [Header("Total Medal")]
    public bool useCountCondition = false;
    public int minMedals = 0;
    public int maxMedals = 999;
    [Header("Must Have Medal")]
    public List<MedalSO> mustHaveMedals;
    public List<MedalSO> mustNotHaveMedals;

    private void Awake()
    {
        EvaluateAndApply();
    }

    public void EvaluateAndApply()
    {
        if (MedalManager.Instance == null) return;

        bool conditionsMet = true;

        if (useCountCondition)
        {
            int currentTotal = MedalManager.Instance.ownedMedals.Count;
            if (currentTotal < minMedals || currentTotal >= maxMedals)
            {
                conditionsMet = false;
            }
        }

        if (conditionsMet && mustHaveMedals.Count > 0)
        {
            foreach (var medal in mustHaveMedals)
            {
                if (!MedalManager.Instance.ownedMedals.Contains(medal))
                {
                    conditionsMet = false;
                    break;
                }
            }
        }

        if (conditionsMet && mustNotHaveMedals.Count > 0)
        {
            foreach (var medal in mustNotHaveMedals)
            {
                if (MedalManager.Instance.ownedMedals.Contains(medal))
                {
                    conditionsMet = false;
                    break;
                }
            }
        }

        if (conditionsMet)
        {

            gameObject.SetActive(actionIfMet == ActionType.Show);
        }
        else
        {
            gameObject.SetActive(actionIfMet != ActionType.Show);
        }
    }
}