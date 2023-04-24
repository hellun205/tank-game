using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour {
  private bool isActivating = true;
  private Color color;

  private Image image;

  private void Awake() {
    image = GetComponent<Image>();
  }

  private void Update() {
    if (isActivating) {
      color = image.color;

      if (color.a > 0) {
        color.a -= Time.deltaTime;
        image.color = color;
      } else {
        isActivating = false;
      }
    }
  }
}