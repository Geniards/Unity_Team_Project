using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_FloatJudgeResult : MonoBehaviour
{
    public GameObject GetResultObject(E_ResultType _result)
    {
        switch(_result)
        {
            //case E_ResultType.Great:
            //    return ObjPoolManager.Instance.GetObject<GameObject>(E_Pool.);
            //case E_ResultType.Perfect:
            //    return ObjPoolManager.Instance.GetObject<GameObject>(E_Pool.);
            default:
                Debug.Log("없음");
                return null;
    
        }
    }

    public void SpawnResult(E_ResultType _result, Vector3 _spawnPos)
    {
        GameObject _gameObj = GetResultObject(_result);
        Instantiate(_gameObj, _spawnPos, Quaternion.identity);
    }
    
}
