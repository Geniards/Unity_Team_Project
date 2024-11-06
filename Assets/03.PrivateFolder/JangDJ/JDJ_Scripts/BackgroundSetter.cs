using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetter : MonoBehaviour
{
    private void Start()
    {
        int stageNumber = DataManager.Instance.SelectedStageData.StageNumber;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (stageNumber - 1 == i)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(DataManager.Instance.SelectedStageData.StageNumber - 1).gameObject.SetActive(true);
    }
}
