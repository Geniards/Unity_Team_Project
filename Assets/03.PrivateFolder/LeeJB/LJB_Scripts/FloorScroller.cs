using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _tileWidth;
    [SerializeField] private float _scrollDelay;

    private bool _isScrolling = false;

    private void Start()
    {
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, StopScrolling, this);
        EventManager.Instance.AddAction(E_Event.PLAYERDEAD, StopScrolling, this);

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

    private void InitializeTiles()
    {
        // 현재 위치 그대로 타일들을 초기화
        for (int index = 0; index < _tiles.Length; index++)
        {
            _tiles[index].transform.position = new Vector3(
                _tiles[index].transform.position.x,
                _tiles[index].transform.position.y,
                _tiles[index].transform.position.z
            );
        }
    }

    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScrolling = true;
    }

    private void MoveFloor()
    {
        for (int index = 0; index < _tiles.Length; index++)
        {
            // 타일을 왼쪽으로 이동
            _tiles[index].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

            // 왼쪽으로 이동한 타일을 오른쪽 끝 타일 옆으로 이동
            if (_tiles[index].transform.position.x <= -_tileWidth * (_tiles.Length - 1) + 18f)
            {
                float rightMost = GetRightMostTile();
                _tiles[index].transform.position = new Vector3(rightMost + _tileWidth, _tiles[index].transform.position.y, _tiles[index].transform.position.z);
            }
        }
    }

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

    private void StopScrolling()
    {
        _isScrolling = false;
    }
}
