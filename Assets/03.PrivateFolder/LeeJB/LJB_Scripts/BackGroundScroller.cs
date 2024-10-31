using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles; // Ÿ�� ������ �迭
    [SerializeField] private float _scrollSpeed; // ��׶��� ��ũ�� �ӵ�
    [SerializeField] private float _tileWidth; // �� Ÿ���� ���� ����
    [SerializeField] private float _scrollDelay; // ��ũ�� ���� ���� �ð�

    private bool _isScrolling = false;

    private void Start()
    {
        InitializeTiles();
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
    }

    private void Update()
    {
        if (_isScrolling)
        {
            MoveFloor(); // MoveFloor �޼��带 ȣ���Ͽ� �ٴ��� ��ũ��
        }
    }

    /// <summary>
    /// Ÿ�� �ʱ� ��ġ ���� �޼���
    /// </summary>
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

    /// <summary>
    /// ������ �ð� ���� ��ũ�� �����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScrolling = true; // �ð��� ���� �� ��ũ�Ѹ� Ȱ��ȭ
    }

    /// <summary>
    /// �ٴ� Ÿ�� �̵� �޼���
    /// </summary>
    private void MoveFloor()
    {
        for (int index = 0; index < _tiles.Length; index++)
        {
            _tiles[index].transform.position += Vector3.left * _scrollSpeed * Time.deltaTime; // �ٴ� Ÿ���� �������� �̵�

            // Ÿ���� ȭ�� ���� ��踦 ����� ���������� ���ġ
            if (_tiles[index].transform.position.x <= -_tileWidth * 1.5f) // Ÿ���� �� �������� �̵��� �� ���ġ
            {
                float rightMost = GetRightMostTile(); // Ÿ���� ���� ������ ��ǥ�� Ÿ�� ���ġ
                _tiles[index].transform.position = new Vector3(rightMost + _tileWidth, _tiles[index].transform.position.y, _tiles[index].transform.position.z);
            }
        }
    }

    /// <summary>
    /// ���� ������ Ÿ���� X��ǥ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>���� ������ Ÿ���� X��ǥ</returns>
    private float GetRightMostTile()
    {
        float maxX = _tiles[0].transform.position.x; // Ÿ�� ���� ������ �κ��� X��ġ ����

        for (int index = 1; index < _tiles.Length; index++) // Ÿ�� X��ġ �� ������Ʈ
        {
            if (_tiles[index].transform.position.x > maxX)
                maxX = _tiles[index].transform.position.x;
        }

        return maxX; // X��ġ �� ��ȯ
    }
}
