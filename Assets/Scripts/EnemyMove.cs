using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float destroyOffset = 2f; // ระยะเลยขอบจอก่อนทำลายตัวเอง

    private Vector2 moveDirection = Vector2.left;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // ฟังก์ชันนี้จะถูกเรียกจาก SpawnManager ตอนเกิด
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // กลับด้านตัวละคร (Flip) ให้หันหน้าไปทางที่เดิน (กรณี Sprite หันซ้ายเป็นค่าเริ่มต้น)
        // *หมายเหตุ: ถ้า Sprite ต้นฉบับของคุณหันขวา ให้สลับเครื่องหมาย > เป็น < ตรงบรรทัดล่างนี้ครับ
        if (moveDirection.x > 0) 
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void Update()
    {
        // เคลื่อนที่ตามทิศทางที่ได้รับมา
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        // แปลงตำแหน่งปัจจุบันของศัตรูเป็นพิกัดหน้าจอ (0 ถึง 1)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // ถ้าเดินไปทางขวา แล้วเลยขอบขวา (1) + ระยะเผื่อ -> ทำลาย
        if (moveDirection.x > 0 && transform.position.x > mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + destroyOffset)
        {
            Destroy(gameObject);
        }
        // ถ้าเดินไปทางซ้าย แล้วเลยขอบซ้าย (0) - ระยะเผื่อ -> ทำลาย
        else if (moveDirection.x < 0 && transform.position.x < mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - destroyOffset)
        {
            Destroy(gameObject);
        }
    }
}