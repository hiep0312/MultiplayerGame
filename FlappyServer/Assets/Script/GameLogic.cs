

using UnityEngine;

public class GameLogic : Singleton<GameLogic>
{
    public GameObject PlayerPrefab => playerPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
}