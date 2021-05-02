using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFoley : MonoBehaviour
{

    private AudioSource m_AudioSource;
    private double time;
    private float filterTime;
    
    public AudioClip defaultSound;
    public AudioClip houseSound;
    public AudioClip plattformSound;

    private string colliderType;


    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        time = AudioSettings.dspTime;
        filterTime = 0.2f;


    }

    private void OnCollisionEnter(Collision col)
    {
        SurfaceColliderType act = col.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>();

        if (act)
        {
            colliderType = act.gameObject.GetComponent<SurfaceColliderType>().GetTerrainType();
            Debug.Log("colliderType = " + colliderType);
        }

        
    }


    private void PlayDynamicFootstep(int foot_number)
    {
        if (AudioSettings.dspTime < time + filterTime)
        {
            return;
        }

        switch (colliderType) // Att switcha olika ljud för olika terrian
        {
            case "House":
                m_AudioSource.PlayOneShot(houseSound);
                break;
            case "Plattform":
                m_AudioSource.PlayOneShot(plattformSound);
                break;
            default:
                m_AudioSource.PlayOneShot(defaultSound);
                break;
        }

        time = AudioSettings.dspTime;
    }
}
