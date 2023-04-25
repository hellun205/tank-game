using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Camera;
using Effect;
using Maze;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Tank {
  public class Tank : MonoBehaviour {
    [Header("Childrens Setting")]
    [SerializeField]
    protected GameObject leftWheel;

    [SerializeField]
    protected GameObject rightWheel;

    [SerializeField]
    protected GameObject head;

    [Header("Tank Setting")]
    [SerializeField]
    protected float moveSpeed = 3f;

    [Header("Bullet Setting")]
    [SerializeField]
    protected Bullet bulletObject;

    [SerializeField]
    protected int bulletMaxCount = 5;

    [SerializeField]
    protected Transform bulletStartPosition;

    [SerializeField]
    protected float bulletSpeed = 3f;

    [SerializeField]
    protected int bulletDamage = 1;

    protected Dictionary<TankChildrens, Animator> animators = new();
    protected Dictionary<TankChildrens, SpriteRenderer> spriteRenderers = new();

    protected float currentMoveSpeed = 0f;
    protected float currentRotateSpeed = 0f;

    protected List<Bullet> bullets = new();

    public int maxHp = 10;

    private int hp;

    public virtual int Hp {
      get => hp;
      set => hp = value;
    }

    protected int CanShootCount => bullets.Count(x => x.AbleToShoot);

    private Rigidbody2D rb;

    protected void Awake() {
      rb = GetComponent<Rigidbody2D>();
      hp = maxHp;
      animators.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<Animator>());
      animators.Add(TankChildrens.RightWheel, rightWheel.GetComponent<Animator>());
      spriteRenderers.Add(TankChildrens.Body, gameObject.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.Head, head.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.RightWheel, rightWheel.GetComponent<SpriteRenderer>());

      SetBulletMaxCount(bulletMaxCount);
    }

    protected void Start() {
    }

    protected void FixedUpdate() {
      AnimationUpdate();
    }

    protected void Update() {
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
        SetDirection(TankChildrens.LeftWheel, (int)currentMoveSpeed);
        SetDirection(TankChildrens.RightWheel, (int)currentMoveSpeed);
      }
    }


    protected void SetDirection(TankChildrens type, int value) => animators[type].SetInteger("Direction", value);

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

    protected void ResetBullet(Bullet bullet) {
      bullet.SetPosition(bulletStartPosition.position, transform.rotation);
    }

    protected void OnCollisionEnter2D(Collision2D col) {
      if (col.gameObject.tag == "Bullet" && !bullets.Contains(col.gameObject.GetComponent<Bullet>())) {
        hp -= col.gameObject.GetComponent<Bullet>().damage;
        Debug.Log(hp);
      }
    }

    protected void Fire() {
      if (CanShootCount > 0) {
        var bullet = bullets.FirstOrDefault(x => x.AbleToShoot);

        ResetBullet(bullet);
        bullet.Enable();
      }
    }

    protected void OnDestroy() {
      foreach (var obj in bullets) {
        Destroy(obj.gameObject);
      }
    }
  }
}