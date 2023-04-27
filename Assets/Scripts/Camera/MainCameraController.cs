using System;
using UnityEngine;

namespace Camera {
  public class MainCameraController : MonoBehaviour {
    private static MainCameraController instance { get; set; }

    private Transform _target;

    private float _smoothing = 0.05f;

    private bool _enabled = false;

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate() {
      if (_enabled) {
        var targetPos = new Vector3(_target.position.x, _target.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, _smoothing);
      }
    }

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
    
    public static Transform target {
      get => instance._target;
      set => instance._target = value;
    }

    public static float smoothing {
      get => instance._smoothing;
      set => instance._smoothing = value;
    }

    public static bool isEnabled {
      get => instance._enabled;
      set => instance._enabled = value;
    }
  }
}