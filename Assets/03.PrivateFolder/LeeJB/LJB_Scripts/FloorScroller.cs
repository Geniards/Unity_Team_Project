using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles; // 타일 프리팹 배열
    [SerializeField] private float _scrollSpeed; // 바닥 스크롤 속도
    [SerializeField] private float _tileWidth; // 각 타일의 가로 길이
    [SerializeField] private float _scrollDelay; // 스크롤 시작 지연 시간

    private bool _isScrolling = false;

    private void Start()
    {
        InitializeTiles();
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
    }

    private void Update()
    {
        if (_isScrolling)
        {
            MoveFloor();
        }
    }

    /// <summary>
    /// 타일 초기 위치 설정 메서드
    /// </summary>
    private void InitializeTiles()
    {
        for (int index = 0; index < _tiles.Length; index++)
        {
            _tiles[index].transform.position = new Vector3(
                _tiles[index].transform.position.x,
                _tiles[index].transform.position.y,
                _tiles[index].transform.position.z
            );
        }
    }

    /// <summary>
    /// 일정 시간 지연 후 스크롤을 시작하는 코루틴
    /// </summary>
    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScrolling = true; // 지연 시간이 지난 후 스크롤링 활성화
    }

    /// <summary>
    /// 바닥 이동 메서드
    /// </summary>
    private void MoveFloor()
    {
        for (int index = 0; index < _tiles.Length; index++)
        {
            _tiles[index].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime; // 바닥 타일을 왼쪽으로 이동

            // 타일이 화면 왼쪽 경계를 벗어나면 오른쪽으로 재배치
            if (_tiles[index].transform.position.x <= -_tileWidth * 15f) // 타일이 더 왼쪽으로 이동한 후 재배치
            {
                float rightMost = GetRightMostTile(); // 타일의 가장 오른쪽 좌표에 타일 재배치
                _tiles[index].transform.position = new Vector3(rightMost + _tileWidth, _tiles[index].transform.position.y, _tiles[index].transform.position.z);
            }
        }
    }

    /// <summary>
    /// 가장 오른쪽 타일의 X좌표를 반환하는 메서드
    /// </summary>
    private float GetRightMostTile()
    {
        float maxX = _tiles[0].transform.position.x; // 타일 가장 오른쪽 부분의 X위치 변수

        for (int index = 1; index < _tiles.Length; index++) // 타일 X위치 값 업데이트
        {
            if (_tiles[index].transform.position.x > maxX)
                maxX = _tiles[index].transform.position.x;
        }

        return maxX; // X위치 값 반환
    }
}
