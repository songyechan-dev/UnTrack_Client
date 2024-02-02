using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
                bgmPlayer.clip = bgms[2].clip;
                break;
            case 3:
                bgmPlayer.clip = bgms[3].clip;
                break;
            case 4:
                bgmPlayer.clip = bgms[4].clip;
                break;
            case 5:
                bgmPlayer.clip = bgms[5].clip;
                break;
            case 6:
                bgmPlayer.clip = bgms[6].clip;
                break;
            default:
                break;
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void ControlBGMVolume()
    {

    }

    public void PlaySFX(string _name)
    {
        for(int i = 0; i < sfxs.Length; i++)
        {
            if (_name.Equals(sfxs[i].name))
            {
                for(int j = 0; j < sfxPlayer.Length; j++)
                {
                    if (!sfxPlayer[i].isPlaying)
                    {
                        sfxPlayer[j].clip = sfxs[i].clip;
                        sfxPlayer[j].Play();

                        return;
                    }
                }

                Debug.Log("::: 사용 가능한 오디오 플레이어 없음 :::");
                return;
            }
        }

        Debug.Log("::: 효과음 존재 안함 :::");
        return;
    }

    public void StopSFX()
    {

    }

    public void ControlSFXVolume()
    {

    }
}
