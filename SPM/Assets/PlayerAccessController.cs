using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class PlayerAccessController : MonoBehaviour {
    
    [SerializeField] private List<MonoBehaviour> components;
    
    private void OnEnable() {
        foreach (MonoBehaviour script in components)
            script.enabled = false;
        EventSystem<ReturnPlayerControl>.RegisterListener(ReturnPlayerControl);
    }

    private void OnDisable() {
        EventSystem<ReturnPlayerControl>.UnregisterListener(ReturnPlayerControl);
    }

    private void ReturnPlayerControl(ReturnPlayerControl control) {
        foreach (MonoBehaviour script in components)
            script.enabled = true;
    }


}
