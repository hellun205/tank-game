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
using Effect = ScreenEffect.Effect;

namespace Maze {
  public class MazeController : MonoBehaviour {
    public static MazeController instance { get; set; }

    public Player tank;

    public Room[] rooms;

    public Information[] informations;

    public int currentIndex { get; private set; }

    public Room GetCurrentRoom() => rooms[currentIndex];

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);

      AllDisable();
      ScreenEffectController.startAlpha = 1f;
    }

    private void AllDisable() {
      foreach (var room in rooms) {
        room.Tilemap.gameObject.SetActive(false);
      }
    }

    public void Move(int index) {
      if (index > 0)
        ScreenEffectController.ShowEffect(new Effect(EffectType.FadeOut, 0.7f, 0.3f), () => PrepareMap(index));
      else
        PrepareMap(0);
    }

    private void PrepareMap(int index) {
      AllDisable();
      currentIndex = index;
      SetText(InfoType.Stage, $"Stage: {currentIndex + 1}");
      var room = GetCurrentRoom();
      room.Tilemap.gameObject.SetActive(true);
      tank.transform.SetPositionAndRotation(room.Tilemap.CellToWorld(room.StartPosition) +
                                            new Vector3(0.5f, 0.5f, 0f), room.StartDirection.ToQuaternion());

      Invoke("AfterPrepare", 0.5f);
    }

    private void AfterPrepare() {
      ScreenEffectController.ShowEffect(new Effect(EffectType.FadeIn, 0.7f));
      tank.canWarp = true;
    }

    public void Next() {
      if (currentIndex + 1 < rooms.Length) {
        Move(currentIndex + 1);
      } else {
        SceneController.ChangeSceneWithEffect("End", new Effect(EffectType.FadeOut, 0.6f),
          new Effect(EffectType.FadeIn, 0.6f), 0.3f);
      }
    }

    public void SetText(InfoType type, string text) {
      var info = informations.FirstOrDefault(x => x.Type == type);

      info.TMP?.SetText(text);
    }

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
  }
}