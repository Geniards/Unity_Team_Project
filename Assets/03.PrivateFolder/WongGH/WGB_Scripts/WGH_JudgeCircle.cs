using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("��ġ����(�ϴ�)")]
    [SerializeField] float _setLeftCheckPos;        // ���� �ϴ� üũ���� �� ����
    [SerializeField] float _setRightCheckPos;       // ������ �ϴ� üũ���� �� ����

    [SerializeField] WGH_PlayerController _player;  
    [SerializeField] Vector2 _circleRight;          // ������ �ϴ� ���� ��
    [SerializeField] Vector2 _circleLeft;           // ���� �ϴ� ���� ��
    public GameObject TestLeftPrefab { get; private set; }
    [SerializeField] GameObject _testRightPrefab;

    public bool Right { get; private set; }
    public bool Left { get; private set; }
    public bool Miss { get; private set; }
    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();

        // �÷��̾�� ��ŭ ������ �Ÿ��� üũ�� �κ��� �������� �⺻�� ����
        // ����
        _circleLeft = new Vector2(_player.transform.position.x + _setLeftCheckPos, _player.transform.position.y);
        // ������
        _circleRight = new Vector2( _player.transform.position.x + _setRightCheckPos, _player.transform.position.y);
        
        if(TestLeftPrefab != null)
        {
            // ���� ������ ����
            Instantiate(TestLeftPrefab, _circleLeft, Quaternion.identity);
        }
        
        if (_testRightPrefab != null)
        {
            // ������ ������ ����
            Instantiate(_testRightPrefab, _circleRight, Quaternion.identity);
        }
    }

    private void Update()
    {
        Debug.DrawLine(_player.transform.position, _circleLeft );
        if( Right && Left)
        {
            Debug.Log("perfect");
        }
        else if( !Right && Left )
        {
            Debug.Log("good");
        }
        else if (Right && !Left)
        {
            Debug.Log("good");
        }
    }
}
