using UnityEngine;

[System.Serializable]
public class BossStat
{
    [SerializeField] private float _hp;
    [SerializeField] private float _moveSpeed;

    public float MoveSpeed => _moveSpeed;
    public float Hp => _hp;

    /// <summary>
    /// 매개변수로 주어지는 값을 멤버 hp 에 더하고, 결과 수치를 반환합니다.
    /// </summary>
    public float AddHp(float addValue)
    {
        _hp += addValue;
        return _hp;
    }
}