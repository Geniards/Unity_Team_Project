using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_RayJudge : MonoBehaviour
{
    [Header("TOP RIGHT 芭府")]
    [SerializeField] float _distTopRightGreat;
    [SerializeField] float _distTopRightPerfect;

    [Header("TOP LEFT 芭府")]
    [SerializeField] float _distTopLeftGreat;
    [SerializeField] float _distTopLeftPerfect;

    [Header("MIDDLE RIGHT 芭府")]
    [SerializeField] float _distMiddleRightGreat;
    [SerializeField] float _distMiddleRightPerfect;

    [Header("MIDDLE LEFT 芭府")]
    [SerializeField] float _distMiddleLeftGreat;
    [SerializeField] float _distMiddleLeftPerfect;

    [Header("BOTTOM RIGHT 芭府")]
    [SerializeField] float _distBottomRightGreat;
    [SerializeField] float _distBottomRightPerfect;

    [Header("BOTTOM LEFT 芭府")]
    [SerializeField] float _distBottomLeftGreat;
    [SerializeField] float _distBottomLeftPerfect;

    [SerializeField] Vector3 _checkTopPos;
    [SerializeField] Vector3 _checkMiddlePos;
    [SerializeField] Vector3 _checkBottomPos;

    bool _topGreatLeft;
    bool _topGreatRight;
    bool _topPerfectLeft;
    bool _topPerfectRight;
    
    bool _middleGreatLeft;
    bool _middleGreatRight;
    bool _middlePerfectLeft;
    bool _middlePerfectRight;

    bool _bottomGreatLeft;
    bool _bottomGreatRight;
    bool _bottomPerfectLeft;
    bool _bottomPerfectRight;

    private void Start()
    {
        _checkTopPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.TOP);
        _checkMiddlePos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.MIDDLE);
        _checkBottomPos = GameManager.NoteDirector.GetCheckPoses(E_SpawnerPosY.BOTTOM);
    }

    private void Update()
    {
        Vector3 _originTop = _checkTopPos;
        Vector3 _originMiddle = _checkMiddlePos;
        Vector3 _originBottom = _checkBottomPos;

        if(Physics.Raycast(_originTop, Vector3.right, out RaycastHit hitTopRIght, _distTopRightGreat))
        {
            if(hitTopRIght.distance <= _distTopRightGreat)
            {
                _topGreatRight = true;
            }
            else if(hitTopRIght.distance <= _distTopRightPerfect)
            {
                _topPerfectRight = true;
            }
        }

        if (Physics.Raycast(_originTop, Vector3.left, out RaycastHit hitTopLeft, _distTopLeftGreat))
        {
            if (hitTopLeft.distance <= _distTopLeftGreat)
            {
                _topGreatLeft = true;
            }
            else if (hitTopLeft.distance <= _distTopLeftGreat)
            {
                _topPerfectLeft = true;
            }
        }

        if (Physics.Raycast(_originMiddle, Vector3.right, out RaycastHit hitMiddleRIght, _distMiddleRightGreat))
        {
            if (hitMiddleRIght.distance <= _distMiddleRightGreat)
            {
                _middleGreatRight = true;
            }
            else if (hitMiddleRIght.distance <= _distMiddleRightGreat)
            {
                _middlePerfectRight = true;
            }
        }

        if (Physics.Raycast(_originMiddle, Vector3.left, out RaycastHit hitMiddleLeft, _distMiddleLeftGreat))
        {
            if (hitMiddleLeft.distance <= _distMiddleLeftGreat)
            {
                _middleGreatLeft = true;
            }
            else if (hitMiddleLeft.distance <= _distMiddleLeftGreat)
            {
                _middlePerfectLeft = true;
            }
        }

        if (Physics.Raycast(_originMiddle, Vector3.right, out RaycastHit hitBottomRIght, _distBottomRightGreat))
        {
            if (hitBottomRIght.distance <= _distBottomRightGreat)
            {
                _bottomGreatRight = true;
            }
            else if (hitBottomRIght.distance <= _distBottomRightGreat)
            {
                _bottomPerfectRight = true;
            }
        }

        if (Physics.Raycast(_originMiddle, Vector3.right, out RaycastHit hitBottomLeft, _distBottomLeftGreat))
        {
            if (hitBottomLeft.distance <= _distBottomLeftGreat)
            {
                _bottomGreatLeft = true;
            }
            else if (hitBottomLeft.distance <= _distBottomLeftGreat)
            {
                _bottomPerfectLeft = true;
            }
        }
    }
}
