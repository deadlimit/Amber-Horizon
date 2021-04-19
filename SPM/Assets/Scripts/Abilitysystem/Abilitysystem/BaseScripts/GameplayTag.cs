using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    //Istället för enums, typen är viktig
    public abstract class GameplayTag : GameplayEntity
    {
        public bool Is(GameplayTag Other) => Other.GetType().IsAssignableFrom(GetType());
    }

    public static class GameplayTags {

        public static Type MovementAbilityTag => typeof(MovementTag);
        public static Type BlackHoleAbilityTag => typeof(AttackTag);
        public static Type AimingTag => typeof(AimingTag);

        public static Type MeleeTag => typeof(MeleeTag);

    }
}
