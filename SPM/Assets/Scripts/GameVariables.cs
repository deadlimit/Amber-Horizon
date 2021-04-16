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
        //ingen aning om den wipas vid r�tt tillf�lle h�r, vill ha persistent data men egentligen inte f�r just denna variabel
        keyFragments = 0;
    }
    public bool RequiredNoOfKeys() { return ++keyFragments >= requiredKeyNumber; }
    public void EnableDash() { dashAvailable = true; }
}
