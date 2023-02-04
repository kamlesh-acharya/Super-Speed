using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    public AudioSource musicPlayer;
    public AudioClip[] allMusic;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.clip = allMusic[Random.Range(0, allMusic.Length)];
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
