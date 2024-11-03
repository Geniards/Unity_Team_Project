using UnityEngine;

public class NextButton : BaseButton
{
    public override void MyAction()
    {
        if (DataManager.Instance.NextStageData == null)
            _button.interactable = false;

        DataManager.Instance.SelectedStageData
            = DataManager.Instance.NextStageData;

        DataManager.Instance.NextStageData =
            DataManager.Instance.SelectedStageData.NextStage;

        SceneController.Instance.LoadScene(E_SceneType.STAGE);
    }
}
