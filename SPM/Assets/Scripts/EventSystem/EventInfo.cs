using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EventCallbacks
{
    public abstract class EventInfo
    {
        //grundinfo

        public string EventDescription; 
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

    public class AbilityUsed : EventInfo {
        public readonly GameplayAbility ability;

        public AbilityUsed(GameplayAbility gameplayAbility) {
            ability = gameplayAbility;
        }
    }
    
    public class PlayerDiedEvent : EventInfo
    {
        public PlayerController player { get; }
        public PlayerDiedEvent(GameObject player)
        {
            this.player = player.GetComponent<PlayerController>();
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

    //Själva scenetransitionen
    public class SceneTransitEvent : EventInfo {

        public readonly string Scene;

        public SceneTransitEvent(string sceneName) {
            Scene = sceneName;
        }

    }
    
    //För scene-transit animationer och dylikt
    public class StartSceneTransitEvent : EventInfo {
        public readonly string Scene;

        public StartSceneTransitEvent(string sceneName) {
            Scene = sceneName;
        }
    }

    public class CheckPointActivatedEvent : EventInfo {
        public readonly AudioClip audio;

        public CheckPointActivatedEvent(AudioClip audio) {
            this.audio = audio;
        }
    }

    public class InteractTriggerEnter : EventInfo {
        public string UIMessage;

        public InteractTriggerEnter(string message) {
            UIMessage = message;
        }
    }
    public class InteractTriggerExit : EventInfo {}
    
    public class EnterTransitView : EventInfo {}
}

