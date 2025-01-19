using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginAnimTrigger : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        Debug.Log("LoadScene");
        SceneManager.LoadSceneAsync(1); 
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        Debug.Log("test");  
    }
}
