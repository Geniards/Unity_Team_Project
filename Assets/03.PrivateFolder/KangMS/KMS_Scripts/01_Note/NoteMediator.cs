using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMediator : MonoBehaviour,IManager
{
    private static NoteMediator _instance = null;
    public static NoteMediator Instance => _instance;

    private List<Note> _notesList = new List<Note>();

    /// <summary>
    /// 노트 등록
    /// </summary>
    public void Register(Note note)
    {
        if (!_notesList.Contains(note))
        {
            _notesList.Add(note);
        }
    }

    /// <summary>
    /// // 노트 제거
    /// </summary>
    public void Unregister(Note note)
    {
        if (_notesList.Contains(note))
        {
            _notesList.Remove(note);
        }
    }

    /// <summary>
    /// 호출시 생성된 노트를 active를 false로 전환 후 pool로 반환
    /// </summary>
    public void AllNoteReturn()
    {
        for (int i = _notesList.Count-1; i >= 0; i--)
        {
            _notesList[i].ReturnToPool();
        }
    }

    public void Init()
    {
        _instance = this;
        EventManager.Instance.AddAction(E_Event.NOTE_CLEAR, AllNoteReturn, this);
    }
}
