using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image fill;

    private int maxHealth;

    private void Awake()
    {
        if (fill == null)
        {
            Debug.LogError("HUD requires a fill Image.", this);
        }
    }

    private void OnEnable()
    {
        GameController.OnPlayerSpawned += SetupHealthbar;
        PlayerHealth.OnPlayerTakeDamage += UpdateBar;
    }

    private void OnDisable()
    {
        GameController.OnPlayerSpawned -= SetupHealthbar;
        PlayerHealth.OnPlayerTakeDamage -= UpdateBar;
    }

    private void SetupHealthbar(GameObject playerObject)
    {
        if (playerObject == null || !playerObject.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            Debug.LogError("HUD could not find PlayerHealth on the spawned player.", this);
            return;
        }

        maxHealth = playerHealth.maxHealth;
        UpdateBar(playerHealth.currentHealth);
    }

    private void UpdateBar(int currentHealth)
    {
        if (fill != null)
        {
            fill.fillAmount = maxHealth > 0 ? Mathf.Clamp01((float)currentHealth / maxHealth) : 0f;
        }
    }
}
