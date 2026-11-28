using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefeb;
    private GameObject player;
    public static Action<GameObject> OnPlayerSpawned;

    private void Awake()
    {
        player = Instantiate(playerPrefeb);
    }

    private void Start()
    {
        OnPlayerSpawned?.Invoke(player);
    }

}
