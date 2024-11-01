using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    private const string AttackKey = "Attack";
    private const string BossDefeatKey = "BossDefeat";

    [SerializeField] private Animator _anim;

    public void PlayRushReadyAnim()
    {
        _anim.SetTrigger(AttackKey);
    }

    public void PlayBossWinAnim()
    {
        _anim.SetBool(BossDefeatKey, false);
    }

    public void PlayBossDefeatAnim()
    {
        _anim.SetBool(BossDefeatKey, true);
    }

}
