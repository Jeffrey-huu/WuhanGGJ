using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SmallBubble : MonoBehaviour
{
    static public SmallBubble instance;

    public float followSpeed = 0.1f; 
    public float maxScale = 5; 

    private Renderer bubbleRenderer;

    void Awake()
    {
        instance = this;
        bubbleRenderer = GetComponent<SpriteRenderer>();
        SetVisiable(false);
    }

    public void SetScale(float scale)
    {
        float realScale = scale * maxScale;
        transform.localScale = new Vector3(realScale, realScale, realScale);
    }

    public void SetVisiable(bool visiable)
    {
        bubbleRenderer.enabled = visiable;
        if(visiable==false)SetScale(0.1f);
        // gameObject.SetActive(visiable);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Input.mousePosition;

        // 将屏幕坐标转换为世界坐标
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 设置 z 坐标，确保 SmallBubble 在正确的平面上
        mousePosition.z = transform.position.z;

        // 使用 Lerp 方法平滑移动到鼠标位置
        transform.position = Vector3.Lerp(transform.position, mousePosition, followSpeed);
    }
}
