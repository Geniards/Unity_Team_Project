using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScroller : MonoBehaviour
{
    [SerializeField] private float groundScrollSpeed = 2.0f;            // 바닥 스크롤 속도
    private Vector3 _startPosition;                                     // 바닥의 초기 위치

    void Start()
    {
        _startPosition = transform.position;                            // 바닥의 시작 위치
    }

    void Update()
    {
        ScrollGround();                                                 // Floor 스크롤 메서드 호출
    }

    private void ScrollGround()
    {
        float newPos = Mathf.Repeat(Time.time * groundScrollSpeed, 20); // 스크롤 계산 위치
        transform.position = _startPosition + Vector3.left * newPos;    // 바닥의 위치 업데이트
    }
}
