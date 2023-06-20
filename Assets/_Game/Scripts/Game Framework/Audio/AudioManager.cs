using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, ISaveManager
{
    public static AudioManager Instance { get; private set; }   

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixerSnapshot[] musicSnapshotOn_Off = new AudioMixerSnapshot[2];

    [SerializeField] List<AudioClip> throwWeaponVFXList;
    [SerializeField] List<AudioClip> hitVFXList;
    [SerializeField] List<AudioClip> deadVFXList;

    private bool isMusicOn;
    private bool isVibrationOn = true;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayThrowWeaponSound(Vector3 position)
    {
        transform.position = position;
        int index = Random.Range(0, throwWeaponVFXList.Count);
        audioSource.PlayOneShot(throwWeaponVFXList[index]);
    }

    public void PlayHitSound(Vector3 position)
    {
        transform.position = position;
        int index = Random.Range(0, hitVFXList.Count);
        audioSource.PlayOneShot(hitVFXList[index]);
    }

    public void PlayDeadSound(Vector3 position)
    {
        transform.position = position;
        int index = Random.Range(0, deadVFXList.Count);
        audioSource.PlayOneShot(deadVFXList[index]);
    }

    public void ToggleMusic(bool isOff)
    {
        isMusicOn = !isOff;

        if(!isOff)
        {
            masterMixer.TransitionToSnapshots(musicSnapshotOn_Off, new float[] { 1f, 0f}, 0f);
        }
        else
        {
            masterMixer.TransitionToSnapshots(musicSnapshotOn_Off, new float[] { 0f, 1f }, 0f);
        }
    }

    public void ToggleVibration(bool isOn)
    {
        //TODO: implement turn On/Off vibration.
    }

    public void LoadData(GameData data)
    {
        isMusicOn = data.isMusicOn;
        isVibrationOn = data.isVibrationOn;

        ToggleMusic(!isMusicOn);
        ToggleVibration(isVibrationOn);
    }

    public void SaveData(ref GameData data)
    {
        data.isMusicOn = isMusicOn;
        data.isVibrationOn = isVibrationOn;
    }
}
