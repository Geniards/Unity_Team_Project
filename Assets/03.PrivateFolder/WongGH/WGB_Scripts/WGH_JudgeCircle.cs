using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("��ġ����")]
    [SerializeField] float _perfectRadius;      // perfect ���� ������
    [SerializeField] float _greatRadius;        // great   ���� ������

    [Header("��Ÿ")]
    [SerializeField] WGH_PlayerController _player;
    [SerializeField] private E_SpawnerPosY _judgePosY;
    private Vector3 _judgeStandardBottomPos;
    private Vector3 _judgeStandardMiddlePos;
    private Vector3 _judgeStandardTopPos;

    GameObject _greatCircleBottomObj;
    GameObject _perfectCircleBottomObj;
    GameObject _greatCircleMiddleObj;
    GameObject _perfectCircleMiddleObj;
    GameObject _greatCircleTopObj;
    GameObject _perfectCircleTopObj;

    CircleCollider2D _greatCircleBottomCollider;
    CircleCollider2D _perfectCircleBottomCollider;
    CircleCollider2D _greatCircleMiddleCollider;
    CircleCollider2D _perfectCircleMiddleCollider;
    CircleCollider2D _greatCircleTopCollider;
    CircleCollider2D _perfectCircleTopCollider;
    // TODO : ��Ʈ �����͸� �����־�� ��
    public Note note;

    public bool _isGreatCircleIn;
    public bool _isPerfectCircleIn;
    
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
    }

    private void Update()
    {
    }

    #region
    /// <summary>
    /// ������ Ȱ��ȭ On/Off �޼���
    /// </summary>
    public void SetBottomCircleOn()
    {
        _greatCircleBottomObj.SetActive(true);
        _perfectCircleBottomObj.SetActive(true);
    }
    
    public void SetBottomCircleOff()
    {
        _greatCircleBottomObj.SetActive(false);
        _perfectCircleBottomObj.SetActive(false);
    }
    
    public void SetMiddleCircleOn()
    {
        _greatCircleMiddleObj.SetActive(true);
        _perfectCircleMiddleObj.SetActive(true);
    }
    public void SetMiddleCircleOff()
    {
        _greatCircleMiddleObj.SetActive(false);
        _perfectCircleMiddleObj.SetActive(false);
    }

    public void SetTopCircleOn()
    {
        _greatCircleTopObj.SetActive(true);
        _perfectCircleTopObj.SetActive(true);
    }

    public void SetTopCircleOff()
    {
        _greatCircleTopObj.SetActive(false);
        _perfectCircleTopObj.SetActive(false);
    }
    #endregion
    private void CreatCircle()
    {
        // Bottom Great
        _greatCircleBottomObj = new GameObject("JudgeBottomCircle_Great");                      // ������Ʈ ����
        _greatCircleBottomObj.transform.position = _judgeStandardBottomPos;                     // CheckPos�� ����
        _greatCircleBottomCollider = _greatCircleBottomObj.AddComponent<CircleCollider2D>();    // 2D Collider �߰�
        _greatCircleBottomCollider.radius = _greatRadius;                                       // ������ ������ ���̸�ŭ�� ũ��� ����
        _greatCircleBottomCollider.isTrigger = true;                                            // Trigger ���·� ��ȯ
        _greatCircleBottomCollider.AddComponent<WGH_JudgeGreatCircleCollider>();                
        _greatCircleBottomCollider.AddComponent<Rigidbody2D>();                                 // �浹������ ���� Rigidbody �߰�
        _greatCircleBottomCollider.GetComponent<Rigidbody2D>().isKinematic = true;              // Kinematic ���·� ��ȯ
        // Bottom Perfect
        _perfectCircleBottomObj = new GameObject("JudgeBottomCircle_Perfect");
        _perfectCircleBottomObj.transform.position = _judgeStandardBottomPos;
        _perfectCircleBottomCollider = _perfectCircleBottomObj.AddComponent<CircleCollider2D>();
        _perfectCircleBottomCollider.radius = _perfectRadius;
        _perfectCircleBottomCollider.isTrigger = true;
        _perfectCircleBottomCollider.AddComponent<WGH_JudgePerfectCircleCollider>();
        _perfectCircleBottomCollider.AddComponent<Rigidbody2D>();
        _perfectCircleBottomCollider.GetComponent<Rigidbody2D>().isKinematic = true;
        // Middle Great
        _greatCircleMiddleObj = new GameObject("JudgeMiddleCircle_Great");
        _greatCircleMiddleObj.transform.position = _judgeStandardMiddlePos;
        _greatCircleMiddleCollider = _greatCircleMiddleObj.AddComponent<CircleCollider2D>();
        _greatCircleMiddleCollider.radius = _greatRadius;
        _greatCircleMiddleCollider.isTrigger = true;
        _greatCircleMiddleCollider.AddComponent<WGH_JudgeGreatCircleCollider>();
        _greatCircleMiddleCollider.AddComponent<Rigidbody2D>();
        _greatCircleMiddleCollider.GetComponent<Rigidbody2D>().isKinematic = true;
        // Middle Perfect
        _perfectCircleMiddleObj = new GameObject("JudgeMiddleCircle_Perfect");
        _perfectCircleMiddleObj.transform.position = _judgeStandardMiddlePos;
        _perfectCircleMiddleCollider = _perfectCircleMiddleObj.AddComponent<CircleCollider2D>();
        _perfectCircleMiddleCollider.radius = _perfectRadius;
        _perfectCircleMiddleCollider.isTrigger = true;
        _perfectCircleMiddleCollider.AddComponent<WGH_JudgePerfectCircleCollider>();
        _perfectCircleMiddleCollider.AddComponent<Rigidbody2D>();
        _perfectCircleMiddleCollider.GetComponent<Rigidbody2D>().isKinematic = true;
        // Top Great
        _greatCircleTopObj = new GameObject("JudgeTopCircle_Great");
        _greatCircleTopObj.transform.position = _judgeStandardTopPos;
        _greatCircleTopCollider = _greatCircleTopObj.AddComponent<CircleCollider2D>();
        _greatCircleTopCollider.radius = _greatRadius;
        _greatCircleTopCollider.isTrigger = true;
        _greatCircleTopCollider.AddComponent<WGH_JudgeGreatCircleCollider>();
        _greatCircleTopCollider.AddComponent<Rigidbody2D>();
        _greatCircleTopCollider.GetComponent<Rigidbody2D>().isKinematic = true;
        // Top Perfect
        _perfectCircleTopObj = new GameObject("JudgeTopCircle_Perfect");
        _perfectCircleTopObj.transform.position = _judgeStandardTopPos;
        _perfectCircleTopCollider = _perfectCircleTopObj.AddComponent<CircleCollider2D>();
        _perfectCircleTopCollider.radius = _perfectRadius;
        _perfectCircleTopCollider.isTrigger = true;
        _perfectCircleTopCollider.AddComponent<WGH_JudgePerfectCircleCollider>();
        _perfectCircleTopCollider.AddComponent<Rigidbody2D>();
        _perfectCircleTopCollider.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
