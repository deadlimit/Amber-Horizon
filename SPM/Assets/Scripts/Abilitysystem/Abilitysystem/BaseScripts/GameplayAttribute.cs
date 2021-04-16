using System;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class GameplayAttribute : GameplayEntity
    {
        public bool Is(GameplayAttribute Other) => Other.GetType().IsAssignableFrom(GetType());
        public bool Is(Type Other) => Other.IsAssignableFrom(GetType());
    }

    [Serializable]
    public struct GameplayAttributeSetEntry
    {
        public GameplayAttribute Attribute;
        public float Value;
    }

    public static class GameplayAttributes
    {
        public static Type HealthAttribute => typeof(HealthAttribute);
    }
    
}
