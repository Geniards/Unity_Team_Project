using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{
    [System.Serializable]
    public class ObjectGroup
    {
        [SerializeField] private GameObject[] _objects;      // 그룹 내 오브젝트 배열
        [SerializeField] private float _scrollSpeed;         // 그룹별 스크롤 속도
        [SerializeField] private float _minSpacing;          // 최소 간격
        [SerializeField] private float _maxSpacing;          // 최대 간격

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

    [SerializeField] private ObjectGroup[] _objectGroups;    // 그룹 오브젝트를 관리하는 배열
    [SerializeField] private Transform _parentTransform;     // 자식으로 생성할 부모 오브젝트의 위치 참조
    [SerializeField] private float _scrollDelay;             // 실행 후 스크롤까지의 지연 시간
    [SerializeField] private float _destroyOffset;           // 부모 오브젝트 위치 기준으로 제거할 위치 오프셋
    [SerializeField] private float _spawnOffset;             // 부모 오브젝트 위치 기준으로 생성할 위치 오프셋

    private bool _isScroller = false;
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
    }

    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScroller = true; // 지연 시간 후 스크롤링 활성화
    }

    private void Update()
    {
        if (!_isScroller) return; // 스크롤러가 활성화될 때까지

        foreach (ObjectGroup group in _objectGroups)
        {
            ScrollObjects(group);  // 각 그룹에 스크롤 적용
        }
    }

    private void ScrollObjects(ObjectGroup group)
    {
        for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _spawnedObjects[i];
            obj.transform.position += Vector3.left * group.GetScrollSpeed() * Time.deltaTime;

            // 부모 오브젝트 위치 기준으로 설정 범위만큼 벗어난 오브젝트는 제거
            if (obj.transform.position.x <= _parentTransform.position.x - _destroyOffset)
            {
                Destroy(obj);
                _spawnedObjects.RemoveAt(i);
            }
        }

        // 부모 오브젝트 위치 기준으로 설정 범위를 벗어났을 때 새 오브젝트 생성
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

        // 새 오브젝트의 Y축 위치를 prefab 원본 위치로 유지
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
