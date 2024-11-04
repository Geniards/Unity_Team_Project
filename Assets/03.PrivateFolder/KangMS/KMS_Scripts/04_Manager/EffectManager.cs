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
    /// 요청 좌표에 요청하는 이펙트타입을 재생합니다.
    /// </summary>
    public void PlayFX(Vector3 requestPos, E_VFX fxType) // 이펙트 발생요인 = 노트 파괴, 마우스 입력, 반격노트 이펙트, 입력할때 발생, 리턴 전에 
    {
        Animation anim = null;

        switch (fxType)
        {
            case E_VFX.NOTE:
                anim = ObjPoolManager.Instance.GetObject(E_Pool.NOTE_FX).GetComponent<FxObject>().Anim;
                break;
            case E_VFX.SWORD:
                anim = ObjPoolManager.Instance.GetObject(E_Pool.SWORD_FX).GetComponent<FxObject>().Anim;
                break;
            case E_VFX.MOUSE_INPUT:
                anim = ObjPoolManager.Instance.GetObject(E_Pool.MOUSE_INPUT_FX).GetComponent<FxObject>().Anim;
                break;
        }

        anim.Play();

        //ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
        //fx.gameObject.transform.position = requestPos;
        //fx.Play();
    }

    /// <summary>
    /// Follow 타입형 이펙트 재생연출을 위한 메서드로
    /// 미완성 기능입니다.
    /// </summary>
    //public void PlayFX(Transform requestTr) // 활용성 고려중
    //{
    //    ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
    //    fx.transform.SetParent(requestTr);
    //    fx.transform.position = requestTr.position;
    //    fx.Play();
    //}
}
