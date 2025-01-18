using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public BubbleController bubbleController;
    public TextMeshProUGUI bubbleValue;
    public void UpdateValue()
    {
        bubbleValue.text = bubbleController.currentAsset.ToString();
    }

    void Update()
    {
        UpdateValue();
    }
}
