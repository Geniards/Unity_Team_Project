using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WGH_AreaJudge : MonoBehaviour
{
    [SerializeField] float _greatDistance;
    [SerializeField] float _perfectDistance;
    private int _combo;
    int _perfectCount;
    int _greatCount;

    Vector3 _checkTopPos;
    Vector3 _checkMiddlePos;
    Vector3 _checkBottomPos;
    Vector3 _curPos;

    public Note Note { get; private set; }
    WGH_PlayerController _playerController;
    WGH_FloatJudgeResult _floatResult;
    Rigidbody2D _playerRigid;

    private KeyCode _inputKey;
    private bool _isInputProcessing;                                    // 키를 입력 받았는가를 확인할 bool 변수
    private bool _isInputedDoubleKey;                                   // 동시 입력 처리를 할 2번째 키를 입력받았는가를 확인할 bool 변수
    [SerializeField, Range(0.01f, 0.5f)] private float _judgeTime;

    [Header("테스트용 임시 프리팹")]
    [SerializeField] GameObject _great;
    [SerializeField] GameObject _perfect;

    private void Start()
    {
        _checkTopPos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.TOP);
        _checkMiddlePos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.MIDDLE);
        _checkBottomPos = GameManager.Director.GetCheckPoses(E_SpawnerPosY.BOTTOM);
        _playerController = FindAnyObjectByType<WGH_PlayerController>();
        _floatResult = GetComponent<WGH_FloatJudgeResult>();
        _playerRigid = _playerController.GetComponent<Rigidbody2D>();
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, GetBossScore, this);
    }
    
    private void Update()
    {
        Debug.Log(_combo);
        if(_isInputProcessing == false && !_playerController.IsDamaged && !_playerController.IsDied && !_playerController.IsContact)
        {
            if (Input.GetKeyDown(KeyCode.F))
                _inputKey = KeyCode.F;
            else if (Input.GetKeyDown(KeyCode.J))
                _inputKey = KeyCode.J;
            else
                return;

            StartCoroutine(StartInputCheck(_inputKey));
        }
        #region 이전코드
        //if (!_playerController.IsDied)
        //{
        //    if (!_playerController.IsDamaged)
        //    {
        //        
        //        else if(Input.GetKeyDown(KeyCode.J) && !_isFPress)
        //        {
        //            _isJPress = true;
        //            StartCoroutine(InputAfterKeyJ());
        //        }
        //        if (Input.GetKeyDown(KeyCode.J))
        //        {
        //            _jPressTime = Time.time;
        //            _isJPress = true;
        //        }
        //        
        //        if (Input.GetKeyDown(KeyCode.F))
        //        {
        //            _fPressTime = Time.time;
        //            _isFPress = true;
        //        }
        //        
        //        if (Mathf.Abs(_jPressTime - _fPressTime) <= 0.2f && _isJPress && _isFPress)
        //        {
        //            CheckNote(_checkMiddlePos);
        //            _jPressTime = -1;
        //            _fPressTime = -1;
        //            _isJPress = false;
        //            _isFPress = false;
        //        }
        //        else
        //        {
        //            if (_isJPress && !_isFPress && Input.GetKeyUp(KeyCode.J))
        //            {
        //                CheckNote(_checkBottomPos);
        //                _isJPress = false;
        //            }
        //            if (_isFPress && !_isJPress && Input.GetKeyUp(KeyCode.F))
        //            {
        //                CheckNote(_checkTopPos);
        //                _isFPress = false;
        //            }
        //        }
        //    }
        //}
        #endregion
    }
    public int CheckCurCombo()
    {
        return _combo;
    }
    
    public int CheckPerfectCount()
    {
        return _perfectCount;
    }
    
    public int CheckGreatCount()
    {
        return _greatCount;
    }
    public void AddCombo()
    {
        _combo++;
    }
    public void AddPerfectCount()
    {
        _perfectCount++;
    }
    /// <summary>
    /// 콤보 리셋
    /// </summary>
    public void SetComboReset()
    { _combo = 0; }
    /// <summary>
    /// 노트판정 메서드
    /// </summary>
    public void CheckNote(Vector3 checkPos, E_Boutton button)
    {
        this._curPos = checkPos;
        Vector2 aPoint = new Vector2(_curPos.x - _greatDistance / 2, _curPos.y - _greatDistance / 4);
        Vector2 bPoint = new Vector2(_curPos.x + _greatDistance / 2, _curPos.y + _greatDistance / 4);
        Collider2D[] hits = Physics2D.OverlapAreaAll(aPoint, bPoint);
        Debug.DrawLine(aPoint,bPoint, Color.blue, 0.5f);
        
        if(hits.Length == 0 )
            Note = null;
        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out Note note))
            {
                Note = note;
                float _distance = Vector2.Distance(_curPos, hit.transform.position);
                Debug.DrawLine(aPoint + new Vector2(0, _greatDistance / 4), bPoint - new Vector2(0, _greatDistance / 4), Color.blue, 0.5f);
                if (_distance <= _perfectDistance)
                {
                    _combo++;
                    _perfectCount++;
                    Note.OnHit(E_NoteDecision.Perfect, button);
                    _floatResult.SpawnResult(E_NoteDecision.Perfect, hit.transform.position + new Vector3(0, 2, 0)); // PERFECT 프리팹 띄우기
                    if(hit.TryGetComponent(out ScoreNote score))                                                     // 스코어 노트 퍼펙트 점수 처리
                    {
                        CalculateScoreNote(E_NoteDecision.Perfect);
                    }
                    else if(hit.TryGetComponent(out MonsterNote monster))                                            // 스코어 노트 그레이트 점수 처리
                    {
                        CalculateScoreMonster(E_NoteDecision.Perfect);
                    }
                }
                else if(_distance <= _greatDistance + 0.2f)
                {
                    _combo++;
                    _greatCount++;
                    Note.OnHit(E_NoteDecision.Great, button);
                    _floatResult.SpawnResult(E_NoteDecision.Great, hit.transform.position + new Vector3(0, 2, 0));  // GREAT 프리팹 띄우기
                    if(hit.TryGetComponent(out ScoreNote score))                                                    // 몬스터 노트 퍼펙트 점수 처리
                    {
                        CalculateScoreNote(E_NoteDecision.Great);
                    }
                    else if(hit.TryGetComponent(out MonsterNote Monster))                                           // 몬스터 노트 그레이트 점수 처리
                    {
                        CalculateScoreMonster(E_NoteDecision.Great);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 점수노트 점수 처리 메서드
    /// </summary>
    private void CalculateScoreNote(E_NoteDecision result)
    {
        int score = 0;
        if (result == E_NoteDecision.Perfect && Note.isBoss)
        {
            score = Mathf.RoundToInt((100 + 10) * 2 * 2 * ((_combo * 0.01f)+1));
            Debug.Log($"보스 점수 퍼펙{score}");
        }
        else if (result == E_NoteDecision.Great && Note.isBoss)
        {
            score = Mathf.RoundToInt((100 + 10) * 1 * 2 * ((_combo * 0.01f)+1));
            Debug.Log($"보스 점수 그레잇{score}");
        }
        else if (result == E_NoteDecision.Perfect)
        {
            score = Mathf.RoundToInt((100 + 10) * 2 * ((_combo * 0.01f) + 1));
            Debug.Log($"점수 퍼펙{score}");
        }
        else if (result == E_NoteDecision.Great)
        {
            score = Mathf.RoundToInt((100 + 10) * 1 * ((_combo * 0.01f) + 1));
            Debug.Log($"점수 그레잇{score}");
        }
        DataManager.Instance.AddScore(score);
    }
    /// <summary>
    /// 몬스터노트 점수 처리 메서드
    /// </summary>
    private void CalculateScoreMonster(E_NoteDecision result)
    {
        int score = 0;
        if(result == E_NoteDecision.Perfect && Note.isBoss)
        {
            score = Mathf.RoundToInt((100 + 20) * 2 * 2 * ((_combo * 0.01f) + 1));
            Debug.Log($"보스 몬스터 퍼펙{score}");
        }
        else if(result == E_NoteDecision.Great && Note.isBoss)
        {
            score = Mathf.RoundToInt((100 + 20) * 1 * 2 * ((_combo * 0.01f) + 1));
            Debug.Log($"보스 몬스터 그레잇{score}");
        }
        else if(result == E_NoteDecision.Perfect)
        {
            score = Mathf.RoundToInt((100 + 20) * 2 * ((_combo * 0.01f) + 1));
            Debug.Log($"몬스터 퍼펙{score}");
        }
        else if(result == E_NoteDecision.Great)
        {
            score = Mathf.RoundToInt((100 + 20) * 1 * ((_combo * 0.01f) + 1));
            Debug.Log($"몬스터그레잇{score}");
        }
        DataManager.Instance.AddScore(score);
    }
    /// <summary>
    /// 보스 처치시 획득 점수
    /// </summary>
    private void GetBossScore()
    {
        int score = 0;
        score = DataManager.Instance.Boss.Score;
        Debug.Log($"보스점수 {score}");
        score += _playerController.GetHpScore();
        Debug.Log($"체력반영 {score}");
        DataManager.Instance.AddScore(score);

        Debug.Log(DataManager.Instance.CurScore);
    }
    // 첫 입력 코루틴
    IEnumerator StartInputCheck(KeyCode key)
    {
        _isInputProcessing = true;                          
        _isInputedDoubleKey = false;                        

        KeyCode nextKey = KeyCode.None;                                          // 2번째 키를 받아둘 KeyCode
        Action nextAction = null;                                                // 경우에 따라 기능을 달리하기 위한 델리게이트

        // F를 눌렀을 경우 동시입력을 위해 필요한 키를 J로 정하는 조건문
        if (key == KeyCode.F)
        {
            // 상단 제거
            CheckNote(_checkTopPos, E_Boutton.F_BOUTTON);                        // F가 입력될경우 동시입력 여부에 관계없이 진행할 함수

            nextKey = KeyCode.J;                                                 // 다음으로 받으면 동시입력이 진행될 키 지정
            nextAction = () => CheckNote(_checkBottomPos, E_Boutton.J_BOUTTON);  // F가 입력되고나서 J가 입력될 경우 사용할 함수 델리게이트에 할당
        }
        // J를 눌렀을 경우 동시입력을 위해 필요한 키를 F로 정하는 조건문
        else if (key == KeyCode.J)
        {
            // 하단 제거
            CheckNote(_checkBottomPos, E_Boutton.J_BOUTTON);                     // J가 입력될 경우 동시입력 여부에 관계없이 진행할 함수
            nextKey = KeyCode.F;                                                 // 다음으로 받으면 동시입력이 진행될 키 지정
            nextAction = () => CheckNote(_checkTopPos, E_Boutton.F_BOUTTON);     // J가 입력되고 나서 F가 입력될 경우 사용할 함수 델리게이트에 할당
        }

        // 이중 코루틴 시작
        yield return StartCoroutine(InputAfterKey(nextKey, nextAction));

        // 즉시 동작하지 않고 텀을 두고 진행해야 할 동작이 있으면 아래에 지정

        // 동시 입력이 확인 되었으면 작동할 조건문
        if (_isInputedDoubleKey == true)
        {
            // 동시공격 애니메이션
            _playerController.SetAnim("MiddleAttack");
        }
        else
        {
            if (key == KeyCode.F)
            {
                // 판정했을 때 노트가 없을 경우 && 땅에 있는 상태일 경우 "일반 점프" 애니메이션
                if (Note == null && !_playerController.IsAir)
                {
                    _playerController.IsAirControl(true);                        // 플레이어 체공상태 여부 true
                    _playerRigid.position = new Vector3(_playerController.transform.position.x, _checkTopPos.y - 1, 0);
                    StartCoroutine(_playerController.InAirTime());               // 체공 코루틴
                    _playerController.SetAnim("Jump");
                }
                // 판정했을 때 노트가 있을 경우 && 땅에 있는 상태일 경우 "점프 공격" 애니메이션
                else if (Note != null && !_playerController.IsAir)
                {
                    _playerController.IsAirControl(true);                        // 플레이어 체공상태 여부 true
                    _playerRigid.position = new Vector3(_playerController.transform.position.x, _checkTopPos.y - 1, 0);
                    StartCoroutine(_playerController.InAirTime());               // 체공 코루틴
                    _playerController.SetAnim("JumpAttack");
                }
            }
            else if (_playerController.IsAir && key == KeyCode.J)
            {
                _playerController.SetAnim("FallAttack");
                _playerRigid.position = new Vector3(_playerController.transform.position.x, _checkBottomPos.y - 1, 0);
            }
            else if (key == KeyCode.J)
            {
                if (!_playerController.IsAir)
                    _playerController.SetAnim("GroundAttack");
            }
        }
        _isInputProcessing = false;
    }

    // 두번째 입력 코루틴
    IEnumerator InputAfterKey(KeyCode key, Action nextAction)
    {
        float _timer = 0f;                                      // 경과 시간을 받을 타이머

        while (_timer < _judgeTime)                             // 지정한 시간을 넘을 경우 코루틴 종료
        {
            if (Input.GetKeyDown(key))
            {
                nextAction?.Invoke();                           // 할당 해두었던 델리게이트 함수 실행
                _isInputedDoubleKey = true;                     // 동시입력 확인 bool true
                break;
            }
            _timer += Time.deltaTime;
            yield return null;
        }
    }
}
