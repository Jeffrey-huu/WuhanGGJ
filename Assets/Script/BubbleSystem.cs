using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    public static BubbleSystem instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DoSomething()
    {
        Debug.Log("Singleton is doing something.");
    }
}