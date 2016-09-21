using UnityEngine;
using System.Collections;

public class hoverandclick : MonoBehaviour {

    AudioSource audio4,audio5;
	// Use this for initialization
	void Start () {
        audio4 = GameObject.Find("architect").GetComponent<AudioSource>();
        audio5 = GameObject.Find("firefighter").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

	
	}

    public void OnmouseOver()
    {
        audio4.Play();
    }
    public void OnMouseClick()
    {
        audio5.Play();
    }
}
