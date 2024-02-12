using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SOUNDTYPE
{
    WALK = 0,          // Player 걷는 소리
    PICKUP = 1,        // Player 아이템 잡는 소리
    DIG = 2,           // Player 장애물 제거 소리
    TRAIN_START = 3,   // 기차 출발
    TRAIN_MOVE = 4,    // 기차 이동
    TRAIN_CRASH = 5,   // 기차 충돌 
    FACTORY_WORK = 6,  // 제작소 제작 중
    FACTORY_FIRE = 7,  // 제작소 화재 발생
    WATER = 8,         // 제작소 화재 진압 
    UI_CLICK = 9,      // UI 클릭 소리
    UI_GAZE = 10,      // UI 게이지 소리
    UPGRADE = 11       // 업그레이드 성공
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instnce()
    {
        return instance;
    }

    [SerializeField]
    AudioClip[] bgms;
    [SerializeField]
    AudioClip[] sfxs;

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
        bgmPlayer.volume = PlayerPrefs.GetFloat("bgm_Volume");
    }

    public void PlayBGM(int _sceneNumber)
    {
        switch (_sceneNumber)
        {
            case 0:
                bgmPlayer.clip = bgms[0];
                bgmPlayer.Play();
                break;
            case 1:
                bgmPlayer.clip = bgms[1];
                bgmPlayer.Play();
                break;
            case 2:
                bgmPlayer.clip = bgms[1];
                bgmPlayer.Play();
                break;
            case 3:
                bgmPlayer.clip = bgms[2];
                bgmPlayer.Play();
                break;
            case 4:
                bgmPlayer.clip = bgms[1];
                bgmPlayer.Play();
                break;
            case 5:
                bgmPlayer.clip = bgms[3];
                bgmPlayer.Play();
                break;
            case 6:
                bgmPlayer.clip = bgms[4];
                bgmPlayer.Play();
                break;
            default:
                break;
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public  void ChangeBGMVolume()
    {
        bgmPlayer.volume = bgmVolume;
    }

    public void SaveBGMVolume()
    {
        bgmVolume = bgmSlider.value;
        
    }

    public void PlaySFX(AudioSource _audioSource, SOUNDTYPE _soundType)
    {
        if (sfxs[(int)_soundType] != null)
        {
            _audioSource.clip = sfxs[(int)_soundType];
            _audioSource.Play();
        }
        else
        {
            Debug.Log("::: 해당 SFX 없음 :::");
        }
    }

    public void StopSFX(AudioSource _audioSource, SOUNDTYPE _soundType)
    {
        if (_audioSource.clip == sfxs[(int)_soundType])
        {
            
            _audioSource.Stop();
        }
        else
        {
            Debug.Log("::: 해당 SFX 없음 :::");
        }
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
