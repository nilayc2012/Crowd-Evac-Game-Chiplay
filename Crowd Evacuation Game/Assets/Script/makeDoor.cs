using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class makeDoor : MonoBehaviour {
    RaycastHit hitInfo = new RaycastHit();
    RaycastHit hit = new RaycastHit();
    Transform temp;
    Transform childtemp;
    public GameObject door;
    public GameObject rightWall;
    public float WidthOfDoor ;
    public float wallHeight ;
    GameObject[] points;
    public static bool recomputeFlag;
    public Text errorText;
    public int timer = 0;
    bool setTimer = false;


    // Use this for initialization
    void Start () {
        
        recomputeFlag = false;
        setTimer = false;
        timer = 0;
    }


	// Update is called once per frame
	void Update () {

            timer--;
 

   
        if (timer == 1)
        {
            setTimer = false;
            if (GameObject.Find("ErrorText") != null)
            {
                GameObject.Find("ErrorText").GetComponentInChildren<Text>().text = " ";
                GameObject.Find("ErrorText").SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
               
                temp = hitInfo.transform;
            
                if(temp.tag!="door" && temp.tag != "wall")
                {
                    GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
                    foreach (GameObject g in objects)
                    {
                        if (g.name == "ErrorText")
                        {
                            g.SetActive(true);
                            g.GetComponentInChildren<Text>().text = "Doors can only be placed on inner walls";
                            timer = 70;

                        }
                    }
                }
                    //get the wall:
                    //if this is wall then good
                    if (temp.tag == "door")
                {
                    transform.gameObject.GetComponent<bringUpMenu>().nOfDoors--;
                    //delete door. make one wall
                    childtemp = temp.GetChild(0);
                    //vertical
                    if (childtemp.rotation.y == 0)
                    {
                        //get down wall:
                        GameObject downWall=null;
                        if (Physics.Raycast(new Vector3(childtemp.position.x+ WidthOfDoor/2 + .01f, childtemp.position.y+3f, childtemp.position.z ), new Vector3(0, -3, 0), out hit))
                        {
                            
                            Transform wall = hit.transform;
                            if (wall.tag == "wall")
                            {
                               
                                downWall = wall.gameObject;
                            }
                        }
                        //get UpWall
                        GameObject upWall=null;
                        if (Physics.Raycast(new Vector3(childtemp.position.x - WidthOfDoor / 2 - .01f, childtemp.position.y + 3f, childtemp.position.z), new Vector3(0, -3, 0), out hit))
                        {
                           
                            Transform wall = hit.transform;
                            if (wall.tag == "wall")
                            {
                               
                                upWall = wall.gameObject;
                            }
                        }
                        if (downWall != null&&upWall!=null)
                        {
                            float topCoord = upWall.transform.position.x - upWall.transform.localScale.x / 2;
                            float bottomCoord = downWall.transform.position.x + downWall.transform.localScale.x / 2;
                            float width = Math.Abs(topCoord - bottomCoord);
                            float middle = (topCoord + bottomCoord) / 2;
                            upWall.transform.position = new Vector3(middle, childtemp.position.y, childtemp.position.z);
                            upWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                            Destroy(downWall);
                        }
                       else if(upWall==null && downWall!=null)
                        {
                            float topCoord = childtemp.gameObject.transform.position.x - childtemp.gameObject.transform.localScale.x / 2;
                            float bottomCoord = downWall.transform.position.x + downWall.transform.localScale.x / 2;
                            float width = Math.Abs(topCoord - bottomCoord);
                            float middle = (topCoord + bottomCoord) / 2;
                            downWall.transform.position = new Vector3(middle, childtemp.position.y, childtemp.position.z);
                            downWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        else if(upWall!=null && downWall==null)
                        {
                            float topCoord = upWall.transform.position.x - upWall.transform.localScale.x / 2;
                            float bottomCoord = childtemp.gameObject.transform.position.x + childtemp.gameObject.transform.localScale.x / 2;
                            float width = Math.Abs(topCoord - bottomCoord);
                            float middle = (topCoord + bottomCoord) / 2;
                            upWall.transform.position = new Vector3(middle, childtemp.position.y, childtemp.position.z);
                            upWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        else if(upWall==null && downWall==null)
                        {
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        
                    }
                    //horizontal
                    else
                    {
                        //get left wall:
                        GameObject leftWall = null;
                        if (Physics.Raycast(new Vector3(childtemp.position.x , childtemp.position.y + 3f, childtemp.position.z - WidthOfDoor / 2 -.01f), new Vector3(0, -3, 0), out hit))
                        {
                            Transform wall = hit.transform;
                            if (wall.tag == "wall")
                            {
                                leftWall = wall.gameObject;
                            }
                        }
                        //get right wall
                        GameObject rightWall = null;
                        if (Physics.Raycast(new Vector3(childtemp.position.x , childtemp.position.y + 3f, childtemp.position.z + WidthOfDoor / 2+ .01f), new Vector3(0, -3, 0), out hit))
                        {
                            Transform wall = hit.transform;
                            if (wall.tag == "wall")
                            {
                                rightWall = wall.gameObject;
                            }
                        }
                        if (leftWall != null && rightWall != null)
                        {
                            

                            float leftCoord = leftWall.transform.position.z - leftWall.transform.localScale.x / 2;
                            float rightCoord = rightWall.transform.position.z + rightWall.transform.localScale.x / 2;
                            float width = Math.Abs(rightCoord- leftCoord);
                            float middle = (leftCoord + rightCoord) / 2 ;
                            leftWall.transform.position = new Vector3(leftWall.transform.position.x, leftWall.transform.position.y, middle);
                            leftWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                            Destroy(rightWall);
                        }
                        else if(leftWall == null && rightWall != null)
                        {
                            float leftCoord = childtemp.gameObject.transform.position.z - childtemp.gameObject.transform.localScale.x / 2;
                            float rightCoord = rightWall.transform.position.z + rightWall.transform.localScale.x / 2;
                            float width = Math.Abs(rightCoord - leftCoord);
                            float middle = (leftCoord + rightCoord) / 2;
                            rightWall.transform.position = new Vector3(childtemp.gameObject.transform.position.x, childtemp.gameObject.transform.position.y, middle);
                            rightWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        else if(leftWall != null && rightWall == null)
                        {
                            float leftCoord = leftWall.transform.position.z - leftWall.transform.localScale.x / 2;
                            float rightCoord = childtemp.gameObject.transform.position.z + childtemp.gameObject.transform.localScale.x / 2;
                            float width = Math.Abs(rightCoord - leftCoord);
                            float middle = (leftCoord + rightCoord) / 2;
                            leftWall.transform.position = new Vector3(leftWall.transform.position.x, leftWall.transform.position.y, middle);
                            leftWall.transform.localScale = new Vector3(width, wallHeight, .1f);
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        else if(leftWall == null && rightWall == null)
                        {
                            Destroy(childtemp.gameObject);
                            Destroy(temp.gameObject);
                        }
                        

                    }

               }


                if (temp.tag == "wall")
                {
					
						if(Camera.main.GetComponent<bringUpMenu>().nOfDoors == Camera.main.GetComponent<bringUpMenu>().totalnofDoors)
						{
							GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
						foreach (GameObject g in objects)
                        {
                            if (g.name == "ErrorText")
                            {
								 g.SetActive(true);
								g.GetComponentInChildren<Text>().text = "No more doors left to place";
                               timer=100;
                                
                            }
                        }
						}

                    //if its wall then split it up:

                    //this is the point where hit: hitInfo.point;
                    if (temp.GetComponent<InActiveScript>() == null)
                    {

                        if (ConnectionScript.falseListTrans.Contains(temp.transform))
                        {
                            GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

                            foreach (GameObject g in objects)
                            {
                                if (g.name == "ErrorText")
                                {
                                    g.SetActive(true);

                                    timer = 180;
                                    g.GetComponentInChildren<Text>().text = "Doors cannot be placed on an unconnected wall";
                                }
                            }


                        }
                        else
                        {
                            //vertical
                            if (temp.rotation.y == 0)
                            {

                                float xminUp = temp.position.x - temp.localScale.x / 2;
                                float xmaxUp = hitInfo.point.x - WidthOfDoor / 2;
                                float xminDown = hitInfo.point.x + WidthOfDoor / 2;
                                float xmaxDown = temp.position.x + temp.localScale.x / 2;
                                float widthUp = Math.Abs(xmaxUp - xminUp);
                                float widthDown = Math.Abs(xmaxDown - xminDown);
                                float midUp = xminUp + Mathf.Abs(xminUp - xmaxUp) / 2;
                                float midDown = xminDown + Mathf.Abs(xminDown - xmaxDown) / 2;

                                if (Math.Abs(hitInfo.point.x - xminUp) > WidthOfDoor / 2 && Math.Abs(xmaxDown - hitInfo.point.x) > WidthOfDoor / 2)
                                {
                                    transform.gameObject.GetComponent<bringUpMenu>().nOfDoors++;

                                    if (transform.gameObject.GetComponent<bringUpMenu>().nOfDoors > transform.gameObject.GetComponent<bringUpMenu>().totalnofDoors)
                                    {
                                        transform.gameObject.GetComponent<bringUpMenu>().nOfDoors--;
                                        return;
                                    }

                                    temp.position = new Vector3(midUp, temp.position.y, temp.position.z);
                                    temp.localScale = new Vector3(widthUp, temp.localScale.y, temp.localScale.z);
                                   
                                    GameObject obj = Instantiate(rightWall);
                                    
                                    obj.tag = "wall";
                                    obj.transform.position = new Vector3(midDown, temp.position.y, temp.position.z);
                                    obj.transform.localScale = new Vector3(widthDown, temp.localScale.y, temp.localScale.z);
                                    obj.transform.rotation = temp.rotation;
                                    //if this is a user placed wall
                                    if (temp.GetComponent<NewWallScript>() != null)
                                    {
                                        obj.AddComponent<NewWallScript>();
                                        obj.GetComponent<NewWallScript>().enabled = false;
                                        temp.GetComponent<NewWallScript>().neighbors.Add(obj);
                                        obj.GetComponent<NewWallScript>().neighbors.Add(temp.gameObject);
                                    }
                                    //place door
                                    GameObject Doorobj = Instantiate(door);
                                    Doorobj.SetActive(true);
                                    Doorobj.tag = "door";
                                    Doorobj.transform.position = new Vector3(hitInfo.point.x+WidthOfDoor/2, temp.position.y, temp.position.z);
                                    Doorobj.transform.localScale = new Vector3(WidthOfDoor, 1.2f,.1f);
                                    Doorobj.transform.rotation = temp.rotation;
                                    Doorobj.GetComponent<WallsNextToIt>().rightWall = obj;
                                    Doorobj.GetComponent<WallsNextToIt>().leftWall = temp.gameObject;


                                }

                            }
                            //horizontal
                            else
                            {

                                float zminLeft = temp.position.z - temp.localScale.x / 2;
                                float zmaxLeft = hitInfo.point.z - WidthOfDoor / 2;
                                float zminRight = hitInfo.point.z + WidthOfDoor / 2;
                                float zmaxRight = temp.position.z + temp.localScale.x / 2;
                                float widthLeft = zmaxLeft - zminLeft;
                                float widthRight = zmaxRight - zminRight;
                                float midLeft = zminLeft + Mathf.Abs(zminLeft - zmaxLeft) / 2;
                                float midRight = zminRight + Mathf.Abs(zmaxRight - zminRight) / 2;
                                if (hitInfo.point.z - zminLeft > WidthOfDoor / 2 && zmaxRight - hitInfo.point.z > WidthOfDoor / 2)
                                {

                                    transform.gameObject.GetComponent<bringUpMenu>().nOfDoors++;

                                    if (transform.gameObject.GetComponent<bringUpMenu>().nOfDoors > transform.gameObject.GetComponent<bringUpMenu>().totalnofDoors)
                                    {
                                        transform.gameObject.GetComponent<bringUpMenu>().nOfDoors--;
                                        return;
                                    }
                                    temp.position = new Vector3(temp.position.x, temp.position.y, midLeft);
                                    temp.localScale = new Vector3(widthLeft, temp.localScale.y, temp.localScale.z);
                                    GameObject obj = Instantiate(rightWall);
                                    obj.tag = "wall";
                                    obj.transform.position = new Vector3(temp.position.x, temp.position.y, midRight);
                                    obj.transform.localScale = new Vector3(widthRight, temp.localScale.y, temp.localScale.z);
                                    obj.transform.rotation = temp.rotation;
                                    if (temp.GetComponent<NewWallScript>() != null)
                                    {
                                        obj.AddComponent<NewWallScript>();
                                        obj.GetComponent<NewWallScript>().enabled = false;
                                        temp.GetComponent<NewWallScript>().neighbors.Add(obj);
                                        obj.GetComponent<NewWallScript>().neighbors.Add(temp.gameObject);
                                    }
                                    //place door
                                    GameObject Doorobj = Instantiate(door);
                                    Doorobj.SetActive(true);
                                    Doorobj.tag = "door";
                                    Doorobj.transform.position = new Vector3(temp.position.x, temp.position.y, hitInfo.point.z-WidthOfDoor/2);
                                    Doorobj.transform.localScale = new Vector3(WidthOfDoor, 1.2f, .1f);
                                    Doorobj.transform.eulerAngles = new Vector3(temp.rotation.x,90,temp.rotation.z);
                                    Doorobj.GetComponent<WallsNextToIt>().rightWall = obj;
                                    Doorobj.GetComponent<WallsNextToIt>().leftWall = temp.gameObject;

                                }

                            }


                        }


                    }
                    else
                    {
                        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

                        foreach (GameObject g in objects)
                        {
                            if (g.name == "ErrorText")
                            {
                                g.SetActive(true);

                                timer = 180;
                                g.GetComponentInChildren<Text>().text = "Doors cannot be placed on this wall";
                            }
                        }
                    }
                }
            }
        }

    }
}
