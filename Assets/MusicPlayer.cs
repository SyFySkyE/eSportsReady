using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Music Tracks")]
    [SerializeField] AudioClip[] musicTracks;

    private AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        int index = Random.Range(0, musicTracks.Length);
        musicSource.clip = musicTracks[index];
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
