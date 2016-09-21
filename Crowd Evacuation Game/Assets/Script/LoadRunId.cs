using UnityEngine;
using System.Collections;

public class LoadRunId : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
	
		string url="http://crowdevac.com/get_run_id.php?id=1";
		//string url="http://localhost/get_run_id.php?id=1";
				WWW www = new WWW(url);
				yield return www;
				nameStore.runId=www.text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
