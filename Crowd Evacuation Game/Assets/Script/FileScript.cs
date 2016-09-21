using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Net;
using UnityEngine.SceneManagement;


public class FileScript : MonoBehaviour
{
    public static List<List<Transform>> tempListList;
    public static float time;
	public static bool checkedHM=false;
	public static bool replayFlag=false;
	public static bool checkedBHM=false;
    public bool flag;
    bool headFlag = false;
	public Text mytext;
	public GameObject heatPlane;
	private IEnumerator coroutine;
    // Use this for initialization
    void Start()
    {
        flag = false;
        headFlag = false;
        tempListList = new List<List<Transform>>();
        time = 0;
        FileScript.time = bringUpMenu.mytimer;
    }

    // Update is called once per frame

    public void createXML()
    {
        if (!flag)
        {

            flag = true;
               // Debug.Log("no xml file yet");
				//WebClient web=new WebClient();
				//web.Credentials = new System.Net.NetworkCredential("username", "password");
				//String xml=web.DownloadString(new Uri("https://crowdevac.com/XMLUserData/"+ SceneManager.GetActiveScene().name + ".xml"));
				//web.DownloadFile(new Uri("https://crowdevac.com/XMLUserData/"+ SceneManager.GetActiveScene().name + ".xml"), SceneManager.GetActiveScene().name + ".xml");
                //XmlDocument doc = new XmlDocument();
				//Application.OpenURL("http://localhost/store_data.php?scene="+SceneManager.GetActiveScene().name);
				//WebRequest request = WebRequest.Create("http://localhost/store_data.php?scene="+SceneManager.GetActiveScene().name);
 
							
				//request.Method="GET";
				//WebResponse response = request.GetResponse();
				//Stream stream=response.GetResponseStream();
				//StreamReader reader = new StreamReader(stream, Encoding.UTF8);
				//String xml = reader.ReadToEnd();
				
				XmlDocument doc = new XmlDocument();
				doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<document>\n</document>");
				XmlNode document = doc.SelectSingleNode("document");

                XmlNode userdata = doc.CreateElement("User-Data");
                document.AppendChild(userdata);

            Debug.Log(nameStore.name);
                XmlNode playerid = doc.CreateElement("Player-ID");
                playerid.InnerText=nameStore.name;
                userdata.AppendChild(playerid);
				
				XmlNode runid = doc.CreateElement("Run-ID");
                runid.InnerText=nameStore.runId.ToString();
                userdata.AppendChild(runid);
				
				XmlNode replay = doc.CreateElement("Replay");
                replay.InnerText=FileScript.replayFlag.ToString();
                userdata.AppendChild(replay);
				
				XmlNode chm = doc.CreateElement("Checked-Heatmap");
                chm.InnerText=FileScript.checkedHM.ToString();
                userdata.AppendChild(chm);
				FileScript.checkedHM=false;

				XmlNode cbhm = doc.CreateElement("Checked-BestHeatmap");
                cbhm.InnerText=FileScript.checkedBHM.ToString();
                userdata.AppendChild(cbhm);

            int l = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().value;
            string losstr = GameObject.Find("los").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

            l = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().value;
            string loastr = GameObject.Find("loa").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

            l = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().value;
            string homostr = GameObject.Find("homo").transform.GetChild(1).GetComponent<Dropdown>().options[l].text;

            XmlNode los = doc.CreateElement("LevelOfService");
                los.InnerText = losstr;
                userdata.AppendChild(los);



            XmlNode loa = doc.CreateElement("LevelOfAggression");
            loa.InnerText = loastr;
            userdata.AppendChild(loa);

            XmlNode homo = doc.CreateElement("Homogeneity");
            homo.InnerText = homostr;
            userdata.AppendChild(homo);

            XmlNode wallsnode = doc.CreateElement("Walls");
                userdata.AppendChild(wallsnode);
            Debug.Log("hey hello " + tempListList[0].Count);
                foreach(Transform wall in tempListList[0])
                {
                    XmlNode wallnode = doc.CreateElement("Wall");
                    wallsnode.AppendChild(wallnode);

                    XmlNode position = doc.CreateElement("Position");
                    XmlNode rotation = doc.CreateElement("Rotation");
                    XmlNode scale = doc.CreateElement("Scale");
                    wallnode.AppendChild(position);
                    wallnode.AppendChild(rotation);
                    wallnode.AppendChild(scale);

                    XmlNode pxnode = doc.CreateElement("X");
                    pxnode.InnerText = wall.position.x.ToString();
                    XmlNode pynode = doc.CreateElement("Y");
                    pynode.InnerText = wall.position.y.ToString();
                    XmlNode pznode = doc.CreateElement("Z");
                    pznode.InnerText = wall.position.z.ToString();

                    position.AppendChild(pxnode);
                    position.AppendChild(pynode);
                    position.AppendChild(pznode);

                    //rotation
                    XmlNode rxnode = doc.CreateElement("X");
                    rxnode.InnerText = wall.rotation.x.ToString();
                    XmlNode rynode = doc.CreateElement("Y");
                    rynode.InnerText = wall.rotation.y.ToString();
                    XmlNode rznode = doc.CreateElement("Z");
                    rznode.InnerText = wall.rotation.z.ToString();

                    rotation.AppendChild(rxnode);
                    rotation.AppendChild(rynode);
                    rotation.AppendChild(rznode);


                    //scale

                    XmlNode sxnode = doc.CreateElement("X");
                    sxnode.InnerText = wall.localScale.x.ToString();
                    XmlNode synode = doc.CreateElement("Y");
                    synode.InnerText = wall.localScale.y.ToString();
                    XmlNode sznode = doc.CreateElement("Z");
                    sznode.InnerText = wall.localScale.z.ToString();

                    scale.AppendChild(sxnode);
                    scale.AppendChild(synode);
                    scale.AppendChild(sznode);
                }

                XmlNode doorsnode = doc.CreateElement("Doors");
                userdata.AppendChild(doorsnode);

                foreach (Transform door in tempListList[1])
                {
                    XmlNode doornode = doc.CreateElement("Door");
                    doorsnode.AppendChild(doornode);

                    XmlNode position = doc.CreateElement("Position");
                    XmlNode rotation = doc.CreateElement("Rotation");
                    XmlNode scale = doc.CreateElement("Scale");
                    doornode.AppendChild(position);
                    doornode.AppendChild(rotation);
                    doornode.AppendChild(scale);

                    XmlNode pxnode = doc.CreateElement("X");
                    pxnode.InnerText = door.position.x.ToString();
                    XmlNode pynode = doc.CreateElement("Y");
                    pynode.InnerText = door.position.y.ToString();
                    XmlNode pznode = doc.CreateElement("Z");
                    pznode.InnerText = door.position.z.ToString();

                    position.AppendChild(pxnode);
                    position.AppendChild(pynode);
                    position.AppendChild(pznode);

                    //rotation
                    XmlNode rxnode = doc.CreateElement("X");
                    rxnode.InnerText = door.rotation.x.ToString();
                    XmlNode rynode = doc.CreateElement("Y");
                    rynode.InnerText = door.rotation.y.ToString();
                    XmlNode rznode = doc.CreateElement("Z");
                    rznode.InnerText = door.rotation.z.ToString();

                    rotation.AppendChild(rxnode);
                    rotation.AppendChild(rynode);
                    rotation.AppendChild(rznode);


                    //scale

                    XmlNode sxnode = doc.CreateElement("X");
                    sxnode.InnerText = door.localScale.x.ToString();
                    XmlNode synode = doc.CreateElement("Y");
                    synode.InnerText = door.localScale.y.ToString();
                    XmlNode sznode = doc.CreateElement("Z");
                    sznode.InnerText = door.localScale.z.ToString();

                    scale.AppendChild(sxnode);
                    scale.AppendChild(synode);
                    scale.AppendChild(sznode);
                }

                XmlNode pillarsnode = doc.CreateElement("Pillars");
                userdata.AppendChild(pillarsnode);

                foreach (Transform pillar in tempListList[2])
                {
                    XmlNode pillarnode = doc.CreateElement("Pillar");
                    pillarsnode.AppendChild(pillarnode);

                    XmlNode position = doc.CreateElement("Position");
                    XmlNode rotation = doc.CreateElement("Rotation");
                    XmlNode scale = doc.CreateElement("Scale");
                    pillarnode.AppendChild(position);
                    pillarnode.AppendChild(rotation);
                    pillarnode.AppendChild(scale);

                    XmlNode pxnode = doc.CreateElement("X");
                    pxnode.InnerText = pillar.position.x.ToString();
                    XmlNode pynode = doc.CreateElement("Y");
                    pynode.InnerText = pillar.position.y.ToString();
                    XmlNode pznode = doc.CreateElement("Z");
                    pznode.InnerText = pillar.position.z.ToString();

                    position.AppendChild(pxnode);
                    position.AppendChild(pynode);
                    position.AppendChild(pznode);

                    //rotation
                    XmlNode rxnode = doc.CreateElement("X");
                    rxnode.InnerText = pillar.rotation.x.ToString();
                    XmlNode rynode = doc.CreateElement("Y");
                    rynode.InnerText = pillar.rotation.y.ToString();
                    XmlNode rznode = doc.CreateElement("Z");
                    rznode.InnerText = pillar.rotation.z.ToString();

                    rotation.AppendChild(rxnode);
                    rotation.AppendChild(rynode);
                    rotation.AppendChild(rznode);


                    //scale

                    XmlNode sxnode = doc.CreateElement("X");
                    sxnode.InnerText = pillar.localScale.x.ToString();
                    XmlNode synode = doc.CreateElement("Y");
                    synode.InnerText = pillar.localScale.y.ToString();
                    XmlNode sznode = doc.CreateElement("Z");
                    sznode.InnerText = pillar.localScale.z.ToString();

                    scale.AppendChild(sxnode);
                    scale.AppendChild(synode);
                    scale.AppendChild(sznode);
                }

			/*
                //------------------------------------------------------------------------
                XmlNode heatmapNode = doc.CreateElement("Heatmap");
                userdata.AppendChild(heatmapNode);

                Dictionary<Vector2, float> points = Camera.main.GetComponent<collectResults>().allData;
                foreach (KeyValuePair<Vector2, float> keyval in points)
                {
                    XmlNode pointNode = doc.CreateElement("Point");
                    heatmapNode.AppendChild(pointNode);
                    XmlNode positionNode = doc.CreateElement("Position");
                    positionNode.InnerText = "(" + keyval.Key.x + "," + keyval.Key.y + ")";
                    pointNode.AppendChild(positionNode);

                    XmlNode valueNode = doc.CreateElement("Value");
                    valueNode.InnerText = "" + keyval.Value;
                    pointNode.AppendChild(valueNode);
                }

                XmlNode usefulInfo = doc.CreateElement("UsefulInfo");
                userdata.AppendChild(usefulInfo);
                XmlNode AmountOfPeopleEscaped = doc.CreateElement("AmountOfPeopleEscaped");
                AmountOfPeopleEscaped.InnerText = ""+(GameController.maxNumberPeople - GameController.numberLeft);
                usefulInfo.AppendChild(AmountOfPeopleEscaped);
                XmlNode AmountOfPeopleLeft = doc.CreateElement("AmountOfPeopleLeft");
                AmountOfPeopleLeft.InnerText = ("" + GameController.numberLeft);
                usefulInfo.AppendChild(AmountOfPeopleLeft);
                XmlNode totalEscapeTime = doc.CreateElement("totalEscapeTime");
                totalEscapeTime.InnerText = "" + GameController.totalTime;
                usefulInfo.AppendChild(totalEscapeTime);
                XmlNode averageEscapeTime = doc.CreateElement("averageEscapeTime");
                averageEscapeTime.InnerText = "" + (GameController.totalTime / (GameController.maxNumberPeople - GameController.numberLeft));
                usefulInfo.AppendChild(averageEscapeTime);
                







*/

                //--------------------------------------------------------------------------








                XmlNode timenode = doc.CreateElement("Time-Elapsed");
                timenode.InnerText = ""+ bringUpMenu.mytimer;
                userdata.AppendChild(timenode);
				
					MemoryStream mStream = new MemoryStream();
					XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
					writer.Formatting = Formatting.Indented;
					doc.WriteContentTo(writer);
					writer.Flush();
					mStream.Flush();
					mStream.Position = 0;
					StreamReader sReader = new StreamReader(mStream);
					//doc.Save("C:\\Users\\NILAY\\Desktop\\temp1.xml");
					string xml=sReader.ReadToEnd();
					//Debug.Log(xml.Length);
					//var fileStream = File.Create("C:\\Users\\NILAY\\Desktop\\temp.xml");
					/*StreamWriter swr=new StreamWriter(fileStream);
					swr.WriteLine(xml);
					sReader = new StreamReader(fileStream);
					string tempval=sReader.ReadToEnd();
					Debug.Log(tempval.Length);
				Debug.Log("hello"+xml.ToString()+"hello");*/
				//request = WebRequest.Create("http://localhost/store_data.php");
				//request.Method="POST";
				
				string url="http://crowdevac.com/store_data.php";   //----for live
				//string url="http://localhost/store_data.php";    //--for local
				//var response : Response = new Response();
				//mytext.text=SceneManager.GetActiveScene().name;
				WWWForm loginForm = new WWWForm();
				loginForm.AddField("scene",SceneManager.GetActiveScene().name,System.Text.Encoding.UTF8);
				loginForm.AddField("querystring", xml,System.Text.Encoding.UTF8);
        
        // Sending request:
        WWW www = new WWW(url, loginForm); 
		coroutine=WaitForRequest(www);
		StartCoroutine(coroutine);
		
		Dictionary<Vector2, float> points = Camera.main.GetComponent<collectResults>().allData;
        //SpeedAndPosition[] speedPoints = mainCam.GetComponent<collectResults>().speedAndPos.ToArray();
        //Texture2D tex = Heatmap.CreateHeatmap(points, mainCam, 5);
        Texture2D tex = Heatmap.CreateHeatmap(points, Camera.main, heatPlane);
		
		byte[] bytes = tex.EncodeToPNG();
		
		//FileStream fs=new FileStream(Application.dataPath+"/"+SceneManager.GetActiveScene().name+".png",FileMode.Create,FileAccess.ReadWrite);
		//fs.Write(bytes,0,bytes.Length);
		//fs.Close();
		
		url="http://crowdevac.com/store_image.php"; //--live
		//url="http://localhost/store_image.php"; //-- local
		loginForm = new WWWForm();
		loginForm.AddField("scene",SceneManager.GetActiveScene().name,System.Text.Encoding.UTF8);
		loginForm.AddField("runid",nameStore.runId,System.Text.Encoding.UTF8);
		loginForm.AddBinaryData("fileUpload",bytes);
		
		www = new WWW(url, loginForm); 
		coroutine=WaitForRequest(www);
		StartCoroutine(coroutine);
    
        // Waiting for response:
        //yield httpResponse;
    
				
				//doc.Save(SceneManager.GetActiveScene().name + ".xml");
				//web.UploadFile ("https://crowdevac.com/XMLUserData/",SceneManager.GetActiveScene ().name + ".xml");
			 //}
        }
    }
	
IEnumerator WaitForRequest(WWW www)
 {
     yield return www;
     // check for errors
     if (www.error == null)
     {
         //mytext.text="WWW Ok!: " + www.data;
     } else {
         //mytext.text="WWW Error: "+ www.error;
     }    
 }    
}