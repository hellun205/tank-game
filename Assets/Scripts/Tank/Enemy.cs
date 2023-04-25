using System;
using System.Collections;
using System.Linq;
using Maze;
using UnityEngine;

namespace Tank {
  public class Enemy: Tank {
    public bool isEnabled = false;

    public float fireSecond = 4f;
    
    private float angle;

    private void Awake() {
      base.Awake();
    }

    private void Start() {
      base.Start();
      StartCoroutine(FireCoroutine());
    }
    
    private void FixedUpdate() {
      base.FixedUpdate();
      MovingUpdate();
    }
    
    private void Update() {
      base.Update();
    }

    IEnumerator FireCoroutine() {
      start:
      yield return new WaitForSeconds(fireSecond);
      if (isEnabled) Fire();
      goto start;
    }
    
    private void MovingUpdate() {
      if (isEnabled) {
        currentMoveSpeed = moveSpeed;
        // currentRotateSpeed = rotateSpeed * Input.GetAxisRaw("Horizontal");
        
        // 플레이어 쫓기 (회전)
        angle = Mathf.Atan2(Player.Instance.transform.position.y - transform.position.y,
                  Player.Instance.transform.position.x - transform.position.x)
                * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, 0, angle - 90), 0.2f);

        transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
        transform.Rotate(0f, 0f, -currentRotateSpeed * Time.deltaTime);
      }
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (other.gameObject.tag == "Player") {
        isEnabled = false;
      }
    }

    private void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.tag == "Player") {
        isEnabled = true;
      }
    }

    private void OnCollisionEnter2D(Collision2D col) {
      base.OnCollisionEnter2D(col);
      
      if (Hp <= 0) {
        Destroy(gameObject);
      }
    }

    private void OnDestroy() {
      base.OnDestroy();
    }
  }
}