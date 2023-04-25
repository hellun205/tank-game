using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tank {
  public class Demo : MonoBehaviour {
    public Animator[] animators;

    private void Start() {
      foreach (var anim in animators) {
        anim.SetInteger("Direction", 1);
        anim.SetFloat("Speed", 3f);
      }
    }
  }
}