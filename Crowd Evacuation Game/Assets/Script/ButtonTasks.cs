using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonTasks : MonoBehaviour {

    AudioSource audio2;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
	// Use this for initialization
	void Start () {
        audio2 = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void buttonStart()
    {
        SceneManager.LoadScene("EnterName");
    }
    public void buttonSettings()
    {
		SceneManager.LoadScene("Settings");
    }
	    public void buttonGamePlay()
    {
		SceneManager.LoadScene("GamePlay");
    }
    public void Rules()
    {
		SceneManager.LoadScene("Rules");
    }
    public void credits()
    {
		SceneManager.LoadScene("Credits");
    }
    public void quit()
    {
        Application.OpenURL("http://crowdevac.com/survey.html");
    }
    public void leaderboardEnter()
    {
        SceneManager.LoadScene("LeaderBoardDropDown");
    }
    public void architect()
    {
		DestroyObject(GameObject.Find("Game"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void firefighter()
    {

    }
    public void backscript()
    {
        DestroyObject(GameObject.Find("Game"));
        DestroyObject(GameObject.Find("Value"));
        SceneManager.LoadScene("Start");
    }
	public void enterName()
	{
				nameStore.name=GameObject.Find("name").GetComponent<InputField>().text;
        DestroyObject(GameObject.Find("Game"));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
		FileScript.checkedHM=false;
		FileScript.replayFlag=false;


	}


    
}
