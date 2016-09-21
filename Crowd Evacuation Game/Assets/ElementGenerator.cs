using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;

class WorldBounds
{
    public Vector3 topleft, topright, bottomleft, bottomright;

   public WorldBounds()
    {
        topleft = new Vector3(0, 0, 0);
        topright = new Vector3(0, 0, 0);
        bottomleft = new Vector3(0, 0, 0);
        bottomright = new Vector3(0, 0, 0);
    }
}

public class ElementGenerator : MonoBehaviour
{
    //pillar details
    public GameObject pillar;
    public float PillarMaxWidth;
    public float pillarHeight;
    public int noOfPillars;

    //wall details
    public int noOfWalls;

    //Door details
    public int noOfDoors;
    public float WidthOfDoor;
    bool end;
    void Awake()
    {
        //pillar details
        PillarMaxWidth = 1.0f;
        pillarHeight = 2.8f;
        noOfPillars = 1;
        noOfWalls = 2;
        noOfDoors = 6;
        WidthOfDoor = 1.0f;
        end = false;
        //wall details


        //Door details
    }

    // Use this for initialization
    void Start()
    {
        
        generatePillars();
        generatewalls();
        generateDoors();
        simulate();
        Debug.Log(this.gameObject.GetComponent<TimerScript>().endTimer());

    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.GetComponent<TimerScript>().time);
        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
        {
            agent.GetComponent<NavMeshAgent>().SetDestination(GameObject.FindGameObjectWithTag("destinations").transform.position);
            if (Vector3.Distance(agent.transform.position, GameObject.FindGameObjectWithTag("destinations").transform.position) <= 1.0f)
            {
                agent.SetActive(false);
            }
        }

        if (GameObject.FindGameObjectsWithTag("agent").Length == 0 || this.gameObject.GetComponent<timer>().time == 1200)
        {
            end = true;
        }

        if(end)
        {
            Debug.Log("Failed");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }


    //place pillars

    void generatePillars()
    {
        WorldBounds wb = getWorldBounds();

        for (int i = 0; i < noOfPillars; i++)
        {

            float xcord = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
            float zcord = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);
            float ycord = wb.bottomleft.y;

            float PillarWidth = UnityEngine.Random.Range(0.5f, PillarMaxWidth);


            while (!placePillars(xcord, zcord, PillarWidth))
            {
                xcord = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
                zcord = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);
                ycord = wb.bottomleft.y;

                PillarWidth = UnityEngine.Random.Range(0.5f, PillarMaxWidth);
            }

        }
    }

    bool placePillars(float xcord, float zcord, float PillarWidth)
    {
        GameObject pillarObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pillarObj.tag = "pillar";
        pillarObj.AddComponent<NavMeshObstacle>();
        pillarObj.transform.localScale = new Vector3(PillarWidth, pillarHeight, PillarWidth);
        pillarObj.transform.position = new Vector3(xcord, getWorldBounds().bottomleft.y, zcord);

        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");

        foreach(GameObject wall in walls)
        {
            if(wall.transform.rotation.y==0)
            {
                if((pillarObj.transform.position.x+pillarObj.transform.localScale.x/2>=wall.transform.position.x-wall.transform.localScale.x/2)||(pillarObj.transform.position.x - pillarObj.transform.localScale.x / 2 <= wall.transform.position.x + wall.transform.localScale.x / 2))
                {
                    if(((pillarObj.transform.position.z+pillarObj.transform.localScale.x/2>=wall.transform.position.z-wall.transform.localScale.z/2)&& (pillarObj.transform.position.z - pillarObj.transform.localScale.x / 2 < wall.transform.position.z + wall.transform.localScale.z / 2)) || ((pillarObj.transform.position.z - pillarObj.transform.localScale.x / 2 <= wall.transform.position.z + wall.transform.localScale.z / 2)&& (pillarObj.transform.position.z + pillarObj.transform.localScale.x / 2 > wall.transform.position.z - wall.transform.localScale.z / 2)))
                    {
                        removePillar(pillarObj);
                            return false;
                    }
                }
            }
            else
            {
                if ((pillarObj.transform.position.z + pillarObj.transform.localScale.x / 2 >= wall.transform.position.z - wall.transform.localScale.x / 2) || (pillarObj.transform.position.z - pillarObj.transform.localScale.x / 2 <= wall.transform.position.z + wall.transform.localScale.x / 2))
                {
                    if (((pillarObj.transform.position.x + pillarObj.transform.localScale.x / 2 >= wall.transform.position.x - wall.transform.localScale.z / 2) && (pillarObj.transform.position.x - pillarObj.transform.localScale.x / 2 < wall.transform.position.x + wall.transform.localScale.z / 2)) || ((pillarObj.transform.position.x - pillarObj.transform.localScale.x / 2 <= wall.transform.position.x + wall.transform.localScale.z / 2)&& (pillarObj.transform.position.x + pillarObj.transform.localScale.x / 2 > wall.transform.position.x - wall.transform.localScale.z / 2)))
                    {
                        removePillar(pillarObj);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void removePillar(GameObject pillar)
    {
        Destroy(pillar);
    }
    // world bounds

    WorldBounds getWorldBounds()
    {
        GameObject[] outerwalls = GameObject.FindGameObjectsWithTag("outerWall");

        WorldBounds wb = new WorldBounds();

        List<GameObject> horizontalWalls = new List<GameObject>();
        List<GameObject> verticalWalls = new List<GameObject>();

        foreach (GameObject outerWall in outerwalls)
        {
            if (outerWall.transform.rotation.y == 0)
            {
                horizontalWalls.Add(outerWall);
            }
            else
            {
                verticalWalls.Add(outerWall);

            }

        }

        float zmax = Single.MinValue, zmin = Single.MaxValue;


        foreach (GameObject horwall in horizontalWalls)
        {
            if (horwall.transform.position.z > zmax)
            {
                zmax = horwall.transform.position.z;
            }

            if (horwall.transform.position.z < zmin)
            {
                zmin = horwall.transform.position.z;
            }
        }

        float xmax = Single.MinValue, xmin = Single.MaxValue;

        foreach (GameObject verWall in verticalWalls)
        {
            if (verWall.transform.position.x > xmax)
            {
                xmax = verWall.transform.position.x;
            }

            if (verWall.transform.position.x < xmin)
            {
                xmin = verWall.transform.position.x;
            }
        }

        wb.topleft = new Vector3(xmin, verticalWalls[0].transform.position.y, zmax);
        wb.topright = new Vector3(xmax, verticalWalls[0].transform.position.y, zmax);
        wb.bottomleft = new Vector3(xmin, verticalWalls[0].transform.position.y, zmin);
        wb.bottomright = new Vector3(xmax, verticalWalls[0].transform.position.y, zmin);

        return wb;
    }

    void generatewalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        GameObject[] outerwalls = GameObject.FindGameObjectsWithTag("outerWall");

        GameObject[] totalWalls = new GameObject[walls.Length + outerwalls.Length];

        walls.CopyTo(totalWalls, 0);
        outerwalls.CopyTo(totalWalls, walls.Length);

        WorldBounds wb = getWorldBounds();


        bool success = false;

        // for (int i = 0; i < noOfWalls; i++)
        /* {*/

        float xcord = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
        float zcord = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);

        float xcord1 = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
        float zcord1 = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);
        success = false;
        while (!success)
        {


            success = placeWall(xcord, zcord, xcord1, zcord);

        }
        success = false;
        xcord = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
        zcord = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);

        xcord1 = wb.topleft.x + (wb.topright.x - wb.topleft.x) * UnityEngine.Random.Range(0.1f, 1.0f);
        zcord1 = wb.bottomleft.z + (wb.topleft.z - wb.bottomleft.z) * UnityEngine.Random.Range(0.1f, 1.0f);

        while (!success)
        {


            success = placeWall(xcord, zcord, xcord, zcord1);
            //}
        }
    }

    bool placeWall(float x0, float y0,float x1,float y1)
    {
        Debug.Log(x0 + " " + y0 + " " + x1 + " " + y1);
        GameObject newwall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newwall.tag = "wall";
        newwall.name = "newwall";
        newwall.AddComponent<NavMeshObstacle>();

        float distance = Mathf.Sqrt(Mathf.Pow((x0 - x1), 2) + Mathf.Pow((y0 - y1), 2));

        newwall.transform.localScale = new Vector3(distance,3.0f,0.1f);

        newwall.transform.position = new Vector3(Mathf.Min(x0, x1)+Mathf.Abs(x0 - x1)/2,1.22f, Mathf.Min(y0, y1) + Mathf.Abs(y0-y1)/2);

        newwall.transform.eulerAngles = new Vector3(0, Mathf.Atan(Mathf.Abs(y0 - y1) / Mathf.Abs(x0 - x1)) * (180 / Mathf.PI), 0);
        
        GameObject[] pillars = GameObject.FindGameObjectsWithTag("pillar");
        //pillar intersection check
        foreach (GameObject pillar in pillars)
        {
            if (newwall.transform.rotation.y == 0)
            {
                if ((pillar.transform.position.x + pillar.transform.localScale.x / 2 >= newwall.transform.position.x - newwall.transform.localScale.x / 2) || (pillar.transform.position.x - pillar.transform.localScale.x / 2 <= newwall.transform.position.x + newwall.transform.localScale.x / 2))
                {
                    if (((pillar.transform.position.z + pillar.transform.localScale.x / 2 >= newwall.transform.position.z - newwall.transform.localScale.z / 2) && (pillar.transform.position.z - pillar.transform.localScale.x / 2 < newwall.transform.position.z + newwall.transform.localScale.z / 2)) || ((pillar.transform.position.z - pillar.transform.localScale.x / 2 <= newwall.transform.position.z + newwall.transform.localScale.z / 2) && (pillar.transform.position.z + pillar.transform.localScale.x / 2 > newwall.transform.position.z - newwall.transform.localScale.z / 2)))
                    {
                        removePillar(pillar);
                        return false;
                    }
                }
            }
            else
            {
                if ((pillar.transform.position.z + pillar.transform.localScale.x / 2 >= newwall.transform.position.z - newwall.transform.localScale.x / 2) || (pillar.transform.position.z - pillar.transform.localScale.x / 2 <= newwall.transform.position.z + newwall.transform.localScale.x / 2))
                {
                    if (((pillar.transform.position.x + pillar.transform.localScale.x / 2 >= newwall.transform.position.x - newwall.transform.localScale.z / 2) && (pillar.transform.position.x - pillar.transform.localScale.x / 2 < newwall.transform.position.x + newwall.transform.localScale.z / 2)) || ((pillar.transform.position.x - pillar.transform.localScale.x / 2 <= newwall.transform.position.x + newwall.transform.localScale.z / 2) && (pillar.transform.position.x + pillar.transform.localScale.x / 2 > newwall.transform.position.x - newwall.transform.localScale.z / 2)))
                    {
                        removePillar(pillar);
                        return false;
                    }
                }
            }
        }

       /* //wall intersection check
        GameObject[] walls1 = GameObject.FindGameObjectsWithTag("wall");
        foreach (GameObject wall in walls1)
        {
            if (Doorobj.transform.rotation.y == 0 && wall.transform.rotation.y != 0)
            {
                if ((wall.transform.position.x + wall.transform.localScale.x / 2 >= Doorobj.transform.position.x - Doorobj.transform.localScale.z / 2) || (wall.transform.position.x - wall.transform.localScale.x / 2 <= Doorobj.transform.position.x + Doorobj.transform.localScale.z / 2))
                {
                    if (((wall.transform.position.z + wall.transform.localScale.z / 2 >= Doorobj.transform.position.z - Doorobj.transform.localScale.x / 2) && (wall.transform.position.z - wall.transform.localScale.z / 2 < Doorobj.transform.position.z + Doorobj.transform.localScale.x / 2)) || ((wall.transform.position.z - wall.transform.localScale.z / 2 <= Doorobj.transform.position.z + Doorobj.transform.localScale.x / 2) && (wall.transform.position.z + wall.transform.localScale.z / 2 > Doorobj.transform.position.z - Doorobj.transform.localScale.x / 2)))
                    {
                        removeWall(Doorobj);
                        return false;
                    }
                }
            }
            else if (Doorobj.transform.rotation.y != 0 && wall.transform.rotation.y == 0)
            {
                if ((wall.transform.position.z + wall.transform.localScale.x / 2 >= Doorobj.transform.position.z - Doorobj.transform.localScale.z / 2) || (wall.transform.position.z - wall.transform.localScale.x / 2 <= Doorobj.transform.position.z + Doorobj.transform.localScale.z / 2))
                {
                    if (((wall.transform.position.x + wall.transform.localScale.z / 2 >= Doorobj.transform.position.x - Doorobj.transform.localScale.x / 2) && (wall.transform.position.x - wall.transform.localScale.z / 2 < Doorobj.transform.position.x + Doorobj.transform.localScale.x / 2)) || ((wall.transform.position.x - wall.transform.localScale.z / 2 <= Doorobj.transform.position.x + Doorobj.transform.localScale.x / 2) && (wall.transform.position.x + wall.transform.localScale.z / 2 > Doorobj.transform.position.x - Doorobj.transform.localScale.x / 2)))
                    {
                        removeWall(Doorobj);
                        return false;
                    }
                }
            }
        }*/

        return true;

    }


    void removeWall(GameObject obj)
    {
        Debug.Log("destroyed " + obj.name);
        Destroy(obj);
    }

    void generateDoors()
    {
        GameObject door = null;
        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        foreach (GameObject g in objects)
        {
            if (g.name == "Hinge")
            {
                door = g;
            }
        }

        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");


        bool success = false;

        for (int i = 0; i < noOfDoors; i++)
        {
            success = false;
            while (!success)
            {
                System.Random r = new System.Random();
                int index = r.Next(0, walls.Length);

                success = placeDoors(index, walls, door);

            }
        }
    }

    bool placeDoors(int index, GameObject[] walls, GameObject door)
    {
        Debug.Log("Chosen wall " + walls[index].name);
        GameObject Doorobj = null;
        //vertical
        if (walls[index].transform.rotation.y == 0)
        {

            float chosenX = UnityEngine.Random.Range(walls[index].transform.position.x - walls[index].transform.localScale.x / 2, walls[index].transform.position.x + walls[index].transform.localScale.x / 2);
            float xminUp = walls[index].transform.position.x - walls[index].transform.localScale.x / 2;
            float xmaxUp = chosenX - WidthOfDoor / 2;
            float xminDown = chosenX + WidthOfDoor / 2;
            float xmaxDown = walls[index].transform.position.x + walls[index].transform.localScale.x / 2;
            float widthUp = Math.Abs(xmaxUp - xminUp);
            float widthDown = Math.Abs(xmaxDown - xminDown);
            float midUp = xminUp + Mathf.Abs(xminUp - xmaxUp) / 2;
            float midDown = xminDown + Mathf.Abs(xminDown - xmaxDown) / 2;

            if (Math.Abs(chosenX - xminUp) > WidthOfDoor / 2 && Math.Abs(xmaxDown - chosenX) > WidthOfDoor / 2)
            {

                walls[index].transform.position = new Vector3(midUp, walls[index].transform.position.y, walls[index].transform.position.z);
                walls[index].transform.localScale = new Vector3(widthUp, walls[index].transform.localScale.y, walls[index].transform.localScale.z);

                GameObject obj = Instantiate(GameObject.Find("RightWall"));
                obj.name = "doowall";
                obj.tag = "wall";
                obj.transform.position = new Vector3(midDown, walls[index].transform.position.y, walls[index].transform.position.z);
                obj.transform.localScale = new Vector3(widthDown, walls[index].transform.localScale.y, walls[index].transform.localScale.z);
                obj.transform.rotation = walls[index].transform.rotation;

                //place door
                Doorobj = Instantiate(door);
                Doorobj.SetActive(true);
                Doorobj.tag = "door";
                Doorobj.transform.position = new Vector3(chosenX + WidthOfDoor / 2, walls[index].transform.position.y, walls[index].transform.position.z);
                Doorobj.transform.localScale = new Vector3(WidthOfDoor, 1.2f, .1f);
                Doorobj.transform.rotation = walls[index].transform.rotation;


            }
            else
            {
                return false;
            }

        }
        //horizontal
        else
        {
            float chosenZ = UnityEngine.Random.Range(walls[index].transform.position.z - walls[index].transform.localScale.x / 2, walls[index].transform.position.z + walls[index].transform.localScale.x / 2);

            float zminLeft = walls[index].transform.position.z - walls[index].transform.localScale.x / 2;
            float zmaxLeft = chosenZ - WidthOfDoor / 2;
            float zminRight = chosenZ + WidthOfDoor / 2;
            float zmaxRight = walls[index].transform.position.z + walls[index].transform.localScale.x / 2;
            float widthLeft = zmaxLeft - zminLeft;
            float widthRight = zmaxRight - zminRight;
            float midLeft = zminLeft + Mathf.Abs(zminLeft - zmaxLeft) / 2;
            float midRight = zminRight + Mathf.Abs(zmaxRight - zminRight) / 2;
            if (chosenZ - zminLeft > WidthOfDoor / 2 && zmaxRight - chosenZ > WidthOfDoor / 2)
            {

                walls[index].transform.position = new Vector3(walls[index].transform.position.x, walls[index].transform.position.y, midLeft);
                walls[index].transform.localScale = new Vector3(widthLeft, walls[index].transform.localScale.y, walls[index].transform.localScale.z);
                GameObject obj = Instantiate(GameObject.Find("RightWall"));
                obj.tag = "wall";
                obj.name = "doowall";
                obj.transform.position = new Vector3(walls[index].transform.position.x, walls[index].transform.position.y, midRight);
                obj.transform.localScale = new Vector3(widthRight, walls[index].transform.localScale.y, walls[index].transform.localScale.z);
                obj.transform.rotation = walls[index].transform.rotation;

                //place door
                Doorobj = Instantiate(door);
                Doorobj.SetActive(true);
                Doorobj.tag = "door";
                Doorobj.transform.position = new Vector3(walls[index].transform.position.x, walls[index].transform.position.y, chosenZ - WidthOfDoor / 2);
                Doorobj.transform.localScale = new Vector3(WidthOfDoor, 1.2f, .1f);
                Doorobj.transform.eulerAngles = new Vector3(walls[index].transform.rotation.x, 90, walls[index].transform.rotation.z);


            }
            else
            {
                return false;
            }
            /*
            //wall intersection check
            GameObject[] walls1 = GameObject.FindGameObjectsWithTag("wall");
            foreach(GameObject wall in walls1)
            {
    if (Doorobj.transform.rotation.y == 0 && wall.transform.rotation.y!=0)
    {
        if ((wall.transform.position.x + wall.transform.localScale.x / 2 >= Doorobj.transform.position.x - Doorobj.transform.localScale.z / 2) || (wall.transform.position.x - wall.transform.localScale.x / 2 <= Doorobj.transform.position.x + Doorobj.transform.localScale.z / 2))
        {
            if (((wall.transform.position.z + wall.transform.localScale.z / 2 >= Doorobj.transform.position.z - Doorobj.transform.localScale.x / 2) && (wall.transform.position.z - wall.transform.localScale.z / 2 < Doorobj.transform.position.z + Doorobj.transform.localScale.x / 2)) || ((wall.transform.position.z - wall.transform.localScale.z / 2 <= Doorobj.transform.position.z + Doorobj.transform.localScale.x / 2) && (wall.transform.position.z + wall.transform.localScale.z / 2 > Doorobj.transform.position.z - Doorobj.transform.localScale.x / 2)))
            {
                removeWall(Doorobj);
                return false;
            }
        }
    }
    else if (Doorobj.transform.rotation.y != 0 && wall.transform.rotation.y == 0)
    {
        if ((wall.transform.position.z + wall.transform.localScale.x / 2 >= Doorobj.transform.position.z - Doorobj.transform.localScale.z / 2) || (wall.transform.position.z - wall.transform.localScale.x / 2 <= Doorobj.transform.position.z + Doorobj.transform.localScale.z / 2))
        {
            if (((wall.transform.position.x + wall.transform.localScale.z / 2 >= Doorobj.transform.position.x - Doorobj.transform.localScale.x / 2) && (wall.transform.position.x - wall.transform.localScale.z / 2 < Doorobj.transform.position.x + Doorobj.transform.localScale.x / 2)) || ((wall.transform.position.x - wall.transform.localScale.z / 2 <= Doorobj.transform.position.x + Doorobj.transform.localScale.x / 2) && (wall.transform.position.x + wall.transform.localScale.z / 2 > Doorobj.transform.position.x - Doorobj.transform.localScale.x / 2)))
            {
                removeWall(Doorobj);
                return false;
            }
        }
    }
}*/


        }
        return true;
    }

    List<GameObject> getDoors()
    {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag("wall"));
    }
    List<GameObject> getPillars()
    {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag("pillar"));
    }
    List<GameObject> getEditableWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        List<GameObject> newwalls = new List<GameObject>();

        foreach(GameObject wall in walls)
        {
            if(wall.name=="newwall")
            {
                newwalls.Add(wall);
            }
        }
        return newwalls;
    }
    List<GameObject> getWalls()
    {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag("wall"));
    }

    void simulate()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");

        GameObject agentProto=null;
        GameObject[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        foreach (GameObject g in objects)
        {
            if (g.name == "agent")
            {
                g.SetActive(true);
                agentProto = g;
                
            }
        }

        foreach (GameObject wall in walls)
        {
                if (wall.transform.rotation.y == 0)
                {
                    GameObject agent = Instantiate(agentProto);
                    agent.tag = "agent";
                    agent.AddComponent<NavMeshAgent>();
                    agent.SetActive(false);
                    agent.transform.position = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z - 1.0f);
                agent = Instantiate(agentProto);
                agent.tag = "agent";
                    agent.AddComponent<NavMeshAgent>();
                    agent.SetActive(false);
                    agent.transform.position = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + 1.0f);

                }
                else
                {
                GameObject agent = Instantiate(agentProto);
                    agent.tag = "agent";
                    agent.AddComponent<NavMeshAgent>();
                    agent.SetActive(false);
                    agent.transform.position = new Vector3(wall.transform.position.x - 1.0f, wall.transform.position.y, wall.transform.position.z);
                agent = Instantiate(agentProto);
                agent.tag = "agent";
                    agent.AddComponent<NavMeshAgent>();
                    agent.SetActive(false);
                    agent.transform.position = new Vector3(wall.transform.position.x + 1.0f, wall.transform.position.y, wall.transform.position.z);
                }
            
        }

        agentProto.SetActive(false);
        int count = 0;

        objects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        foreach (GameObject g in objects)
        {
            if (g.tag == "agent")
            {
                g.SetActive(true);
                count++;
            }
        }

        
        this.gameObject.GetComponent<TimerScript>().startTimer();

        foreach (GameObject agent in GameObject.FindGameObjectsWithTag("agent"))
        {
            agent.GetComponent<NavMeshAgent>().SetDestination(GameObject.FindGameObjectWithTag("destinations").transform.position);
        }


        Debug.Log(this.GetComponent<EndStatus>().end);

            
        
    }

}