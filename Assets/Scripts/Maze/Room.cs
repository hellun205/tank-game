using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Maze {
  [System.Serializable]
  public class Room {
    public Tilemap tilemap;

    public Direction startDirection { get; set; } = Direction.Up;
    
    public Vector3Int startPosition { get; set; }
    
    public Vector3Int[] endPositions { get; set; }

    public bool isReady { get; set; } = false;
    
    public MazeStage mazeStage { get; set; }

    public Room(MazeStage mazeStage,Tilemap tilemap, Direction startDirection) {
      this.mazeStage = mazeStage;
      this.tilemap = tilemap;
      this.startDirection = startDirection;
    }
  }
}