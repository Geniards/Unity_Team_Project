using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    [SerializeField] private GameObject _text;

    [SerializeField] private float _delayTime;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if(_timer >= _delayTime)
        {
            _timer = 0;
            _text.SetActive(!_text.activeSelf);
        }

        if(Input.anyKeyDown)
        {
            SceneController.Instance.LoadScene(E_SceneType.LOAD);
        }
    }
}
