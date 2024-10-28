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
    /// ��û ��ǥ�� ����Ʈ�� ����մϴ�.
    /// </summary>
    public void PlayFX(Vector3 requestPos)
    {
        ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
        fx.gameObject.transform.position = requestPos;
        fx.Play();
    }

    /// <summary>
    /// Follow Ÿ���� ����Ʈ ��������� ���� �޼����
    /// �̿ϼ� ����Դϴ�.
    /// </summary>
    public void PlayFX(Transform requestTr) // Ȱ�뼺 �����
    {
        ParticleSystem fx = ObjPoolManager.Instance.GetObject<ParticleSystem>(E_Pool.HIT_VFX);
        fx.transform.SetParent(requestTr);
        fx.transform.position = requestTr.position;
        fx.Play();
    }
}
