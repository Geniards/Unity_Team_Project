using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvent : MonoBehaviour
{
    [SerializeField] private BossController _boss;

    public void DeadEnd()
    {
        Debug.Log("보스 애님 종료");
        _boss.DeadActionEnd();
    }
}
