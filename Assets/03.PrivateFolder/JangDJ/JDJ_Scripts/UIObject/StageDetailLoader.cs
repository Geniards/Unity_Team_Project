using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageDetailLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _stageNumber;
    [SerializeField] private TextMeshProUGUI _stageName;
    [SerializeField] private TextMeshProUGUI _songTitle;
    [SerializeField] private TextMeshProUGUI _description;

    [SerializeField] private Image _mainImg;


    private void OnEnable()
    {
        SetData();
    }

    private void SetData()
    {
        StageData data = DataManager.Instance.SelectedStageData;

        _stageName.text = data.StageName;
        _stageNumber.text = $"Stage{data.StageNumber}";
        _songTitle.text = data.SongTitle;
        _description.text = data.Description;
        _mainImg.sprite = LoadTitleImage(data.StageNumber);
    }

    private Sprite LoadTitleImage(int stageNumber)
    {
        return Resources.Load<Sprite>($"Images/Stage/Title/Stage{stageNumber}");
    }
}
