using UnityEngine;
using System.Collections;

public class changeCameraMode : MonoBehaviour
{
    float theScreenWidth;
    float theScreenHeight;
    int Boundary  = 50; // distance from edge scrolling starts
    int speed = 5;
    public bool doesMouseMoveOnBorder = false;
    // Use this for initialization
    void Start()
    {
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
        doesMouseMoveOnBorder = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.orthographic)
        {
            Camera.main.gameObject.GetComponent<MouseLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            if (doesMouseMoveOnBorder)
            {
                moveWhenMouseOnBorder();
            }
        }
        if (Input.GetKeyDown("c"))
        {
            //make camera perspective if orthographic
            if (Camera.main.orthographic == true)
            {
                changeToPerspective();
                doesMouseMoveOnBorder = false;
            }
            //make camera orthographic if not already
            else
            {
                changeToOrthographic();
                doesMouseMoveOnBorder = true;

            }

        }
    }

    public void changeToPerspective()
    {
        Camera.main.orthographic = false;
        Camera.main.gameObject.GetComponent<heatMapControls>().enabled = false;
        Camera.main.gameObject.GetComponent<MouseLook>().enabled = true;
        Camera.main.gameObject.GetComponent<mainCameraControls>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void changeToOrthographic()
    {
        Camera.main.orthographic = true;
        Camera.main.gameObject.GetComponent<heatMapControls>().enabled = true;
        Camera.main.gameObject.GetComponent<MouseLook>().enabled = false;
        Camera.main.gameObject.GetComponent<mainCameraControls>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Camera.main.gameObject.transform.position = new Vector3(0, 10, 0);
        Camera.main.gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    void moveWhenMouseOnBorder()
    {
        if (Input.mousePosition.x > theScreenWidth - Boundary)
        {
            Camera.main.gameObject.transform.position = Camera.main.gameObject.transform.position + new Vector3(speed * Time.deltaTime, 0, 0); // move on +X axis
        }
        if (Input.mousePosition.x < 0 + Boundary)
        {
            Camera.main.gameObject.transform.position = Camera.main.gameObject.transform.position - new Vector3(speed * Time.deltaTime, 0, 0); // move on -X axis
        }
        if (Input.mousePosition.y > theScreenHeight - Boundary)
        {
            Camera.main.gameObject.transform.position = Camera.main.gameObject.transform.position + new Vector3(0, 0, speed * Time.deltaTime); // move on +Z axis
        }
        if (Input.mousePosition.y < 0 + Boundary)
        {

            Camera.main.gameObject.transform.position = Camera.main.gameObject.transform.position - new Vector3(0, 0, speed * Time.deltaTime); // move on -Z axis
        }
    }
}
