using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{

    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private AudioMixMode MixMode;

    public float volumeValue;

    [SerializeField] private Slider volumeSlider;
    public void OnChangeSlider(float Value)
    {

        switch (MixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                AudioSource.volume = Value;
                volumeValue = Value;
                break;
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 100));
                volumeValue = Value;
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                volumeValue = Value;
                Debug.Log(volumeValue);
                break;
        }
    }

    private void Start()
    {
        volumeValue = 1.0f;
        volumeSlider.value = volumeValue;
        Mixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
    }

    public float GetVolume() { return volumeValue; }

    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}
