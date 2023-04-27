using System;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenEffect {
  [RequireComponent(typeof(Image))]
  public class EffectImage : MonoBehaviour {
    private static EffectImage instance { get; set; }
    private Image image;
    
    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);
      
      image = GetComponent<Image>();

      image.color = new Color(0f, 0f, 0f, 0f);
      ScreenEffectController.image = image;
    }

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
  }
}