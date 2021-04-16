using AbilitySystem;
using UnityEngine;

public class AbilityLoader {

    public static T CreateAbility<T, TU>() where T : GameplayAbility where TU : GameplayTag {
        T ability = ScriptableObject.CreateInstance(typeof(T)) as T;
        ability.AbilityTag = ScriptableObject.CreateInstance(typeof(TU)) as TU;

        return ability;
    }
    
}
