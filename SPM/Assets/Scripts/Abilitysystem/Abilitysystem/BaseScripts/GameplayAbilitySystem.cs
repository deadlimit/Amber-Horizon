using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EventCallbacks;
using UnityEngine.EventSystems;

namespace AbilitySystem
{
    public class GameplayAbilitySystem : MonoBehaviour
    {
        private Dictionary<Type, float> AttributeSet = new Dictionary<Type, float>();
        private Dictionary<Type, GameplayAbility> GrantedAbilities = new Dictionary<Type, GameplayAbility>();
        private Dictionary<Type, Action<float>> OnAttributeChanged = new Dictionary<Type, Action<float>>();
        private Dictionary<Type, Func<float, float>> AttributeSetCalculations = new Dictionary<Type, Func<float, float>>();
        private Dictionary<GameplayEffect, int> ActiveEffects = new Dictionary<GameplayEffect, int>();
        private HashSet<GameplayTag> ActiveTags = new HashSet<GameplayTag>();
        private HashSet<GameplayAbility> AbilitiesOnCooldown = new HashSet<GameplayAbility>();

        private float dashCooldownBonus = 0;

        AbilityEntity entity;
        private void Awake()
        {
            entity = GetComponent<AbilityEntity>();

            //register ChangeCooldownBonus to PieceAbosorbed event.
            EventSystem<PieceAbsorbed>.RegisterListener(ChangeCooldownBonus);
        }
        public void RegisterAttributeSet(List<GameplayAttributeSetEntry> Set)
        {
            Set.ForEach(Entry => AttributeSet.Add(Entry.Attribute.GetType(), Entry.Value));
        }

        public void RegisterAttributeCalculation(Type Attribute, Func<float, float> Calculation)
        {
            if (!AttributeSetCalculations.ContainsKey(Attribute))
            {
                AttributeSetCalculations.Add(Attribute, Calculation);
            }
            else
            {
                AttributeSetCalculations[Attribute] += Calculation;
            }
        }

        public float? GetAttributeValue(Type Attribute)
        {
            if (AttributeSet.ContainsKey(Attribute))
            {
                return AttributeSet[Attribute];
            }
            return null;
        }

        public bool ApplyEffectToSelf(GameplayEffect Effect) {
            bool retur = false;
            if (Effect.BlockedByTags.Any(Tag => ActiveTags.Contains(Tag)))
            {
                return false;
            }

            if (Effect.Attribute != null)
            {
                TryApplyAttributeChange(Effect.Attribute.GetType(), Effect.Value);
                retur = true;
            }

            Effect.AppliedTags.ForEach(Tag => ActiveTags.Add(Tag));

            switch (Effect.EffectType)
            {
                case EffectDurationType.Instant: break;
                case EffectDurationType.Duration:
                {
                    StartCoroutine(RemoveAfterTime(Effect));
                    if (!ActiveEffects.ContainsKey(Effect))
                    {
                        ActiveEffects.Add(Effect, 1);
                        Effect.AppliedTags.ForEach((Tag) => ActiveTags.Add(Tag));

                    }
                    else
                    {
                        ActiveEffects[Effect]++;
                    }
                } break;
                case EffectDurationType.Infinite:
                {
                    if (!ActiveEffects.ContainsKey(Effect))
                    {
                        ActiveEffects.Add(Effect, 1);
                    }
                    else
                    {
                        ActiveEffects[Effect]++;
                    }
                } break;
            }
            return retur;
        }

        public bool TryApplyEffectToOther(GameplayEffect effect, GameplayAbilitySystem Other) {
           return Other.ApplyEffectToSelf(effect);
        }

        public void TryApplyAttributeChange(Type Attribute, float Value)
        {
            if (AttributeSet.ContainsKey(Attribute))
            {
                // Hijack
                if (AttributeSetCalculations.ContainsKey(Attribute))
                {
                    var Calculations = AttributeSetCalculations[Attribute];
                    foreach (var Calc in Calculations.GetInvocationList())
                    {
                        Value = ((Func<float, float>)Calc)(Value);
                    }
                }
                
                ModifyAttributeValue(Attribute, Value);

                //vad gör denna? Inget finns i OnAttributeChanged?
                //OnAttributeChanged[Attribute]?.Invoke(AttributeSet[Attribute]);
            }
            else
                Debug.LogError(Attribute + " not found in " + this.gameObject);
        }

        private void ModifyAttributeValue(Type Attribute, float Value)
        {
            AttributeSet[Attribute] += Value;
            GameplayAttributeSetEntry gaSet;
            foreach (GameplayAttributeSetEntry set in entity.AttributeSet)
            {
                if (set.Attribute.GetType() == Attribute)
                {
                    gaSet = set;
                    if (AttributeSet[Attribute] > gaSet.Value)
                        AttributeSet[Attribute] = gaSet.Value;
                    if (AttributeSet[Attribute] < 0)
                        AttributeSet[Attribute] = 0;

                }
            }
        }

        public void GrantAbility(GameplayAbility Ability) {
            
            if (GrantedAbilities == null)
                GrantedAbilities = new Dictionary<Type, GameplayAbility>();
            
            if (!GrantedAbilities.ContainsKey(Ability.AbilityTag.GetType()))
                GrantedAbilities.Add(Ability.AbilityTag.GetType(), Ability);
            else
                GrantedAbilities[Ability.AbilityTag.GetType()] = Ability;
        }

        public void RevokeAbility(Type AbilityTag)
        {
            GrantedAbilities.Remove(AbilityTag);
        }
        public void RevokeAbility(GameplayTag AbilityTag)
        {
            RevokeAbility(AbilityTag.GetType());
        }
        
        public bool TryActivateAbilityByTag(Type AbilityTag)
        {
            if (GrantedAbilities.TryGetValue(AbilityTag, out var Ability)) {
                
                if (!AbilitiesOnCooldown.Contains(Ability) && !Ability.BlockedByTags.Any(Tag => ActiveTags.Contains(Tag)) 
                    && !Ability.RequiredTags.Any(Tag => !ActiveTags.Contains(Tag))) {

                    if (Ability.Cooldown && Ability.Cooldown.EffectType is EffectDurationType.Duration) {
                        AbilitiesOnCooldown.Add(Ability);
                        StartCoroutine(RemoveAfterTime(Ability));
                    }

                    if (!Ability.BlockedByTags.Any(Tag => ActiveTags.Contains(Tag))) {
                        Ability.Activate(this);
                        EventSystem<AbilityUsed>.FireEvent(new AbilityUsed(Ability));
                        return true;
                    }

                }
            }

            return false;
        }

        public void TryDeactivateAbilityByTag(Type AbilityTag)
        {
            if(GrantedAbilities.TryGetValue(AbilityTag, out var Ability))
            {
                Ability.Deactivate(this);
            }
        }

        public void RemoveTag(GameplayTag Tag)
        {
            ActiveTags.Remove(Tag);

        }

        public IEnumerator RemoveAfterTime(GameplayEffect Effect)
        {
            
            yield return new WaitForSeconds(Effect.Duration);
            
            ActiveEffects[Effect]--;
            
            if (ActiveEffects[Effect] <= 0) {
                
                ActiveEffects.Remove(Effect);
                Effect.AppliedTags.ForEach((Tag) => DebugTag(Tag));
                
            }

        }
        void DebugTag(GameplayTag Tag)
        {
            ActiveTags.Remove(Tag);
            Debug.Log("Removing Tag: " + Tag);
        }

        public IEnumerator RemoveAfterTime(GameplayAbility ability) {

            //yield return new WaitForSeconds(ability.Cooldown.Duration);

            float end = Time.time + ability.Cooldown.Duration;

            //if dash
            if (ability.name == "Dash")
            {

                //This is if only want to lowerthe cooldown when the dash is on cooldown.
                //So the player can't save a cooldownbonus for later.
                //NOTE: this has an equivalent in PlayerUI
                ResetCooldownBouns();
                while (Time.time + dashCooldownBonus < end)
                {
                    //do nothing
                    yield return null;
                }
                
                ResetCooldownBouns();
            }

            //else, its is not dash.
            //the same but without the bonus in the while case. and don't reset the bonus value.
            else 
            {
                while (Time.time < end)
                {
                    //do nothing
                    yield return null;
                }
            }

            //Debug.Log("In GAS. RemoveAfterTime() for " + ability.name);

            AbilitiesOnCooldown.Remove(ability);

        }

        public GameplayAbility GetAbilityByTag(Type AbilityTag)
        {
            if (GrantedAbilities.ContainsKey(AbilityTag))
            {
                return GrantedAbilities[AbilityTag];
            }

            return null;
        }


        private void ChangeCooldownBonus(PieceAbsorbed pieceAbsorbed) 
        {
            dashCooldownBonus += pieceAbsorbed.pieceValue;
            //Debug.Log("in change. bonus is now" + dashCooldownBonus);
        }

        private void ResetCooldownBouns() 
        {
            dashCooldownBonus = 0f;
        }

    }
}
