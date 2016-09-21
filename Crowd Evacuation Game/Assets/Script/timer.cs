using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

    public float time =0;
    public float subtime = 0;
    bool flag;
	// Use this for initialization
	void Start () {
        time = 0f;
        subtime = 0f;
        flag = false;
        //bringUpMenu.mytimer = 0;
        //GameController.totalTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 1.0f) {
            time = time + 1.0f;//time + Time.deltaTime;
        }
        else
        {
            subtime = subtime + 1.0f;
        }
	}


}
