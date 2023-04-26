using System;
using System.Collections;
using System.Collections.Generic;
using ScreenEffect;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene {
  public class SceneController : MonoBehaviour {
    private static SceneController instance { get; set; }

    public Image image;

    private string changeSceneName;

    private Effect afterEffectTemp;

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    private void Change(string name) {
      changeSceneName = name;
      LoadScene();
    }

    private void ChangeWithEffect(string name, Effect beforeEffect, Effect afterEffect, float changeDelay = 0f) {
      changeSceneName = name;
      afterEffectTemp = afterEffect;

      ScreenEffectController.ShowEffect(beforeEffect, () =>
        StartCoroutine(LoadSceneWithEffect(afterEffectTemp)), changeDelay);
    }

    private void LoadScene() => SceneManager.LoadScene(changeSceneName);

    private IEnumerator LoadSceneWithEffect(Effect effect) {
      // 참고: https://stackoverflow.com/questions/52722160/in-unity-after-loadscene-is-there-common-way-to-wait-all-monobehaviourstart-t
      // Start loading the scene
      AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(changeSceneName, LoadSceneMode.Single);
      // Wait until the level finish loading
      while (!asyncLoadLevel.isDone)
        yield return null;
      // Wait a frame so every Awake and Start method is called
      yield return new WaitForEndOfFrame();
      ScreenEffectController.ShowEffect(effect);
    }

    public static void ChangeScene(string name) => instance.Change(name);

    public static void ChangeSceneWithEffect
      (string name, Effect beforeEffect, Effect afterEffect, float changeDelay = 0f) =>
      instance.ChangeWithEffect(name, beforeEffect, afterEffect, changeDelay);

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
  }
}