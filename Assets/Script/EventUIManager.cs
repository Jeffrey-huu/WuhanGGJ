using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUIManager : MonoBehaviour
{
    public Canvas eventDescription;
    public EventManager eventmanager;
    public GameObject eventpre;
    public GameObject eventgroup;

    void Start()
    {
        eventDescription.enabled = false;
        GenerateEvent();
    }
    public void GenerateEvent()
    {
        foreach (Transform child in eventgroup.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        int count = 0;
        while (i < eventmanager.eventarr.Length && count < 2)
        {
            if (!eventmanager.eventarr[i].isfinished)
            {
                Debug.Log(i);
                GameObject eventButton = Instantiate(eventpre, eventgroup.transform);
                eventButton.GetComponentInChildren<TextMeshProUGUI>().text = eventmanager.eventarr[i].name;
                eventButton.GetComponent<EventController>().eventindex = i;
                count++;
            }
            i++;
        }
    }

    public void OnEventClick(int eventIndex)
    {
        Debug.Log(eventIndex);
        eventmanager.eventarr[eventIndex].isfinished = true;
        eventDescription.enabled = true;
        eventDescription.GetComponentInChildren<TextMeshProUGUI>().text = eventmanager.eventarr[eventIndex].summary;
    }

    public void CloseEventUI()
    {
        eventDescription.enabled = false;
        GenerateEvent();
    }
}
