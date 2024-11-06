using UnityEngine;

public class Dummy : MonoBehaviour
{
    private void Start()
    {
        SceneController.Instance.LoadScene(E_SceneType.START);
    }
}
