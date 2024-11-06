using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageDataSetter : MonoBehaviour
{
    [SerializeField] private GameObject _targetPopup;
    [SerializeField] private Button _button;
    [SerializeField] private StageDataSetter _nextSetter;
    public StageDataSetter NextSetter => _nextSetter;

    [SerializeField] private StageData _stage = new StageData();
    public StageData StageData => _stage;

    private void Start()
    {
        if(_nextSetter != null)
            _stage.SetNextStage(_nextSetter);

        Path path = GetComponent<Path>();

        _button.onClick.AddListener(() =>
        {
            path.SetDest();
            path.Pointer.SaveLastPosIdx();
        });
    }

    public void ShowDetail()
    {
        DataManager.Instance.SelectedStageData = this._stage;
        DataManager.Instance.NextStageData = _stage.NextStage;
        _targetPopup.SetActive(true);
    }
}
