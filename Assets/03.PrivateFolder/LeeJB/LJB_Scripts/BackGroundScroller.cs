using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;        
    [SerializeField] private float _scrollSpeed = 1.0f; // ��� ��ũ�� �ӵ�
    [SerializeField] private float _tileWidth;          // �� Ÿ���� ���� ����

    private void Start()
    {
        // �ʱ� Ÿ�� ��ġ
       // for (int i = 0; i < tiles.Length; i++)
       // {
       //     tiles[i].transform.position = new Vector3(i * _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
       // }
    }

    private void Update()
    {
        MoveBackground();
    }

    // ����� �������� �̵���Ű��, ȭ�� ������ ��� Ÿ���� ���ġ
    private void MoveBackground()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;

            // ȭ�� ������ ��� Ÿ���� ������ ������ ���ġ
            if (tiles[i].transform.position.x <= -_tileWidth)
            {
                float rightMostX = GetRightMostTileX();
                tiles[i].transform.position = new Vector3(rightMostX + _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
            }
        }
    }

    // ���� ���� �����ʿ� �ִ� Ÿ���� x ��ǥ�� ã��
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
