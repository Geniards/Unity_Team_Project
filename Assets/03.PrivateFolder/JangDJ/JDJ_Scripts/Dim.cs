using UnityEngine;
using UnityEngine.UI;

public class Dim : MonoBehaviour
{
    [SerializeField] private bool _isBlockDim;

    private void Start()
    {
        if(_isBlockDim == false)
        {
            GetComponent<Image>().raycastTarget = false;
        }
    }
}
