using System;
using System.Linq;
using UnityEngine;

namespace Tank {
  public class Bullet : MonoBehaviour {
    public int damage = 1;
    public float speed = 2f;
    public string[] damageableTags;

    public bool AbleToShoot => !IsShooting;

    public bool IsShooting { get; private set; } = false;

    private CircleCollider2D circleCol2D;
    private SpriteRenderer sr;

    private void Awake() {
      circleCol2D = GetComponent<CircleCollider2D>();
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      if (IsShooting) {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
      }
    }

    private void OnTriggerEnter2D(Collider2D col) {
      if (damageableTags.Contains(col.tag)) {
        Disable();
      }
    }

    public void Disable() {
      IsShooting = false;
      circleCol2D.enabled = false;
      sr.enabled = false;
    }

    public void Enable() {
      IsShooting = true;
      circleCol2D.enabled = true;
      sr.enabled = true;
    }

    public void SetPosition(Vector3 position, Quaternion rotation) {
      transform.SetPositionAndRotation(position, rotation);
    }
  }
}