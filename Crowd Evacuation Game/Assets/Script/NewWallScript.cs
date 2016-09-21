using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class NewWallScript : MonoBehaviour {

    RaycastHit hitInfo;
    Transform temp;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool selected;
    Vector3 curpos;
    bool leftflag = false;
    bool rightflag = false;
    float timer = 0;
    public List<GameObject> neighbors = new List<GameObject>();
    public int nOfWalls = 0;
    float moveDistance;

    // Use this for initialization
    void Start () {
        hitInfo = new RaycastHit();
        selected = false;
     
        leftflag = false;
        rightflag = false;
        timer = 0;
        moveDistance = .03f;
    }

    // Update is called once per frame
    void Update()
    {
        //if the wall is selected:
        if(selected)
        {
            //if up arrow, move wall up(up,right,left,down are determined by camera rotation)
            if (Input.GetKey(KeyCode.UpArrow))
            {
                makeWall.placingWall = -1;
                float yRotation = Camera.main.transform.eulerAngles.y;
                if (yRotation <=45&& yRotation>=0|| yRotation <= 360 && yRotation >= 315)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + moveDistance);
                }
                else if(yRotation <= 135 && yRotation > 45)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation <= 225 && yRotation > 135)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - moveDistance);
                }
                else if (yRotation < 315 && yRotation > 225)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                
            }
            //if down arrow, move wall down(up,right,left,down are determined by camera rotation)
            if (Input.GetKey(KeyCode.DownArrow))
            {
                makeWall.placingWall = -1;
                float yRotation = Camera.main.transform.eulerAngles.y;
                if (yRotation <= 45 && yRotation >= 0 || yRotation <= 360 && yRotation >= 315)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - moveDistance);
                }
                else if (yRotation <= 135 && yRotation > 45)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation <= 225 && yRotation > 135)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + moveDistance);
                }
                else if (yRotation < 315 && yRotation > 225)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }

        
            }
            //if right arrow, move wall right(up,right,left,down are determined by camera rotation)
            if (Input.GetKey(KeyCode.RightArrow))
            {
                makeWall.placingWall = -1;
                float yRotation = Camera.main.transform.eulerAngles.y;
                if ((yRotation <= 45 && yRotation >= 0) || (yRotation <= 360 && yRotation >= 315))
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x+ moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation <= 135 && yRotation > 45)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - moveDistance);
                }
                else if (yRotation <= 225 && yRotation > 135)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation < 315 && yRotation > 225)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + moveDistance);
                }
            }
            //if left arrow, move wall left(up,right,left,down are determined by camera rotation)
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                makeWall.placingWall = -1;
                float yRotation = Camera.main.transform.eulerAngles.y;
                if (yRotation <= 45 && yRotation >= 0 || yRotation <= 360 && yRotation >= 315)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x- moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation <= 135 && yRotation > 45)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + moveDistance);
                }
                else if (yRotation <= 225 && yRotation > 135)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + moveDistance, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (yRotation < 315 && yRotation > 225)
                {
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - moveDistance);
                }
            }
            //if I key pressed, make wall longer
            if (Input.GetKey(KeyCode.I))
            {
                makeWall.placingWall = -1;
                GameObject[] outer=GameObject.FindGameObjectsWithTag("outerWall");

                bool leftflag = false;
                bool rightflag = false;
                if (gameObject.transform.rotation.y == 0)
                {
                    float right = gameObject.transform.position.x + (transform.localScale.x / 2);
                    float left = gameObject.transform.position.x - (transform.localScale.x / 2);
                    
                    foreach (GameObject g in outer)
                    {

                        if (g.transform.rotation.y != 0)
                        {
                            float top = g.transform.position.x + g.transform.localScale.z / 2;
                            float bottom= g.transform.position.x - g.transform.localScale.z / 2;
                            if (right>=top && left<bottom)
                            {
                                rightflag = true;
                            }
                            else if(left<= bottom && right>top)
                            {
                                leftflag = true;
                            }

                        }

                    }
                    if(!leftflag && !rightflag)
                    {
                        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + 0.005f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
                    }

                }
                else
                {
                    float right = this.gameObject.transform.position.z + (this.transform.localScale.x / 2);
                    float left = this.gameObject.transform.position.z - (this.transform.localScale.x / 2);
                    foreach (GameObject g in outer)
                    {

                        if (g.transform.rotation.y == 0)
                        {
                            float top = g.transform.position.z + g.transform.localScale.z / 2;
                            float bottom= g.transform.position.z - g.transform.localScale.z / 2;
                            if (right >= top && left < bottom)
                                {
                                rightflag = true;
                            }
                            else if(left<=bottom && right > top)
                            {
                                leftflag = true;
                            }

                        }

                    }

                    if (!leftflag && !rightflag)
                    {
                        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + 0.005f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
                    }
                }

            }
            //if J key pressed, make wall less length
            if (Input.GetKey(KeyCode.J) && this.gameObject.transform.localScale.x >= 2f)
            {
                makeWall.placingWall = -1;
                this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x-0.005f, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            }
        }
        //if mouse clicked, select wall
        if (Input.GetMouseButton(0))
        {
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if this object is clicked then selected=true
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                temp = hitInfo.transform;

                if (temp.transform.gameObject == this.gameObject)
                {
                    timer++;

                    GameObject[] objs = GameObject.FindGameObjectsWithTag("outerWall");
                    List<float> verticals=new List<float>();
                    List<float> horizontals=new List<float>();
                    foreach (GameObject obj in objs)
                    {
                        if(obj.transform.rotation.y!=0)
                        {
                            verticals.Add(obj.transform.position.x);
                        }
                        else if(obj.transform.rotation.y == 0)
                        {
                            horizontals.Add(obj.transform.position.z);
                        }
                    }

                    if(transform.rotation.y==0)
                    {
                        if ((transform.position.x - transform.localScale.x / 2) <= verticals.Max()&& (transform.position.x + transform.localScale.x / 2) <= verticals.Max() && (transform.position.x + transform.localScale.x / 2) >= verticals.Min()&& (transform.position.x - transform.localScale.x / 2) >= verticals.Min())
                        {
                            curpos = transform.position;
                        }
                        else if(!((transform.position.x - transform.localScale.x / 2) > verticals.Max() && (transform.position.x + transform.localScale.x / 2) > verticals.Max()))
                        {
                            curpos = transform.position;
                        }
                        else if (!((transform.position.x - transform.localScale.x / 2) < verticals.Min() && (transform.position.x + transform.localScale.x / 2) < verticals.Min()))
                        {
                            curpos = transform.position;
                        }
                        else 
                        {
                            transform.position = curpos;
                        }
                    }
                    else
                    {
                        if ((transform.position.z - transform.localScale.x / 2) <= horizontals.Max()&& (transform.position.z + transform.localScale.x / 2) <= horizontals.Max() && (transform.position.z + transform.localScale.x / 2) >= horizontals.Min()&& (transform.position.z - transform.localScale.x / 2) >= horizontals.Min())
                        {
                            curpos = transform.position;
                        }
                        else if (!((transform.position.z - transform.localScale.x / 2) > horizontals.Max() && (transform.position.z + transform.localScale.x / 2) > horizontals.Max()))
                        {
                            curpos = transform.position;
                        }
                        else if (!((transform.position.z - transform.localScale.x / 2) < horizontals.Min() && (transform.position.z + transform.localScale.x / 2) < horizontals.Min()))
                        {
                            curpos = transform.position;
                        }
                        else
                        {
                            transform.position = curpos;
                        }
                    }

                    
                    selected = true;
                    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

                    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                }
                //if some other object is clicked then this object isn't selected
                else
                {
                    selected = false;
                }
            }


        }

        else if (Input.GetMouseButtonUp(0)) {

            if(timer>10 )
            {

                timer = 0;
                makeWall.placingWall = -1;
            }

            else
            {
                timer = 0;
            }
            GameObject[] objs = GameObject.FindGameObjectsWithTag("outerWall");
            List<float> verticals = new List<float>();
            List<float> horizontals = new List<float>();
            foreach (GameObject obj in objs)
            {
                if (obj.transform.rotation.y != 0)
                {
                    verticals.Add(obj.transform.position.x);
                }
                else if (obj.transform.rotation.y == 0)
                {
                    horizontals.Add(obj.transform.position.z);
                }
            }


            if (transform.rotation.y == 0)
            {
                
                if (((transform.position.x - transform.localScale.x / 2) > verticals.Max() && (transform.position.x + transform.localScale.x / 2) > verticals.Max()))
                {
                    transform.position = new Vector3(verticals.Max() - transform.localScale.x / 2, transform.position.y, transform.position.z);
                }
                else if (((transform.position.x - transform.localScale.x / 2) < verticals.Min() && (transform.position.x + transform.localScale.x / 2) < verticals.Min()))
                {
                    transform.position = new Vector3(verticals.Min() + transform.localScale.x / 2, transform.position.y, transform.position.z);
                }
                if((transform.position.z >= (horizontals.Max()-2f)))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, horizontals.Max() - 2f);
                }
                else if ((transform.position.z <= (horizontals.Min() + 2f)))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, horizontals.Min() + 2f);
                }


            }
            else
            {
                if (((transform.position.z - transform.localScale.x / 2) > horizontals.Max() && (transform.position.z + transform.localScale.x / 2) > horizontals.Max()))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, horizontals.Max() - transform.localScale.x / 2);
                }
                else if (((transform.position.z - transform.localScale.x / 2) < horizontals.Min() && (transform.position.z + transform.localScale.x / 2) < horizontals.Min()))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, horizontals.Min() + transform.localScale.x / 2);
                }
                if ((transform.position.x >= (verticals.Max() - 2f)))
                {
                    transform.position = new Vector3(verticals.Max() - 2f, transform.position.y, transform.position.z);
                }
                else if ((transform.position.x <= (verticals.Min() + 2f)))
                {
                    transform.position = new Vector3(verticals.Min() + 2f, transform.position.y, transform.position.z);
                }

            }

            GameObject[] outer = GameObject.FindGameObjectsWithTag("outerWall");

            if (this.gameObject.transform.rotation.y == 0)
            {
                float right = this.gameObject.transform.position.x + (this.transform.localScale.x / 2);
                float left = this.gameObject.transform.position.x - (this.transform.localScale.x / 2);

                float errorTop = 0, errorBottom = 0;

                foreach (GameObject g in outer)
                {

                    if (g.transform.rotation.y != 0)
                    {
                        float top = g.transform.position.x + g.transform.localScale.z / 2;
                        float bottom = g.transform.position.x - g.transform.localScale.z / 2;
             
                        if (right > top - 0.3f && left < bottom && curpos.x<g.transform.position.x)
                        {
                            rightflag = true;
                            errorTop = top;
                            break;
                        }
                        else if (left < bottom + 0.3f && right > top && curpos.x > g.transform.position.x)
                        {

                            leftflag = true;
                            errorBottom = bottom;
                            break;
                        }

                    }

                }
                if (leftflag)
                {

                    leftflag = false;
                    transform.position = new Vector3(errorBottom + transform.localScale.x / 2, transform.position.y, transform.position.z);

                }
                if (rightflag)
                {
                    rightflag = false;
                    transform.position = new Vector3(errorTop - transform.localScale.x / 2, transform.position.y, transform.position.z);
   
                }

            }
            else
            {
                float right = this.gameObject.transform.position.z + (this.transform.localScale.x / 2);
                float left = this.gameObject.transform.position.z - (this.transform.localScale.x / 2);
                float errorTop = 0, errorBottom = 0;
                foreach (GameObject g in outer)
                {

                    if (g.transform.rotation.y == 0)
                    {
                        float top = g.transform.position.z + g.transform.localScale.z / 2;
                        float bottom = g.transform.position.z - g.transform.localScale.z / 2;
                        if (right > top - 0.3f && left < bottom && curpos.z < g.transform.position.z)
                        {
                            rightflag = true;
                            errorTop = top;
                            break;
                        }
                        else if (left < bottom + 0.3f && right > top && curpos.z > g.transform.position.z)
                        {
                            leftflag = true;
                            errorBottom = bottom;
                            break;
                        }

                    }

                }

                if (leftflag)
                {
                    //dontMove = true;
                    leftflag = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y, errorBottom + transform.localScale.x / 2);
                }
                if (rightflag)
                {
                    //dontMove = true;
                    rightflag = false;
                    transform.position = new Vector3(transform.position.x, transform.position.y, errorTop - transform.localScale.x / 2);
                }

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                temp = hitInfo.transform;

                if (temp.transform.gameObject == this.gameObject)
                {
                    this.gameObject.SetActive(false);
                    foreach(GameObject g in neighbors)
                    {
                        Destroy(g);
                    }
                    Destroy(gameObject);
                    Camera.main.GetComponent<bringUpMenu>().nOfWalls--;
                }
            }
        }
    }

    void OnMouseDrag()
    {

        if (timer > 10)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            transform.position = new Vector3(curPosition.x, transform.position.y, curPosition.z);
        }
       
    }

}
