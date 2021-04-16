using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    [CreateAssetMenu()]
    public class FireAbility : GameplayAbility
    {
        public override void Activate(GameplayAbilitySystem Owner)
        {
            // wait for animation etc.
            Commit(Owner);
            GameplayAbilitySystem OtherAS = null; // hit.GetComponent<GameplayAbilitySystem>();
            OtherAS.ApplyEffectToSelf(AppliedEffect);
        }
    }
}