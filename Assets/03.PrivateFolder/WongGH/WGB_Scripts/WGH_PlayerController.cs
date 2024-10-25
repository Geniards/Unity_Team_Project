using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WGH_PlayerController : MonoBehaviour
{
    [Header("��ġ����")]
    [SerializeField] float _inAirTime;                  // ü���ð�
    [SerializeField] float _jumpHeight;                 // ���� �� �÷��̾��� ���� ��ġ

    [Header("����")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 _groundPos { get; private set; }    // ���� ��ġ��
    public Vector2 _jumPos { get; private set; }       // ���� ��ġ��
    Vector2 _rigidYPos;                                // ĳ���� Y ���̰�
    [SerializeField] GameObject _judgeCircle;

    [Header("��Ÿ")]
    private bool _isAir;                               // ü�� ����
    Coroutine _IsAirRountine;                          // ü�� �ڷ�ƾ
    
    private void Awake()
    {
        _inAirTime = 0.3f;
        _jumpHeight = 5f; 

        // ����
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _groundPos = new Vector2(transform.position.x, transform.position.y + 0.6f);    // �ϰ��ϴ� ������ ��� ��¦ ������ ���������� �� ����
        _jumPos = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _judgeCircle = FindAnyObjectByType<WGH_JudgeCircle>().gameObject;
    }

    private void Update()
    {
        _rigidYPos = Camera.main.WorldToScreenPoint(_rigid.position);
        if(_rigidYPos.y > Screen.height * 0.5f)
        {
            _judgeCircle.GetComponent<WGH_JudgeCircle>().enabled = false;
        }
        else if(_rigidYPos.y < Screen.height * 0.5f) 
        {
            _judgeCircle.GetComponent<WGH_JudgeCircle>().enabled = true;
        }

        // ���� Ű�� ������ ���
        if(!_isAir && Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isAir = true;
            SetAnim("Jump");
            _rigid.position = _jumPos;
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

            // �ϰ� ����
            SetAnim("FallAttack");
            _rigid.position = _groundPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if(collision.collider.tag == "Ground")
        //{
        _isAir = false;
        //}
        // if(collision.collider.tag == "Monster")
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

    public void SetAnim(string animName)
    {
        _anim.Play(animName);
    }
}
