using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionBar : MonoBehaviour
{
    Transform tr;
    static public EmotionBar instance;
    public float offset;

    public int emotionValue;

    void Awake()
    {
        instance = this;
        tr = transform;
        emotionValue = 50;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateScale();
    }

    //情绪最大值：100，最小值：0
    public void AddEmotion(int value)
    {
        emotionValue = Mathf.Clamp(emotionValue + value, 0, 100);
    }

    public void DecreaseEmotion(int value)
    {
        emotionValue = Mathf.Clamp(emotionValue - value, 0, 100);
    }

    private void UpdateScale()
    {
        float scale = emotionValue / offset;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, 1, 1), Time.deltaTime * 5);
    }
}
