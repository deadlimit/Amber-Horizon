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

    public class UnlockEvent : EventInfo {
        public readonly int ID;

        public UnlockEvent(int doorID) {
            ID = doorID;
        }
    }

    public class PlayerHitEvent : EventInfo {

        public readonly Transform culprit;
        public readonly GameplayEffect appliedEffect;
        public readonly PlayerController player;
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
        public HashSet<TransitUnit> TransitUnits;
        public TransitUnit ActivatedTransitUnit;
    }
    
    public class EnterTransitViewEvent : EventInfo {

        public readonly TransitCameraFocusInfo TransitCameraFocusInfo;
            
        public EnterTransitViewEvent(TransitCameraFocusInfo transitCameraFocusInfo) {
            TransitCameraFocusInfo = transitCameraFocusInfo;
        }
    }
    
    public class ResetCameraFocus : EventInfo {}
    
    public class LoadMainMenu : EventInfo {}
    public class ExitMainMenu : EventInfo {}
    
    
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
    
    public class PlayerReviveEvent : EventInfo 
    {
        public PlayerController player { get; }
        public PlayerReviveEvent(GameObject player)
        {
            this.player = player.GetComponent<PlayerController>();
        }
    }

    public class StartHitAnimationEvent : EventInfo
    {
        public Transform culprit;
        public GameplayEffect appliedEffect { get; }
        public StartHitAnimationEvent(GameplayEffect appliedEffect, Transform culprit)
        {
            this.appliedEffect = appliedEffect;
            this.culprit = culprit;
        }
    }


    //Is this in use? 
    public class ActivatePlayerControl : EventInfo {
        public readonly bool Activate;

        public ActivatePlayerControl(bool activate) {
            Activate = activate;
        }
    }

    public class NewLevelLoadedEvent : EventInfo {}

    public class ShowKeyText : EventInfo {
        public readonly bool ShowText;
        
        public ShowKeyText(bool showKeyText) {
            ShowText = showKeyText;
        }
    }
}

