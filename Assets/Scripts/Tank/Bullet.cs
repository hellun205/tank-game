using System;
using System.Linq;
using UnityEngine;

namespace Tank {
  public class Bullet : MonoBehaviour {
    public delegate void _onDisabled(Bullet sender);

    public delegate void _onEnabled(Bullet sender);

    public event _onDisabled OnDisabled;

    public event _onEnabled OnEnabled;
    
    public int damage = 1;
    public float speed = 2f;
    public string[] damageableTags;

    public bool AbleToShoot => !isShooting;

    public bool isShooting { get; private set; } = false;

    private CircleCollider2D circleCol2D;
    private SpriteRenderer sr;

    private void Awake() {
      circleCol2D = GetComponent<CircleCollider2D>();
      sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
      if (isShooting) {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
      }
    }

    private void OnTriggerEnter2D(Collider2D col) {
      if (damageableTags.Contains(col.tag)) {
        Disable();
      }
    }

    public void Disable() {
      OnDisabled?.Invoke(this);
      isShooting = false;
      circleCol2D.enabled = false;
      sr.enabled = false;
    }

    public void Enable() {
      OnEnabled?.Invoke(this);
      isShooting = true;
      circleCol2D.enabled = true;
      sr.enabled = true;
    }

    public void SetPosition(Vector3 position, Quaternion rotation) {
      transform.SetPositionAndRotation(position, rotation);
    }
  }
}