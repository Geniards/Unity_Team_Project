using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image loadSpinner;


    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("02.LoadScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false; // 90�۱����� �ε��ϰ� ��ٸ�

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null; //�ݺ����� ������ �������� ȭ���� ���ŵ��� ����.

            if (op.progress < 0.9f)
            {
                loadSpinner.transform.Rotate(new Vector3(0,0,-200)*Time.deltaTime);
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loadSpinner.transform.Rotate(new Vector3(0,0,-100) * Time.deltaTime);
            
                if (timer >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            
            }
        }
    }
}
