using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 백그라운드 오브젝트 랜덤 생성 기본 틀 작성. 추 후에 여유 있을 시 구현 예정
public class TestScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] Tiles;         // 타일 프리팹
    [SerializeField] private float ScrollSpeed;          // 스크롤 속도
    [SerializeField] private float TileWidth;            // 타일 길이
    [SerializeField] private int NumberOfTiles;          // 시작 타일 개수

    private List<GameObject> ActiveTiles = new List<GameObject>();

    private void Start()
    {
        // 랜덤 타일 배치
        for (int index = 0; index < NumberOfTiles; index++)
        {
            SpawnTile(index);
        }
    }

    private void Update()
    {
        MoveBackground();
    }

    /// <summary>
    /// 타일을 생성합니다.
    /// </summary>
    /// <param name="index">생성할 타일의 인덱스</param>
    private void SpawnTile(int index)
    {
        GameObject tile = Instantiate(Tiles[Random.Range(0, Tiles.Length)]); // 랜덤 타일 선택

        float xPosition = index * TileWidth;
        tile.transform.position = new Vector3(xPosition, 0, 0); // y, z 좌표는 필요에 따라 설정

        ActiveTiles.Add(tile);
    }

    private void MoveBackground()
    {
        for (int i = ActiveTiles.Count - 1; i >= 0; i--) // 역순 반복
        {
            ActiveTiles[i].transform.position += Vector3.left * ScrollSpeed * Time.deltaTime;

            // 타일 넓이만큼 이동 후 재배치
            if (ActiveTiles[i].transform.position.x <= -TileWidth)
            {
                float rightMost = GetRightMostTile();
                Destroy(ActiveTiles[i]); // 기존 타일 제거
                ActiveTiles.RemoveAt(i); // 리스트에서 제거

                SpawnTile(i); // 랜덤 타일 생성
            }
        }
    }

    /// <summary>
    /// 가장 오른쪽에 위치한 타일의 X좌표값을 반환합니다.
    /// </summary>
    /// <returns>가장 오른쪽 타일의 X좌표</returns>
    private float GetRightMostTile()
    {
        float maxX = ActiveTiles[0].transform.position.x;

        for (int i = 1; i < ActiveTiles.Count; i++)
        {
            if (ActiveTiles[i].transform.position.x > maxX)
                maxX = ActiveTiles[i].transform.position.x;
        }

        return maxX;
    }
}
*/
