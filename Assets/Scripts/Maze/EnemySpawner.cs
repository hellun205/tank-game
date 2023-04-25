using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maze {
  public class EnemySpawner : MonoBehaviour {
    public GameObject enemy;
    private Tilemap tm;
    
    private void Awake() {
      tm = GetComponent<Tilemap>();
    }

    private void Start() {
      foreach(var pos in tm.cellBounds.allPositionsWithin)
      {
        if(!tm.HasTile(pos)) continue;
        
        var tile = tm.GetTile<TileBase>(pos);
        if (tile.name == "spawn_enemy") {
          Instantiate(enemy, tm.CellToWorld(pos), enemy.transform.rotation);
        }
      }
    }
  }
}