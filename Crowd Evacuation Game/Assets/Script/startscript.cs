using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class startscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(() => { nameStore.name =GameObject.Find("name").GetComponent<InputField>().text; SceneManager.LoadScene(3); });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
