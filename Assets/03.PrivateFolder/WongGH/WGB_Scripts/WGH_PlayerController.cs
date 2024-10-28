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
    [SerializeField] int _maxHp;
    [SerializeField] int _curHp;
    [SerializeField] float _invincivilityTime;          // �����ð�

    [Header("����")]
    [SerializeField] Rigidbody2D _rigid;
    [SerializeField] Animator _anim;
    public Vector2 GroundPos { get; private set; }    // ���� ��ġ��
    public Vector2 JumPos { get; private set; }       // ���� ��ġ��
    Vector2 _rigidYPos;                                // ĳ���� Y ���̰�
    [SerializeField] WGH_JudgeCircle _judgeCircle;

    [Header("��Ÿ")]
    private bool _isAir;                               // ü�� ����
    bool _canJump = true;
    Coroutine _IsAirRountine;                          // ü�� �ڷ�ƾ
    bool _isDamaged;                                   // �ǰ� ����
    float fpresstime = 0;
    private void Awake()
    {
        _maxHp = 3;
        _curHp = _maxHp;
        // ����
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        // �ϰ��ϴ� ������ ��� ��¦ ������ ���������� �� ����
        GroundPos = new Vector2(transform.position.x, transform.position.y + _fallAttackHeight);    
        
        JumPos = new Vector2(transform.position.x, transform.position.y + _jumpHeight);
        _judgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }
    private void Start()
    {
        _judgeCircle.SetTopCircleOff();
        _judgeCircle.SetMiddleCircleOff();
        _judgeCircle.SetBottomCircleOn();

    }
    private void Update()
    {
        if(!_isAir)
        {
            _judgeCircle.SetBottomCircleOn();
        }
        if (!_isDamaged)
        {
            if(Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.F))
            {
                _rigid.position = GroundPos;
                _rigid.isKinematic = true;
                _judgeCircle.SetTopCircleOff();
                _judgeCircle.SetMiddleCircleOn();
                _judgeCircle.SetBottomCircleOff();
                SetAnim("JumpAttack2");
                if (_judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                }
                else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                {
                    _judgeCircle.note.OnHit(E_NoteDecision.Great);
                }
            }
            //else
            //{
            //    // ����Ű�� ������ ���
            //    if(!_isAir && Input.GetKeyDown(KeyCode.F) && !Input.GetKey(KeyCode.J))
            //    {
            //        _judgeCircle.SetTopCircleOn();
            //        _judgeCircle.SetMiddleCircleOff();
            //        _judgeCircle.SetBottomCircleOff();
            //        if (!_isAir)
            //        {
            //            _rigid.position = JumPos;
            //        }
            //        _isAir = true;
            //
            //
            //        if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
            //        {
            //            SetAnim("JumpAttack1");
            //            _judgeCircle.note.OnHit(E_NoteDecision.Great);
            //        }
            //        if (_judgeCircle._isPerfectCircleIn)
            //        {
            //            SetAnim("JumpAttack1");
            //            _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
            //        }
            //        
            //        else
            //        {
            //            SetAnim("Jump");
            //        }
            //    }
            //}
            // ���� Ŭ���� �ƴ� ���
            else
            {
                // �ߴ� ���ݶ� �״� kinematic �ٽ� ����
                _rigid.isKinematic = false;

                // ���� Ű�� ������ ���
                if (Input.GetKey(KeyCode.F))
                {
                    fpresstime += Time.deltaTime;
                    
                    if (fpresstime >= 0.05f)
                    {
                        _isAir = true;                          // ü�� ����   
                        if(_canJump)
                        {
                            _rigid.position = JumPos;           // ĳ���Ͱ� ������ ��ġ�� �����̵�
                            _rigid.isKinematic = true;
                            _rigid.isKinematic = false;
                            _canJump = false;
                        }
                        
                        _IsAirRountine = StartCoroutine(InAirTime()); // ü�� �ڷ�ƾ

                        _judgeCircle.SetTopCircleOn();      // Top �ݶ��̴� Ȱ��ȭ
                        _judgeCircle.SetMiddleCircleOff();
                        _judgeCircle.SetBottomCircleOff();  // Bottom �ݶ��̴� ��Ȱ��ȭ
                        if (!_judgeCircle._isGreatCircleIn)
                        {
                            SetAnim("Jump");
                        }
                        else if (_judgeCircle._isGreatCircleIn)
                        {
                            SetAnim("JumpAttack1");
                            if (_judgeCircle._isPerfectCircleIn)
                            {
                                _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                            }
                            else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                            {
                                _judgeCircle.note.OnHit(E_NoteDecision.Great);
                            }
                        }
                    }
                }
                else if(Input.GetKeyUp(KeyCode.F))
                {
                    fpresstime = 0;
                }
            
                // ���� Ű�� ������ ��� && ���� ���� ���
                if (!_isAir  && Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    _judgeCircle.SetMiddleCircleOff();
                    if (_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
                    }
                    else if (_judgeCircle._isGreatCircleIn && !_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Great);
                    }
                    // �ϴ� ����
                    SetAnim("GroundAttack");
            
                }
                // ���� Ű�� ������ ��� && ���߿� ���� ���
                if (_isAir && Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.F))
                {
                    _judgeCircle.SetBottomCircleOn();
                    _judgeCircle.SetMiddleCircleOff();
                    _judgeCircle.SetTopCircleOff();
                    if (_judgeCircle._isPerfectCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Perfect);
            
                    }
                    else if (_judgeCircle._isGreatCircleIn)
                    {
                        _judgeCircle.note.OnHit(E_NoteDecision.Great);
                    }
                    _rigid.position = GroundPos;
                    
                    // �ϰ� ����
                    SetAnim("FallAttack");
                    if (_IsAirRountine != null)
                    {
                        StopCoroutine(_IsAirRountine);
                        _rigid.isKinematic = false;
                    }
                }
            }
        }
        else
        {
            // TODO : �÷��̾� �ǰ� �ִϸ��̼�
            SetAnim("Run");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO : ���� tag ���̱�
        // if(collision.collider.tag == "Ground")
        //{
        _judgeCircle.SetTopCircleOff();
        _judgeCircle.SetBottomCircleOn();
        if(collision.collider.TryGetComponent(out BoxCollider2D boxColllider))
        {
            _canJump = true;
            _isAir = false;
        }
        
        //}
        if(collision.collider.TryGetComponent(out Note note) && !_isDamaged)
        {
            _isDamaged = true;
            _rigid.position = new Vector2(GroundPos.x, GroundPos.y - (_fallAttackHeight-0.1f));
            _rigid.isKinematic = true;
            _curHp -= 1;
            StartCoroutine(Invincibility());
            StartCoroutine(Clicker());
        }
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

    // ĳ���� �ǰ� �����Ÿ�
    IEnumerator Clicker()
    {
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.25f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.25f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        yield return new WaitForSeconds(0.25f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield break;
    }
    // ����
    IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(_invincivilityTime);
        _isDamaged = false;
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
