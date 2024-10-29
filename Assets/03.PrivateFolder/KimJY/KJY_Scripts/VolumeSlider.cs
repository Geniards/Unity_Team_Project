using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        volumeSlider.value = DataManager.Instance.BGMVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);    
    }

    private void SetVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }
}
