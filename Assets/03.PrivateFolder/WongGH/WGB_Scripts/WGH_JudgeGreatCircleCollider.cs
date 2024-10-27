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
            Debug.Log("great �浹!");
            _JudgeCircle.note = note;
            _JudgeCircle._isGreatCircleIn = true;
        }
        
        
        // judgeCircle�� Note�� �Ʒ��� �ִ� getcomponent Note�� �������� �ȴ�.
        //collision.GetComponent<Note>().
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ����� �� judgecircle�� note�� null�� ��ȯ
        if (collision.TryGetComponent(out Note note))
        {
            _JudgeCircle.note = null;
            _JudgeCircle._isGreatCircleIn = false;
            Debug.Log("great ���!");
        }
    }
}
