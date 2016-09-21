using UnityEngine;
using System.Collections;

public class hoverScript : MonoBehaviour {

    AudioSource audio1,audio3;
	// Use this for initialization
	void Start () {
        audio1 = this.GetComponent<AudioSource>();
        audio3 = GameObject.Find("Button (1)").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseOver()
    {
        audio1.Play();
    }

    public void OnMouseClick()
    {
        audio3.Play();
    }

}
