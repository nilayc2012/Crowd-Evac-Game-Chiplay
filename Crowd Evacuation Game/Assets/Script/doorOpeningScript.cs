using UnityEngine;
using System.Collections;
using System;

public class doorOpeningScript : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 previousPos;
    private Quaternion previousRot;
    private bool collided = false;
    private int timer = 0;
    bool startcount;
    Vector3 defaultRot;
    Vector3 openRot;
    public AudioClip openclip;
    public AudioClip closeclip;
    bool closing, opening;
    // Use this for initialization
    void Start () {
        //rb = GetComponent<Rigidbody>();
        closing = false;
        opening = false;
        defaultRot = transform.eulerAngles;
        if (transform.rotation.y == 0)
            openRot = new Vector3(transform.rotation.x, 90, transform.rotation.z);
        else
            openRot = new Vector3(transform.rotation.x, 180, transform.rotation.z);
        collided = false;
        startcount = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (collided)
        {
            if (Mathf.Abs(transform.eulerAngles.y - openRot.y) <= 1)
            {
                collided = false;
            }
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * 2.0f);
            opening = true;
            //Debug.Log(Math.Floor(transform.rotation.y)+" "+ Math.Floor(openRot.y));

        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * 2.0f);
            closing = true;
            opening = false;
        }

        if (opening && Mathf.Abs(transform.eulerAngles.y - defaultRot.y) >= 30)
        {
            transform.GetChild(0).gameObject.GetComponent<NavMeshObstacle>().enabled = true;
        }
        if (closing && Mathf.Abs(transform.eulerAngles.y - defaultRot.y) < 30)
        {
            transform.GetChild(0).gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        }


    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("agent"))
        {

                AudioSource.PlayClipAtPoint(openclip, other.transform.position, 1.0f);
            collided = true;
        }

    }
}
