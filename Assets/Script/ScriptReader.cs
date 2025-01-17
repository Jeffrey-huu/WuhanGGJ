using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 脚本的内容格式
[System.Serializable]
public class ScriptData
{
    public string content;//文本内容
    public int control;//控制类型
    public int value;//控制值
}

// 脚本数据项封装
[System.Serializable]
public class DataWrapper
{
    public ScriptData[] items;  // 数据项列表
}

// 脚本读取器
// 默认放置在 Resources/scripts.json
public class ScriptReader : MonoBehaviour
{
    public String fileName = "scripts";//脚本文件名称
    static ScriptReader instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        if (jsonFile != null)
        {
            string jsonString = jsonFile.text;
            ParseJson(jsonString);
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }

    }

    ScriptData[] ParseJson(string jsonString)
    {
        ScriptData[] dataArray = JsonUtility.FromJson<DataWrapper>("{\"items\":" + jsonString + "}").items;

        if (dataArray == null)
        {
            Debug.LogError("Failed to parse JSON!");
            return null;
        }

        return dataArray;

    }

    public void PrintDataArray(ScriptData[] dataArray)
    {
        if (dataArray != null)
        {
            // 输出数组内容
            for (int i = 0; i < dataArray.Length; i++)
            {
                Debug.Log("Content: " + dataArray[i].content);
                Debug.Log("Control: " + dataArray[i].control);
                Debug.Log("Value: " + dataArray[i].value);
            }
        }
        else
        {
            Debug.LogError("Argument is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
