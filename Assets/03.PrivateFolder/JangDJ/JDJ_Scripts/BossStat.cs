using UnityEngine;

[System.Serializable]
public class BossStat
{
    [SerializeField] private int _hp;
    [SerializeField] private string _name;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _restCount;
    [SerializeField] private int _attackCount;

    public float MoveSpeed => _moveSpeed;
}