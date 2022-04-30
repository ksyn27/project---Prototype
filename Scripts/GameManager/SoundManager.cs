using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource backsound;
    public AudioClip[] bgmList;
    

    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);


    }
    public void BackgroundVol(float vol)
    {
        mixer.SetFloat("backGround", Mathf.Log10(vol) * 20);
    }
    public void EffectVol(float vol)
    {
        mixer.SetFloat("effectSound", Mathf.Log10(vol) * 20);
    }
    public void MasterVol(float vol)
    {
        mixer.SetFloat("masterSound", Mathf.Log10(vol) * 20);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgmList.Length; i++)
        {
            if (arg0.name != "Boss")
            {
                if (arg0.name == bgmList[i].name)
                {
                    BacksoundPlay(bgmList[i]);
                }
            }
            if(arg0.name == "Boss")
            {
                if (bgmList[i].name == "Dungeon")
                    BacksoundPlay(bgmList[i]);
            }
        }
        
    }

    public void SoundPlay(string soundString , AudioClip audioClip)
    {
        GameObject sp = new GameObject(soundString + "Sound");
        AudioSource audioSource = sp.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("EffectSound")[0];
        audioSource.clip = audioClip;
        audioSource.volume = 0.3f;
        audioSource.Play();
        Destroy(sp, audioClip.length);
    }
    
    public void BacksoundPlay(AudioClip audioClip)
    {
        backsound.outputAudioMixerGroup = mixer.FindMatchingGroups("BackGround")[0];
        backsound.clip = audioClip;
        backsound.loop = true;
        backsound.volume = 0.3f;
        backsound.Play();

    }
    public void BossSound()
    {
        for (int i = 0; i < bgmList.Length; i++)
        {
            if (bgmList[i].name == "Boss")
                BacksoundPlay(bgmList[i]);
        }
    }


}
