using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("��ġ����")]
    [SerializeField] float _inAirTime;                  // ü���ð�                         / ���� �� : 0.3f
    [SerializeField] float _fallAttackHeight;           // �ϰ� ���� �� �÷��̾��� ���� ��ġ  / ���� �� : 0.4f
    [SerializeField] float _jumpHeight;                 // ���� �� �÷��̾��� ���� ��ġ      / ���� �� : 5f

    [Header("����")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 GroundPos { get; private set; }    // ���� ��ġ��
    public Vector2 JumPos { get; private set; }       // ���� ��ġ��
    Vector2 _rigidYPos;                                // ĳ���� Y ���̰�
    [SerializeField] WGH_JudgeCircle _judgeCircle;

    [Header("��Ÿ")]
    private bool _isAir;                               // ü�� ����
    Coroutine _IsAirRountine;                          // ü�� �ڷ�ƾ
    
    private void Awake()
    {
        // ����
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // �ϰ��ϴ� ������ ��� ��¦ ������ ���������� �� ���� => TODO : �� �ν����Ϳ��� ������ �� �ֵ��� ����
        GroundPos = new Vector2(transform.position.x, transform.position.y + _fallAttackHeight);    
        
        JumPos = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _judgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }
    private void Start()
    {
        _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.BOTTOM);
    }
    private void Update()
    {
        // �÷��̾� ���̿� ���� CheckPosY �� ����
        //_rigidYPos = Camera.main.WorldToScreenPoint(_rigid.position);
        //if(_rigidYPos.y > Screen.height * 0.5f)
        //{
        //    _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.TOP);
        //}
        //else if(_rigidYPos.y < Screen.height * 0.5f) 
        //{
        //    _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.BOTTOM);
        //}

        // ���� Ű�� ������ ���
        if(!_isAir && Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.TOP);
            _isAir = true;
            SetAnim("Jump");
            _rigid.position = JumPos;
            // ü�� �ڷ�ƾ
            _IsAirRountine = StartCoroutine(InAirTime());
            
        }

        // ���� Ű�� ������ ��� && ���� ���� ���
        if(_isAir == false && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // �ϴ� ����
            SetAnim("GroundAttack");
        }
        // ���� Ű�� ������ ��� && ���߿� ���� ���
        else if (_isAir && Input.GetKeyDown(KeyCode.J))
        {
            if (_IsAirRountine != null)
            {
                StopCoroutine(_IsAirRountine);
                _rigid.isKinematic = false;
            }
            _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.BOTTOM);
            // �ϰ� ����
            SetAnim("FallAttack");
            _rigid.position = GroundPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.)
        // if(collision.collider.tag == "Ground")
        //{
        _isAir = false;
        _judgeCircle.ChangeJudgePosY(E_SpawnerPosY.BOTTOM);
        //}
        // if(collision.collider.tag == "Monster" || collision.collider.tag == "Obstacle")
        //{
        // TODO : �ǰ�����
        //}
    }

    // ü�� �ð� ���� �ڷ�ƾ
    IEnumerator InAirTime()
    {
        _rigid.isKinematic = true;
        yield return new WaitForSeconds(_inAirTime);
        _rigid.isKinematic = false;
        yield break;
    }
    /// <summary>
    /// �ִϸ��̼� ���� �޼���
    /// </summary>
    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
