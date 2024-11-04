using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private List<RectTransform> _points;
    [SerializeField] private RectTransform _moveObject;

    private int _destIdx = 0;
    private int _curIdx = 0;

    private Coroutine _moveRoutine;
    private Coroutine _stepRoutine;

    private void Start()
    { //  저장된 데이터가 있는경우 불러오기
        for (int i = 0; i < _points.Count; i++)
        {
            _points[i].GetComponent<Path>().Pointer = this;
        }

        int idx = LoadLastPosIdx();
        _moveObject.anchoredPosition = _points[idx].anchoredPosition;
        _curIdx = idx;
    }

    public int LoadLastPosIdx()
    {
        if (PlayerPrefs.HasKey(UIManager.LOBBY_IDX_KEY))
            return PlayerPrefs.GetInt(UIManager.LOBBY_IDX_KEY);

        return 0;
    }

    public void SaveLastPosIdx()
    {
        PlayerPrefs.SetInt(UIManager.LOBBY_IDX_KEY, _curIdx);
    }

    private IEnumerator MoveRoutine(int idx)
    {
        Vector2 dest = _points[idx].anchoredPosition;
        Vector2 start = _moveObject.anchoredPosition;

        float dist = Vector3.Distance(dest, start);
        float t = 0;

        while (true)
        {
            if (t >= 1)
                break;

            t += (Time.deltaTime * _speed) / dist;

            _moveObject.anchoredPosition = Vector2.Lerp(start, dest, t);

            yield return null;
        }

        _moveObject.anchoredPosition = dest;

        if (_points[idx].TryGetComponent<Path>(out Path path))
        {
            _curIdx = path.PathNumber;

            if (_curIdx == _destIdx)
            {
                if (path.TryGetComponent<StageDataSetter>(out StageDataSetter data))
                {
                    data.ShowDetail();
                }
            }
        }
    }

    public void SetPosIdx(int idx)
    {
        if (_moveRoutine != null)
        { StopCoroutine(_moveRoutine); }
        if (_stepRoutine != null)
        { StopCoroutine(_stepRoutine); }

        _destIdx = idx;

        _stepRoutine = StartCoroutine(StepMoveRoutine());
    }

    public IEnumerator StepMoveRoutine()
    {
        int dir = 1;

        if (_curIdx > _destIdx)
            dir = -1;
        else if (_curIdx == _destIdx)
            yield return MoveRoutine(_curIdx);

        while (true)
        {
            if (_curIdx == _destIdx)
                break;

            yield return MoveRoutine(_curIdx + dir);
        }
    }
}
