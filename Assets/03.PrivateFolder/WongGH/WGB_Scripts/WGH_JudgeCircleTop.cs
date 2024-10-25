using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircleTop : MonoBehaviour
{
    [Header("��ġ����(���)")]
    [SerializeField] float _setLeftCheckTopPos;     // ���� ��� üũ���� �� ����
    [SerializeField] float _setRightCheckTopPos;    // ������ ��� üũ���� �� ����

    [SerializeField] WGH_PlayerController _player;
    [SerializeField] Vector2 _circleTopRight;          // ������ ��� ���� ��
    [SerializeField] Vector2 _circleTopLeft;           // ���� ��� ���� ��
}
