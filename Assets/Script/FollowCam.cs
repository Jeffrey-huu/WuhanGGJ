using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public FollowCam instance;
    static private float cameraZ;
    public GameObject target;

    void Awake()
    {
        instance=this;
        cameraZ = transform.position.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //平滑跟随target
    void FixedUpdate()
    {
        Vector3 destination = target.transform.position;
        destination.z = cameraZ;
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 5);
    }
}
