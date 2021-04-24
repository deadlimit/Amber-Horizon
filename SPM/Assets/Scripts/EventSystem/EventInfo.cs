using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;

namespace EventCallbacks
{
    public abstract class EventInfo
    {
        //grundinfo

        public string EventDescription; 
    }
    public class DebugEventInfo : EventInfo
    {
        public int VerbosityLevel;
        new public string EventDescription = "DebugEventInfo:: ";
    }
    public class UnitDeathEventInfo : EventInfo
    {
        public GameObject UnitGO; 
    }

    public class UnitClickedEventInfo : EventInfo
    {

    }
    public class KeyPickUpEvent : EventInfo { }
    public class UnlockEvent : EventInfo { }

    public class CameraFocusEvent : EventInfo {

        public readonly Transform newFocusTarget;

        public CameraFocusEvent(Transform newTarget) {
            newFocusTarget = newTarget;
        }
        
    }

    public class PlayerHitEvent : EventInfo {

        public readonly Transform enemyTransform;
        public GameplayAbility ability;
        public PlayerHitEvent(Transform enemyTransform, GameplayAbility ability) {
            this.enemyTransform = enemyTransform;
            this.ability = ability;
        }

    }

    public class ExplosionEvent : EventInfo
    {
        public Vector3 location;
        public ExplosionEvent(Vector3 location)
        {
            this.location = location; 
        }
    }
    
}

