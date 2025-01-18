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


}
