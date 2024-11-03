using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvent : MonoBehaviour
{
    [SerializeField] private BossController _boss;

    public void DeadEnd()
    {
        _boss.DeadActionEnd();
    }
}
