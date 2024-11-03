using UnityEngine;

public class Test : MonoBehaviour
{
    public Animator animator;                               // Animator 컴포넌트
    public RuntimeAnimatorController baseController;        // 기본 Animator Controller
    public AnimatorOverrideController overrideController;   // Override Controller
    public AnimationOverrideData overrideData;              // 스크립트오브젝트 데이터


    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator가 할당되지 않았습니다.");
            return;
        }

        if (baseController == null)
        {
            Debug.LogError("기본 Animator Controller가 할당되지 않았습니다.");
            return;
        }

        // AnimatorOverrideController 생성 및 설정
        overrideController = new AnimatorOverrideController(baseController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{

        //    // 'Run' 상태의 애니메이션 클립을 새로운 클립으로 교체
        //    overrideController["bat-fly-Animation"] = overrideData.clip03;
        //    //animator.Play("Run", -1, 0f);
        //    Debug.Log("애니메이션 클립 이름: " + overrideData.clip03);
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{

        //    // 'Run' 상태의 애니메이션 클립을 새로운 클립으로 교체
        //    // []안에는 Motion의 이름이 그대로 들어가야 한다.
        //    overrideController["bat-fly-Animation"] = overrideData.clip02;
        //    //animator.Play("Run", -1, 0f);
        //    Debug.Log("애니메이션 클립 이름: " + overrideData.clip02);
        //}

    }
}
