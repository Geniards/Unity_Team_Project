using UnityEngine;

[System.Serializable]
public class BossStat
{
    [SerializeField] private float _hp;
    [SerializeField] private string _name;
    [SerializeField] private int _score;

    public float Hp => _hp;
    public string Name => _name;
    public int Score => _score;

    /// <summary>
    /// 매개변수로 주어지는 값을 멤버 hp 에 더하고, 결과 수치를 반환합니다.
    /// </summary>
    public float AddHp(float addValue)
    {
        _hp += addValue;
        return _hp;
    }

    public float SetHp(float value)
    {
        this._hp = value;
        return _hp;
    }

    /// <summary>
    /// 현재 개체의 데이터를 target으로 복사합니다.
    /// </summary>
    public void CopyData(BossStat target)
    {
        target._hp = this._hp;
        target._name = this._name;
    }

    public void InitScore(int value)
    { this._score = value; }

    public void ReduceScore()
    { _score /= 2; }
}