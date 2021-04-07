using System.Collections;
using UnityEngine;

public static class MaterialManipulator {

    public static void Dissolve(MonoBehaviour gameObject, Material material, float timeBeforeResolve, float dissolveSpeed) {
        gameObject.StartCoroutine(DissolveProcess(material, timeBeforeResolve, dissolveSpeed));
    }
    
    private static IEnumerator DissolveProcess(Material material, float timeBeforeResolve, float dissolveSpeed) {
        
        while (material.GetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208") < 2) {
            material.SetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208", material.GetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208") + dissolveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(timeBeforeResolve / 2);
        
        while (material.GetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208") > -2) {
            material.SetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208", material.GetFloat("Vector1_514c1d1c3a2c490d9cb8e4c3ce390208") - dissolveSpeed * Time.deltaTime);
            yield return null;
        }
        
    }

}
