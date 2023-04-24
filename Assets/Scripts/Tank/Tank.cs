using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tank {
  public class Tank : MonoBehaviour {
    [Header("Childrens Setting")]
    [SerializeField]
    private GameObject leftWheel;

    [SerializeField]
    private GameObject rightWheel;

    [SerializeField]
    private GameObject head;
    
    [Header("Tank Setting")]
    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float rotateSpeed = 50f;

    [SerializeField]
    private KeyCode fireKey = KeyCode.Space;
    
    [Header("Bullet Setting")]
    [SerializeField]
    private Bullet bulletObject;
    
    [SerializeField]
    private int bulletMaxCount = 5;

    [SerializeField]
    private Transform bulletStartPosition;

    [SerializeField]
    private float bulletSpeed = 3f;
    
    [SerializeField]
    private int bulletDamage = 1;

    private Dictionary<TankChildrens, Animator> animators = new();
    private Dictionary<TankChildrens, SpriteRenderer> spriteRenderers = new();
    
    private float currentMoveSpeed = 0f;
    private float currentRotateSpeed = 0f;

    private List<Bullet> bullets = new();

    private int canShootCount => bullets.Count(x => x.AbleToShoot);

    private void Awake() {
      animators.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<Animator>());
      animators.Add(TankChildrens.RightWheel, rightWheel.GetComponent<Animator>());
      spriteRenderers.Add(TankChildrens.Body, gameObject.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.Head, head.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.RightWheel, rightWheel.GetComponent<SpriteRenderer>());
      
      SetBulletMaxCount(bulletMaxCount);
    }

    private void Update() {
      MovingUpdate();
      AnimationUpdate();
      ShootingUpdate();
    }

    private void MovingUpdate() {
      currentMoveSpeed = moveSpeed * Input.GetAxisRaw("Vertical");
      currentRotateSpeed = rotateSpeed * Input.GetAxisRaw("Horizontal");

      transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
      transform.Rotate(0f, 0f, -currentRotateSpeed * Time.deltaTime);
    }

    private void AnimationUpdate() {
      animators[TankChildrens.LeftWheel].SetFloat("Speed", moveSpeed);
      animators[TankChildrens.RightWheel].SetFloat("Speed", moveSpeed);
      if (currentRotateSpeed > 0) {
        SetDirection(TankChildrens.LeftWheel, 1);
        SetDirection(TankChildrens.RightWheel, -1);
      } else if (currentRotateSpeed < 0) {
        SetDirection(TankChildrens.LeftWheel, -1);
        SetDirection(TankChildrens.RightWheel, 1);
      } else {
        SetDirection(TankChildrens.LeftWheel, (int) currentMoveSpeed);
        SetDirection(TankChildrens.RightWheel, (int) currentMoveSpeed);
      }
    }

    private void ShootingUpdate() {
      if (Input.GetKeyDown(fireKey) && canShootCount > 0) {
        var bullet = bullets.FirstOrDefault(x => x.AbleToShoot);
        
        ResetBullet(bullet);
        bullet.Enable();
      }
    }

    private void SetDirection(TankChildrens type, int value) => animators[type].SetInteger("Direction", value);

    public void SetColor(TankChildrens type, Color color) {
      switch (type) {
        case TankChildrens.All:
          foreach (var sr in spriteRenderers.Values) {
            sr.color = color;
          }

          break;

        case TankChildrens.AllWheel:
          spriteRenderers[TankChildrens.LeftWheel].color = color;
          spriteRenderers[TankChildrens.RightWheel].color = color;
          break;

        default:
          spriteRenderers[type].color = color;
          break;
      }
    }

    public void SetBulletMaxCount(int count) {
      if (bullets.Count > 0) {
        foreach (var bullet in bullets) {
          Destroy(bullet.gameObject);
        }
        bullets.Clear();
      }

      bulletMaxCount = count;
      for (var i = 0; i < bulletMaxCount; i++) {
        var obj = Instantiate(bulletObject);
        obj.Disable();
        ResetBullet(obj);
        obj.speed = bulletSpeed;
        obj.damage = bulletDamage;
        bullets.Add(obj);
      }
    }

    private void ResetBullet(Bullet bullet) {
      bullet.SetPosition(bulletStartPosition.position, transform.rotation);
    }
  }
}