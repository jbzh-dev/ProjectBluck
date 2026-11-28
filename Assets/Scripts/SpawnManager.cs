using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyPrefab;
    [SerializeField] public InputAction spawnAction;
    
    [SerializeField] private float spawnX = 20f;
    [SerializeField] private float spawnRangeY = 20f;

    private void Start()
    {
        spawnAction.Enable();
    }

    private void Update()
    {
        if (spawnAction.triggered)
        {
            Vector3 spawnPosition = new Vector3(spawnX, Random.Range(-spawnRangeY, spawnRangeY), 0f);
            int enemyIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyIndex], spawnPosition, enemyPrefab[enemyIndex].transform.rotation);
        }
    }
}
