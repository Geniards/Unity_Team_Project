using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("수치조절(하단)")]
    #region
    //[SerializeField] float _setLeftCheckPos;        // 왼쪽 체크지점 값 변경
    //[SerializeField] float _setRightCheckPos;       // 오른쪽 체크지점 값 변경
    #endregion
    //[SerializeField] float _checkLineDist;
    [SerializeField] float _perfectRadius;      // perfect 원의 반지름
    [SerializeField] float _greatRadius;        // great   원의 반지름

    [Header("기타")]
    [SerializeField] WGH_PlayerController _player;
    [SerializeField] private E_SpawnerPosY _judgePosY;
    private Vector3 _judgeStandardBottomPos;
    private Vector3 _judgeStandardMiddlePos;
    private Vector3 _judgeStandardTopPos;

    #region
    //[SerializeField] Vector2 _perfetLineLeft;           // 퍼펙트 라인 왼쪽 벡터 값
    //[SerializeField] Vector2 _ferfectLineRight;          // 퍼펙트 라인 오른쪽 벡터 값
    
    public GameObject PerfetLeftPrefab;
    [SerializeField] GameObject _perfectRightPrefab;

    public bool Right { get; private set; }
    public bool Left { get; private set; }
    //public bool Miss { get; private set; }
    #endregion

    public void ChangeJudgePosY(E_SpawnerPosY posY)
    {
        this._judgePosY = posY;
    }

    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();
        //_judgeStandardBottomPos = GameManager.NoteDirector.GetCheckPoses(_judgePosY);
        _judgeStandardBottomPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.BOTTOM);
        _judgeStandardMiddlePos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.MIDDLE);
        _judgeStandardTopPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.TOP);

        //_standardCheckPoses[(int)E_SpawnerPosY.BOTTOM] = 가장 하단의 체크포인트 좌표
        //_standardCheckPoses[(int)E_SpawnerPosY.MIDDLE] = 중단의 체크포인트 좌표
        //_standardCheckPoses[(int)E_SpawnerPosY.TOP] = 가장 상단의 체크포인트 좌표

        CreatCircle();
        
        #region
        // TODO : 기준점(checkPos, 판정원)을 기준으로 왼쪽, 오른쪽에 생성


        //// 기준점과 얼만큼 떨어진 거리에 체크할 부분을 생성할지 기본값 결정
        //// 왼쪽
        //_perfetLineLeft = new Vector2(_judgeStandardPos.x - _checkLineDist, _judgeStandardPos.y);
        //// 오른쪽
        //_ferfectLineRight = new Vector2(_judgeStandardPos.x + _checkLineDist, _judgeStandardPos.y);

        //if(TestLeftPrefab != null)
        //{
        //    // 왼쪽 프리팹 생성
        //Instantiate(PerfetLeftPrefab, _perfetLineLeft, Quaternion.identity);
        //}

        //if (_testRightPrefab != null)
        //{
        //    // 오른쪽 프리팹 생성
        //Instantiate(_perfectRightPrefab, _ferfectLineRight, Quaternion.identity);
        //}
        #endregion
    }

    private void CreatCircle()
    {
        GameObject _greatCircleBottomObj = new GameObject("JudgeBottomCircle_Great");
        _greatCircleBottomObj.transform.position = _judgeStandardBottomPos;
        CircleCollider2D _greatCircleBottomCollider = _greatCircleBottomObj.AddComponent<CircleCollider2D>();
        _greatCircleBottomCollider.radius = _greatRadius;
        _greatCircleBottomCollider.isTrigger = true;

        GameObject _perfectCircleBottomObj = new GameObject("JudgeBottomCircle_Perfect");
        _perfectCircleBottomObj.transform.position = _judgeStandardBottomPos;
        CircleCollider2D _perfectCircleBottomCollider = _perfectCircleBottomObj.AddComponent<CircleCollider2D>();
        _perfectCircleBottomCollider.radius = _perfectRadius;
        _perfectCircleBottomCollider.isTrigger = true;

        GameObject _greatCircleMiddleObj = new GameObject("JudgeMiddleCircle_Great");
        _greatCircleMiddleObj.transform.position = _judgeStandardMiddlePos;
        CircleCollider2D _greatCircleMiddleCollider = _greatCircleMiddleObj.AddComponent<CircleCollider2D>();
        _greatCircleMiddleCollider.radius = _greatRadius;
        _greatCircleMiddleCollider.isTrigger = true;
        
        GameObject _perfectCircleMiddleObj = new GameObject("JudgeMiddleCircle_Perfect");
        _perfectCircleMiddleObj.transform.position = _judgeStandardMiddlePos;
        CircleCollider2D _perfectCircleMiddleCollider = _perfectCircleMiddleObj.AddComponent<CircleCollider2D>();
        _perfectCircleMiddleCollider.radius = _perfectRadius;
        _perfectCircleMiddleCollider.isTrigger = true;

        GameObject _greatCircleTopObj = new GameObject("JudgeTopCircle_Great");
        _greatCircleTopObj.transform.position = _judgeStandardTopPos;
        CircleCollider2D _greatCircleTopCollider = _greatCircleTopObj.AddComponent<CircleCollider2D>();
        _greatCircleTopCollider.radius = _greatRadius;
        _greatCircleTopCollider.isTrigger = true;
        
        GameObject _perfectCircleTopObj = new GameObject("JudgeTopCircle_Perfect");
        _perfectCircleTopObj.transform.position = _judgeStandardTopPos;
        CircleCollider2D _perfectCircleTopCollider = _perfectCircleTopObj.AddComponent<CircleCollider2D>();
        _perfectCircleTopCollider.radius = _perfectRadius;
        _perfectCircleTopCollider.isTrigger = true;
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
    public void SetRightCheckTrue()
    {
        this.Right = true;
    }
    
    public void SetRightCheckFalse()
    {
        this.Right = false;
    }
    
    public void SetLeftCheckTrue()
    {
        this.Left = true;
    }
    public void SetLeftCheckFalse()
    {
        this.Left = false;
    }
    #endregion
}
