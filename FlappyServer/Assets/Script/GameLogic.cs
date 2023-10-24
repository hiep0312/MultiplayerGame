

using UnityEngine;

public class GameLogic : Singleton<GameLogic>
{
    public static bool IsPlaying = true;
    
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject WallPrefab => wallPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject wallPrefab;
    
}