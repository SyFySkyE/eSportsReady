using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    [SerializeField] private PlayerStats player; // Music changes based on player stress    

    [SerializeField] private AudioClip chillMusic;
    [SerializeField] private AudioClip stressedMusic;
    [SerializeField] private AudioClip superStressedMusic;

    private AudioSource audioSource;
    private StressLevels currentLevel = StressLevels.Chillin;

    private void Awake()
    {
        player.OnStressChange += Player_OnStressChange;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Player_OnStressChange(string obj)
    {
        if (obj == this.currentLevel.ToString()) // StressChange gets called every time the player takes an Action. We don't wanna constantly restart the music if the stress level is the same.
        {
            return;
        }
        else
        {
            switch (obj)
            {
                case "Chillin":
                    this.currentLevel = StressLevels.Chillin;
                    audioSource.clip = chillMusic;
                    break;
                case "Stressed":
                    this.currentLevel = StressLevels.Stressed;
                    audioSource.clip = stressedMusic;
                    break;
                case "AHHHHHH":
                    this.currentLevel = StressLevels.AHHHHHH;
                    audioSource.clip = superStressedMusic;
                    break;                    
            }
            audioSource.Play();
        }                 
    }
}
