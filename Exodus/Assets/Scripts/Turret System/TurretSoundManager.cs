using Malee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSoundManager : MonoBehaviour
{
    static TurretSoundManager instance;
    public static TurretSoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TurretSoundManager>();
            return instance;
        }
    }

    [Reorderable]
    public TurretSounds sounds;

    public void PlaySound(string soundName, bool reset = false)
    {
        foreach (TurretSound ts in sounds)
        {
            if (ts.soundName == soundName)
            {
                ts.PlaySound(reset);
                break;
            }
        }
    }
}

[System.Serializable]
public class TurretSounds : ReorderableArray<TurretSound> { }

[System.Serializable]
public class TurretSound
{
    public string soundName;
    public AudioClip clip;
    public AudioSource clipDestination;

    public void PlaySound(bool reset)
    {
        if (!clipDestination.isPlaying || reset)
        {
            clipDestination.clip = clip;
            clipDestination.Play();
        }
    }
}