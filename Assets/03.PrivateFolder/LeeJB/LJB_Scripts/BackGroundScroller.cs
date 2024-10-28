using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed = 1.0f;    // ��� ��ũ�� �ӵ�
    [SerializeField] private GameObject[] backgroundTiles;          // ���� ��� Ÿ�� �迭
    private List<GameObject> _activeTiles = new List<GameObject>(); // Ȱ��ȭ�� Ÿ�� ���
    private float _tileWidth;                                       // Ÿ���� �ʺ�

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
        ScrollBackground();                                       // ��� ��ũ�� �޼��� ȣ��
    }

    private void InitializeExistingTiles()
    {
        // �̹� ��ġ�� Ÿ�ϵ��� _activeTiles ����Ʈ�� �߰�
        for (int i = 0; i < backgroundTiles.Length; i++)
        {
            _activeTiles.Add(backgroundTiles[i]);
        }
    }

    private void ScrollBackground()
    {
        // ��� Ÿ���� �������� �̵�
        foreach (var tile in _activeTiles)
        {
            tile.transform.position += Vector3.left * backgroundScrollSpeed * Time.deltaTime;
        }

        // ù ��° Ÿ���� ȭ���� ����� ��ġ�� ������ ������ �̵�
        if (_activeTiles[0].transform.position.x <= -_tileWidth)
        {
            GameObject firstTile = _activeTiles[0];
            _activeTiles.RemoveAt(0); // ù ��° Ÿ�� ����
            // ù ��° Ÿ���� ������ Ÿ���� ������ ���� ��ġ
            firstTile.transform.position = _activeTiles[_activeTiles.Count - 1].transform.position + Vector3.right * _tileWidth;
            _activeTiles.Add(firstTile); // �������� �߰�
        }
    }
}
