using System;

namespace ScreenEffect {
  public struct Effect {
    public EffectType type;

    public float speed;

    public float delay;
    
    public Effect(EffectType type = EffectType.ImmediatelyIn, float speed = 0f, float delay = 0f) {
      this.type = type;
      this.speed = speed;
      this.delay = delay;
    }
  }
}