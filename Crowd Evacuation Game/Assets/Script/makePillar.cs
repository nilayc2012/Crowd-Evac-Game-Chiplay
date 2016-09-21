using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class makePillar : MonoBehaviour
{
    RaycastHit hitInfo = new RaycastHit();
    Transform temp;
    public GameObject pillar;
    float PillarWidth;
    public float pillarHeight;
	int timer;
	
    void Start()
    {
		timer=0;
    }
    // Update is called once per frame
    void Update()
    {
		timer--;
		if (timer == 1)
        {
            GameObject.Find("ErrorText").GetComponentInChildren<Text>().text = " ";
            GameObject.Find("ErrorText").SetActive(false);
        }
		
        if (Input.GetMouseButtonDown(0))
        {
            PillarWidth= transform.gameObject.GetComponent<bringUpMenu>().pillarWidth;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                temp = hitInfo.transform;
                //get the wall:
                //if this is wall then good
                if (temp.tag == "pillar")
                {
                    //delete door. make one wall
                    transform.gameObject.GetComponent<bringUpMenu>().nOfPillars--;
                    Destroy(temp.gameObject);
                }




                    }

            if (temp == null || temp.gameObject == null)
            {
                return;
            }

            if (temp.tag != "floor" && temp.tag != "pillar")
            {
                GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
                foreach (GameObject g in objects)
                {
                    if (g.name == "ErrorText")
                    {
                        g.SetActive(true);
                        g.GetComponentInChildren<Text>().text = "Can't Place a Pillar Here!!";
                        timer = 70;

                    }
                }
            }

            //then can place what I need
            if (temp.tag == "floor"){
				
				if(Camera.main.GetComponent<bringUpMenu>().nOfPillars == Camera.main.GetComponent<bringUpMenu>().totalnofPillars)
						{
							GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
						foreach (GameObject g in objects)
                        {
                            if (g.name == "ErrorText")
                            {
								 g.SetActive(true);
								g.GetComponentInChildren<Text>().text = "No more pillars left to place";
                               timer=100;
                                
                            }
                        }
						}
				
                    transform.gameObject.GetComponent<bringUpMenu>().nOfPillars++;
                    if (transform.gameObject.GetComponent<bringUpMenu>().nOfPillars > transform.gameObject.GetComponent<bringUpMenu>().totalnofPillars)
                    {
                    
                        transform.gameObject.GetComponent<bringUpMenu>().nOfPillars--;
                        return;
                    }
                    GameObject Pillarobj = Instantiate(pillar);
                Pillarobj.tag = "pillar";
                    Pillarobj.transform.localScale = new Vector3(PillarWidth, pillarHeight, PillarWidth);
                    Pillarobj.transform.position = new Vector3(hitInfo.point.x, .22f, hitInfo.point.z);
                }


                
                
            }
        }
    }

