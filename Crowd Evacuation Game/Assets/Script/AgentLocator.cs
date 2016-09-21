using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AgentLocator : MonoBehaviour {

    bool flagonce;
	// Use this for initialization
	void Start () {
        flagonce = false;
	}

    // Update is called once per frame
    void Update()
    {


        if (!flagonce)
        {

            GameObject[] outers = GameObject.FindGameObjectsWithTag("outerWall");

            List<float> zcords = new List<float>();
            List<float> xcords = new List<float>();

            foreach (GameObject outer in outers)
            {
                xcords.Add(outer.transform.position.x);
                zcords.Add(outer.transform.position.z);
            }


            GameObject[] agents = GameObject.FindGameObjectsWithTag("agent");

            foreach (GameObject agent in agents)
            {
                if (agent.transform.position.x < xcords.Min())
                {
                    agent.transform.position = new Vector3(xcords.Min()+1f, agent.transform.position.y, zcords.Max()-1f);
                }

                if (agent.transform.position.x > xcords.Max())
                {
                    agent.transform.position = new Vector3(xcords.Max()-1f, agent.transform.position.y, zcords.Min()+1f);
                }

                if (agent.transform.position.z < zcords.Min())
                {
                    agent.transform.position = new Vector3(xcords.Min()+1f, agent.transform.position.y, zcords.Min()+1f);
                }

                if (agent.transform.position.z > zcords.Max())
                {
                    agent.transform.position = new Vector3(xcords.Max()-2f, agent.transform.position.y, zcords.Max()-2f);
                }

            }
            flagonce = true;
        }
    }
}
