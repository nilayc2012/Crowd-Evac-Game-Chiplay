using UnityEngine;
using System.Collections;

public class heatMapControls : MonoBehaviour
{

    public float speed = 50.0f; //max speed of camera
    public Camera cam;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(); //create (0,0,0)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.W) )
            {
                if (cam.orthographicSize > 1)
                {
                    cam.orthographicSize--;
                }
                return;
            }
            if (Input.GetKey(KeyCode.S) )
            {

                cam.orthographicSize++;

                return;
            }

        }
        //Movement of Camera
        if (Input.GetKey(KeyCode.W) )
        {
            dir.y = dir.y + .01f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y = dir.y - .01f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = dir.x - .01f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = dir.x + .01f;
        }

        dir.Normalize();

        transform.Translate(dir * speed * Time.deltaTime);
    }
}
