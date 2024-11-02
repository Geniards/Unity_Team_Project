using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    private const string AttackKey = "Attack";
    //private const string BossDefeatKey = "BossDefeat";
    private const string BOSSWIN_KEY = "Fail";
    private const string BOSSDEFEAT_KEY = "Success";

    [SerializeField] private Animator _anim;

    public void PlayRushReadyAnim()
    {
        _anim.SetTrigger(AttackKey);
    }

    public void PlayBossWinAnim()
    {
        _anim.Play(BOSSWIN_KEY);
    }

    public void PlayBossDeadAnim()
    {
        _anim.Play(BOSSDEFEAT_KEY);
    }

}
