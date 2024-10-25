using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("수치조절(하단)")]
    [SerializeField] float _setLeftCheckPos;        // 왼쪽 하단 체크지점 값 변경
    [SerializeField] float _setRightCheckPos;       // 오른쪽 하단 체크지점 값 변경

    [SerializeField] WGH_PlayerController _player;  
    [SerializeField] Vector2 _circleRight;          // 오른쪽 하단 벡터 값
    [SerializeField] Vector2 _circleLeft;           // 왼쪽 하단 벡터 값
    public GameObject _testLeftPrefab { get; private set; }
    [SerializeField] GameObject _testRightPrefab;

    public bool _right;
    public bool _left;
    public bool _miss;
    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();

        // 플레이어와 얼만큼 떨어진 거리에 체크할 부분을 생성할지 기본값 결정
        // 왼쪽
        _circleLeft = new Vector2(_player.transform.position.x + _setLeftCheckPos, _player.transform.position.y);
        // 오른쪽
        _circleRight = new Vector2( _player.transform.position.x + _setRightCheckPos, _player.transform.position.y);
        
        if(_testLeftPrefab != null)
        {
            // 왼쪽 프리팹 생성
            Instantiate(_testLeftPrefab, _circleLeft, Quaternion.identity);
        }
        
        if (_testRightPrefab != null)
        {
            // 오른쪽 프리팹 생성
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
