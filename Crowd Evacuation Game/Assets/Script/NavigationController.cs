using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour {

    public Transform chosenTarget;
    GameObject[] targets;
    NavMeshAgent agent;
    Animator anim;

    static System.Random r;
    Vector3 currentPosition;
    public NavMeshPath path;
    public static bool startFlag;
    public static int value;
    public int loc;
    public AudioClip siren;
    bool collided;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    public int seeExit;
    int timePassed;
    public Transform finalExit;
    public int ready;
    public int start;

    public Vector3 startPosition;
    // Use this for initialization
    void Start () {
        path = new NavMeshPath();

        int l = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().value;
        string los = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        l = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().value;
        string loa = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        l = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().value;
        string homo = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

        assignAttributes(los,loa,homo);
        seeExit = 0;
        timePassed = 0;
        anim = GetComponent<Animator>();

        //TEMP
        ready = 1;
        anim.enabled = true;
        start = 1;
        //ready = 0;-----
        //start = 0;------

        collided = false;
        startFlag = false;
        targets = GameObject.FindGameObjectsWithTag("destinations");
        

        agent = GetComponent<NavMeshAgent>();

		
        if(GetComponent<newagentscript>()!=null){
		agent.enabled = false;

        transform.position = startPosition;

        agent.enabled = true;
		}
		else{
			this.gameObject.SetActive(false);
		}
        /* if (targets.Length == 1)
             loc = 1;*/
        loc = bringUpMenu.r.Next(1, targets.Length+1);
        chosenTarget = targets[loc - 1].transform;

    }
	
	// Update is called once per frame
	void Update () {

        anim.SetFloat("speed", agent.velocity.magnitude);

        if (bringUpMenu.running==1)
        {
            //If not yet ready but have a path and the path isn't pending, then we can make it ready
            /*if (ready == 0 && agent.hasPath == true && agent.pathPending == false)
            {
                ready = 1;
                GameController.numberOfAgentsReady++;
                anim.enabled = false;
            }
            //if this agent is ready but not everyone else is, wait
            if (ready == 1 && GameController.maxNumberPeople - 1 > GameController.numberOfAgentsReady)
            {

                agent.Stop();
                anim.enabled = false;
                return;
            }
            else if (ready == 1)
            {

                anim.enabled = true;
                agent.Resume();
                start = 1;
            }*/
            agent.SetDestination(chosenTarget.position);
           /* if (makeDoor.recomputeFlag || agent.path.status == NavMeshPathStatus.PathInvalid || collided)
            {
                Debug.Log("Something is wrong");
                NavMesh.CalculatePath(this.gameObject.transform.position, chosenTarget.position, NavMesh.AllAreas, path);
                agent.path = path;
                collided = false;
            }
            else// if (path == null)
            {
                agent.SetDestination(chosenTarget.position);

            }*/


            //check if agent sees the exit
            if (seeExit == 0 && ready == 1)
            {
                //for each exit see if the player sees it 
                //find all the targets
                targets = GameObject.FindGameObjectsWithTag("destinations");
                for (int i = 0; i < targets.Length; i++)
                {
                    float distanceTo = Vector3.Distance(gameObject.transform.position, targets[i].transform.position);
                    if (distanceTo < 100f)
                    {
                        RaycastHit hit;
                        Vector3 rayDirection = gameObject.transform.position - targets[i].transform.position;
                        if (Physics.Raycast(targets[i].transform.position, rayDirection, out hit, 100f))
                        {
                            //if you hit this agent then agent sees exit
                            if (hit.transform.gameObject == gameObject)//transform.tag=="agent")
                            {
                                if (targets[i].transform.position != chosenTarget.position)
                                {
                                    agent.ResetPath();
                                }
                                agent.SetDestination(targets[i].transform.position);

                                seeExit = 1;
                                chosenTarget = targets[i].transform;
                                finalExit = targets[i].transform;
                                return;
                            }
                        }
                    }
                }
            }


            //go 360 to check what the other agents are doing. If 75% are doing one thing(going to same place), then the agent does it too
            timePassed++;
            if (timePassed > 20 && seeExit == 0 && ready == 1)
            {
                timePassed = 0;
                /*for(int i =0; i<360; i++)
                {
                    RaycastHit hit;
                    Vector3 rayDirection = Vector3.forward + i;
                    if (Physics.Raycast(targets[i].transform.position, rayDirection, out hit, 100f))
                    {
                        //if door ignore it
                        if (hit.transform.gameObject == gameObject)//transform.tag=="agent")
                        {
                            Debug.Log("its an agent. WOW!!!");
                            agent.SetDestination(targets[i].transform.position);
                            seeExit = 1;
                            chosenTarget = targets[i].transform;
                            return;
                        }
                    }
                }*/
                //the array of targets index should match that of the array of number of people with that destination
                targets = GameObject.FindGameObjectsWithTag("destinations");
                int[] nOfPeopleWithTarget = new int[targets.Length];
                int nOfPeople = 0;
                Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 15f);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].tag == "agent")
                    {
                        Vector3 destination = hitColliders[i].gameObject.GetComponent<NavMeshAgent>().destination;
                        for (int j = 0; j < targets.Length; j++)
                        {
                            if (targets[j].transform.position == destination)
                            {
                                nOfPeopleWithTarget[j]++;
                                nOfPeople++;
                            }
                        }
                    }
                }
                int maxPeople = 0;
                int whereMax = 0;
                //now go through nOfPeopleWithTarget and see what the biggest one is and see

                for (int i = 0; i < nOfPeopleWithTarget.Length; i++)
                {
                    if (maxPeople < nOfPeopleWithTarget[i])
                    {
                        maxPeople = nOfPeopleWithTarget[i];
                        whereMax = i;
                    }
                }

                if (maxPeople >= .75f * nOfPeople)
                {
                    //then set destination to what 75% of people believe
                    agent.SetDestination(targets[whereMax].transform.position);
                    chosenTarget = targets[whereMax].transform;
                }
            }



            if (agent.velocity == Vector3.zero)
            {
                agent.ResetPath();
                NavMesh.CalculatePath(this.gameObject.transform.position, chosenTarget.position, NavMesh.AllAreas, path);
                agent.path = path;
                agent.Resume();
            }


        }

            if (chosenTarget!=null&&Vector3.Distance(chosenTarget.position, this.gameObject.transform.position) <= 2)
            {
                    //GameController.totalTime += this.gameObject.GetComponent<timer>().time;
                //bringUpMenu.setTimer = false;
                GameController.totalTime = GameController.totalTime + ((this.GetComponent<timer>().time + (this.GetComponent<timer>().subtime*10)) * 0.0165f);
                this.gameObject.SetActive(false);
            }
            

        
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("door"))
        {

            collided = true;
        }

    }

    //for this character, randomly assign attributes:
    //so assign random speed
    //assign random acceleration
    //assign random height(between current and lower so that go underneath floor) for both navmesh and actual object
    //assign random radius for actual object
    //assign avoidancePriority
    void assignAttributes(string los,string loa,string homog)
    {
        NavMeshAgent navMeshComponent = gameObject.GetComponent<NavMeshAgent>();

        //navMeshComponent.obstacleAvoidanceType=ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //figure out speed
        //should be between 3 and 4
        float speedValue = UnityEngine.Random.value;

        //For crippled people
        /*if (speedValue < .05f)
        {
            speedValue = UnityEngine.Random.value;
            navMeshComponent.speed = 1f + 2 * speedValue;//2-5
        }*/
        if (loa.Equals("Low"))
        {
            // Debug.Log("lowloa");
            if (speedValue <= 0.7f)
            {
                navMeshComponent.speed = 3f + 4 * speedValue;
                navMeshComponent.acceleration = 3 + 2 * speedValue;//2-5
            }
            else
            {
                navMeshComponent.speed = 5f + 3 * speedValue;
                navMeshComponent.acceleration = 6 + 2 * speedValue;//2-5
            }



        }
        else if (loa.Equals("Medium"))
        {
            // Debug.Log("medloa");
            if (speedValue <= .5f)
            {
                navMeshComponent.speed = 3f + 4 * speedValue;
                navMeshComponent.acceleration = 3 + 2 * speedValue;//2-5
            }
            else
            {
                navMeshComponent.speed = 5f + 3 * speedValue;
                navMeshComponent.acceleration = 6 + 2 * speedValue;//2-5
            }

        }
        else
        {
            // Debug.Log("highloa");
            if (speedValue <= .3f)
            {
                navMeshComponent.speed = 3f + 4 * speedValue;
                navMeshComponent.acceleration = 3 + 2 * speedValue;//2-5
            }
            else
            {
                navMeshComponent.speed = 5f + 3 * speedValue;
                navMeshComponent.acceleration = 6 + 2 * speedValue;//2-5
            }


        }


        if (homog.Equals("Low"))
        {
            // Debug.Log("homolow");
            //figure out agent height
            navMeshComponent.height = 1.7f;
            float mass = UnityEngine.Random.value;
            gameObject.GetComponent<Rigidbody>().mass = gameObject.GetComponent<Rigidbody>().mass * 10 * mass;
            float heightValue = UnityEngine.Random.value;
            //the transform scale should be between .9 and 1.10
            gameObject.transform.localScale = new Vector3(.9f + .2f * heightValue, .9f + .2f * heightValue, .9f + .2f * heightValue);
            //1+  0-.3
            //figure out agent width. radius increased automatically
            float radiusValue = UnityEngine.Random.value;

            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - .4f + .8f * radiusValue, gameObject.transform.localScale.y, gameObject.transform.localScale.z - .4f + .8f * radiusValue);

        }
        else if (homog.Equals("Medium"))
        {
            //Debug.Log("homomed");
            //figure out agent height
            navMeshComponent.height = 1.7f;
            float heightValue = UnityEngine.Random.value;
            //the transform scale should be between .9 and 1.10

            float mass = UnityEngine.Random.value;
            if (mass <= 0.5f)
                gameObject.GetComponent<Rigidbody>().mass = gameObject.GetComponent<Rigidbody>().mass * 10 * mass;

            if (heightValue <= 0.5f)
                gameObject.transform.localScale = new Vector3(.9f + .2f * heightValue, .9f + .2f * heightValue, .9f + .2f * heightValue);
            //1+  0-.3
            //figure out agent width. radius increased automatically
            float radiusValue = UnityEngine.Random.value;

            if (radiusValue <= 0.5f)
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - .4f + .8f * radiusValue, gameObject.transform.localScale.y, gameObject.transform.localScale.z - .4f + .8f * radiusValue);

        }


        //assign random avoidancePriority:
        navMeshComponent.avoidancePriority = (int)(99f * UnityEngine.Random.value);

    }

}
