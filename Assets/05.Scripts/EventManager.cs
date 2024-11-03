using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour, IManager
{
    private static EventManager _instance = null;
    public static EventManager Instance => _instance;

    private Transform _mainDirectory = null;
    private Dictionary<E_Event, Transform> _directorys =
        new Dictionary<E_Event, Transform>();

    private Dictionary<E_Event, List<EventContainer>> _events
        = new Dictionary<E_Event, List<EventContainer>>();

    public void Init()
    {
        _instance = this;
        _mainDirectory = new GameObject().transform;
        _mainDirectory.SetParent(transform);
        _mainDirectory.name = "EventList";
    }

    private void CreateDirectory(E_Event eventName)
    {
        if (_directorys.ContainsKey(eventName) == true)
            return;

        Transform newDir = new GameObject().transform;

        _directorys.Add(eventName, newDir);
        newDir.SetParent(_mainDirectory);
        newDir.name = $"{eventName}";
    }

    /// <summary>
    /// 해당 이벤트를 호출시킵니다.
    /// </summary>
    public void PlayEvent(E_Event eventType)
    {
        if (_events.ContainsKey(eventType) == false)
            return;

        foreach (var eventObject in _events[eventType])
        {
            eventObject.Run();
        }
    }

    /// <summary>
    /// 대상 이벤트 호출시 동작할 기능을 추가합니다.
    /// </summary>
    public void AddAction(E_Event eventType, Action action, MonoBehaviour user)
    {
        // 기존 이벤트 정보가 없었다면 새로운 경로등록 후 이벤트목록 생성
        if (_events.ContainsKey(eventType) == false)
        {
            CreateDirectory(eventType);
            _events.Add(eventType, new List<EventContainer>());
        }

        EventContainer newEvent = ObjPoolManager.Instance.
            GetObject<EventContainer>(E_Pool.EVENT);
        newEvent.transform.SetParent(_directorys[eventType]);

        newEvent.Initialize(action, user);
        _events[eventType].Add(newEvent);
    }

    /// <summary>
    /// 대상 이벤트에서 대상 기능을 삭제합니다.
    /// </summary>
    public void RemoveAction(E_Event eventType, Action action)
    {
        if(_events.ContainsKey(eventType) == false)
            throw new Exception("대상 이벤트가 없으나 기능을 삭제하려 합니다.");

        foreach (var eventObject in _events[eventType])
        {
            if(eventObject.IsSameAction(action))
            {
                eventObject.Return();
                return;
            }
        }

        throw new Exception("대상 이벤트가 없으나 기능을 삭제하려 합니다.");
    }

    /// <summary>
    /// 대상 이벤트의 모든 기능목록을 삭제합니다.
    /// </summary>
    public void ClearEvent(E_Event eventType)
    {
        foreach (var eventObject in _events[eventType])
        {
            eventObject.Return();
        }
    }

    /// <summary>
    /// 이벤트 목록 전체를 삭제합니다.
    /// </summary>
    public void ClearAllEvents()
    {
        foreach (var eventList in _events)
        {
            foreach (var eventObject in eventList.Value)
            {
                eventObject.Return();
            }
        }
    }

    /// <summary>
    /// Null 로 전환된 기능들을 전부 삭제합니다.
    /// </summary>
    public void CleanNulls()
    {
        foreach (var eventList in _events)
        {
            foreach (var eventObject in eventList.Value)
            {
                eventObject.AutoDestroy();
            }
        }
    }
}
