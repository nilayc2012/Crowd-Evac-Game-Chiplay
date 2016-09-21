using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class archscript : MonoBehaviour {

    public GameObject button;
	// Use this for initialization
	void Start () {

        this.GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("Game").GetComponent<ButtonTasks>().architect(); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
