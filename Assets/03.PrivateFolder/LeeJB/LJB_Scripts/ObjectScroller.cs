using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{
    [System.Serializable]
    public class ObjectGroup
    {
        [SerializeField] private GameObject[] _objects;
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private float _minSpacing;
        [SerializeField] private float _maxSpacing;

        public GameObject[] GetObjects() { return _objects; }
        public float GetScrollSpeed() { return _scrollSpeed; }
        public float GetMinSpacing() { return _minSpacing; }
        public float GetMaxSpacing() { return _maxSpacing; }
    }

    [SerializeField] private ObjectGroup[] _objectGroups;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private float _scrollDelay;
    [SerializeField] private float _destroyOffset;
    [SerializeField] private float _spawnOffset;

    private bool _isScroller = false;
    private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(StartScrollingAfterDelay(_scrollDelay));
        EventManager.Instance.AddAction(E_Event.BOSSDEAD, StopScrolling, this);
        EventManager.Instance.AddAction(E_Event.PLAYERDEAD, StopScrolling, this);
    }

    private IEnumerator StartScrollingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isScroller = true;
    }

    private void Update()
    {
        if (!_isScroller) return;

        foreach (ObjectGroup group in _objectGroups)
        {
            ScrollObjects(group);
        }
    }

    private void ScrollObjects(ObjectGroup group)
    {
        for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _spawnedObjects[i];
            obj.transform.position += Vector3.left * group.GetScrollSpeed() * Time.deltaTime;

            if (obj.transform.position.x <= _parentTransform.position.x - _destroyOffset)
            {
                Destroy(obj);
                _spawnedObjects.RemoveAt(i);
            }
        }

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

        newObj.transform.position = new Vector3(rightMostX + randomSpacing, newObj.transform.position.y, newObj.transform.position.z);
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

    private void StopScrolling()
    {
        _isScroller = false;
    }
}