using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [SerializeField] WGH_PlayerController _player;
    [SerializeField] Vector2 _circleRight;
    [SerializeField] Vector2 _circleLeft;

    private void Start()
    {
        _circleRight = new Vector2( _player.transform.position.x + 3f, _player.transform.position.y);
    }
}
