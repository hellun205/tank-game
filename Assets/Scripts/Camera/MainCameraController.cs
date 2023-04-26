using System;
using UnityEngine;

namespace Camera {
  public class MainCameraController : MonoBehaviour {
    public static MainCameraController instance { get; private set; }

    public Transform target;

    public float smoothing = 0.05f;

    public bool _enabled = true;

    private void Awake() {
      if (instance == null) instance = this;
      else Destroy(gameObject);
    }

    private void FixedUpdate() {
      if (_enabled) {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
      }
    }

    private void OnDestroy() {
      if (instance == this) instance = null;
    }
  }
}