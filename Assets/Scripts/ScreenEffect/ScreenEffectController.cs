using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenEffect {
  public class ScreenEffectController : MonoBehaviour {
    private static ScreenEffectController instance { get; set; }

    private Image _image;
    
    public static Image image {
      get => instance._image;
      set {
        instance._image = value;
        instance.ChangeAlpha(startAlpha);
      }
    }

    private bool isActivating;

    private EffectType state;

    [CanBeNull]
    private Action tmpCallback;

    private float speed;

    private float _startAlpha = 0f;

    private Effect tmpEffect;

    private float tmpCallbackDelay;

    
    public static float startAlpha {
      get => instance._startAlpha;
      set => instance._startAlpha = value;
    }

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    private void Update() {
      if (isActivating) {
        switch (state) {
          case EffectType.FadeIn:
            if (_image.color.a > 0)
              ChangeAlpha(_image.color.a - (Time.deltaTime * speed)); 
            else
              End();
            break;

          case EffectType.FadeOut:
            if (_image.color.a < 1) 
              ChangeAlpha(_image.color.a + (Time.deltaTime * speed));
            else
              End();
            break;
        }
      }
    }
    
    private void ChangeAlpha(float value) {
      var tmpColor = _image.color;
      tmpColor.a = value;
      _image.color = tmpColor;
    }

    private void ShowInv(Effect effect, [CanBeNull] Action callback = null, float callbackDelay = 0f) {
      tmpEffect = effect;
      tmpCallback = callback;
      tmpCallbackDelay = tmpCallbackDelay;
      Invoke("Show", effect.delay);
    }

    private void Show() {
      if (!isActivating) {
        var activeAnim = true;
        Color tmpColor;

        speed = tmpEffect.speed;
        tmpColor = _image.color;

        switch (tmpEffect.type) {
          case EffectType.FadeIn:
            ChangeAlpha(1f);
            break;

          case EffectType.FadeOut:
            ChangeAlpha(0f);
            break;

          case EffectType.ImmediatelyOut:
            ChangeAlpha(1f);
            activeAnim = false;
            break;
          
          case EffectType.ImmediatelyIn:
            ChangeAlpha(0f);
            activeAnim = false;
            break;

          default:
            throw new NotImplementedException();
        }

        state = tmpEffect.type;
        isActivating = activeAnim;

        if (tmpEffect.type == EffectType.ImmediatelyOut || tmpEffect.type == EffectType.ImmediatelyIn)
          End();
      }
    }

    private void End() {
      isActivating = false;
      Invoke("InvokeCallback", tmpCallbackDelay);
    }
    
    private void InvokeCallback() => tmpCallback?.Invoke();

    private void ForceExit() {
      if (isActivating) {
        isActivating = false;

        switch (state) {
          case EffectType.FadeIn:
            ChangeAlpha(0f);
            break;

          case EffectType.FadeOut:
            ChangeAlpha(1f);
            break;
        }

        End();
      }
    }

    public static void ShowEffect(Effect effect, [CanBeNull] Action callback = null, float callbackDelay = 0f) =>
      instance.ShowInv(effect, callback);

    public static void ExitEffect() => instance.ForceExit();

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
  }
}