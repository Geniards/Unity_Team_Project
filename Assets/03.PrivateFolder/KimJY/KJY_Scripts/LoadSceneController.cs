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
        op.allowSceneActivation = false; // 90퍼까지만 로드하고 기다림

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null; //반복문이 끝나기 전까지는 화면이 갱신되지 않음.

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
