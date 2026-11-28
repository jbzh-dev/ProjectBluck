using UnityEngine;
using UnityEngine.UI;

public class TitleLightFlicker : MonoBehaviour
{
    public Image Image_light;          // ปล่อยว่างได้ ถ้าสคริปต์อยู่บน Image เอง
    [Range(0f, 1f)] public float minAlpha = 0.3f;
    [Range(0f, 1f)] public float maxAlpha = 1f;
    public float speed = 2f;          // ยิ่งมากยิ่งกระพริบเร็ว

    void Awake()
    {
        if (Image_light == null) Image_light = GetComponent<Image>();
    }

    void Update()
    {
        // Sin วิ่งระหว่าง -1 ถึง 1 แล้วแปลงเป็น 0 ถึง 1
        float t = (Mathf.Sin(Time.unscaledTime * speed) + 1f) / 2f;
        Color c = Image_light.color;
        c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        Image_light.color = c;
    }
}