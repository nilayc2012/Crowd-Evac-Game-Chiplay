using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System.Text;
using System.Net;
using System.IO;

public class leaderScript : MonoBehaviour {

    public Text index;
    public Text pname;
    public Text score;
    public GameObject panel;
    public string los, loa, homo;

    string playername;
    float mintime = System.Single.PositiveInfinity;
	// Use this for initialization
	IEnumerator Start () {
        float x = panel.transform.position.x;
        float y = panel.transform.position.y;

        for (int i = 1; i <= 5; i++)
        {
            mintime = System.Single.PositiveInfinity;
            playername = "";
            Debug.Log("hello");
            /*WebRequest request = WebRequest.Create("http://localhost/store_data.php?scene=Level"+i.ToString());
                    request.Method="GET";
                    WebResponse response = request.GetResponse();
                    Stream stream=response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    string xml = reader.ReadToEnd();*/
            Debug.Log(datapassing.los+" "+datapassing.loa+" "+datapassing.homo);
           string url = "http://crowdevac.com/store_data.php?scene=Level" + i.ToString()+"&los="+datapassing.los+"&loa="+datapassing.loa+"&homo="+datapassing.homo;
            //string url="http://localhost/store_data.php?scene=Level"+i.ToString();
            WWW www = new WWW(url);
            yield return www;
            string xml = www.text;
            Debug.Log(xml);
            if (xml != null) { 
            string[] xmlarr = xml.Split('+');
                Debug.Log(xmlarr.Length);

                playername = xmlarr[0];
                Debug.Log(xmlarr[0]);
            mintime = System.Single.Parse(xmlarr[1]);
        }
        /*XmlDocument doc = new XmlDocument();

            doc.LoadXml(xml);
            Debug.Log("hello" + xml.Length);
            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
            {
                float nodetime=0.0f;
                string tempname="";
                foreach (XmlNode cnode in node.ChildNodes)
                {
                    if (cnode.Name== "Player-ID")
                    {
                        tempname = cnode.InnerText;
                    }
                    if(cnode.Name== "Time-Elapsed")
                    {
                        nodetime= System.Single.Parse(cnode.InnerText);
                    }

                }

                if(nodetime<mintime)
                {
                    mintime = nodetime;
                    playername = tempname;
                }

            }*/

            if (mintime != System.Single.PositiveInfinity){
            GameObject newPanel = Instantiate(panel);
            GameObject lbody = GameObject.FindGameObjectWithTag("leaderbody");

            newPanel.transform.parent = lbody.transform;
            newPanel.transform.position = new Vector3(x, y, 0);
                //Debug.Log("Hello " + playername + " hhh " + mintime);
            newPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = playername;
            newPanel.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Level "+i.ToString();
            newPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = mintime.ToString();

		}
            y = y - 60;
       
		}

    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
