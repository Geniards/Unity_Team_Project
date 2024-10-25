using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScroller : MonoBehaviour
{
    [SerializeField] private float groundScrollSpeed = 2.0f;            // �ٴ� ��ũ�� �ӵ�
    private Vector3 _startPosition;                                     // �ٴ��� �ʱ� ��ġ

    void Start()
    {
        _startPosition = transform.position;                            // �ٴ��� ���� ��ġ
    }

    void Update()
    {
        ScrollGround();                                                 // Floor ��ũ�� �޼��� ȣ��
    }

    private void ScrollGround()
    {
        float newPos = Mathf.Repeat(Time.time * groundScrollSpeed, 20); // ��ũ�� ��� ��ġ
        transform.position = _startPosition + Vector3.left * newPos;    // �ٴ��� ��ġ ������Ʈ
    }
}
