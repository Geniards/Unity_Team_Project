using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Note;
using static TreeEditor.TreeEditorHelper;

public class DBScoreNote : Note, IPoolingObj
{
    [Header("노트 세팅")]
    [SerializeField] private GameObject upNote;
    [SerializeField] private GameObject downNote;
    [SerializeField] private GameObject bodyColl;

    [Header("검기 노트 프리팹")]
    [SerializeField] private GameObject swordWaveNotePrefab;
    [SerializeField] private Transform bossTransform;

    private bool _upHit = false;
    private bool _downHit = false;
    private E_NoteDecision _decision;

    public E_Pool MyPoolType => E_Pool.DBSCORE_NOTE;

    public override void Initialize(Vector3 endPoint, float speed, int damage = 0, int stageNumber = 1, E_NoteType noteType = E_NoteType.None, E_SpawnerPosY notePosition = E_SpawnerPosY.BOTTOM)
    {
        base.Initialize(endPoint, speed, damage, stageNumber, noteType, notePosition);

        upNote.SetActive(true);
        downNote.SetActive(true);

        //    // TotalHeight를 기준으로 upNote와 downNote의 위치 설정
        float halfHeight = GameManager.Director.TotalHeight / 2f;

        // upNote는 위쪽에 배치
        if (upNote != null)
        {
            upNote.transform.position = transform.position + new Vector3(0, halfHeight, 0);
            Vector3 upNoteEndPoint = new Vector3(endPoint.x, upNote.transform.position.y, endPoint.z);
        }

        // downNote는 아래쪽에 배치
        if (downNote != null)
        {
            downNote.transform.position = transform.position - new Vector3(0, halfHeight, 0);
            Vector3 downNoteEndPoint = new Vector3(endPoint.x, downNote.transform.position.y, endPoint.z);
        }

        // bodyColl의 위치는 중간에 배치 (DBScoreNote의 기본 위치와 동일)
        if (bodyColl != null)
        {
            bodyColl.transform.position = transform.position;

            // bodyColl의 y축 크기를 두 노트 사이의 거리로 설정
            Vector3 bodyScale = bodyColl.transform.localScale;
            bodyColl.transform.localScale = new Vector3(bodyScale.x, halfHeight * 2, bodyScale.z);
        }
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override void OnHit(E_NoteDecision decision, E_Boutton button)
    {

        if (button == E_Boutton.F_BOUTTON)
        {
            _upHit = true;

            if (_upHit && _downHit)
            {
                DBHit(button);
            }

            upNote.SetActive(false);
        }
        if (button == E_Boutton.J_BOUTTON)
        {
            _downHit = true;
            
            if (_upHit && _downHit)
            {
                DBHit(button);
            }

            downNote.SetActive(false);
        }
    }

    /// <summary>
    /// 위, 아래 노트가 사라질시 동시노트도 사라지게 동작하는 메서드.
    /// </summary>
    private void DBHit(E_Boutton button)
    {
        Debug.Log("Hit On");

        if (!_isHit)
        {
            _isHit = true;
            ShowEffect();

            gameObject.SetActive(false);
            Debug.Log("DB노트 사라짐.");


            //if (isBoss)
            //{
            //    ReflectNote();
            //}

            ReturnToPool();
        }


    }

    public void ReflectNote()
    {
        Debug.Log("반사노트(검기노트)에 대한 오브젝트 풀로 전환 후 해당 노트는 삭제시킨다.");
        if (swordWaveNotePrefab != null)
        {
            GameObject swordWaveNote = Instantiate(swordWaveNotePrefab, transform.position, Quaternion.identity);
            SwordWaveNote swordWave = swordWaveNote.GetComponent<SwordWaveNote>();
            swordWave.InitializeSwordWave(bossTransform, speed, damage);
        }
    }

    public void Return()
    {
        ObjPoolManager.Instance.ReturnObj(MyPoolType, this.gameObject);
    }

    /// <summary>
    /// NoteMediator에서 등록된 노트를 제거하고 objPool로 반환.
    /// </summary>
    public override void ReturnToPool()
    {
        GameManager.Mediator.Unregister(this);
        Return();
    }
}
