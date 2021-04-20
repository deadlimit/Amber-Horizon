using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public enum EffectDurationType
    {
        Instant,
        Duration,
        Infinite,
    }
    [CreateAssetMenu()]
    public class GameplayEffect : GameplayEntity
    {
        // TODO: Array?
        public GameplayAttribute Attribute;
        public float Value;

        public float Duration;
        public EffectDurationType EffectType;

        public List<GameplayTag> AppliedTags = new List<GameplayTag>();

        public List<GameplayTag> BlockedByTags = new List<GameplayTag>();
        // TODO: RequiredTags;
    }
}
