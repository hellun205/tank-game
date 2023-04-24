using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main {
  public class SceneController : MonoBehaviour {
    public Image image;

    private bool isActivating = false;
    private string temp;
    private Color color;

    private void Update() {
      if (isActivating) { 
        color = image.color;
        
        if (color.a < 1) {
          color.a += Time.deltaTime;
          image.color = color;
        } else {
          isActivating = false;
          IEnumerator StartAnim() {
            yield return new WaitForSeconds(0.7f);
            
            SceneManager.LoadScene(temp);
          }

          StartCoroutine(StartAnim());
        }
      }
    }
    
    public void ChangeScene(string name) {
      temp = name;
      isActivating = true;
    }
  }
}