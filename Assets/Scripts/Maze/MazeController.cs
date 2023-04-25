using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Effect;
using Tank;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maze {
  public class MazeController : MonoBehaviour {
    public static MazeController Instance { get; set; }

    public Player tank;

    public Room[] rooms;

    public Information[] informations;

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
      SetText(InfoType.Stage, $"Stage: {currentIndex + 1}");
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
        yield return new WaitForSeconds(0.7f);
        EffectController.Instance.Effect(Effects.FadeOut, callBack: () => {
          if (currentIndex + 1 < rooms.Length) {
            Move(currentIndex + 1);
          } else {
            SceneManager.LoadScene("End");
          }
        });
      }

      StartCoroutine(StartAnim());
    }

    public void SetText(InfoType type, string text) {
      var info = informations.FirstOrDefault(x => x.Type == type);

      if (info.TMP != null) info.TMP.SetText(text);
    }
  }
}