using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 12f;

    [Header("Vertical Wave (ขยับขึ้นลง)")]
    [Range(0f, 1f)]
    [SerializeField] private float waveChance = 0.4f;      // โอกาสที่ตัวนี้จะขยับขึ้นลง (0.4 = 40%)
    [SerializeField] private Vector2 amplitudeRange = new Vector2(0.5f, 2f);   // ระยะขึ้นลง
    [SerializeField] private Vector2 frequencyRange = new Vector2(1f, 3f);     // ความถี่ (ยิ่งมากยิ่งขยับถี่)

    [Header("Cleanup")]
    [SerializeField] private float destroyOffset = 2f;

    private Vector2 moveDirection = Vector2.left;
    private Camera mainCamera;

    private float speed;
    private bool useWave;
    private float amplitude;
    private float frequency;
    private float phase;

    private Vector2 basePosition; // ตำแหน่งตามเส้นทางหลัก (ยังไม่รวมการขยับขึ้นลง)

    private void Awake()
    {
        speed = Random.Range(minSpeed, maxSpeed);

        useWave = Random.value < waveChance;
        if (useWave)
        {
            amplitude = Random.Range(amplitudeRange.x, amplitudeRange.y);
            frequency = Random.Range(frequencyRange.x, frequencyRange.y);
            phase = Random.Range(0f, Mathf.PI * 2f); // เฟสสุ่ม ไม่ให้ทุกตัวขยับพร้อมกัน
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        basePosition = transform.position;
    }

    // ถูกเรียกจาก SpawnManager ตอนเกิด
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // *ถ้า Sprite ต้นฉบับหันขวา ให้สลับเครื่องหมาย > เป็น <
        if (moveDirection.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void Update()
    {
        // เดินไปตามเส้นทางหลัก
        basePosition += moveDirection * speed * Time.deltaTime;

        // บวกการขยับขึ้นลงทับลงไป (ถ้าตัวนี้เป็นตัวที่ขยับ)
        float waveOffset = useWave
            ? Mathf.Sin(Time.time * frequency + phase) * amplitude
            : 0f;

        transform.position = new Vector3(
            basePosition.x,
            basePosition.y + waveOffset,
            transform.position.z);

        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        float x = transform.position.x;

        if (moveDirection.x > 0 && x > mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + destroyOffset)
        {
            Destroy(gameObject);
        }
        else if (moveDirection.x < 0 && x < mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}