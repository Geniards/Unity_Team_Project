using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{
    [System.Serializable]
    public class ObjectGroup
    {
        [SerializeField] private GameObject[] _objects;      // �׷� �� ������Ʈ �迭
        [SerializeField] private float _scrollSpeed;         // �׷캰 ��ũ�� �ӵ�
        [SerializeField] private float _minSpacing;          // �ּ� ����
        [SerializeField] private float _maxSpacing;          // �ִ� ����

        public GameObject[] GetObjects()
        {
            return _objects;
        }

        public float GetScrollSpeed()
        {
            return _scrollSpeed;
        }

        public float GetMinSpacing()
        {
            return _minSpacing;
        }

        public float GetMaxSpacing()
        {
            return _maxSpacing;
        }
    }

    [SerializeField] private ObjectGroup[] _objectGroups;    // �׷� ������Ʈ�� �����ϴ� �迭
    [SerializeField] private Transform _parentTransform;     // �ڽ����� ������ �θ� ������Ʈ�� ��ġ ����
    [SerializeField] private float _scrollDelay;             // ���� �� ��ũ�ѱ����� ���� �ð�
    [SerializeField] private float _destroyOffset;           // �θ� ������Ʈ ��ġ �������� ������ ��ġ ������
    [SerializeField] private float _spawnOffset;             // �θ� ������Ʈ ��ġ �������� ������ ��ġ ������

    private bool _isScroller = false;
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
    }

    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScroller = true; // ���� �ð� �� ��ũ�Ѹ� Ȱ��ȭ
    }

    private void Update()
    {
        if (!_isScroller) return; // ��ũ�ѷ��� Ȱ��ȭ�� ������

        foreach (ObjectGroup group in _objectGroups)
        {
            ScrollObjects(group);  // �� �׷쿡 ��ũ�� ����
        }
    }

    private void ScrollObjects(ObjectGroup group)
    {
        for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _spawnedObjects[i];
            obj.transform.position += Vector3.left * group.GetScrollSpeed() * Time.deltaTime;

            // �θ� ������Ʈ ��ġ �������� ���� ������ŭ ��� ������Ʈ�� ����
            if (obj.transform.position.x <= _parentTransform.position.x - _destroyOffset)
            {
                Destroy(obj);
                _spawnedObjects.RemoveAt(i);
            }
        }

        // �θ� ������Ʈ ��ġ �������� ���� ������ ����� �� �� ������Ʈ ����
        if (_spawnedObjects.Count == 0 || _spawnedObjects[_spawnedObjects.Count - 1].transform.position.x < _parentTransform.position.x + _spawnOffset)
        {
            SpawnRandomObject(group);
        }
    }

    private void SpawnRandomObject(ObjectGroup group)
    {
        GameObject prefab = group.GetObjects()[Random.Range(0, group.GetObjects().Length)];
        GameObject newObj = Instantiate(prefab, _parentTransform);

        float rightMostX = _spawnedObjects.Count > 0 ? GetRightMostObjectPosition(group) : _parentTransform.position.x;
        float randomSpacing = Random.Range(group.GetMinSpacing(), group.GetMaxSpacing());

        // �� ������Ʈ�� Y�� ��ġ�� prefab ���� ��ġ�� ����
        newObj.transform.position = new Vector3(rightMostX + randomSpacing, prefab.transform.position.y, newObj.transform.position.z);
        _spawnedObjects.Add(newObj);
    }

    private float GetRightMostObjectPosition(ObjectGroup group)
    {
        float maxX = _spawnedObjects.Count > 0 ? _spawnedObjects[0].transform.position.x : 0f;
        foreach (GameObject obj in _spawnedObjects)
        {
            if (obj.transform.position.x > maxX)
                maxX = obj.transform.position.x;
        }
        return maxX;
    }
}
