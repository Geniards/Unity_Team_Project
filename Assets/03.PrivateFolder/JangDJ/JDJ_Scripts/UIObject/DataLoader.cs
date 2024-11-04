using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Image _image;

    private void Start()
    {
        DataLoad();
    }

    public void DataLoad()
    {
        StartCoroutine(Rotate());
        StartCoroutine(CSVLoading());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.back * _rotateSpeed);

            yield return null;
        }
    }

    private IEnumerator CSVLoading()
    {
        yield return GetComponent<CSVLoader>().LoadAllPatterns();

        yield return new WaitForSeconds(1f);

        SceneController.Instance.LoadScene(E_SceneType.LOBBY);
    }
}
