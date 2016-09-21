using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConnectionScript : MonoBehaviour {

    public static bool notConnected;
    public GameObject errorText;
    public Material falseMatter;
    public Material trueMatter;
    public static List<Transform> falseListTrans;



    // Use this for initialization
    void Start () {
        notConnected = false;
	}
	
	// Update is called once per frame
	void Update () {

        GameObject[] walltemp = GameObject.FindGameObjectsWithTag("wall");

        GameObject[] outers = GameObject.FindGameObjectsWithTag("outerWall");

        GameObject[] walls = new GameObject[walltemp.Length+outers.Length];
        walltemp.CopyTo(walls, 0);
        outers.CopyTo(walls, walltemp.Length);


        List<GameObject> trueList = new List<GameObject>();
        List<GameObject> falseList = new List<GameObject>();
        falseListTrans = new List<Transform>();

        foreach (GameObject wall1 in walls)
        {


            if (wall1.tag != "outerWall" && wall1.transform.localScale.x >= 2f)
            {
                
                if (wall1.gameObject.transform.rotation.y == 0)
                {
                    int count = 0;
                    float left = wall1.gameObject.transform.position.x - wall1.gameObject.transform.localScale.x / 2;
                    float right = wall1.gameObject.transform.position.x + wall1.gameObject.transform.localScale.x / 2;

                    List<float> xcords = new List<float>();
                    bool leftFlag = false;
                    bool rightFlag = false;

                    foreach (GameObject wall2 in walls)
                    {
                        if (wall1 != wall2 && wall2.transform.rotation.y != 0)
                        {

                            if (!xcords.Contains(wall2.transform.position.x)){

                                float top = wall2.gameObject.transform.position.x - wall2.gameObject.transform.localScale.z / 2;
                                float bottom = wall2.gameObject.transform.position.x + wall2.gameObject.transform.localScale.z / 2;
                                if (right >= top && right <= bottom+0.05f&&!rightFlag)
                                {
                                    xcords.Add(wall2.transform.position.x);
                                    count++;
                                    rightFlag = true;
                                }
                                else if (left >= top-0.05f && left <= bottom&&!leftFlag)
                                {
                                    count++;
                                    xcords.Add(wall2.transform.position.x);
                                    leftFlag = true;
                                }
                            }
                        }
                    }
                    if (count < 2)
                    {
                        bool leftdoorflag = false;
                        bool rightDoorFlag = false;
                        if (!leftFlag)
                        {
                            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
                            foreach (GameObject door in doors)
                            {
                                float left2 = door.transform.GetChild(0).transform.position.x - door.transform.GetChild(0).transform.localScale.x / 2;
                                float right2 = door.transform.GetChild(0).transform.position.x + door.transform.GetChild(0).transform.localScale.x / 2;

                                if ((left >= right2-0.05 && left >= left2) && wall1.transform.position.z == door.transform.GetChild(0).transform.position.z)
                                {
                                  leftdoorflag = true;
                                  break;
                                }
                            }
                               
                         }
                        
                        if(!rightFlag)
                        {
                            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
                            
                            foreach (GameObject door in doors)
                            {

                                float left2 = door.transform.GetChild(0).transform.position.x + door.transform.GetChild(0).transform.localScale.x / 2;
                                float right2 = door.transform.GetChild(0).transform.position.x - door.transform.GetChild(0).transform.localScale.x / 2;

                                if ((right <= right2+0.05f && right <= left2) && wall1.transform.position.z == door.transform.GetChild(0).transform.position.z)
                                {
                                    rightDoorFlag = true;
                                    break;
                                }


                            }
                        }


                        if (!leftdoorflag && !rightDoorFlag)
                        {
                            falseList.Add(wall1);
                        }
                    }
                    else
                    {
                        trueList.Add(wall1);
                    }
                }
                else
                {
                    int count = 0;
                    float left = wall1.gameObject.transform.position.z - wall1.gameObject.transform.localScale.x / 2;
                    float right = wall1.gameObject.transform.position.z + wall1.gameObject.transform.localScale.x / 2;

                    List<float> zcords = new List<float>();

                    bool leftFlag = false;
                    bool rightFlag = false;

                    foreach (GameObject wall2 in walls)
                    {

                        if (wall1 != wall2 && wall2.transform.rotation.y == 0)
                        {

                            if (!zcords.Contains(wall2.transform.position.z))
                            {
                                float top = wall2.gameObject.transform.position.z + wall2.gameObject.transform.localScale.z / 2;
                                float bottom = wall2.gameObject.transform.position.z - wall2.gameObject.transform.localScale.z / 2;

                                if (right >= bottom && right <= top+0.05f&&!rightFlag)
                                {
                                    zcords.Add(wall2.transform.position.z);
                                    rightFlag = true;
                                    count++;
                                }
                                else if (left <= top && left >= bottom-0.05f&&!leftFlag)
                                {
                                    zcords.Add(wall2.transform.position.z);

                                    leftFlag = true;
                                    count++;

                                }

                            }
                        }
                  }

                    if (wall1.GetComponent<NewWallScript>() != null)
                   

                    if (count < 2)
                    {
                        bool leftdoorflag = false;
                        bool rightDoorFlag = false;
                        if (!leftFlag)
                        {
                            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
                            foreach (GameObject door in doors)
                            {
                                float left2 = door.transform.position.z - door.transform.localScale.x / 2;
                                float right2 = door.transform.position.z + door.transform.localScale.x / 2;

                                if ((left >= right2-0.05 && left >= left2&&!leftdoorflag) && wall1.transform.position.x == door.transform.position.x)
                                {
                                    leftdoorflag = true;
                                    break;
                                }
                            }

                        }

                        if (!rightFlag)
                        {
                            GameObject[] doors = GameObject.FindGameObjectsWithTag("door");

                            foreach (GameObject door in doors)
                            {

                                float left2 = door.transform.GetChild(0).transform.position.z + door.transform.localScale.x / 2;
                                float right2 = door.transform.GetChild(0).transform.position.z - door.transform.localScale.x / 2;

                                if ((right <= right2+0.05f && right <= left2&&!rightDoorFlag) && wall1.transform.position.x == door.transform.position.x)
                                {
                                    rightDoorFlag = true;
                                    break;
                                }


                            }
                        }

                        if (!leftdoorflag && !rightDoorFlag)
                        {
                            falseList.Add(wall1);
                        }
                        
                    }
                    else
                    {
                        trueList.Add(wall1);
                    }
                }
            }
        }

        if (falseList.Count >0)
        {
            notConnected = true;
            errorText.GetComponent<Text>().text = "Walls marked in red are not correctly placed";

            foreach(GameObject obj in falseList)
            {
                obj.GetComponent<Renderer>().material = falseMatter;
                falseListTrans.Add(obj.transform);
            }
        }
        else
        {
            notConnected = false;
            //errorText.GetComponent<Text>().text = "";
        }
        
        foreach(GameObject obj in trueList)
        {
            obj.GetComponent<Renderer>().material = trueMatter;
        }


    }
}
