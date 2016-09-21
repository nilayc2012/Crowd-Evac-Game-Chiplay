using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MidModalPanel : MonoBehaviour {
    public Button cont;
    public Button restart;
    public Button quit;

    public GameObject modalPanelObject;

    private static MidModalPanel modalPanel;

    public static MidModalPanel Instance()
    {
        if(!modalPanel)
        {
            modalPanel = FindObjectOfType(typeof(MidModalPanel)) as MidModalPanel;
            if (!modalPanel)
                Debug.LogError("one active modal panel script needed");
        }
        return modalPanel;
    }

    public void Choice(UnityAction repEvent, UnityAction qEvent)
    {
        modalPanelObject.SetActive(true);
        cont.onClick.RemoveAllListeners();
        cont.onClick.AddListener(ClosePanel);

        restart.onClick.RemoveAllListeners();
        restart.onClick.AddListener(repEvent);

        quit.onClick.RemoveAllListeners();
        quit.onClick.AddListener(qEvent);


        cont.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        bringUpMenu.pause = false;
        if(!bringUpMenu.crowdflag && !bringUpMenu.simflag)
            bringUpMenu.noList = false;
        else if(bringUpMenu.simflag)
        {
            Camera.main.GetComponent<MouseLook>().enabled = true;
            Camera.main.GetComponent<mainCameraControls>().enabled = true;
        }
        else
        {
            GameObject.Find("Canvascrowd").GetComponent<Canvas>().enabled=true;
        }
        modalPanelObject.SetActive(false);
    }
}




