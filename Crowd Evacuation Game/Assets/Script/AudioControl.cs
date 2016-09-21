using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    AudioSource audio;
	
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.M))
		{
			AudioListener.volume=0;
		}
		if(Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			AudioListener.volume=AudioListener.volume+0.1f;
		}
		if(Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			if(AudioListener.volume>=0.1f)
			AudioListener.volume=AudioListener.volume-0.1f;
		}
	
        if(bringUpMenu.running==1)
        {
            audio.Stop();
        }

	}
}
