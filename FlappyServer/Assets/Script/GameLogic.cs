

using UnityEngine;

public class GameLogic : Singleton<GameLogic>
{
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject WallPrefab => wallPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject wallPrefab;
    
}