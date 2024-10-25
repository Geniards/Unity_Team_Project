using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour, IManager
{
    public static EffectManager Instance { get; private set; }
    
    public void Init() 
    {
        Instance = this;
    }

    [SerializeField] private List<GameObject> _effectPrefabs;

    public void PlayEffect(int effectIndex, float effectTime, Vector3 pos, Quaternion quaternion)
    {
        if(effectIndex < 0 || effectIndex >= _effectPrefabs.Count)
        {
            Debug.Log("�߸��� ����Ʈ �ε��� �Դϴ�.");
            return;
        }

        GameObject effect = Instantiate(_effectPrefabs[effectIndex], pos, Quaternion.identity);
        Destroy(effect, effectTime);
    }
}
