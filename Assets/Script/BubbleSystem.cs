using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    public static BubbleSystem instance;

    private void Awake()
    {
        // 确保场景中只有一个该单例的实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 以下是单例类的其他功能函数，可以根据需要添加
    public void DoSomething()
    {
        Debug.Log("Singleton is doing something.");
    }
}