using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main {
  public class ReButton : MonoBehaviour {
    private Button btn;

    private void Awake() {
      btn = GetComponent<Button>();
      btn.onClick.AddListener(() => {
        SceneManager.LoadScene("Maze");
      });
    }
  }
}