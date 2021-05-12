using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;
using UnityEngine.Rendering;


namespace EventCallbacks
{
    public abstract class EventInfo
    {
        //grundinfo

        public string EventDescription; 
    }
    
    public class KeyPickUpEvent : EventInfo { }
    public class UnlockEvent : EventInfo { }

    public class PlayerHitEvent : EventInfo {

        public readonly Transform culprit;
        public readonly GameplayEffect appliedEffect;
        public PlayerHitEvent(Transform culprit, GameplayEffect appliedEffect) {
            this.culprit = culprit;
            this.appliedEffect = appliedEffect;
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
        public readonly Checkpoint checkpoint;
        public readonly AudioClip audio;
        public CheckPointActivatedEvent(Checkpoint checkpoint, AudioClip audio) {
            this.checkpoint = checkpoint;
            this.audio = audio;
            
        }
    }

    public class InteractTriggerEnterEvent : EventInfo {
        public string UIMessage;

        public InteractTriggerEnterEvent(string message) {
            UIMessage = message;
        }
    }
    public class InteractTriggerExitEvent : EventInfo {}

    public struct TransitCameraFocusInfo {

    }
    
    public class EnterTransitViewEvent : EventInfo {

        public readonly HashSet<TransitUnit> TransitUnits;
        public readonly TransitUnit ActivatedTransitUnit;
        public EnterTransitViewEvent(HashSet<TransitUnit> transitUnits, TransitUnit activatedFrom) {
            TransitUnits = transitUnits;
            ActivatedTransitUnit = activatedFrom;
        }
    }

    public class NewCameraFocus : EventInfo {
        public readonly Transform Target;
        public readonly bool OrthographicView;
        public NewCameraFocus(Transform target, bool orhtographic) {
            Target = target;
            OrthographicView = orhtographic;
        }
    }

    public class ResetCameraFocus : EventInfo { }
    
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

