using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public EventManager eventmanager;
    public GameObject eventpre;
    public GameObject eventgroup;

    int index = 0;
    void Start()
    {
        GenerateEvent();
    }
    public void GenerateEvent()
    {
        foreach (Transform child in eventgroup.transform)
        {
            Destroy(child.gameObject);
        }
        int count = 0;
        if (eventmanager.finishedNum < eventmanager.eventarr.Length - 1)
        {
            while (index < eventmanager.eventarr.Length && count < 2)
            {
                if (!eventmanager.eventarr[index].isfinished)
                {
                    Debug.Log(index);
                    GameObject eventButton = Instantiate(eventpre, eventgroup.transform);
                    TextMeshProUGUI[] textComponents = eventButton.GetComponentsInChildren<TextMeshProUGUI>();
                    textComponents[1].text = eventmanager.eventarr[index].name;
                    textComponents[0].text = eventmanager.eventarr[index].summary;
                    eventButton.GetComponent<EventController>().eventindex = index;
                    count++;
                }
                index++;
                if (index == eventmanager.eventarr.Length)
                {
                    index = 0;
                }
            }
        }
        else if (eventmanager.finishedNum < eventmanager.eventarr.Length)
        {
            while (index < eventmanager.eventarr.Length)
            {
                if (!eventmanager.eventarr[index].isfinished)
                {
                    Debug.Log(index);
                    GameObject eventButton = Instantiate(eventpre, eventgroup.transform);
                    TextMeshProUGUI[] textComponents = eventButton.GetComponentsInChildren<TextMeshProUGUI>();
                    textComponents[1].text = eventmanager.eventarr[index].name;
                    textComponents[0].text = eventmanager.eventarr[index].summary;
                    eventButton.GetComponent<EventController>().eventindex = index;
                    break;
                }
                index++;
                if (index == eventmanager.eventarr.Length)
                {
                    index = 0;
                }
            }
        }

    }


}
