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
        public readonly GameplayAbility ability;
 
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
        public readonly int ID;
        public CheckPointActivatedEvent(AudioClip audio, int ID) {
            this.audio = audio;
            this.ID = ID;
        }
    }

    public class InteractTriggerEnterEvent : EventInfo {
        public string UIMessage;

        public InteractTriggerEnterEvent(string message) {
            UIMessage = message;
        }
    }
    public class InteractTriggerExitEvent : EventInfo {}

    public class EnterTransitViewEvent : EventInfo {
        public readonly HashSet<TransitUnit> TransitUnits;
        public readonly TransitUnit ActivatedTransitUnit;
        public EnterTransitViewEvent(HashSet<TransitUnit> transitUnits, TransitUnit ActivatedTransitUnit) {
            TransitUnits = transitUnits;
            this.ActivatedTransitUnit = ActivatedTransitUnit;
        }
    }
    
    public class ExitTransitViewEvent : EventInfo {}

    public class SoundEffectEvent : EventInfo {
        public readonly AudioClip SFX;

        public SoundEffectEvent(AudioClip SFX) {
            this.SFX = SFX;
        }
    }

    public class EnterSlowMotionEvent : EventInfo {
        public readonly float duration;

        public EnterSlowMotionEvent(float duration) {
            this.duration = duration;
        }
    }

    public class DisplayUIMessage : EventInfo {
        public readonly string UIMessage;
        public readonly float duration;
        public readonly bool PlayUISFX;
        public DisplayUIMessage(string message, float duration, bool PlayUISFX) {
            UIMessage = message;
            this.duration = duration;
            this.PlayUISFX = PlayUISFX;
        }
    }
    
}

