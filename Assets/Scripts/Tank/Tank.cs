using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tank {
  public class Tank : MonoBehaviour {
    [SerializeField]
    private GameObject leftWheel;

    [SerializeField]
    private GameObject rightWheel;

    [SerializeField]
    private GameObject head;

    private Dictionary<TankChildrens, Animator> animators = new();

    private Dictionary<TankChildrens, SpriteRenderer> spriteRenderers = new();

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float rotateSpeed = 50f;

    private float currentMoveSpeed = 0f;
    private float currentRotateSpeed = 0f;


    private void Awake() {
      animators.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<Animator>());
      animators.Add(TankChildrens.RightWheel, rightWheel.GetComponent<Animator>());
      spriteRenderers.Add(TankChildrens.Body, gameObject.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.Head, head.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.LeftWheel, leftWheel.GetComponent<SpriteRenderer>());
      spriteRenderers.Add(TankChildrens.RightWheel, rightWheel.GetComponent<SpriteRenderer>());
    }

    private void Update() {
      MovingUpdate();
      AnimationUpdate();
    }

    private void MovingUpdate() {
      currentMoveSpeed = moveSpeed * Input.GetAxisRaw("Vertical");
      currentRotateSpeed = rotateSpeed * Input.GetAxisRaw("Horizontal");

      transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
      transform.Rotate(0f, 0f, -currentRotateSpeed * Time.deltaTime);
    }

    private void AnimationUpdate() {
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
  }
}