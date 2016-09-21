using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Xml;
using System.IO;

public class GetHeatMap : MonoBehaviour {

    public GameObject heatMapPlane;
    public GameObject heatPlane;
    public GameObject light;
    public GameObject mainCam;
    public Material colormat;

    // Use this for initialization
    void Start () {
        mainCam.transform.GetComponent<Camera>().orthographic = true;
        displayHeatMapFromFile();

	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                mainCam.transform.GetComponent<Camera>().orthographicSize--;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                mainCam.transform.GetComponent<Camera>().orthographicSize++;
            }
        }
	
	}

    public void hideAllUsersItems()
    {
        // hide all existing walls, doors, and pillars
        GameObject[] allWalls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall1 in allWalls)
        {
            //if (wall1.GetComponent<NewWallScript> () != null) {
            MeshRenderer render = wall1.gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = false;
            //}
        }
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject door1 in allDoors)
        {
            MeshRenderer render = door1.gameObject.GetComponentInChildren<MeshRenderer>();
            if (render != null)
            {
                render.enabled = false;
            }
        }
        GameObject[] allPillars = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillar1 in allPillars)
        {
            MeshRenderer render = pillar1.gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = false;
        }
    }

    void displayHeatMapFromFile()
    {
        hideAllUsersItems();
        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath+"/InputXMLS/Level1.xml");

        XmlNode userdata = doc.LastChild.FirstChild;

        string runId="";
        Debug.Log(doc.LastChild.FirstChild.Name);
        foreach (XmlNode node in userdata.ChildNodes)
        {
            if (node.Name == "Run-ID")
            {
                runId = node.InnerText;
            }

            if (node.Name == "Walls")
            {
                // for each wall
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    GameObject Wallobj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Wallobj.name = "besthmWall";
                    Wallobj.tag = "wall";
                    Wallobj.GetComponent<Renderer>().material = colormat;
                    // for each wall info (Position, Rotation, Scale)
                    foreach (XmlNode transformInfo in cnode.ChildNodes)
                    {
                        float X = 0;
                        float Y = 0;
                        float Z = 0;

                        if (transformInfo.Name == "Position")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Wallobj.transform.position = new Vector3(X, Y, Z);
                        }
                        if (transformInfo.Name == "Rotation")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);

                                    if (Y > .001)
                                    {
                                        Y = 90;
                                    }
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }

                            Wallobj.transform.eulerAngles = new Vector3(X, Y, Z);
                            //Wallobj.transform.rotation = Quaternion.Euler(X, Y, Z);
                        }
                        if (transformInfo.Name == "Scale")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Wallobj.transform.localScale = new Vector3(X, Y, Z);
                        }
                    }

                    Wallobj.SetActive(true);
                }
            }

            if (node.Name == "Pillars")
            {
                // for each pillar
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    GameObject Pillarobj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    Pillarobj.name = "besthmPillar";
                    Pillarobj.tag = "pillar";
                    Pillarobj.GetComponent<Renderer>().material = colormat;

                    // for each door info (Position, Rotation, Scale)
                    foreach (XmlNode transformInfo in cnode.ChildNodes)
                    {
                        float X = 0;
                        float Y = 0;
                        float Z = 0;

                        if (transformInfo.Name == "Position")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Pillarobj.transform.position = new Vector3(X, Y, Z);
                        }
                        if (transformInfo.Name == "Rotation")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);

                                    if (Y > .001)
                                    {
                                        Y = 90;
                                    }
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Pillarobj.transform.rotation = Quaternion.Euler(X, Y, Z);
                        }
                        if (transformInfo.Name == "Scale")
                        {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes)
                            {
                                if (transformInfoVariable.Name == "X")
                                {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y")
                                {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z")
                                {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Pillarobj.transform.localScale = new Vector3(X, Y, Z);
                        }
                    }
                    Pillarobj.SetActive(true);
                }
            }
        
    }
        
        byte[] bytes = File.ReadAllBytes(Application.dataPath + "/InputXMLS/" + runId + ".png");
        mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);

        heatMapPlane.SetActive(true);
        light.SetActive(false);
        heatPlane.SetActive(true);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Heatmap.CreateRenderPlane(tex);



    }
}
