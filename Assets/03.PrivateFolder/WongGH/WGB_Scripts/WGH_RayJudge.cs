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

    public Note Note { get; private set; }

    float _fPressTime;
    float _jPressTime;
    bool _isFpress;
    bool _isJpress;

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
            _jPressTime = Time.time;
            _isJpress = true;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _fPressTime = Time.time;
            _isFpress = true;
        }
        
        if(Mathf.Abs(_jPressTime - _fPressTime) <= 0.2f && _isJpress && _isFpress)
        {
            CheckNote(_checkMiddlePos);
            _jPressTime = -1;
            _fPressTime = -1;
            _isJpress = false;
            _isFpress = false;
        }
        else
        {
            if(_isJpress && !_isFpress && Input.GetKeyUp(KeyCode.J))
            {
                CheckNote(_checkBottomPos);
                _isJpress = false;
            }
            if(_isFpress && !_isJpress && Input.GetKeyUp(KeyCode.F))
            {
                CheckNote(_checkTopPos);
                _isFpress = false;
            }
        }
    }
    /// <summary>
    /// 노트판정 메서드
    /// </summary>
    public void CheckNote(Vector3 checkPos)
    {
        this._curPos = checkPos;
        Vector2 aPoint = new Vector2(_curPos.x - _greatDistance / 2, _curPos.y - _greatDistance / 4);
        Vector2 bPoint = new Vector2(_curPos.x + _greatDistance / 2, _curPos.y + _greatDistance / 4);
        Collider2D[] hits = Physics2D.OverlapAreaAll(aPoint, bPoint);
        Debug.DrawLine(aPoint,bPoint, Color.blue, 1f);
        foreach (Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out Note note))
            {
                Note = note;
                float _distance = Vector2.Distance(_curPos, hit.transform.position);
                Debug.DrawLine(aPoint + new Vector2(0, _greatDistance / 4), bPoint - new Vector2(0, _greatDistance / 4), Color.blue, 1);
                if (_distance <= _perfectDistance)
                {
                    Note.OnHit(E_NoteDecision.Perfect);
                }
                else if(_distance <= _greatDistance + 0.1f)
                {
                    Note.OnHit(E_NoteDecision.Great);
                }
            }
            else
            {
                Note = null;
            }
        }
    }
    //public void CheckNoteRight(Vector3 checkPos)
    //{
    //    this._curPos = checkPos;
    //    RaycastHit2D hitRight = Physics2D.Raycast(_curPos, Vector3.right, _greatDistance);
    //    if (hitRight.collider.TryGetComponent(out Note note1))
    //    {
    //        _note = note1;
    //        if (hitRight.distance <= _perfectDistance)
    //        {
    //            _note.OnHit(E_NoteDecision.Perfect);
    //        }
    //        else if (hitRight.distance <= _greatDistance)
    //        {
    //            _note.OnHit(E_NoteDecision.Great);
    //        }
    //    }
    //}
    //public void CheckNoteLeft(Vector3 checkPos)
    //{
    //    this._curPos = checkPos;
    //    RaycastHit2D hitLeft = Physics2D.Raycast(_curPos, Vector3.left, _greatDistance);
    //    if (hitLeft.collider.TryGetComponent(out Note note2))
    //    {
    //        _note = note2;
    //        if (hitLeft.distance <= _perfectDistance)
    //        {
    //            _note.OnHit(E_NoteDecision.Perfect);
    //        }
    //        else if (hitLeft.distance <= _greatDistance)
    //        {
    //            _note.OnHit(E_NoteDecision.Great);
    //        }
    //    }
    //}
    
}
