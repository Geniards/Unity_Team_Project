using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;        // 타일 프리펩
    [SerializeField] private float _scrollSpeed;        // 백그라운드 스크롤 속도
    [SerializeField] private float _tileWidth;          // 각 타일의 가로 길이

    private void Start()
    {
        // 시작 타일 배치
        // for (int i = 0; i < tiles.Length; i++)
        // {
        //     tiles[i].transform.position = new Vector3(i * _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
        // }
    }

    private void Update()
    {
        MoveBackground();
    }

    /// <summary>
    /// 백그라운드 타일을 왼쪽으로 이동시키고 재배치
    /// </summary>
    private void MoveBackground()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime; // 백그라운드 타일을 왼쪽으로 이동

            // 타일 넓이만큼 이동 후 재배치
            if (tiles[i].transform.position.x <= -_tileWidth) // 타일의 넓이만큼 이동 하였을 경우
            {
                float rightMost = GetRightMostTile(); // 타일의 가장 오른쪽 좌표에 타일 재배치
                tiles[i].transform.position = new Vector3(rightMost + _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
            }
        }
    }

    /// <summary>
    /// 가장 오른쪽에 위치한 타일 X좌표값을 반환
    /// </summary>
    private float GetRightMostTile()
    {
        float maxX = tiles[0].transform.position.x; // 타일 가장 오른쪽 부분의 X위치 변수
        for (int i = 1; i < tiles.Length; i++) // 타일 X위치 값 업데이트
        {
            if (tiles[i].transform.position.x > maxX)
                maxX = tiles[i].transform.position.x;
        }
        return maxX; // X위치 값 반환
    }
}
