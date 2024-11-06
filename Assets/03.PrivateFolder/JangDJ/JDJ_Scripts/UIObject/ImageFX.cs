using UnityEngine;

public class ImageFX : FxObject, IUIFxObject
{
    private RectTransform _tr;

    [SerializeField] private Canvas _myCanvas;
    public Canvas Canvas => _myCanvas;

    private Vector2 _anchorPos;

    public void SetPos(Vector3 standardPos) // 스크린 좌표를 받는다.
    {
        _myCanvas.worldCamera = Camera.main;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _myCanvas.transform as RectTransform,
            standardPos, _myCanvas.worldCamera, out Vector2 temp);

        _tr.anchoredPosition = temp;
    }
}