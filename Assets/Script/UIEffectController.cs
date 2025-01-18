using UnityEngine;
using UnityEngine.UI;

public class UIEffectController : MonoBehaviour
{
    public float scaleSpeed = 0.5f; // 缩放的速度
    public float rotationSpeed = 20f; // 旋转的速度，降低到适合的程度
    public float shakeAngle = 10f; // 摇晃幅度
    public float minScale = 0.8f; // 最小缩放比例
    public float maxScale = 1f; // 最大缩放比例
    private Vector3 minScaleVec, maxScaleVec;

    private Vector3 initialPosition;
    private float time;

    void Start()
    {
        // 设置初始缩放值为某个特定的值
        minScaleVec = minScale * transform.localScale;
        maxScaleVec = maxScale * transform.localScale;
    }

    void Update()
    {
        // 放大缩小效果
        float scale = Mathf.PingPong(Time.time * scaleSpeed, 1f);  // 在0到1之间往返
        transform.localScale = Vector3.Lerp(minScaleVec, maxScaleVec, scale);

        // 摇晃效果（小幅度）
        time += Time.deltaTime * rotationSpeed;

        // 旋转效果（小幅度，避免360度旋转）
        float rotation = Mathf.Sin(time * 0.5f) * shakeAngle; // 小幅度旋转，最大旋转角度为±10度
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
