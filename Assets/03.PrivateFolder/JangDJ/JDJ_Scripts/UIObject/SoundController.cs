using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        _slider.value = DataManager.Instance.MasterVolume;
        SoundManager.Instance.SoundSlider = _slider;
        _slider.value -= 0.00001f;
    }
}
