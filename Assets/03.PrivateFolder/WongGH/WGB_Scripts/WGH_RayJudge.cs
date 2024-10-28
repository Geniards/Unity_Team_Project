using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_RayJudge : MonoBehaviour
{
    [SerializeField] float _greatDistance;
    [SerializeField] float _perfectDistance;

    Vector3 _checkTopPos;
    Vector3 _checkMiddlePos;
    Vector3 _checkBottomPos;
    Vector3 _curPos;
    Note _note;
    private void Start()
    {
        _checkTopPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.TOP);
        _checkMiddlePos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.MIDDLE);
        _checkBottomPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.BOTTOM);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CheckNote(_checkBottomPos);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CheckNote(_checkTopPos);

        }
        
    }
    /// <summary>
    /// 노트판정 메서드
    /// </summary>
    /// <param name="checkPos"></param>
    public void CheckNote(Vector3 checkPos)
    {
        this._curPos = checkPos;
        //RaycastHit2D hitRight = Physics2D.Raycast(_curPos, Vector3.right, _greatDistance);
        //RaycastHit2D hitLeft = Physics2D.Raycast(_curPos, Vector3.left, _greatDistance);
        Vector2 aPoint = new Vector2(_curPos.x - _perfectDistance / 2, _curPos.y - _perfectDistance / 2);
        Vector2 bPoint = new Vector2(_curPos.x + _perfectDistance / 2, _curPos.y + _perfectDistance / 2);
        Collider2D[] hits = Physics2D.OverlapAreaAll(aPoint, bPoint);
        Debug.DrawLine(aPoint,bPoint, Color.blue, 1f);
        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out Note note))
            {
                _note = note;
                float _distance = Vector2.Distance(_curPos, hit.transform.position);
                Debug.DrawLine(aPoint + new Vector2(0, _perfectDistance / 2), bPoint - new Vector2(0, _perfectDistance / 2), Color.blue, 1);
                if (_distance <= _perfectDistance)
                {
                    _note.OnHit(E_NoteDecision.Perfect);
                }
                else if(_distance <= _greatDistance)
                {
                    _note.OnHit(E_NoteDecision.Great);
                }
            }
        }
        
        // 양 옆에 노트가 있을 경우
        //if(hitLeft.collider != null && hitRight.collider != null) 
        //{
        //    if (hitLeft.distance > hitRight.distance) // 왼쪽보다 오른쪽이 기준선에 더 가까울경우 == o  | o
        //    {
        //        CheckNoteRight(_curPos);
        //    }
        //    else if (hitLeft.distance < hitRight.distance)
        //    {
        //        CheckNoteLeft(_curPos);
        //    }
        //}
        //// 좌측에만 노트가 있을 경우
        //else if (hitLeft.collider != null)
        //{
        //    CheckNoteLeft(_curPos);
        //}
        //// 우측에만 노트가 있을 경우
        //else if(hitRight.collider != null)
        //{
        //    CheckNoteRight(_curPos);
        //}
    }
    public void CheckNoteRight(Vector3 checkPos)
    {
        this._curPos = checkPos;
        RaycastHit2D hitRight = Physics2D.Raycast(_curPos, Vector3.right, _greatDistance);
        if (hitRight.collider.TryGetComponent(out Note note1))
        {
            _note = note1;
            if (hitRight.distance <= _perfectDistance)
            {
                _note.OnHit(E_NoteDecision.Perfect);
            }
            else if (hitRight.distance <= _greatDistance)
            {
                _note.OnHit(E_NoteDecision.Great);
            }
        }
    }
    public void CheckNoteLeft(Vector3 checkPos)
    {
        this._curPos = checkPos;
        RaycastHit2D hitLeft = Physics2D.Raycast(_curPos, Vector3.left, _greatDistance);
        if (hitLeft.collider.TryGetComponent(out Note note2))
        {
            _note = note2;
            if (hitLeft.distance <= _perfectDistance)
            {
                _note.OnHit(E_NoteDecision.Perfect);
            }
            else if (hitLeft.distance <= _greatDistance)
            {
                _note.OnHit(E_NoteDecision.Great);
            }
        }
    }
    
}
