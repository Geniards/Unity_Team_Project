using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 1.0f;    // 배경 스크롤 속도
    [SerializeField] private GameObject[] backgroundTiles;          // 기존 배경 타일 배열
    private List<GameObject> _activeTiles = new List<GameObject>(); // 활성화된 타일 목록
    private float _tileWidth;                                       // 타일의 너비

    void Start()
    {
        if (backgroundTiles.Length > 0)
        {
            _tileWidth = backgroundTiles[0].GetComponent<Renderer>().bounds.size.x;
            InitializeExistingTiles();
        }
    }

    void Update()
    {
        ScrollBackground();                                       // 배경 스크롤 메서드 호출
    }

    private void InitializeExistingTiles()
    {
        // 이미 배치된 타일들을 _activeTiles 리스트에 추가
        for (int i = 0; i < backgroundTiles.Length; i++)
        {
            _activeTiles.Add(backgroundTiles[i]);
        }
    }

    private void ScrollBackground()
    {
        // 모든 타일을 왼쪽으로 이동
        foreach (var tile in _activeTiles)
        {
            tile.transform.position += Vector3.left * backgroundScrollSpeed * Time.deltaTime;
        }

        // 첫 번째 타일이 화면을 벗어나면 위치를 오른쪽 끝으로 이동
        if (_activeTiles[0].transform.position.x <= -_tileWidth)
        {
            GameObject firstTile = _activeTiles[0];
            _activeTiles.RemoveAt(0); // 첫 번째 타일 제거
            // 첫 번째 타일을 마지막 타일의 오른쪽 끝에 위치
            firstTile.transform.position = _activeTiles[_activeTiles.Count - 1].transform.position + Vector3.right * _tileWidth;
            _activeTiles.Add(firstTile); // 마지막에 추가
        }
    }
}
