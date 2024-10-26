using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("수치조절(하단)")]
    #region
    //[SerializeField] float _setLeftCheckPos;        // 왼쪽 하단 체크지점 값 변경
    //[SerializeField] float _setRightCheckPos;       // 오른쪽 하단 체크지점 값 변경
    #endregion

    [Header("기타")]
    [SerializeField] WGH_PlayerController _player;
    [SerializeField]
    #region
    //[SerializeField] Vector2 _circleRight;          // 오른쪽 하단 벡터 값
    //[SerializeField] Vector2 _circleLeft;           // 왼쪽 하단 벡터 값
    //public GameObject TestLeftPrefab;
    //[SerializeField] GameObject _testRightPrefab;

    //public bool Right { get; private set; }
    //public bool Left { get; private set; }
    //public bool Miss { get; private set; }
    #endregion
    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();
        #region
        // TODO : 기준점(checkPos, 판정원)을 기준으로 왼쪽, 오른쪽에 생성


        //// 플레이어와 얼만큼 떨어진 거리에 체크할 부분을 생성할지 기본값 결정
        //// 왼쪽
        //_circleLeft = new Vector2(_player.transform.position.x + _setLeftCheckPos, _player.transform.position.y);
        //// 오른쪽
        //_circleRight = new Vector2( _player.transform.position.x + _setRightCheckPos, _player.transform.position.y);

        //if(TestLeftPrefab != null)
        //{
        //    // 왼쪽 프리팹 생성
        //    Instantiate(TestLeftPrefab, _circleLeft, Quaternion.identity);
        //}

        //if (_testRightPrefab != null)
        //{
        //    // 오른쪽 프리팹 생성
        //    Instantiate(_testRightPrefab, _circleRight, Quaternion.identity);
        //}
        #endregion
    }

    private void Update()
    {
        #region
        //Debug.DrawLine(_player.transform.position, _circleLeft );
        //if( Right && Left)
        //{
        //Debug.Log("perfect");
        //}
        //else if( !Right && Left )
        //{
        //Debug.Log("good");
        //}
        //else if (Right && !Left)
        //{
        //Debug.Log("good");
        //}
        #endregion
    }

    #region
    /// <summary>
    /// Left / Right 체크 여부 변경 메서드
    /// </summary>
    //public void SetRightCheckTrue()
    //{
    //    this.Right = true;
    //}
    //
    //public void SetRightCheckFalse()
    //{
    //    this.Right = false;
    //}
    //
    //public void SetLeftCheckTrue()
    //{
    //    this.Left = true;
    //}
    //public void SetLeftCheckFalse()
    //{
    //    this.Left = false;
    //}
    #endregion
}
