using System.Linq;
using Camera;
using ScreenEffect;
using Maze;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Tank {
  public class Player : Tank {
    public static Player instance { get; private set; }

    [SerializeField]
    protected float _rotateSpeed = 50f;

    [SerializeField]
    private KeyCode _fireKey = KeyCode.Space;

    [SerializeField]
    private KeyCode warpKey = KeyCode.Space;

    public bool canWarp = true;

    [Header("Information")]
    [SerializeField]
    private Image hpBar;


    private void Awake() {
      base.Awake();
      if (instance == null) instance = this;
      else Destroy(gameObject);
    }

    private void Start() {
      base.Start();
      MainCameraController.instance.target = transform;
      // ScreenEffectController.ShowEffect(new Effect(EffectType.ImmediatelyOut));
      MazeController.playerTank = this;
      MazeController.MoveMap(0);
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
      currentRotateSpeed = _rotateSpeed * Input.GetAxisRaw("Horizontal");

      transform.Translate(Vector3.up * currentMoveSpeed * Time.deltaTime);
      transform.Rotate(0f, 0f, -currentRotateSpeed * Time.deltaTime);
    }

    private void ShootingUpdate() {
      if (Input.GetKeyDown(_fireKey)) Fire();
    }

    private void WarpUpdate() {
      if (!Input.GetKeyDown(warpKey) || !canWarp) return;

      var room = MazeController.currentMap;
      var playerPos = room.tilemap.WorldToCell(transform.position);
      
      if (room.endPositions.Contains(playerPos)) {
        canWarp = false;
        MazeController.MoveNextMap();
      }
    }

    private void OnCollisionEnter2D(Collision2D other) {
      base.OnCollisionEnter2D(other);

      hpBar.fillAmount = (float) Hp / maxHp;
      if (Hp <= 0) {
        SceneController.ChangeSceneWithEffect("Gameover", new Effect(EffectType.FadeOut, 2f),
          new Effect(EffectType.FadeIn, 0.6f), 0.2f);
      }
    }

    private void OnDestroy() {
      base.OnDestroy();
      if (instance == this) instance = null;
    }
  }
}