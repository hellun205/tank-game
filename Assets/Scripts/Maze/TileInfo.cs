using UnityEngine;
using UnityEngine.Tilemaps;

namespace Maze {
  public struct TileInfo {
    public TileBase tile;

    public Vector3Int position;

    public TileInfo(TileBase tile, Vector3Int position) {
      this.tile = tile;
      this.position = position;
    }
  }
}