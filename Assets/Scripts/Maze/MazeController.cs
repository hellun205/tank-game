using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scene;
using ScreenEffect;
using Tank;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Effect = ScreenEffect.Effect;

namespace Maze {
  public class MazeController : MonoBehaviour {
    public static MazeController instance { get; set; }

    public const string EnemyPosition = "spawn_enemy";
    public const string StartPosition = "start_hole";
    public const string NextPosition = "next_stage_hole";

    private List<Room> _rooms { get; set; } = new List<Room>();

    [SerializeField]
    private MazeStage[] stages;

    [SerializeField]
    private Information[] informations;

    private int currentIndex;

    private Room GetCurrentRoom() => _rooms[currentIndex];

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);

      foreach (var stage in stages) {
        _rooms.Add(new Room(stage, stage.tilemap, stage.startDirection));
        Debug.Log($"stage load success: {stage.tilemap.gameObject.name}");
      }
      
      ScreenEffectController.startAlpha = 1f;
      AllDisable();
    }

    private void Start() {
      foreach (var room in _rooms) {
        Debug.Log(room.tilemap.gameObject.name);
      }
    }

    private void AllDisable() {
      foreach (var room in _rooms) {
        room.tilemap.gameObject.SetActive(false);
      }
    }

    private void Move(int index) {
      if (index > 0)
        ScreenEffectController.ShowEffect(new Effect(EffectType.FadeOut, 0.7f, 0.3f),
          () => PrepareMap(index));
      else
        PrepareMap(0);
    }

    private void PrepareMap(int index) {
      AllDisable();
      currentIndex = index;
      SetText(InfoType.Stage, $"Stage: {currentIndex + 1}");

      var room = GetCurrentRoom();
      room.tilemap.gameObject.SetActive(true);
      if (!room.isReady) {
        room.startPosition = room.tilemap.FindTiles(tile => tile.name == StartPosition)[0].position;
        room.endPositions = room.tilemap.FindTiles(tile => tile.name == NextPosition)
          .Select(tile => tile.position).ToArray();
        room.isReady = true;
      }
      
      room.mazeStage.SpawnEnemies();
      Player.instance.transform.SetPositionAndRotation(room.tilemap.CellToWorld(room.startPosition) +
                                             new Vector3(0.5f, 0.5f, 0f), room.startDirection.ToQuaternion());

      Invoke("AfterPrepare", 0.5f);
    }

    private void AfterPrepare() {
      ScreenEffectController.ShowEffect(new Effect(EffectType.FadeIn, 0.7f));
      Player.instance.canWarp = true;
    }

    private void Next() {
      if (currentIndex + 1 < _rooms.Count) {
        Move(currentIndex + 1);
      } else {
        SceneController.ChangeSceneWithEffect("End", new Effect(EffectType.FadeOut, 0.6f),
          new Effect(EffectType.FadeIn, 0.6f), 0.3f);
      }
    }

    private void SetText(InfoType type, string text) {
      var info = informations.FirstOrDefault(x => x.Type == type);

      info.TMP?.SetText(text);
    }

    private void OnDestroy() {
      if (instance == this) instance = null;
    }

    public static Room currentMap => instance.GetCurrentRoom();

    public static void MoveNextMap() => instance.Next();

    public static void MoveMap(int index) => instance.Move(index);

    public static int currentMapIndex => instance.currentIndex;

    public static List<Room> rooms {
      get => instance._rooms;
      set => instance._rooms = value;
    }
  }
}