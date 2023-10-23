
using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] private float delayTime;
    private List<Wall> _listWalls = new List<Wall>();

    private float _time;

    private void Update()
    {
        if (Player.list.Count <= 0) return;
        _time += Time.deltaTime;

        if (_time >= delayTime)
        {
            _listWalls.Add(Wall.Spawn(Wall.CurId++, spawnPoint.position));
            _time -= delayTime;
        }
    }

    public void DeleteWall(Wall wall)
    {
        _listWalls.Remove(wall);
    }

    public void DeleteAll()
    {
        foreach (Wall wall in _listWalls)
        {
            DestroyImmediate(wall);
        }
    }
}