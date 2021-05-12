using AbilitySystem;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEffectPair {

    public GameplayEffect Effect;
    public UnityEvent<Transform> Callback;

    public AnimationEffectPair(GameplayEffect effect, UnityEvent<Transform> callback) {
        Effect = effect;
        Callback = callback;
    }
}
