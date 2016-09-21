using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class makeWall : MonoBehaviour {
    public static int placingWall = 0;
    RaycastHit hitInfo = new RaycastHit();
    Transform temp;
    public GameObject wall;
    Vector2 start = new Vector2(0, 0);
    Vector2 end = new Vector2(0, 0);
    public float wallHeight;
    public float wallCenter;
	int timer=0;
	
    // Use this for initialization
    void Start () {
        placingWall = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
		timer--;
        
		if (timer == 1)
        {
            if (GameObject.Find("ErrorText") != null)
            {
                GameObject.Find("ErrorText").GetComponentInChildren<Text>().text = " ";
                GameObject.Find("ErrorText").SetActive(false);
            }
        }
		
        if (Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                temp = hitInfo.transform;
            }
            if (temp == null)
            {
                return;
            }
            //then can place what I need
            if ((temp.tag == "floor" || temp.tag == "wall" || temp.tag == "pillar" || temp.tag == "outerWall"))
                {
                    
                    placingWall++;
                    //select wall
                    if (placingWall == 1)
                    {
                        GameObject[] wallFinals = GameObject.FindGameObjectsWithTag("wall");
						foreach(GameObject wallFinal in wallFinals)
						{
							if(wallFinal.GetComponent<NewWallScript>()!=null && wallFinal.transform==temp)
							{
								GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

								foreach (GameObject g in objects)
								{
									if (g.name == "ErrorText")
									{
										g.SetActive(true);

										timer = 100;
										g.GetComponentInChildren<Text>().text = "Wall Selected";
									}
                        }
							}

						}
                        //this is starting point
                        start.x = hitInfo.point.x;
                        start.y = hitInfo.point.z;
                    }
                    //place wall
                    else if(placingWall>1)
                    {
                        //this is where place wall. this is ending point
                        end.x = hitInfo.point.x;
                        end.y = hitInfo.point.z;

                        float width = Mathf.Pow(Mathf.Pow((end.y - start.y), 2) + Mathf.Pow((end.x - start.x), 2), .5f);
                        if (width >= 2f)
                        {
                            transform.gameObject.GetComponent<bringUpMenu>().nOfWalls++;
                            if (transform.gameObject.GetComponent<bringUpMenu>().nOfWalls > transform.gameObject.GetComponent<bringUpMenu>().totalnofWalls)
                            {
                                transform.gameObject.GetComponent<bringUpMenu>().nOfWalls--;
                                return;
                            }
                        
                            //make wall from start to end.
                            GameObject obj = Instantiate(wall);
                            obj.tag = "wall";
                            obj.transform.position = new Vector3((start.x + end.x) / 2, wallCenter, (start.y + end.y) / 2);
                        
                            obj.transform.localScale = new Vector3(width, wallHeight, .1f);
                            obj.transform.rotation = Quaternion.identity;

                            float diffx = Mathf.Abs(end.x - start.x);
                            float diffy = Mathf.Abs(end.y - start.y);
                            if (diffx < diffy)
                            {
                                obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            }
                            else
                            {
                                obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            }
                       


                            placingWall = 0;

                            obj.AddComponent<NewWallScript>();
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
							    if(Camera.main.GetComponent<bringUpMenu>().nOfWalls==Camera.main.GetComponent<bringUpMenu>().totalnofWalls)
							    {
								    g.GetComponentInChildren<Text>().text = "No More walls left to place";
							    }
							    else{
                                    g.GetComponentInChildren<Text>().text = "Wall width is too small";
							    }
                                }
                            }
                        
                            placingWall = 0;
                        }
                    }
					else if(placingWall==0)
					{
						GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

								foreach (GameObject g in objects)
								{
									if (g.name == "ErrorText")
									{
										g.SetActive(true);

										timer = 100;
										g.GetComponentInChildren<Text>().text = "Wall Changes Saved";
									}
                        }
					}
					
				}
                else
                {
                    
                    placingWall = 0;
                }

        }
    }
}
