using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*! \An example tracking script that logs the world position of the object it is attached to.
 *
 *  Set the logs per second and where to output the saved positions.  These can be read by the Heatmap class and turned into a Heatmap.
 */

//CHANGED 2.0
public class Tracker : MonoBehaviour
{
    //times each second that 
    public float logsPerSecond = 5f; //logs per second
    private float logSplit = .2f;
    private float timer = 0f;
    public GameObject mainCam;
    public Camera cam;
    public GameObject plane;

    //this tracks wether this is first movement
    int firstMovement;

    float bottomLeftX;
    float bottomLeftZ;
    int maxX;
    int maxY;
    Dictionary<Vector2, float> allData;
    int radius;

    Vector3 originalPosition;

    public void Start()
    {
        //logSplit = 1f / logsPerSecond;
        logSplit = 2;
        bottomLeftX = (plane.transform.position.x - 10 * plane.transform.localScale.x / 2);
        bottomLeftZ = (plane.transform.position.z - 10 * plane.transform.localScale.z / 2);
        maxX = (int)(plane.transform.localScale.x * 50 * 10);
        maxY = (int)(plane.transform.localScale.z * 50 * 10);
        allData = mainCam.GetComponent<collectResults>().allData;
        firstMovement = 0;
        originalPosition = gameObject.transform.position;

        //set beginning color:
        int newX = (int)(30 * (gameObject.transform.position.x - bottomLeftX));
        int newY = (int)(30 * (gameObject.transform.position.z - bottomLeftZ));
        //this is the point on the 2d plane
        Vector2 point = new Vector2(newX, newY);

        //this code sets the beginning point of the agent with a different color
        /*
        radius = 1;
        float numberToBeAdded = 0;
        for (int x = newX - radius; x < newX + radius; x++)
        {
            for (int y = newY - radius; y < newY + radius; y++)
            {
                if (y < 0 || x < 0 || x > maxX || y > maxY)
                {
                    continue;
                }
                point = new Vector2(x, y);
                //add to list
                if (allData.ContainsKey(point))
                {
                    allData[point] = allData[point] + numberToBeAdded;

                }
                else
                {
                    allData.Add(point, numberToBeAdded);
                }
            }
        }*/

        radius = 2;
    }

    public void Update()
    {
        timer++;
        //timer += Time.deltaTime;
        if (firstMovement == 0)
        {
            /* float speed = gameObject.GetComponent<NavMeshAgent>().velocity.magnitude;
             //if (speed < 3.4)

             if (Mathf.Abs(Vector3.Distance(originalPosition, gameObject.transform.position)) < 2)
             {
                 return;
             }
             else if (timer > 30f)
             {
                 Debug.Log("time ran out");
                 firstMovement = 1;
             }
             else
             {
                 firstMovement = 1;
             }*/
            if (gameObject.GetComponent<NavigationController>().start == 1)
            {
                firstMovement = 1;
            }
            else
            {
                return;
            }
        }

        if (timer > logSplit)
        {
            timer = 0f;
            int newX = (int)(30 * (gameObject.transform.position.x - bottomLeftX));
            int newY = (int)(30 * (gameObject.transform.position.z - bottomLeftZ));
            //this is the point on the 2d plane
            Vector2 point = new Vector2(newX, newY);
            float speed = gameObject.GetComponent<NavMeshAgent>().velocity.magnitude;
            float numberToBeAdded = (Mathf.Pow(speed + 1, -1.5f)) / 2;

            for (int x = newX - radius; x < newX + radius; x++)
            {
                for (int y = newY - radius; y < newY + radius; y++)
                {
                    if (y < 0 || x < 0 || x > maxX || y > maxY)
                    {
                        continue;
                    }
                    point = new Vector2(x, y);
                    //add to list
                    if (allData.ContainsKey(point))
                    {
                        if (numberToBeAdded < .05f)
                        {
                            numberToBeAdded = 0;
                        }
                        if (allData[point] < .05f)
                        {
                            allData[point] = .05f;
                        }
                        allData[point] = allData[point] + numberToBeAdded;

                    }
                    else
                    {
                        if (numberToBeAdded < .05f)
                        {
                            numberToBeAdded = .05f;
                        }
                        allData.Add(point, numberToBeAdded);
                    }
                }
            }
        }
    }


}
