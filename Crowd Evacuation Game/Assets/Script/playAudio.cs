using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class playAudio : MonoBehaviour {

    public AudioClip clip;
    AudioSource audio;
    int timer;
	// Use this for initialization
	void Start () {
        timer = 0;
        audio = gameObject.GetComponent<AudioSource>();
        audio.clip = clip;
        audio.loop = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        
        timer++;

        if(timer==30)
        {
            audio.Play();
        }
        if(!audio.isPlaying && timer>30)
        {
            SceneManager.LoadScene("Start");
        }
	}
}
