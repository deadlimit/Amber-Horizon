using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialFromSignal : MonoBehaviour
{
    [SerializeField] private Material m_Material;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        m_Material = skinnedMeshRenderer.material;
        print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    }

    public void ChangeMatRange(float newMatRangeValue) 
    {
        Debug.Log("moo moo");
        m_Material.SetFloat("Range", newMatRangeValue);
    }
}
