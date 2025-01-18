using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EventUIManager eventUIManager;
    public int RoundNum;
    void Start()
    {
        RoundNum = 0;
    }
    public void NextRound()
    {
        Debug.Log("next turn");
        eventUIManager.GenerateEvent();
        RoundNum++;
    }
}
