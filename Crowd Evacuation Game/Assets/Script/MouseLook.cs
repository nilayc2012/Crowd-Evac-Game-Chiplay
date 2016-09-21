using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    GameObject msg;

    int timer;

    float rotationY = 0F;

    void Update()
    {
        //Debug.Log(msg.GetComponentInChildren<Text>().text);
        timer--;

       if (timer == 0)
        {
            msg.GetComponentInChildren<Text>().text = "";
            msg.SetActive(false);
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (sensitivityX < 20 && sensitivityY < 20)
            {
                sensitivityX = sensitivityX + 1;
                sensitivityY = sensitivityY + 1;
                msg.SetActive(true);
                msg.GetComponentInChildren<Text>().text = "Mouse Sensitivity increased";
                timer = 100;
            }
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            if (sensitivityX > 3 && sensitivityY > 3)
            {
                sensitivityX = sensitivityX - 1;
                sensitivityY = sensitivityY - 1;
                msg.SetActive(true);
                msg.GetComponentInChildren<Text>().text = "Mouse Sensitivity decreased";
                timer = 100;
            }
        }
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    void Start()
    {
        timer = 0;
        // Make the rigid body not change rotation
        /*if (rigidbody)
			rigidbody.freezeRotation = true;*/
        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        foreach (GameObject g in objects)
        {
            if (g.name == "ErrorText")
            {
                
                msg = g;
                //Debug.Log(msg.GetComponentInChildren<Text>().text);
            }
        }

    }
}