using UnityEngine;
using UnityEngine.UI;

public class UIEffectController : MonoBehaviour
{
    public float scaleSpeed = 0.5f; // ���ŵ��ٶ�
    public float rotationSpeed = 20f; // ��ת���ٶȣ����͵��ʺϵĳ̶�
    public float shakeAngle = 10f; // ҡ�η���
    public float minScale = 0.8f; // ��С���ű���
    public float maxScale = 1f; // ������ű���
    private Vector3 minScaleVec, maxScaleVec;

    private Vector3 initialPosition;
    private float time;

    void Start()
    {
        // ���ó�ʼ����ֵΪĳ���ض���ֵ
        minScaleVec = minScale * transform.localScale;
        maxScaleVec = maxScale * transform.localScale;
    }

    void Update()
    {
        // �Ŵ���СЧ��
        float scale = Mathf.PingPong(Time.time * scaleSpeed, 1f);  // ��0��1֮������
        transform.localScale = Vector3.Lerp(minScaleVec, maxScaleVec, scale);

        // ҡ��Ч����С���ȣ�
        time += Time.deltaTime * rotationSpeed;

        // ��תЧ����С���ȣ�����360����ת��
        float rotation = Mathf.Sin(time * 0.5f) * shakeAngle; // С������ת�������ת�Ƕ�Ϊ��10��
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
