using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSetter : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(DataManager.Instance.SelectedStageData.StageNumber - 1).gameObject.SetActive(true);
    }
}
