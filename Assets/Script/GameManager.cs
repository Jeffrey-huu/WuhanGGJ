using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UI_Asset uI_Asset;
    public BubbleController bubbleController;
    public EventUIManager eventUIManager;
    public EventManager eventManager;
    public GameObject eventgroup;
    public int RoundNum;
    void Start()
    {
        RoundNum = 0;
    }
    public void NextRound()
    {
        AudioSystem.instance.PlayNextTurnSound();
        uI_Asset.AddAsset(bubbleController.currentAsset / 5);
        foreach (Transform _event in eventgroup.transform)
        {
            int eventindex = _event.GetComponent<EventController>().eventindex;
            eventManager.EventEnd(eventindex);
        }
        Debug.Log("next turn");
        eventUIManager.GenerateEvent();
        eventManager.BuffEffect();
        RoundNum++;
    }

    public void Victory()
    {
        Debug.Log("victory");
    }
    public void Defeat()
    {
        Debug.Log("defeat");
    }
}
