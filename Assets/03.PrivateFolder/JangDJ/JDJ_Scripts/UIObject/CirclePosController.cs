using UnityEngine;

public class CirclePosController : MonoBehaviour
{
    [SerializeField] private RectTransform _base;
    [SerializeField] private RectTransform _topCircle;
    [SerializeField] private RectTransform _botCircle;

    private Vector3 _initWorldPos;
    private Vector2 _convertedInitWolrdPos;

    private void Start()
    {
        _initWorldPos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.MIDDLE);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _base as RectTransform,
            Camera.main.WorldToScreenPoint(_initWorldPos), null, out _convertedInitWolrdPos);

        _base.anchoredPosition = _convertedInitWolrdPos;

        SetSubCircle();
    }

    private void SetSubCircle()
    {
        Vector3 topPos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.TOP);
        Vector3 botPos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.BOTTOM);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _topCircle.parent as RectTransform,
            Camera.main.WorldToScreenPoint(topPos), null, out Vector2 convertTopPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _botCircle.parent as RectTransform,
            Camera.main.WorldToScreenPoint(botPos), null, out Vector2 convertBotPos);

        _topCircle.anchoredPosition = convertTopPos;
        _botCircle.anchoredPosition = convertBotPos;
    }


}
