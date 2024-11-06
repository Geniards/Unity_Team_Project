using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_FloatJudgeResult : MonoBehaviour
{
    public GameObject GetResultObject(E_NoteDecision _result)
    {
        switch(_result)
        {
            case E_NoteDecision.Great:
                return ObjPoolManager.Instance.GetObject(E_Pool.GREAT);
            case E_NoteDecision.Perfect:
                return ObjPoolManager.Instance.GetObject(E_Pool.PERFECT);
            default:
                Debug.Log("없음");
                return null;
        }
    }

    public void SpawnResult(E_NoteDecision _result, Vector3 _spawnPos)
    {
        GameObject _gameObj = GetResultObject(_result);
        _gameObj.transform.position = _spawnPos;
    }
    
}
