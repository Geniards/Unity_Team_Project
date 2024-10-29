using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ��׶��� ������Ʈ ���� ���� �⺻ Ʋ �ۼ�. �� �Ŀ� ���� ���� �� ���� ����
public class TestScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] Tiles;         // Ÿ�� ������
    [SerializeField] private float ScrollSpeed;          // ��ũ�� �ӵ�
    [SerializeField] private float TileWidth;            // Ÿ�� ����
    [SerializeField] private int NumberOfTiles;          // ���� Ÿ�� ����

    private List<GameObject> ActiveTiles = new List<GameObject>();

    private void Start()
    {
        // ���� Ÿ�� ��ġ
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
    /// Ÿ���� �����մϴ�.
    /// </summary>
    /// <param name="index">������ Ÿ���� �ε���</param>
    private void SpawnTile(int index)
    {
        GameObject tile = Instantiate(Tiles[Random.Range(0, Tiles.Length)]); // ���� Ÿ�� ����

        float xPosition = index * TileWidth;
        tile.transform.position = new Vector3(xPosition, 0, 0); // y, z ��ǥ�� �ʿ信 ���� ����

        ActiveTiles.Add(tile);
    }

    private void MoveBackground()
    {
        for (int i = ActiveTiles.Count - 1; i >= 0; i--) // ���� �ݺ�
        {
            ActiveTiles[i].transform.position += Vector3.left * ScrollSpeed * Time.deltaTime;

            // Ÿ�� ���̸�ŭ �̵� �� ���ġ
            if (ActiveTiles[i].transform.position.x <= -TileWidth)
            {
                float rightMost = GetRightMostTile();
                Destroy(ActiveTiles[i]); // ���� Ÿ�� ����
                ActiveTiles.RemoveAt(i); // ����Ʈ���� ����

                SpawnTile(i); // ���� Ÿ�� ����
            }
        }
    }

    /// <summary>
    /// ���� �����ʿ� ��ġ�� Ÿ���� X��ǥ���� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>���� ������ Ÿ���� X��ǥ</returns>
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
