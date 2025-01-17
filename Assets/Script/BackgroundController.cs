using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Transform tr;

    static public BackgroundController instance;

    // 定义一个自定义类来封装 Vector3 和 float
    public class TransformData
    {
        public Vector3 Position { get; set; }//地图应该位移到位置
        public float Scale { get; set; }//地图缩放比例

        public TransformData(Vector3 position, float scale)
        {
            Position = position;
            Scale = scale;
        }
    }


    public string[] nameList;
    public Vector3[] posList;
    public float[] scaleList;

    // string为位置名，后面为地图对应的信息
    [SerializeField]public Dictionary<string, TransformData> transformMap = new Dictionary<string, TransformData>();



    void Awake()
    {
        tr = GetComponent<Transform>();
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeDic();
    }

    //平移到目标点
    public void SmoothMove(Vector3 destination)
    {
        destination.z = 0;
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 5);
    }

    //平缓放缩
    public void SmoothScale(float scale)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, scale), Time.deltaTime * 5);
    }

    //制作字典
    private void MakeDic()
    {
        for (int i = 0; i < nameList.Length; i++)
        {
            if(i>=nameList.Length || i>=posList.Length || i>=scaleList.Length)
            {
                Debug.Log("MakeDic Error");
                return;
            }
            transformMap.Add(nameList[i], new TransformData(posList[i],scaleList[i]));
        }
    }

    //将地图移动到对应的位置
    public void MoveTo(string name)
    {
        if(transformMap.ContainsKey(name))
        {
            SmoothMove(transformMap[name].Position);
            SmoothScale(transformMap[name].Scale);
        }
    }

    void Update()
    {
    }
}