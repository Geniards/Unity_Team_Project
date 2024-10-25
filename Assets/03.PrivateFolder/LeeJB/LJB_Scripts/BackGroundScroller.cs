using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 1.0f;  // ��� ��ũ�� �ӵ�
    [SerializeField] private GameObject[] backgroundTiles;        // ��� Ÿ�� �迭
    [SerializeField] private float tileSpawnInterval = 5.0f;      // Ÿ�� ���� ����
    private Vector3 _startPosition;                               // ����� �ʱ� ��ġ ����
    private float _nextTileSpawnTime = 0.0f;                      // ���� Ÿ�� ���� �ð�

    void Start()
    {
        _startPosition = transform.position;                     // ����� ���� ��ġ ����
    }

    void Update()
    {
        ScrollBackground();                                      // ��� ��ũ�� �޼��� ȣ��

        if (Time.time >= _nextTileSpawnTime)                     // Ÿ�� ���� ����
        {
            SpawnRandomBackgroundTile();                         // ���� Ÿ�� ���� �޼��� ȣ��
            _nextTileSpawnTime = Time.time + tileSpawnInterval;  // ���� Ÿ�� ���� �ð� ����
        }
    }

    private void ScrollBackground()
    {
        float newPos = Mathf.Repeat(Time.time * backgroundScrollSpeed, 20); // ��ũ�� ��ġ ���
        transform.position = _startPosition + Vector3.left * newPos;        // ����� ��ġ ������Ʈ
    }

    private void SpawnRandomBackgroundTile()
    {
        int randomIndex = Random.Range(0, backgroundTiles.Length);         // ���� �ε��� ����
        GameObject randomTile = backgroundTiles[randomIndex];              // ���� Ÿ�� �Ҵ�
        Instantiate(randomTile, transform.position, Quaternion.identity);  // Ÿ�� ����
    }
}
