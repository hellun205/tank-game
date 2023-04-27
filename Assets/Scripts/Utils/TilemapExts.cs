using System;
using System.Collections.Generic;
using Maze;
using UnityEngine.Tilemaps;

namespace Utils {
  public static class TilemapExts {
    public static TileInfo[] FindTiles(this Tilemap tm, Predicate<TileBase> predicate) {
      var tiles = new List<TileInfo>();
      
      foreach (var position in tm.cellBounds.allPositionsWithin) {
        if (!tm.HasTile(position)) continue;

        var tile = tm.GetTile<TileBase>(position);
        if (predicate.Invoke(tile)) tiles.Add(new TileInfo(tile, position));
      }

      return tiles.ToArray();
    }

    public static void FindTilesAndAction(this Tilemap tm, Predicate<TileBase> predicate, Action<TileInfo> action) {
      var tileArray = tm.FindTiles(predicate);

      foreach (var tile in tileArray) {
        action.Invoke(tile);
      }
    }
  }
}