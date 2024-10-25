using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 1.0f;  // 배경 스크롤 속도
    [SerializeField] private GameObject[] backgroundTiles;        // 배경 타일 배열
    [SerializeField] private float tileSpawnInterval = 5.0f;      // 타일 생성 간격
    private Vector3 _startPosition;                               // 배경의 초기 위치 저장
    private float _nextTileSpawnTime = 0.0f;                      // 다음 타일 생성 시간

    void Start()
    {
        _startPosition = transform.position;                     // 배경의 시작 위치 설정
    }

    void Update()
    {
        ScrollBackground();                                      // 배경 스크롤 메서드 호출

        if (Time.time >= _nextTileSpawnTime)                     // 타일 생성 조건
        {
            SpawnRandomBackgroundTile();                         // 랜덤 타일 생성 메서드 호출
            _nextTileSpawnTime = Time.time + tileSpawnInterval;  // 다음 타일 생성 시간 설정
        }
    }

    private void ScrollBackground()
    {
        float newPos = Mathf.Repeat(Time.time * backgroundScrollSpeed, 20); // 스크롤 위치 계산
        transform.position = _startPosition + Vector3.left * newPos;        // 배경의 위치 업데이트
    }

    private void SpawnRandomBackgroundTile()
    {
        int randomIndex = Random.Range(0, backgroundTiles.Length);         // 랜덤 인덱스 선택
        GameObject randomTile = backgroundTiles[randomIndex];              // 랜덤 타일 할당
        Instantiate(randomTile, transform.position, Quaternion.identity);  // 타일 생성
    }
}
