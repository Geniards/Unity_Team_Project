using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeGreatCircleCollider : MonoBehaviour
{
    [SerializeField] WGH_JudgeCircle _JudgeCircle;
    private void Start()
    {
        _JudgeCircle = FindAnyObjectByType<WGH_JudgeCircle>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Note note))
        {
            Debug.Log("great 충돌!");
            _JudgeCircle.note = note;
            _JudgeCircle._isGreatCircleIn = true;
        }
        
        
        // judgeCircle의 Note를 아래에 있는 getcomponent Note로 가져오면 된다.
        //collision.GetComponent<Note>().
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 벗어났을 때 judgecircle의 note를 null로 전환
        if (collision.TryGetComponent(out Note note))
        {
            _JudgeCircle.note = null;
            _JudgeCircle._isGreatCircleIn = false;
            Debug.Log("great 벗어남!");
        }
    }
}
