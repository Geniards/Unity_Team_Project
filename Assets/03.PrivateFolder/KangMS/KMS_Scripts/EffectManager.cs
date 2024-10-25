using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] private List<GameObject> _effectPrefabs;

    private void Awake()
    {
        if(Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(int effectIndex, float effectTime, Vector3 pos, Quaternion quaternion)
    {
        if(effectIndex < 0 || effectIndex >= _effectPrefabs.Count)
        {
            Debug.Log("잘못된 이펙트 인덱스 입니다.");
            return;
        }

        GameObject effect = Instantiate(_effectPrefabs[effectIndex], pos, Quaternion.identity);
        Destroy(effect, effectTime);
    }
}
