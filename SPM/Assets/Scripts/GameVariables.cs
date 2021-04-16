using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class GameVariables : ScriptableObject
{
    [SerializeField] private int keyFragments;
    [SerializeField] private bool dashAvailable;
    private int requiredKeyNumber = 3;

    private void OnEnable()
    {
        //ingen aning om den wipas vid rätt tillfälle här, vill ha persistent data men egentligen inte för just denna variabel
        keyFragments = 0;
    }
    public bool RequiredNoOfKeys() { return ++keyFragments >= requiredKeyNumber; }
    public void EnableDash() { dashAvailable = true; }
}
