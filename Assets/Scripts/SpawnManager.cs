using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    
    [Header("ตั้งค่าสีศัตรู")]
    [Tooltip("ใส่สีที่ต้องการให้สุ่มใน Inspector (ควรใส่ 2 สีขึ้นไป)")]
    [SerializeField] private Color[] enemyColors; 
    
    [Tooltip("ระยะห่างจากขอบจอออกไปด้านนอก (หน่วยเป็น World Unit)")]
    [SerializeField] private float spawnOffset = 1f; 
    
    private Camera mainCamera;
    private int lastColorIndex = -1; // เก็บ Index สีล่าสุดเพื่อกันซ้ำ
    private float spawnTimer;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("SpawnManager requires a camera tagged MainCamera.", this);
            enabled = false;
            return;
        }

        if (enemyPrefab == null || enemyPrefab.Length == 0)
        {
            Debug.LogError("SpawnManager requires at least one enemy prefab.", this);
            enabled = false;
            return;
        }

        if (spawnInterval <= 0f)
        {
            Debug.LogError("Spawn interval must be greater than zero.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemyAtEdge();
        }
    }

    private void SpawnEnemyAtEdge()
    {
        // 1. สุ่มฝั่งซ้ายหรือขวา
        bool spawnLeft = Random.value < 0.5f;
        float viewportX = spawnLeft ? 0f : 1f;

        // 2. สุ่มความสูง
        float viewportY = Random.Range(0.05f, 0.95f);

        // 3. แปลงพิกัดหน้าจอเป็น World Space
        float distanceFromCamera = Mathf.Abs(mainCamera.transform.position.z);
        Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(viewportX, viewportY, distanceFromCamera));

        // 4. ขยับออกนอกขอบจอ
        spawnPosition.x += spawnLeft ? -spawnOffset : spawnOffset;
        spawnPosition.z = 0f;

        // 5. สร้างศัตรู
        int enemyIndex = Random.Range(0, enemyPrefab.Length);
        GameObject spawnedEnemy = Instantiate(enemyPrefab[enemyIndex], spawnPosition, enemyPrefab[enemyIndex].transform.rotation);

        // 6. เปลี่ยนสีศัตรูแบบไม่ซ้ำตัวล่าสุด
        ApplyUniqueRandomColor(spawnedEnemy);

        // 7. ส่งทิศทางให้ EnemyMove
        if (spawnedEnemy.TryGetComponent<EnemyMove>(out EnemyMove enemyMove))
        {
            Vector2 dir = spawnLeft ? Vector2.right : Vector2.left;
            enemyMove.SetDirection(dir);
        }
    }

    private void ApplyUniqueRandomColor(GameObject enemy)
    {
        // เช็กว่ามีสีให้เลือกใน Array และตัวศัตรูมี SpriteRenderer
        if (enemyColors != null && enemyColors.Length > 0 && enemy.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            int newColorIndex = Random.Range(0, enemyColors.Length);

            // ถ้ามีสีให้เลือกมากกว่า 1 สี และสุ่มได้ Index เดิม ให้สุ่มใหม่จนกว่าจะเปลี่ยน
            if (enemyColors.Length > 1)
            {
                while (newColorIndex == lastColorIndex)
                {
                    newColorIndex = Random.Range(0, enemyColors.Length);
                }
            }

            lastColorIndex = newColorIndex; // จำค่าสีล่าสุดไว้
            sr.color = enemyColors[newColorIndex];
        }
    }

}