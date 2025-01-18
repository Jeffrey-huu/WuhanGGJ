using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public mevent[] eventarr;

    public void AddAssetToEvent(int eventindex, int asset)
    {
        Debug.Log(eventarr[eventindex].name + " add " + asset);
    }
}
