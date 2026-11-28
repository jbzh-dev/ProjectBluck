using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float destroyX = -30f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        if(transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}
