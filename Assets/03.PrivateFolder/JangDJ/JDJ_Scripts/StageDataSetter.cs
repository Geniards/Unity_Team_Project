using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSetter : MonoBehaviour
{
    [SerializeField] private GameObject _targetPopup;
    [SerializeField] private Button _button;
    [SerializeField] private StageData _stage = new StageData();
    

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            DataManager.Instance.SelectedStageData = this._stage;
            _targetPopup.SetActive(true);
        });
    }
}
