using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class GameplayAbility : GameplayEntity 
    {
        [SerializeReference] public GameplayTag AbilityTag;
        public GameplayEffect AppliedEffect;
        public GameplayEffect Cost;
        public GameplayEffect Cooldown;

        public List<GameplayTag> BlockedByTags = new List<GameplayTag>();
        // TODO: RequiredTags;

        public abstract void Activate(GameplayAbilitySystem Owner);
        public void Commit(GameplayAbilitySystem Owner)
        {
            if (Cost.Attribute != null)
            {
                // TODO: TryApplyEffectToSelf
                Owner.TryApplyAttributeChange(Cost.Attribute.GetType(), -Cost.Value);
            }
            if (Cooldown != null)
                Owner.ApplyEffectToSelf(Cooldown);
        }
        public bool CanActivate(GameplayAbilitySystem Owner)
        {
            if (Cost.Attribute == null)
            {
                return true;
            }

            float? Value = Owner.GetAttributeValue(Cost.Attribute.GetType());
            return Value != null && Value > Cost.Value;
        }
        
    }
    
}
