using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMediator : MonoBehaviour
{
    private static NoteMediator _instance;
    public static NoteMediator Instance => _instance = new GameObject("NoteMediator").AddComponent<NoteMediator>();

    private List<Note> _notesList = new List<Note>();

    private void Start()
    {
        // EventManager를 통해 BossRush 이벤트 구독
        //EventManager.Instance.AddAction(E_Event.NOTE_CLEAR, AllNoteReturn, this);
    }

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
        foreach (var note in _notesList)
        {
            note.ReturnToPool();
        }
    }
}
