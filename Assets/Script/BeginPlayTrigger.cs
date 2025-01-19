using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BeginPlayTrigger : MonoBehaviour
{
    public GameObject anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("BeginPlayTrigger OnMouseDown");
        AudioSystem.instance.PlayGameBeginSound();
        anim.SetActive(true);
    }
}
