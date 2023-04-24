using System;
using System.Collections;
using System.Collections.Generic;
using Effect;
using UnityEngine;

namespace Maze {
  public class MazeController : MonoBehaviour {
    public static MazeController Instance { get; private set; }

    public Tank.Tank tank;

    public Room[] rooms;

    public int currentIndex { get; private set; }

    public Room GetCurrentRoom() => rooms[currentIndex];

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);

      AllDisable();
    }

    private void AllDisable() {
      foreach (var room in rooms) {
        room.Tilemap.gameObject.SetActive(false);
      }
    }

    public void Move(int index) {
      EffectController.Instance.Black();
      AllDisable();
      currentIndex = index;
      var room = GetCurrentRoom();
      room.Tilemap.gameObject.SetActive(true);
      tank.transform.SetPositionAndRotation(room.Tilemap.CellToWorld(room.StartPosition) + new Vector3(0.5f, 0.5f, 0f),
        room.StartDirection.GetQuaternion());

      IEnumerator StartAnim() {
        yield return new WaitForSeconds(0.5f);
        EffectController.Instance.Effect(Effects.FadeIn);
        tank.canWarp = true;
      }

      StartCoroutine(StartAnim());
    }

    public void Next() {
      IEnumerator StartAnim() {
        yield return new WaitForSeconds(1.2f);
        EffectController.Instance.Effect(Effects.FadeOut, callBack: () => Move(currentIndex + 1));
      }

      StartCoroutine(StartAnim());
    }
  }
}