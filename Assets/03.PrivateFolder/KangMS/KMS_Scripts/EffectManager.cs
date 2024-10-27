using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour, IManager
{
    public static EffectManager Instance { get; private set; }
    
    public void Init() 
    {
        Instance = this;
    }

    /// <summary>
    /// 요청 좌표에 이펙트를 재생합니다.
    /// </summary>
    public void PlayFX(Vector3 requestPos)
    {
        ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
        fx.gameObject.transform.position = requestPos;
        fx.Play();
    }

    /// <summary>
    /// Follow 타입형 이펙트 재생연출을 위한 메서드로
    /// 미완성 기능입니다.
    /// </summary>
    public void PlayFX(Transform requestTr) // 활용성 고려중
    {
        ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
        fx.transform.SetParent(requestTr);
        fx.transform.position = requestTr.position;
        fx.Play();
    }
}
