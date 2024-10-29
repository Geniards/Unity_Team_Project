using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour, IManager
{
    private static EventManager _instance = null;
    public static EventManager Instance => _instance;

    public void Init()
    {
        _instance = this;
    }

    private Dictionary<E_Event, List<Action>> _eventsTable 
        = new Dictionary<E_Event, List<Action>>();

    public void PlayEvent(E_Event eventType)
    {
        foreach (var action in _eventsTable[eventType])
        {
            action();
        }
    }

    /// <summary>
    /// 대상 이벤트 호출시 동작할 기능을 추가합니다.
    /// </summary>
    public void AddAction(E_Event eventType, Action action)
    {
        if (_eventsTable.ContainsKey(eventType) == false)
            _eventsTable.Add(eventType, new List<Action>());

        if (_eventsTable[eventType].Contains(action))
        {
            Debug.Log("이미 등록된 이벤트 재등록 요청중");
            return;
        }

        _eventsTable[eventType].Add(action);
    }

    /// <summary>
    /// 대상 이벤트에서 대상 기능을 삭제합니다.
    /// </summary>
    public void RemoveAction(E_Event eventType, Action action)
    {
        if (_eventsTable.ContainsKey(eventType) == false ||
            _eventsTable[eventType].Contains(action) == false)
                throw new Exception("대상 이벤트가 없으나 기능을 삭제하려 합니다.");

        _eventsTable[eventType].Remove(action);
    }

    /// <summary>
    /// 대상 이벤트의 모든 기능목록을 삭제합니다.
    /// </summary>
    public void ClearEvent(E_Event eventType)
    {
        _eventsTable[eventType] = new List<Action>();
    }

    /// <summary>
    /// 이벤트 목록 전체를 삭제합니다.
    /// </summary>
    public void ClearAllEvents()
    {
        _eventsTable = new Dictionary<E_Event, List<Action>>();
    }

    /// <summary>
    /// Null 로 전환된 기능들을 전부 삭제합니다.
    /// </summary>
    public void CleanNulls()
    {
        foreach (var table in _eventsTable)
        {
            List<Action> tempList = new List<Action>();

            for (int i = 0; i < table.Value.Count; i++)
            {
                if (table.Value[i] == null)
                {
                    Debug.Log("널이 검출됨");
                    continue;
                }
                    

                tempList.Add(table.Value[i]);
            }

            _eventsTable[table.Key] = tempList;
        }
    }
}
