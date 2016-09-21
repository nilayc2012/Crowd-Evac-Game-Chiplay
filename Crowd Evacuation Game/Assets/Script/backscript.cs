using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class backscript : MonoBehaviour {

    public GameObject button;
    // Use this for initialization
    void Start()
    {

        this.GetComponent<Button>().onClick.AddListener(() => { GameObject.Find("Game").GetComponent<ButtonTasks>().backscript(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
