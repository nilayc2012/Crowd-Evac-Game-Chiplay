using UnityEngine;
using System.Collections;

public class wallCollisionDetector : MonoBehaviour {

    public bool collided;
    void Awake()
    {
        collided = false;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider cold)
    {
        if (cold.tag == "pillar")
        {
            collided = true;
        }
    }
}
