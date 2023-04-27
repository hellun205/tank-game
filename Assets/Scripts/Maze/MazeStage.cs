using System;
using System.Collections;
using System.Collections.Generic;
using ScreenEffect;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Utils;

namespace Maze {
  public class MazeStage : MonoBehaviour {
    public static List<GameObject> Enemies = new List<GameObject>();
    
    [SerializeField]
    private GameObject enemy;
    
    public Direction startDirection = Direction.Up;

    public Tilemap tilemap { get; private set; }

    private void Awake() {
      tilemap = GetComponent<Tilemap>();
    }
    
    private void Start() {
      if (Enemies.Count > 0) {
        foreach (var enemy in Enemies) {
          Destroy(enemy);
        }

        Enemies.Clear();
      }
      tilemap.FindTilesAndAction(tile => tile.name == "spawn_enemy", tile => {
        Enemies.Add(Instantiate(enemy, tilemap.CellToWorld(tile.position) + new Vector3(0.5f, 0.5f, 0f),
               enemy.transform.rotation));
      });
    }
  }
}