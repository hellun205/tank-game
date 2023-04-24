using UnityEngine;

namespace Maze {
  public static class DirectionExts {
    public static Quaternion GetQuaternion(this Direction dir) {
      switch (dir) {
        default:
        case Direction.Up:
          return Quaternion.Euler(0f,0f,0f);
        case Direction.Down:
          return Quaternion.Euler(0f,0f,180f);
        case Direction.Left:
          return Quaternion.Euler(0f,0f,90f);
        case Direction.Right:
          return Quaternion.Euler(0f,0f,270f);
      }
    }
  }
}