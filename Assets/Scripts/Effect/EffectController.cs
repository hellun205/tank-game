using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Effect {
  public class EffectController : MonoBehaviour {
    public static EffectController Instance { get; private set; }

    public Image image;

    public bool IsActivating { get; private set; }

    public Effects State { get; private set; }

    private Color color;
    
    [CanBeNull]
    private Action callBackTemp;

    private float fadeSpeed;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Update() {
      if (IsActivating) {
        switch (State) {
          case Effects.FadeIn:
            color = image.color;
            if (color.a > 0) {
              color.a -= Time.deltaTime * fadeSpeed;
            } else {
              End();
            }

            image.color = color;
            break;

          case Effects.FadeOut:
            color = image.color;
            if (color.a < 1) {
              color.a += Time.deltaTime * fadeSpeed;
            } else {
              End();
            }

            image.color = color;
            break;
        }
      }
    }

    public void Effect(Effects type, float speed = 1f, [CanBeNull] Action callBack = null) {
      if (!IsActivating) {
        fadeSpeed = speed;
        callBackTemp = callBack;
        Color tmpColor;
        switch (type) {
          case Effects.FadeIn:
            tmpColor = image.color;
            tmpColor.a = 1f;
            image.color = tmpColor;
            break;
          case Effects.FadeOut:
            tmpColor = image.color;
            tmpColor.a = 0f;
            image.color = tmpColor;
            break;
        }

        IsActivating = true;
        State = type;
      }
    }

    public void Black() {
      var tmpColor = image.color;
      tmpColor.a = 1f;
      image.color = tmpColor;
    }

    private void End() {
      IsActivating = false;
      State = Effects.None;
      callBackTemp?.Invoke();
    }
  }
}