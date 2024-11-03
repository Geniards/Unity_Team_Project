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
        InitializeTiles();
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, StopScrolling, this);
        EventManager.Instance.AddAction(E_Event.PLAYERDEAD, StopScrolling, this);
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
            _tiles[index].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

            if (_tiles[index].transform.position.x <= -_tileWidth * 15f)
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
