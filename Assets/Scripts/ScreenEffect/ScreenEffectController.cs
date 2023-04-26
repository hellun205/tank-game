using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenEffect {
  public class ScreenEffectController : MonoBehaviour {
    private static ScreenEffectController _instance { get; set; }

    private Image _image;
    
    public static Image image {
      get => _instance._image;
      set {
        _instance._image = value;
        _instance.ChangeAlpha(startAlpha);
      }
    }

    private bool _isActivating;

    private EffectType _state;

    [CanBeNull]
    private Action _tmpCallback;

    private float _speed;

    private float _startAlpha = 0f;

    private Effect _tmpEffect;

    private float _tmpCallbackDelay;

    
    public static float startAlpha {
      get => _instance._startAlpha;
      set => _instance._startAlpha = value;
    }

    private void Awake() {
      if (_instance == null) _instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    private void Update() {
      if (_isActivating) {
        switch (_state) {
          case EffectType.FadeIn:
            if (_image.color.a > 0)
              ChangeAlpha(_image.color.a - (Time.deltaTime * _speed)); 
            else
              End();
            break;

          case EffectType.FadeOut:
            if (_image.color.a < 1) 
              ChangeAlpha(_image.color.a + (Time.deltaTime * _speed));
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
      _tmpEffect = effect;
      _tmpCallback = callback;
      _tmpCallbackDelay = _tmpCallbackDelay;
      Invoke("Show", effect.delay);
    }

    private void Show() {
      if (!_isActivating) {
        var activeAnim = true;
        Color tmpColor;

        _speed = _tmpEffect.speed;
        tmpColor = _image.color;

        switch (_tmpEffect.type) {
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

        _state = _tmpEffect.type;
        _isActivating = activeAnim;

        if (_tmpEffect.type == EffectType.ImmediatelyOut || _tmpEffect.type == EffectType.ImmediatelyIn)
          End();
      }
    }

    private void End() {
      _isActivating = false;
      Invoke("InvokeCallback", _tmpCallbackDelay);
    }
    
    private void InvokeCallback() => _tmpCallback?.Invoke();

    private void ForceExit() {
      if (_isActivating) {
        _isActivating = false;

        switch (_state) {
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
      _instance.ShowInv(effect, callback);

    public static void ExitEffect() => _instance.ForceExit();
  }
}