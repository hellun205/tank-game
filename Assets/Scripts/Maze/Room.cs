using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maze {
  [System.Serializable]
  public class Room {
    public Tilemap Tilemap;
    
    public Vector3Int StartPosition;
    
    public Direction StartDirection = Direction.Up;
    
    public Vector3Int EndPosition;
    
  }
}