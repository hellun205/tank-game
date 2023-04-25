using System;
using System.Linq;
using Camera;
using Effect;
using Maze;
using Unity.VisualScripting;
using UnityEngine;

namespace Tank {
  public class Player: Tank {
    public static Player Instance { get; private set; }
    
    [SerializeField]
    protected float rotateSpeed = 50f;
    
    [SerializeField]
    private KeyCode fireKey = KeyCode.Space;
    
    [SerializeField]
    private KeyCode warpKey = KeyCode.Space;
    
    public bool canWarp = true;

    private void Awake() {
      base.Awake();
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
    }

    private void Start() {
      base.Start();
      MainCameraController.Instance.target = transform;
      MazeController.Instance.tank = this;
      
      MazeController.Instance.Move(0);
      EffectController.Instance.Black();
    }

    private void FixedUpdate() {
      base.FixedUpdate();
      MovingUpdate();
    }
    
    private void Update() {
      base.Update();
      ShootingUpdate();
      WarpUpdate();
    }
    
    private void MovingUpdate() {
      currentMoveSpeed = moveSpeed * Input.GetAxisRaw("Vertical");
      currentRotateSpeed = rotateSpeed * Input.GetAxisRaw("Horizontal");

      transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
      transform.Rotate(0f, 0f, -currentRotateSpeed * Time.deltaTime);
    }
    
    private void ShootingUpdate() {
      if (Input.GetKeyDown(fireKey)) Fire();
    }

    private void WarpUpdate() {
      if (!Input.GetKeyDown(warpKey)) return;
      var room = MazeController.Instance.GetCurrentRoom();
      
      if (canWarp && room.Tilemap.WorldToCell(transform.position) == room.EndPosition) {
        canWarp = false;
        MazeController.Instance.Next();
      }
    }

    private void OnCollisionEnter2D(Collision2D other) {
      base.OnCollisionEnter2D(other);
      if (hp <= 0) {
        Debug.Log("dead");
      }
    }

    private void OnDestroy() {
      base.OnDestroy();
    }
  }
}