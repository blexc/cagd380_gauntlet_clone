using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NarratorLine
{
    dontShootFood,
    playerShotFood,
    playerTookDamage,
    cantUseMagicWithout,
    poggers,
    saveKeys,
    foodHeals,
    artifactPoints,
    savePotion,
}

[RequireComponent(typeof(AudioSource))]
public class Narrator : MonoBehaviour
{
    // singleton
    static private Narrator _instance;
    static public Narrator Instance { get { return _instance; } }

    [SerializeField] List<AudioClip> audioClips;
    List<bool> hasPlayed = new List<bool>();
    AudioSource audioSource;

    void Start()
    {
        // ensure only one instance
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;

        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < audioClips.Count; i++)
        {
            hasPlayed.Add(false);
        }
    }

    public void SayLine(NarratorLine line)
    {
        int i = (int)line;
        if (!hasPlayed[i])
        {
            audioSource.clip = audioClips[i];
            audioSource.Play();
            hasPlayed[i] = true;
        }
    }
}





















