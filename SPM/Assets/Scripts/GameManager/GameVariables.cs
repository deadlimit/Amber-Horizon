using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class GameVariables : ScriptableObject
{
    [SerializeField] private int keyFragments;
    [SerializeField] private bool dashAvailable;
    [SerializeField] private int enemiesDefeated;
    private int requiredKeyNumber = 3;

    private void OnEnable()
    {
        keyFragments = 0;
    }
    public bool RequiredNoOfKeys() { 

        return ++keyFragments >= requiredKeyNumber; }
    public void EnableDash() { dashAvailable = true; }
}
