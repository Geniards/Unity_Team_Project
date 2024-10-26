using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("��ġ����(�ϴ�)")]
    #region
    //[SerializeField] float _setLeftCheckPos;        // ���� üũ���� �� ����
    //[SerializeField] float _setRightCheckPos;       // ������ üũ���� �� ����
    #endregion
    //[SerializeField] float _checkLineDist;
    [SerializeField] float _perfectRadius;      // perfect ���� ������
    [SerializeField] float _greatRadius;        // great   ���� ������

    [Header("��Ÿ")]
    [SerializeField] WGH_PlayerController _player;
    [SerializeField] private E_SpawnerPosY _judgePosY;
    private Vector3 _judgeStandardBottomPos;
    private Vector3 _judgeStandardMiddlePos;
    private Vector3 _judgeStandardTopPos;

    #region
    //[SerializeField] Vector2 _perfetLineLeft;           // ����Ʈ ���� ���� ���� ��
    //[SerializeField] Vector2 _ferfectLineRight;          // ����Ʈ ���� ������ ���� ��
    
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

        //_standardCheckPoses[(int)E_SpawnerPosY.BOTTOM] = ���� �ϴ��� üũ����Ʈ ��ǥ
        //_standardCheckPoses[(int)E_SpawnerPosY.MIDDLE] = �ߴ��� üũ����Ʈ ��ǥ
        //_standardCheckPoses[(int)E_SpawnerPosY.TOP] = ���� ����� üũ����Ʈ ��ǥ

        CreatCircle();
        
        #region
        // TODO : ������(checkPos, ������)�� �������� ����, �����ʿ� ����


        //// �������� ��ŭ ������ �Ÿ��� üũ�� �κ��� �������� �⺻�� ����
        //// ����
        //_perfetLineLeft = new Vector2(_judgeStandardPos.x - _checkLineDist, _judgeStandardPos.y);
        //// ������
        //_ferfectLineRight = new Vector2(_judgeStandardPos.x + _checkLineDist, _judgeStandardPos.y);

        //if(TestLeftPrefab != null)
        //{
        //    // ���� ������ ����
        //Instantiate(PerfetLeftPrefab, _perfetLineLeft, Quaternion.identity);
        //}

        //if (_testRightPrefab != null)
        //{
        //    // ������ ������ ����
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
    /// Left / Right üũ ���� ���� �޼���
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
