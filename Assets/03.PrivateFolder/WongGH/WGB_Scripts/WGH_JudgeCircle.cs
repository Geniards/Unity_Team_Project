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
    public GameObject _testLeftPrefab { get; private set; }
    [SerializeField] GameObject _testRightPrefab;

    public bool _right;
    public bool _left;
    public bool _miss;
    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();

        // �÷��̾�� ��ŭ ������ �Ÿ��� üũ�� �κ��� �������� �⺻�� ����
        // ����
        _circleLeft = new Vector2(_player.transform.position.x + _setLeftCheckPos, _player.transform.position.y);
        // ������
        _circleRight = new Vector2( _player.transform.position.x + _setRightCheckPos, _player.transform.position.y);
        
        if(_testLeftPrefab != null)
        {
            // ���� ������ ����
            Instantiate(_testLeftPrefab, _circleLeft, Quaternion.identity);
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
        if( _right && _left)
        {
            Debug.Log("perfect");
        }
        else if( !_right && _left )
        {
            Debug.Log("good");
        }
        else if (_right && !_left)
        {
            Debug.Log("good");
        }
    }
}
