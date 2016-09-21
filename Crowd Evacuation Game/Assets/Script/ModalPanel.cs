using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class ModalPanel : MonoBehaviour {

    public Text score;
    public Button nextlevel;
    public Button replay;
    public Button quit;
    float replayPos;
    float quitPos;
    public GameObject modalPanelObject;

    private static ModalPanel modalPanel;

    public void Start()
    {
        replayPos = replay.gameObject.transform.position.y + 18;
        quitPos = quit.gameObject.transform.position.y + 18;
    }

    public static ModalPanel Instance()
    {
        if (!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!modalPanel)
                Debug.LogError("one active modal panel script needed");
        }
        return modalPanel;
    }

    public void Choice(string score,UnityAction nextEvent,UnityAction repEvent,UnityAction qEvent)
    {
        modalPanelObject.SetActive(true);

        nextlevel.onClick.RemoveAllListeners();
        nextlevel.onClick.AddListener(nextEvent);

        replay.onClick.RemoveAllListeners();
        replay.onClick.AddListener(repEvent);

        quit.onClick.RemoveAllListeners();
        quit.onClick.AddListener(qEvent);

        this.score.text = score;

        if (SceneManager.GetActiveScene().name != "Level5")
        {
            nextlevel.gameObject.SetActive(true);
        }
        else
        {
            nextlevel.gameObject.SetActive(false);
            replay.gameObject.transform.position= new Vector3(replay.gameObject.transform.position.x, replayPos,0);
            quit.gameObject.transform.position = new Vector3(quit.gameObject.transform.position.x, quitPos, 0);

        }
        replay.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }
   public void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }
}
