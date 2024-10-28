using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;        
    [SerializeField] private float _scrollSpeed = 1.0f; // 배경 스크롤 속도
    [SerializeField] private float _tileWidth;          // 각 타일의 가로 길이

    private void Start()
    {
        // 초기 타일 배치
       // for (int i = 0; i < tiles.Length; i++)
       // {
       //     tiles[i].transform.position = new Vector3(i * _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
       // }
    }

    private void Update()
    {
        MoveBackground();
    }

    // 배경을 왼쪽으로 이동시키고, 화면 밖으로 벗어난 타일을 재배치
    private void MoveBackground()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

            // 화면 밖으로 벗어난 타일을 오른쪽 끝으로 재배치
            if (tiles[i].transform.position.x <= -_tileWidth)
            {
                float rightMostX = GetRightMostTileX();
                tiles[i].transform.position = new Vector3(rightMostX + _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
            }
        }
    }

    // 현재 가장 오른쪽에 있는 타일의 x 좌표를 찾음
    private float GetRightMostTileX()
    {
        float maxX = tiles[0].transform.position.x;
        for (int i = 1; i < tiles.Length; i++)
        {
            if (tiles[i].transform.position.x > maxX)
                maxX = tiles[i].transform.position.x;
        }
        return maxX;
    }
}
