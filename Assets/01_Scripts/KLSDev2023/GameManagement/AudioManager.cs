using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioManager Instnce()
    {
        return instance;
    }

    [SerializeField]
    Sound[] bgms = null;
    [SerializeField]
    Sound[] sfxs = null;

    public AudioSource bgmPlayer = null;
    public AudioSource[] sfxPlayer = null;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public float bgmVolume;
    public float sfxVolume;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(this);
    }

    public void PlayBGM(int _sceneNumber)
    {
        switch (_sceneNumber)
        {
            case 0:
                bgmPlayer.clip = bgms[0].clip;
                break;
            case 1:
                bgmPlayer.clip = bgms[1].clip;
                break;
            case 2:
                bgmPlayer.clip = bgms[1].clip;
                break;
            case 3:
                bgmPlayer.clip = bgms[2].clip;
                break;
            case 4:
                bgmPlayer.clip = bgms[1].clip;
                break;
            case 5:
                bgmPlayer.clip = bgms[3].clip;
                break;
            case 6:
                bgmPlayer.clip = bgms[4].clip;
                break;
            default:
                break;
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void ChangeBGMVolume()
    {
        bgmPlayer.volume = bgmVolume;
    }

    public void SaveBGMVolume()
    {
        bgmVolume = bgmSlider.value;
    }

    public void PlaySFX(AudioSource _audioSource, string _name)
    {
        for(int i = 0; i < sfxs.Length; i++)
        {
            if (_name.Equals(sfxs[i].name))
            {
                _audioSource.clip = sfxs[i].clip;
                _audioSource.Play();

                return;
            }
        }

        Debug.Log("::: 효과음 존재 안함 :::");
        return;
    }

    public void StopSFX()
    {

    }

    public void ChangeSFXVolume()
    {
        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i].volume = sfxVolume;
        }
    }

    public void SaveSFXVolume()
    {
        sfxVolume = sfxSlider.value;
    }
}
