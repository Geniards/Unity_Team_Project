using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    private const string AttackKey = "Attack";
    private const string BossWin = "BossWin";

    [SerializeField] private Animator _anim;

    public void PlayRushReadyAnim()
    {
        _anim.SetTrigger(AttackKey);
    }

    public void PlayBossWinAnim()
    {
        _anim.SetBool(BossWin, true);
    }

    public void PlayBossDefeatAnim()
    {
        _anim.SetBool(BossWin, false);
    }

}
