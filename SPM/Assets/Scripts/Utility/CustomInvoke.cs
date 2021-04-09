using System;
using System.Collections;
using UnityEngine;

public static class CustomInvoke {

    public static void Invoke(this MonoBehaviour monoBehaviour, Action action, float delay) {
        monoBehaviour.StartCoroutine(Invoke(action, delay));
    }

    public static void Invoke(this MonoBehaviour monoBehaviour, Action action) {
        monoBehaviour.StartCoroutine(Invoke(action, 0));
    }
    
    private static IEnumerator Invoke(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action();
    }
    
}
