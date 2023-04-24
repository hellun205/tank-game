using System;
using UnityEngine;

namespace Camera {
  public class MainCameraController : MonoBehaviour {
    public static MainCameraController Instance { get; private set; }

    public Transform target;

    public float smoothing = 0.05f;

    public bool _enabled = true;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void FixedUpdate() {
      if (_enabled) {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
      }
    }
  }
}