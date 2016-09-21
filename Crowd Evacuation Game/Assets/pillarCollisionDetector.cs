using UnityEngine;
using System.Collections;

public class pillarCollisionDetector : MonoBehaviour {

    public bool collided;
    // Use this for initialization
    void Awake()
    {
        collided = false;
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider cold)
    {
        if(cold.tag=="wall" || cold.tag=="door")
        {
            collided = true;
        }
    }
}
