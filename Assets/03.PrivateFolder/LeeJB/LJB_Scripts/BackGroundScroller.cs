using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles; // 타일 프리팹 배열
    [SerializeField] private float _scrollSpeed; // 백그라운드 스크롤 속도
    [SerializeField] private float _tileWidth; // 각 타일의 가로 길이
    [SerializeField] private float _scrollDelay; // 스크롤 시작 지연 시간

    private bool _isScrolling = false;

    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, StopScrolling, this);
        EventManager.Instance.AddAction(E_Event.PLAYERDEAD, StopScrolling, this);

        InitializeTiles();
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));

        // 보스사망 및 플레이어 사망 이벤트가 발생하면 StopScrolling 메서드 실행
    }

    private void Update()
    {
        if (_isScrolling)
        {
            MoveFloor(); // MoveFloor 메서드를 호출하여 바닥을 스크롤
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
    /// 설정한 시간 이후 스크롤 시작하는 코루틴
    /// </summary>
    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScrolling = true; // 시간이 지난 후 스크롤링 활성화
    }

    /// <summary>
    /// 바닥 타일 이동 메서드
    /// </summary>
    private void MoveFloor()
    {
        for (int index = 0; index < _tiles.Length; index++)
        {
            _tiles[index].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

            // 타일이 화면 왼쪽 경계를 벗어나면 오른쪽으로 재배치
            if (_tiles[index].transform.position.x <= -_tileWidth * 1.5f)
            {
                float rightMost = GetRightMostTile();
                _tiles[index].transform.position = new Vector3(rightMost + _tileWidth, _tiles[index].transform.position.y, _tiles[index].transform.position.z);
            }
        }
    }

    /// <summary>
    /// 가장 오른쪽 타일의 X좌표를 반환하는 메서드
    /// </summary>
    /// <returns>가장 오른쪽 타일의 X좌표</returns>
    private float GetRightMostTile()
    {
        float maxX = _tiles[0].transform.position.x;

        for (int index = 1; index < _tiles.Length; index++)
        {
            if (_tiles[index].transform.position.x > maxX)
                maxX = _tiles[index].transform.position.x;
        }

        return maxX;
    }

    /// <summary>
    /// 스크롤 정지 메서드
    /// </summary>
    private void StopScrolling()
    {
        _isScrolling = false;
    }
}
