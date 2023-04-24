using System;
using System.Linq;
using UnityEngine;

namespace Tank {
  [RequireComponent(typeof(CircleCollider2D))]
  public class BulletDisabler : MonoBehaviour {

    public float Range {
      get => circleCol2D.radius;
      set => circleCol2D.radius = value;
    }

    private CircleCollider2D circleCol2D;

    private void Awake() {
      circleCol2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (other.tag == "Bullet") {
        var bullet = other.GetComponent<Bullet>();
        
        bullet.Disable();
      }
    }
  }
}