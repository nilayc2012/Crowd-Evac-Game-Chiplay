using UnityEngine;
using System.Collections;
using System;

public class TimerScript : MonoBehaviour {

    public int time;
    bool timer;
	// Use this for initialization
	void Start () {
        time = 0;
        timer = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(timer)
        {
            time++;
        }
	}

    public void startTimer()
    {
        timer = true;
    }
    public double endTimer()
    {
        timer = false;
        return Convert.ToDouble(time / Time.deltaTime);
    }
}
