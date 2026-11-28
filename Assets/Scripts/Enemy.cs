using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 3;
    
    public void HitPlayer()
    {
        FindAnyObjectByType<PlayerHealth>().DamagePlayer(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.DamagePlayer(damage);
        }
    }
}
