using Scene;
using ScreenEffect;
using UnityEngine;

namespace Main {
  public class MainScene : MonoBehaviour {
    public void LoadMaze() {
      ScreenEffectController.startAlpha = 1f;
      SceneController.ChangeSceneWithEffect("Maze", new Effect(EffectType.FadeOut, 0.6f),
        new Effect(EffectType.ImmediatelyOut), 0.3f);
    }
  }
}