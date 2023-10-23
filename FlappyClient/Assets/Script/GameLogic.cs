using UnityEngine;

public class GameLogic : Singleton<GameLogic>
{
    public GameObject LocalPlayerPrefab => localPlayerPrefab;
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject WallPrefab => wallPrefab;
    

    [Header("Prefabs")]
    [SerializeField] private GameObject localPlayerPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject wallPrefab;
    
}