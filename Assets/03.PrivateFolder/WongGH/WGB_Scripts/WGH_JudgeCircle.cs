using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_JudgeCircle : MonoBehaviour
{
    [Header("��ġ����(�ϴ�)")]
    #region
    //[SerializeField] float _setLeftCheckPos;        // ���� �ϴ� üũ���� �� ����
    //[SerializeField] float _setRightCheckPos;       // ������ �ϴ� üũ���� �� ����
    #endregion

    [Header("��Ÿ")]
    [SerializeField] WGH_PlayerController _player;
    [SerializeField]
    #region
    //[SerializeField] Vector2 _circleRight;          // ������ �ϴ� ���� ��
    //[SerializeField] Vector2 _circleLeft;           // ���� �ϴ� ���� ��
    //public GameObject TestLeftPrefab;
    //[SerializeField] GameObject _testRightPrefab;

    //public bool Right { get; private set; }
    //public bool Left { get; private set; }
    //public bool Miss { get; private set; }
    #endregion
    private void Start()
    {
        _player = FindAnyObjectByType<WGH_PlayerController>();
        #region
        // TODO : ������(checkPos, ������)�� �������� ����, �����ʿ� ����


        //// �÷��̾�� ��ŭ ������ �Ÿ��� üũ�� �κ��� �������� �⺻�� ����
        //// ����
        //_circleLeft = new Vector2(_player.transform.position.x + _setLeftCheckPos, _player.transform.position.y);
        //// ������
        //_circleRight = new Vector2( _player.transform.position.x + _setRightCheckPos, _player.transform.position.y);

        //if(TestLeftPrefab != null)
        //{
        //    // ���� ������ ����
        //    Instantiate(TestLeftPrefab, _circleLeft, Quaternion.identity);
        //}

        //if (_testRightPrefab != null)
        //{
        //    // ������ ������ ����
        //    Instantiate(_testRightPrefab, _circleRight, Quaternion.identity);
        //}
        #endregion
    }

    private void Update()
    {
        #region
        //Debug.DrawLine(_player.transform.position, _circleLeft );
        //if( Right && Left)
        //{
        //Debug.Log("perfect");
        //}
        //else if( !Right && Left )
        //{
        //Debug.Log("good");
        //}
        //else if (Right && !Left)
        //{
        //Debug.Log("good");
        //}
        #endregion
    }

    #region
    /// <summary>
    /// Left / Right üũ ���� ���� �޼���
    /// </summary>
    //public void SetRightCheckTrue()
    //{
    //    this.Right = true;
    //}
    //
    //public void SetRightCheckFalse()
    //{
    //    this.Right = false;
    //}
    //
    //public void SetLeftCheckTrue()
    //{
    //    this.Left = true;
    //}
    //public void SetLeftCheckFalse()
    //{
    //    this.Left = false;
    //}
    #endregion
}
