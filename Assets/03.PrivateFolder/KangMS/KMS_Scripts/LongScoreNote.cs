using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongScoreNote : Note
{
    [Header("노트형태")]
    [SerializeField] private Transform head;  
    [SerializeField] private Transform tail;  
    [SerializeField] private Transform body;  
    [SerializeField] private Transform maskLayer;

    private bool _isTouching = false;
    private Vector3 _initialMaskScale;
    private float _noteLength;
    private double lastDspTime;

    private void Awake()
    {
        Initialize(endPoint, speed, scoreValue, 10);
    }

    //public override void Initialize(Vector3 endPoint, float speed, float scoreValue, float length = 0)
    //{
    //    base.Initialize(endPoint, speed, scoreValue, length);
    //    _noteLength = length;
    //    SetNoteLength();
    //    lastDspTime = AudioSettings.dspTime;
    //}

    private void Start()
    {
        _initialMaskScale = maskLayer.localScale;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            OnHit(E_NoteDecision.Perfect, E_Boutton.None);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            ReleaseTouch();
        }
    }

    /// <summary>
    /// 롱 노트의 길이에 맞게 머리와 꼬리 위치 조정.
    /// </summary>
    private void SetNoteLength()
    {
        tail.position = head.position + Vector3.left * _noteLength;
        float bodyLength = Vector3.Distance(head.position, tail.position);
        body.localScale = new Vector3(bodyLength, body.localScale.y, body.localScale.z);
        maskLayer.localScale = new Vector3(bodyLength, body.localScale.y, body.localScale.z);
        body.position = (head.position + tail.position) / 2;
        maskLayer.position = body.position;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton boutton)
    {
        if (!_isTouching && !isMoving)
        {
            _isTouching = true;
            isMoving = false;
            StartCoroutine(LongNoteCoroutine(decision));  // 롱 노트 진행
        }
    }

    /// <summary>
    /// 터치가 유지되는 동안 몸통을 줄이고, 터치를 놓으면 실패로 처리.
    /// </summary>
    private IEnumerator LongNoteCoroutine(E_NoteDecision decision)
    {
        Vector3 initialHeadPosition = head.position;
        while (_isTouching && Vector3.Distance(head.position,tail.position) > 0.1f)
        {
            double currentDspTime = AudioSettings.dspTime;
            double deltaTime = currentDspTime - lastDspTime;
            lastDspTime = currentDspTime;

            tail.position = Vector3.MoveTowards(tail.position, head.position, (float)(speed * deltaTime));

            float currentDistance = Vector3.Distance(initialHeadPosition,tail.position);
            float scaleRatio = currentDistance / _noteLength;
            maskLayer.localScale = new Vector3(scaleRatio * _initialMaskScale.x, _initialMaskScale.y, _initialMaskScale.z);

            yield return null;
        }

        if (_isTouching && Vector3.Distance(head.position, tail.position) <= 0.1f)
        {
            CalculateScore(decision);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("롱 노트 실패");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 터치를 놓으면 실패로 처리
    /// </summary>
    public void ReleaseTouch()
    {
        _isTouching = false;
    }

    public override float OnDamage()
    {
        return damage;
    }
}
