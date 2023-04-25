﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maze {
  public class EnemySpawner : MonoBehaviour {
    public static List<GameObject> Enemies = new List<GameObject>();
    public GameObject enemy;
    private Tilemap tm;
    
    private void Awake() {
      tm = GetComponent<Tilemap>();
      if (Enemies.Count > 0) {
        foreach (var enemy in Enemies) {
          Destroy(enemy);
        }
        Enemies.Clear();
      }
    }

    private void Start() {
      foreach(var pos in tm.cellBounds.allPositionsWithin)
      {
        if(!tm.HasTile(pos)) continue;
        
        var tile = tm.GetTile<TileBase>(pos);
        if (tile.name == "spawn_enemy") {
          Enemies.Add(Instantiate(enemy, tm.CellToWorld(pos) + new Vector3(0.5f, 0.5f, 0f), enemy.transform.rotation));
        }
      }
    }
  }
}