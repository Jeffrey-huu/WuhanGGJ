using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    public static BubbleSystem instance;

    private void Awake()
    {
        // ȷ��������ֻ��һ���õ�����ʵ��
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // �����ǵ�������������ܺ��������Ը�����Ҫ����
    public void DoSomething()
    {
        Debug.Log("Singleton is doing something.");
    }
}