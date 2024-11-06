using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStartButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(StartStageScene);
    }

    private void StartStageScene()
    {
        DataManager.Instance.ApplySelectStageData();
        SceneController.Instance.LoadScene(E_SceneType.STAGE);
        SoundManager.Instance.FadeBGM(false, 0.25f, 0);
    }
}
