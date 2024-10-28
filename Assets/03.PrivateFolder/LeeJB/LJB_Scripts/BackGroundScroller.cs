using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;        // Ÿ�� ������
    [SerializeField] private float _scrollSpeed;        // ��׶��� ��ũ�� �ӵ�
    [SerializeField] private float _tileWidth;          // �� Ÿ���� ���� ����

    private void Start()
    {
        // ���� Ÿ�� ��ġ
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
    /// ��׶��� Ÿ���� �������� �̵���Ű�� ���ġ
    /// </summary>
    private void MoveBackground()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime; // ��׶��� Ÿ���� �������� �̵�

            // Ÿ�� ���̸�ŭ �̵� �� ���ġ
            if (tiles[i].transform.position.x <= -_tileWidth) // Ÿ���� ���̸�ŭ �̵� �Ͽ��� ���
            {
                float rightMost = GetRightMostTile(); // Ÿ���� ���� ������ ��ǥ�� Ÿ�� ���ġ
                tiles[i].transform.position = new Vector3(rightMost + _tileWidth, tiles[i].transform.position.y, tiles[i].transform.position.z);
            }
        }
    }

    /// <summary>
    /// ���� �����ʿ� ��ġ�� Ÿ�� X��ǥ���� ��ȯ
    /// </summary>
    private float GetRightMostTile()
    {
        float maxX = tiles[0].transform.position.x; // Ÿ�� ���� ������ �κ��� X��ġ ����
        for (int i = 1; i < tiles.Length; i++) // Ÿ�� X��ġ �� ������Ʈ
        {
            if (tiles[i].transform.position.x > maxX)
                maxX = tiles[i].transform.position.x;
        }
        return maxX; // X��ġ �� ��ȯ
    }
}
