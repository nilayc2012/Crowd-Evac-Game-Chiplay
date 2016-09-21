using UnityEngine;
using System.Collections;

public class checkActive : MonoBehaviour {

    public AudioClip alarm;
    AudioSource audio;
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        audio.clip = alarm;

	}
	
	// Update is called once per frame
	void Update () {

        if (GameController.startSiren)
        {
            if(!audio.isPlaying)
            audio.Play();

        }
        else
        {
            if(audio.isPlaying)
            audio.Stop();
        }

    }
}
