using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;
using System.Xml;
using System;


public class bringUpMenu : MonoBehaviour
{
    public float squareFactor;
    //for SHowData
    public Camera mainCamera;
    public Canvas menuWithEndMid;
    public Canvas serviceMenu;
    //objects to disappear
    public GameObject endGame;
    public GameObject midGame;
    public GameObject light;
    public GameObject canvasDev;
    public static bool crowdflag;
    public static bool noList;
    //object to make appear
    public GameObject heatMapPlane;
    public GameObject heatPlane;
    //object to appear
    //public GameObject image;
    private IEnumerator coroutine;

    //public static bool running=false;
    public static int running = 0;
    //running=0: not running yet. beginning of level
    //running=1: simulating
    //running=2: restarting
    //running=3: everyone escaped. no quitting
    public Canvas menu;
    bool menuEnabled = false;
    public GameObject mainCam;
    public int nOfDoors = 0;
    public int totalnofDoors = 5;
    public int nOfPillars = 0;
    public int totalnofPillars = 5;
    public int nOfWalls = 0;
    public int totalnofWalls = 5;
    public float doorWidth;
    public float pillarWidth;
    public int NoOfAgents;

    public Text showWidthPillarS;
    public Slider pillarWidthS;
    public GameObject agentObj;

    //make active when restart
    public GameObject restartLevel;
    public GameObject infoButton;
    public GameObject heatMapButton;
    public GameObject nextLevelButton;
    public GameObject bestHeatMapButton;

    public static bool operating = false;
    //text that show max number of doors and pillars
    public Text textDoors;
    public Text textPillars;
    public Text textWall;
    GameObject[] walls;
    GameObject[] outerWall;

    public static float mytimer = 0.0f;
    public static bool setTimer = false;
    public static bool pause;

    private MidModalPanel midmodalPanel;
    private UnityAction restart;
    private UnityAction quit;

    public static System.Random r;

    //Text for DisplayInfo
    public Text amountOfPeopleEscaped;
    public Text amountOfPeopleLeft;
    public Text totalEscapeTime;
    public Text averageEscapeTime;
    public static float subTime = 0.0f;
    int timer = 0;
    bool timeFlag = false;
    public static float timeConst = 0.0f;
    public static float changeTimeConst = 0.0f;
    public static float planearea;
    public static bool simflag;

    void Awake()
    {
        simflag = false;
        midmodalPanel = MidModalPanel.Instance();
        restart = new UnityAction(replayFuction);
        quit = new UnityAction(quitFunction);
        r = new System.Random();
        Cursor.visible = true;
        timer = 0;
        Time.timeScale = 1.0f;
        noList = false;

    }

    public void endGameReplay()
    {

        //if not running then cant bring up this menu
        if (running == 0)
        {
            actuallyRestart();

        }
        running = 2;
        //deactivate all agents
        GameObject[] agents = GameObject.FindGameObjectsWithTag("agent");

        foreach (GameObject a in agents)
        {
            Destroy(a);
        }


        //stop camera movemen
        /*mainCam.GetComponent<changeCameraMode>().changeToOrthographic();
        mainCam.GetComponent<changeCameraMode>().doesMouseMoveOnBorder = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;*/
        mainCam.GetComponent<changeCameraMode>().doesMouseMoveOnBorder = false;

        //deactivate other menus
        menuWithEndMid.enabled = true;
        endGame.SetActive(false);
        midGame.SetActive(false);
        canvasDev.SetActive(false);


        //stop camera movement
        mainCam.GetComponent<MouseLook>().enabled = false;
        mainCam.GetComponent<mainCameraControls>().enabled = false;
        mainCam.GetComponent<heatMapControls>().enabled = true;
        mainCamera.orthographic = true;

        //place cam right
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            mainCamera.orthographicSize = 8;
            mainCam.transform.position = new Vector3(0f, 14f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            mainCam.transform.position = new Vector3(-6.7f, 50f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 30;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            mainCam.transform.position = new Vector3(-6.7f, 100f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }
        else if (SceneManager.GetActiveScene().name == "Level4")
        {
            mainCam.transform.position = new Vector3(-6.7f, 200f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }
        else if (SceneManager.GetActiveScene().name == "Level5")
        {
            mainCam.transform.position = new Vector3(-6.7f, 400f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }

        //display heatmap button:
        heatMapButton.SetActive(true);
        bestHeatMapButton.SetActive(true);
        //display restart button
        restartLevel.SetActive(true);
        //display information button
        infoButton.SetActive(true);
        nextLevelButton.SetActive(true);

    }

    public void replayFuction()
    {
        //if not running then cant bring up this menu
        if (running == 0)
        {
            actuallyRestart();
            return;

        }
        running = 2;
        //deactivate all agents
        GameObject[] agents = GameObject.FindGameObjectsWithTag("agent");

        foreach (GameObject a in agents)
        {
            Destroy(a);
        }



        //deactivate other menus
        menuWithEndMid.enabled = true;
        endGame.SetActive(false);
        midGame.SetActive(false);
        canvasDev.SetActive(false);

        mainCam.GetComponent<changeCameraMode>().doesMouseMoveOnBorder = false;

        //stop camera movement
        mainCam.GetComponent<MouseLook>().enabled = false;
        mainCam.GetComponent<mainCameraControls>().enabled = false;
        mainCam.GetComponent<heatMapControls>().enabled = true;
        mainCamera.orthographic = true;

        //place cam right
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            mainCamera.orthographicSize = 8;
            mainCam.transform.position = new Vector3(0f, 14f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            mainCam.transform.position = new Vector3(-6.7f, 50f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 30;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            mainCam.transform.position = new Vector3(-6.7f, 100f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }
        else if (SceneManager.GetActiveScene().name == "Level4")
        {
            mainCam.transform.position = new Vector3(-6.7f, 200f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }
        else if (SceneManager.GetActiveScene().name == "Level5")
        {
            mainCam.transform.position = new Vector3(-6.7f, 400f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
            mainCamera.orthographicSize = 80;
        }

        //display heatmap button:
        heatMapButton.SetActive(true);
        bestHeatMapButton.SetActive(true);
        //display restart button
        restartLevel.SetActive(true);
        //display information button
        infoButton.SetActive(true);
    }

    public void displayHeatMap()
    {
        //put heatmap stuff in here
        showAllUsersItems();
        destroyAllBestHeatmapItems();
        ShowHeatMap();
        FileScript.checkedHM = true;
        //createPNG();
    }

    public void displayBestHeatMap()
    {
        FileScript.checkedBHM = true;
        hideAllUsersItems();
        destroyAllBestHeatmapItems();
        StartCoroutine(getHeatMapData());
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

    public void showAllUsersItems()
    {
        // show all existing walls, doors, and pillars
        GameObject[] allWalls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall1 in allWalls)
        {
            //if (wall1.GetComponent<NewWallScript> () != null) {
            MeshRenderer render = wall1.gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = true;
            //}
        }
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject door1 in allDoors)
        {
            MeshRenderer render = door1.gameObject.GetComponentInChildren<MeshRenderer>();
            if (render != null)
            {
                render.enabled = true;
            }
        }
        GameObject[] allPillars = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillar1 in allPillars)
        {
            MeshRenderer render = pillar1.gameObject.GetComponentInChildren<MeshRenderer>();
            render.enabled = true;
        }
    }

    public void destroyAllBestHeatmapItems()
    {
        GameObject[] allWalls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall1 in allWalls)
        {
            if (wall1.name == "besthmWall")
            {
                Destroy(wall1);
            }
        }
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject door1 in allDoors)
        {
            if (door1.name == "besthmDoor")
            {
                Destroy(door1);
            }
        }
        GameObject[] allPillars = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillar1 in allPillars)
        {
            if (pillar1.name == "besthmPillar")
            {
                Destroy(pillar1);
            }
        }
    }

    IEnumerator getHeatMapData()
    {
        int l = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().value;
        string los = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        l = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().value;
        string loa = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        l = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().value;
        string homo = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        string tlos = "", tloa = "", thomo = "";

        float mintime = System.Single.PositiveInfinity;
        string runId = "";

        string url = "http://crowdevac.com/getheatmapdata.php";  //---live
                                                                 //string url="http://localhost/store_data.php?scene="+SceneManager.GetActiveScene().name;   //---local
        WWWForm loginform = new WWWForm();
        loginform.AddField("scene", SceneManager.GetActiveScene().name);
        loginform.AddField("loa", loa);
        loginform.AddField("los", los);
        loginform.AddField("homo", homo);

        WWW www = new WWW(url,loginform);
        yield return www;
        string xml = www.text;
        Debug.Log(xml);
        XmlDocument doc = new XmlDocument();

        doc.LoadXml(xml);
        XmlNode userdata = doc.FirstChild;
        foreach (XmlNode node in userdata.ChildNodes)
        {
            Debug.Log(node.Name);
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
                    Wallobj.GetComponent<Renderer>().material.color = new Color(177F / 255F, 229F / 255F, 229F / 255F, 30F / 255F);
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
            if (node.Name == "Doors")
            {
                // for each door
                /*foreach (XmlNode cnode in node.ChildNodes) {
                    GameObject doortmp = new GameObject();
                    GameObject Doorobj = Instantiate(doortmp);
                    Doorobj.name = "besthmDoor";
                    Doorobj.tag = "door";

                    // for each door info (Position, Rotation, Scale)
                    foreach (XmlNode transformInfo in cnode.ChildNodes) {
                        float X = 0;
                        float Y = 0;
                        float Z = 0;

                        if (transformInfo.Name == "Position") {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes) {
                                if (transformInfoVariable.Name == "X") {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y") {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z") {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Doorobj.transform.position = new Vector3 (X,Y,Z);
                        }
                        if (transformInfo.Name == "Rotation") {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes) {
                                if (transformInfoVariable.Name == "X") {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y") {
                                    Y = float.Parse(transformInfoVariable.InnerText);

                                    if (Y > .001) {
                                        Y = 90;
                                    }
                                }
                                if (transformInfoVariable.Name == "Z") {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Doorobj.transform.rotation = Quaternion.Euler(X, Y, Z);
                        }
                        if (transformInfo.Name == "Scale") {
                            foreach (XmlNode transformInfoVariable in transformInfo.ChildNodes) {
                                if (transformInfoVariable.Name == "X") {
                                    X = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Y") {
                                    Y = float.Parse(transformInfoVariable.InnerText);
                                }
                                if (transformInfoVariable.Name == "Z") {
                                    Z = float.Parse(transformInfoVariable.InnerText);
                                }
                            }
                            Doorobj.transform.localScale = new Vector3 (X,Y,Z);
                        }
                    }
                    Doorobj.SetActive(true);
                    //tempDoors.Add(Doorobj);
                }*/
            }
            if (node.Name == "Pillars")
            {
                // for each pillar
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    GameObject Pillarobj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    Pillarobj.name = "besthmPillar";
                    Pillarobj.tag = "pillar";
                    Pillarobj.GetComponent<Renderer>().material.color = new Color(0F, 0F, 0F);

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

        url = "http://crowdevac.com/get_image.php?runid=" + runId + "&scene=" + SceneManager.GetActiveScene().name;  //---live
                                                                                                                     //url="http://localhost/get_image.php?runid="+runId+"&scene="+SceneManager.GetActiveScene().name;   //---local

        www = new WWW(url);
        yield return www;


        //byte[] bytes = new byte[image.Length * sizeof(char)];
        //System.Buffer.BlockCopy(image.ToCharArray(), 0, bytes, 0, bytes.Length);
        byte[] bytes = www.bytes;

        mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
        //hide info stuff
        amountOfPeopleEscaped.gameObject.SetActive(false);
        amountOfPeopleLeft.gameObject.SetActive(false);
        totalEscapeTime.gameObject.SetActive(false);
        averageEscapeTime.gameObject.SetActive(false);



        //bring up canvas

        //image.SetActive(true);
        heatMapPlane.SetActive(true);
        //turn light
        light.SetActive(false);
        heatPlane.SetActive(true);
        //make the heatmap
        //SpeedAndPosition[] speedPoints = mainCam.GetComponent<collectResults>().speedAndPos.ToArray();
        //Texture2D tex = Heatmap.CreateHeatmap(points, mainCam, 5);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Heatmap.CreateRenderPlane(tex);
        //points.Clear();

    }

    public void displayInfo()
    {
        //put heatmap stuff in here
        ShowInfo();
    }



    public void actuallyRestart()
    {
        showAllUsersItems();
        destroyAllBestHeatmapItems();

        FileScript.time = mytimer;
        if (running == 1)
        {

            //GameObject.Find("FileController").GetComponent<FileScript>().flag=true;
            //GameObject.Find("FileController").GetComponent<FileScript>().createXML();
            if (!GameObject.Find("FileController").GetComponent<FileScript>().flag)//XMLXML
                GameObject.Find("FileController").GetComponent<FileScript>().createXML();//XMLXML
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls)
        {
            if (wall.GetComponent<NewWallScript>() != null)
            {

                wall.GetComponent<NewWallScript>().nOfWalls = nOfWalls;
            }

            wall.tag = "prevwall";
            DontDestroyOnLoad(wall);
        }
        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject door in doors)
        {
            //door.GetComponent<doorOpeningScript>().enabled = false;
            if (door.transform.rotation.y <= 50)
                door.transform.eulerAngles = new Vector3(door.transform.rotation.x, 0.0f, door.transform.rotation.z);
            else
                door.transform.eulerAngles = new Vector3(door.transform.rotation.x, 90.0f, door.transform.rotation.z);
            DontDestroyOnLoad(door);
        }
        GameObject[] pillars = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillar in pillars)
        {
            DontDestroyOnLoad(pillar);
        }

        FileScript.replayFlag = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //restartWithSaved();
    }



    public void restartWithSaved()
    {
        showAllUsersItems();
        destroyAllBestHeatmapItems();

        running = 0;

        //deactivate other menus
        menuWithEndMid.enabled = true;
        endGame.SetActive(false);
        midGame.SetActive(false);
        heatMapButton.SetActive(false);
        bestHeatMapButton.SetActive(false);
        infoButton.SetActive(false);
        restartLevel.SetActive(false);
        nextLevelButton.SetActive(false);
        amountOfPeopleEscaped.enabled = false;
        amountOfPeopleLeft.enabled = false;
        totalEscapeTime.enabled = false;
        averageEscapeTime.enabled = false;
        canvasDev.SetActive(true);

        //start camera movement
        mainCam.GetComponent<MouseLook>().enabled = true;
        mainCam.GetComponent<mainCameraControls>().enabled = true;
        mainCam.GetComponent<heatMapControls>().enabled = false;
        mainCamera.orthographic = false;
        mainCamera.transform.position = new Vector3(0, 50, 0);
        mainCamera.transform.rotation = Quaternion.identity;
        mainCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        //activate necessary
        light.SetActive(true);
        heatPlane.SetActive(false);
        //Destroy(GameObject.fin("agent (Clone)"));


        //restarting level means changing camera angle and position and deleting all temp agents



    }


    void quitFunction()
    {
        destroyAllObjects();
        SceneManager.LoadScene("Start");
    }
    public void destroyAllObjects()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls)
        {
            Destroy(wall);

        }
        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject door in doors)
        {
            Destroy(door);
        }
        GameObject[] pillars = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillar in pillars)
        {
            Destroy(pillar);
        }
    }

    void Start()
    {
        simflag = false;
        crowdflag = false;
        subTime = 0.0f;
        timeFlag = false;
        timeConst = 0.0f;
        serviceMenu.enabled = false;
        changeTimeConst = 0.0f;

        //plane are calculation

        planearea = GameObject.FindGameObjectWithTag("floor").transform.localScale.x * GameObject.FindGameObjectWithTag("floor").transform.localScale.z * squareFactor;
        Debug.Log("planearea is " + planearea);

        //if there are prevwalls, delete all other walls
        GameObject prevwall = GameObject.FindGameObjectWithTag("prevwall");
        if (prevwall != null)
        {
            GameObject[] wall = GameObject.FindGameObjectsWithTag("wall");
            nOfWalls = 0;
            foreach (GameObject w in wall)
            {
                if (w.GetComponent<NewWallScript>() != null)
                {
                    nOfWalls = w.GetComponent<NewWallScript>().nOfWalls;
                }
                else if (w.name == "RightWall")
                {

                }
                else
                {
                    Destroy(w);
                }
            }
            GameObject[] prevwalls = GameObject.FindGameObjectsWithTag("prevwall");
            foreach (GameObject w in prevwalls)
            {
                if (w.GetComponent<NewWallScript>() != null)
                {
                    nOfWalls = w.GetComponent<NewWallScript>().nOfWalls;
                }
                w.tag = "wall";
            }

        }




        GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
        nOfDoors = 0;
        foreach (GameObject d in doors)
        {
            nOfDoors++;
            //d.GetComponent<doorOpeningScript>().enabled = true;
        }
        if (nOfDoors < 0)
        {

            nOfDoors = 0;
        }

        GameObject[] pillars = GameObject.FindGameObjectsWithTag("pillar");
        nOfPillars = 0;
        foreach (GameObject d in pillars)
        {
            nOfPillars++;
        }
        if (nOfPillars < 0)
        {
            nOfPillars = 0;
        }

        mytimer = 0.0f;
        setTimer = false;
        operating = false;
        pause = false;
        doorWidth = 1;
        running = 0;
        menuEnabled = false;
        outerWall = GameObject.FindGameObjectsWithTag("outerWall");
        // GameObject.FindGameObjectWithTag("agent").SetActive(false);

        /* GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
         foreach(GameObject wall1 in walls)
         {
             foreach(GameObject wall2 in walls)
             {
                 if(wall1!=wall2&&wall1.transform.position.x==wall2.transform.position.x && wall1.transform.position.y == wall2.transform.position.y && wall1.transform.position.z == wall2.transform.position.z)
                 {
                     Debug.Log(wall1.transform.position.x + " " + wall1.transform.position.y + " " + wall1.transform.position.z);
                 }
             }
         }*/
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main.GetComponent<makeDoor>().enabled == false && Camera.main.GetComponent<makePillar>().enabled == false && Camera.main.GetComponent<makeWall>().enabled == false)
        {
            if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))&&(menuEnabled==false && serviceMenu.enabled==false&&GameObject.Find("midGame") ==null))
            {
                GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
                foreach (GameObject g in objects)
                {
                    if (g.name == "ErrorText")
                    {
                        g.SetActive(true);
                        g.GetComponentInChildren<Text>().text = "Click Space and select an item";//No more walls and pillars left to place";
                        timer = 100;
                    }
                }
            }
        }

        if (!timeFlag)
        {
            timeConst = Time.deltaTime;
            timeFlag = true;
        }
        timer--;

        if (timer == 1)
        {
            if (GameObject.Find("ErrorText") != null)
            {
                GameObject.Find("ErrorText").GetComponentInChildren<Text>().text = " ";
                GameObject.Find("ErrorText").SetActive(false);
            }
        }
        if (setTimer)
        {
            if (Input.GetKey(KeyCode.F))
            {
                Time.timeScale = 20;
                subTime = subTime + 1.0f;
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {

                Time.timeScale = 1.0f;
                mytimer = mytimer + 1.0f;
            }

            else
            {
                mytimer = mytimer + 1.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;

            if (pause)
            {
                menuEnabled = false;
                menu.enabled = false;
                serviceMenu.enabled = false;
                noList = true;
                Camera.main.GetComponent<MouseLook>().enabled = false;
                mainCam.GetComponent<mainCameraControls>().enabled = false;
                midmodalPanel.Choice(restart, quit);
                GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

                foreach (GameObject g in objects)
                {
                    if (g.name == "ErrorText")
                    {
                        g.GetComponentInChildren<Text>().text = "";
                        g.SetActive(false);

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

                    }
                }
                midmodalPanel.ClosePanel();

                if (!crowdflag && !simflag)
                    noList = false;
                else if(simflag)
                {
                    Camera.main.GetComponent<MouseLook>().enabled = true;
                    mainCam.GetComponent<mainCameraControls>().enabled = true;
                }
                else
                    serviceMenu.enabled = true;

            }
        }

        if (running == 0 && !pause)
        {
            if (Input.GetKeyDown("space") && !noList && !crowdflag)
            {
                menuEnabled = !menuEnabled;
                GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

                foreach (GameObject g in objects)
                {
                    if (g.name == "ErrorText")
                    {
                        g.GetComponentInChildren<Text>().text = "";
                        g.SetActive(false);

                    }
                }
            }
            if (menuEnabled == true)
            {

                showWidthPillarS.text = "Pillar Width: " + pillarWidthS.value;
                textDoors.text = "Doors Placed: " + nOfDoors + "/" + totalnofDoors;
                textPillars.text = "Pillars Placed: " + nOfPillars + "/" + totalnofPillars;
                textWall.text = "Walls Placed: " + nOfWalls + "/" + totalnofWalls;

                if (!((nOfPillars == totalnofPillars) && (nOfWalls == totalnofWalls)) || ConnectionScript.notConnected)
                {
                    GameObject.Find("submit").GetComponent<Button>().interactable = false;
                    if (!ConnectionScript.notConnected)
                        GameObject.Find("errorText").GetComponent<Text>().text = "Place all walls and pillars to start simulation";

                }
                else
                {
                    if ((nOfPillars == totalnofPillars) && (nOfWalls == totalnofWalls))
                    {
                        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
                        foreach (GameObject g in objects)
                        {
                            if (g.name == "ErrorText")
                            {
                                g.SetActive(true);
                                g.GetComponentInChildren<Text>().text = "";//No more walls and pillars left to place";
                                timer = 100;

                            }
                        }
                    }
                    GameObject.Find("submit").GetComponent<Button>().interactable = true;
                    GameObject.Find("errorText").GetComponent<Text>().text = "";

                }
                menu.enabled = true;
                mainCam.GetComponent<MouseLook>().enabled = false;
                mainCam.GetComponent<mainCameraControls>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mainCam.GetComponent<makeDoor>().enabled = false;
                mainCam.GetComponent<makePillar>().enabled = false;
                mainCam.GetComponent<makeWall>().enabled = false;
                GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
                foreach (GameObject wall in walls)
                {
                    if (wall.GetComponent<NewWallScript>() != null)
                    {
                        wall.GetComponent<NewWallScript>().enabled = false;
                    }
                }
            }
            else
            {
                menu.enabled = false;
                if (!crowdflag)
                {
                    mainCam.GetComponent<MouseLook>().enabled = true;
                    mainCam.GetComponent<mainCameraControls>().enabled = true;
                }
                //if orthographic, cursor mode should be none
                if (mainCamera.orthographic)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    if (!crowdflag)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.Confined;
                    }
                }

            }

            pillarWidth = pillarWidthS.value;
        }
    }


    public void placeDoor()
    {
        mainCam.GetComponent<makeDoor>().enabled = true;
        mainCam.GetComponent<makePillar>().enabled = false;
        mainCam.GetComponent<makeWall>().enabled = false;
        menu.enabled = false;
        menuEnabled = false;
        mainCam.GetComponent<MouseLook>().enabled = true;
        mainCam.GetComponent<mainCameraControls>().enabled = true;
        operating = true;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls)
        {
            if (wall.GetComponent<NewWallScript>() != null)
            {
                wall.GetComponent<NewWallScript>().enabled = false;
            }
        }

    }


    public void placePillar()
    {
        operating = true;
        mainCam.GetComponent<makeDoor>().enabled = false;
        mainCam.GetComponent<makePillar>().enabled = true;
        mainCam.GetComponent<makeWall>().enabled = false;
        menu.enabled = false;
        menuEnabled = false;
        mainCam.GetComponent<MouseLook>().enabled = true;
        mainCam.GetComponent<mainCameraControls>().enabled = true;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls)
        {
            if (wall.GetComponent<NewWallScript>() != null)
            {
                wall.GetComponent<NewWallScript>().enabled = false;
            }
        }
    }

    public void placeWall()
    {
        operating = false;
        mainCam.GetComponent<makeDoor>().enabled = false;
        mainCam.GetComponent<makePillar>().enabled = false;
        mainCam.GetComponent<makeWall>().enabled = true;
        menu.enabled = false;
        menuEnabled = false;
        mainCam.GetComponent<MouseLook>().enabled = true;
        mainCam.GetComponent<mainCameraControls>().enabled = true;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls)
        {
            if (wall.GetComponent<NewWallScript>() != null)
            {
                wall.GetComponent<NewWallScript>().enabled = true;
            }
        }
    }

    public void configureCrowd()
    {
        serviceMenu.enabled = true;
        crowdflag = true;
        menuEnabled = false;
        menu.enabled = false;
        mainCam.GetComponent<MouseLook>().enabled = false;
        mainCam.GetComponent<mainCameraControls>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void clickedSubmit()
    {
        crowdflag = false;
        simflag = true;
        int l = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().value;
        string los = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        int uppercount = 0, lowercount = 0;

        if (los.Contains("A"))
        {
            lowercount = Convert.ToInt32(0.20 * planearea);
            uppercount = Convert.ToInt32(0.27 * planearea);
        }
        else if (los.Contains("B"))
        {
            lowercount = Convert.ToInt32(0.31 * planearea);
            uppercount = Convert.ToInt32(0.42 * planearea);
        }
        else if (los.Contains("C"))
        {
            lowercount = Convert.ToInt32(0.43 * planearea);
            uppercount = Convert.ToInt32(0.72 * planearea);
        }
        else if (los.Contains("D"))
        {
            lowercount = Convert.ToInt32(0.73 * planearea);
            uppercount = Convert.ToInt32(1.08 * planearea);
        }
        else if (los.Contains("E"))
        {
            lowercount = Convert.ToInt32(1.09 * planearea);
            uppercount = Convert.ToInt32(2.17 * planearea);
        }
        else
        {
            lowercount = Convert.ToInt32(2.18 * planearea);
            uppercount = Convert.ToInt32(2.5 * planearea);
        }

        List<List<Transform>> tempListList = new List<List<Transform>>();
        List<Transform> wallList = new List<Transform>();
        List<Transform> doorList = new List<Transform>();
        List<Transform> pillarList = new List<Transform>();
        //For XML or JSON Storage
        GameObject[] wallFinals = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wallFinal in wallFinals)
        {
            wallList.Add(wallFinal.transform);
        }
        tempListList.Add(wallList);

        GameObject[] doorFinals = GameObject.FindGameObjectsWithTag("door");
        foreach (GameObject doorFinal in doorFinals)
        {
            doorList.Add(doorFinal.transform);
        }

        tempListList.Add(doorList);

        GameObject[] pillarFinals = GameObject.FindGameObjectsWithTag("pillar");
        foreach (GameObject pillarFinal in pillarFinals)
        {
            pillarList.Add(pillarFinal.transform);
        }

        tempListList.Add(pillarList);

        FileScript.tempListList = tempListList;

        running = 1;
        mainCam.GetComponent<makeDoor>().enabled = false;
        mainCam.GetComponent<makePillar>().enabled = false;
        mainCam.GetComponent<makeWall>().enabled = false;
        GameObject.Find("GameController").GetComponent<ConnectionScript>().enabled = false;
        menu.enabled = false;
        menuEnabled = false;
        serviceMenu.enabled = false;
        mainCam.GetComponent<MouseLook>().enabled = true;
        mainCam.GetComponent<mainCameraControls>().enabled = true;
        GameController.startSiren = true;
        setTimer = true;

        if (!Input.GetKeyDown("space"))
        {


            walls = GameObject.FindGameObjectsWithTag("wall");


            foreach (GameObject wall in walls)
            {
                if (wall.GetComponent<NewWallScript>() != null)
                {
                    if (wall.transform.rotation.y == 0)
                    {
                        GameObject agent = Instantiate(agentObj);
                        agent.AddComponent<newagentscript>();
                        agent.SetActive(false);
                        agent.GetComponent<NavigationController>().startPosition = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z - 1.0f);
                        agent = Instantiate(agentObj);
                        agent.AddComponent<newagentscript>();
                        agent.SetActive(false);
                        agent.GetComponent<NavigationController>().startPosition = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + 1.0f);

                    }
                    else
                    {
                        GameObject agent = Instantiate(agentObj);
                        agent.AddComponent<newagentscript>();
                        agent.SetActive(false);
                        agent.GetComponent<NavigationController>().startPosition = new Vector3(wall.transform.position.x - 1.0f, wall.transform.position.y, wall.transform.position.z);
                        agent = Instantiate(agentObj);
                        agent.AddComponent<newagentscript>();
                        agent.SetActive(false);
                        agent.GetComponent<NavigationController>().startPosition = new Vector3(wall.transform.position.x + 1.0f, wall.transform.position.y, wall.transform.position.z);
                    }
                }
            }

            GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            System.Random r = new System.Random();
            int count = 0;
            int spawnCount = r.Next(lowercount, uppercount);
            foreach (GameObject g in objects)
            {
                if (g.tag == "agent")
                {
                    if (count == spawnCount)
                    {
                        break;
                    }
                    else
                    {
                        count++;
                        g.SetActive(true);
                    }
                }
            }

            if (count < spawnCount)
            {
                System.Random rd = new System.Random();
                Transform floorplane = GameObject.FindGameObjectWithTag("floor").transform;
                double xleft = floorplane.position.x - 10 * floorplane.localScale.x / 2;
                double xright = floorplane.position.x + 10 * floorplane.localScale.x / 2;
                double ztop = floorplane.position.z + 10 * floorplane.localScale.z / 2;
                double zbottom = floorplane.position.z - 10 * floorplane.localScale.z / 2;
                for (int i = 0; i < (spawnCount - count); i++)
                {
                    GameObject agent = Instantiate(agentObj);
                    agent.AddComponent<newagentscript>();
                    agent.SetActive(true);

                    agent.GetComponent<NavigationController>().startPosition = new Vector3(Convert.ToSingle((rd.NextDouble() * (xright - xleft)) + xleft), floorplane.position.y, Convert.ToSingle((rd.NextDouble() * (ztop - zbottom)) + zbottom));

                }
            }


        }
        GameController.dont = true;

    }

    public void restartGame()
    {
        //destroyAllObjects();
        //SceneManager.LoadScene("Level2");
        actuallyRestart();
    }



    public void ShowHeatMap()
    {
        mainCam.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
        //hide info stuff
        amountOfPeopleEscaped.gameObject.SetActive(false);
        amountOfPeopleLeft.gameObject.SetActive(false);
        totalEscapeTime.gameObject.SetActive(false);
        averageEscapeTime.gameObject.SetActive(false);



        //bring up canvas

        //image.SetActive(true);
        heatMapPlane.SetActive(true);
        //turn light
        light.SetActive(false);
        heatPlane.SetActive(true);
        //make the heatmap
        Dictionary<Vector2, float> points = mainCam.GetComponent<collectResults>().allData;
        //SpeedAndPosition[] speedPoints = mainCam.GetComponent<collectResults>().speedAndPos.ToArray();
        //Texture2D tex = Heatmap.CreateHeatmap(points, mainCam, 5);
        Texture2D tex = Heatmap.CreateHeatmap(points, mainCamera, heatPlane);


        byte[] bytes = tex.EncodeToPNG();
        Heatmap.CreateRenderPlane(tex);
        //points.Clear();
    }

    /*void createPNG()
	{
		var width1 = Screen.width;
		var height1 = Screen.height;
		var tex1 = new Texture2D(width1, height1, TextureFormat.RGB24, false);
		tex1.ReadPixels( new Rect(0, 0, width1, height1), 0, 0);
		tex1.Apply ();
		
		var bytes = tex1.EncodeToPNG();
		
		//string url="http://crowdevac.com/store_image.php";
		string url="http://localhost/store_image.php"; 
		WWWForm loginForm = new WWWForm();
		loginForm.AddField("scene",SceneManager.GetActiveScene().name,System.Text.Encoding.UTF8);
		loginForm.AddField("runid",nameStore.runId,System.Text.Encoding.UTF8);
		loginForm.AddBinaryData("fileUpload",bytes);
		
		WWW www = new WWW(url, loginForm); 
		coroutine=WaitForRequest(www);
		StartCoroutine(coroutine);
	}
	IEnumerator WaitForRequest(WWW www)
	{
     yield return www;
     // check for errors
     if (www.error == null)
     {
         //mytext.text="WWW Ok!: " + www.text;
     } else {
         //mytext.text="WWW Error: "+ www.error;
     }  
	}   */
    public void ShowInfo()
    {
        mainCam.transform.localEulerAngles = new Vector3(-90f, 270f, 0f);
        //display number of people who escaped
        //display number of people left
        //display average time
        //display total time
        //image.SetActive(false);
        heatMapPlane.SetActive(false);
        light.SetActive(true);
        //find heatmaprending and delete it   Heatmap Render Plane
        GameObject heatmapPlane = GameObject.Find("Heatmap Render Plane");
        if (heatmapPlane != null)
        {
            Destroy(heatmapPlane);
        }

        amountOfPeopleEscaped.gameObject.SetActive(true);
        amountOfPeopleLeft.gameObject.SetActive(true);
        totalEscapeTime.gameObject.SetActive(true);
        averageEscapeTime.gameObject.SetActive(true);
        amountOfPeopleEscaped.text = "The amount of people escaped is " + (GameController.maxNumberPeople - GameController.numberLeft);
        amountOfPeopleLeft.text = "The amount of people left is " + GameController.numberLeft;
        totalEscapeTime.text = "The total escape time is " + GameController.totalTime;
        averageEscapeTime.text = "The average escape time is " + (GameController.totalTime / (GameController.maxNumberPeople - GameController.numberLeft));
    }
}