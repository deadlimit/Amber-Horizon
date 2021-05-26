using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFoley : MonoBehaviour
{
    private AudioSource[] m_AudioSource;
    private double time;
    private float filterTime;
    
    public AudioClip defaultSound;
    public AudioClip houseSound;
    public AudioClip plattformSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip dieSound;

    private string colliderType;

    
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponents<AudioSource>();
        time = AudioSettings.dspTime;
        filterTime = 0.2f;
        
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider != null)
            {
                GameObject gameObj = hit.collider.gameObject;
                SurfaceColliderType act = gameObj.GetComponent<SurfaceColliderType>();

                if (act)
                {
                    colliderType = act.gameObject.GetComponent<SurfaceColliderType>().GetTerrainType();
                    
                }
            }
        }
    }

    /*private void OnCollisionEnter(Collision col)
    {
        SurfaceColliderType act = col.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>();
     
        if (act)
        {
            colliderType = act.gameObject.GetComponent<SurfaceColliderType>().GetTerrainType();
            Debug.Log("colliderType = " + colliderType);
        }


    }*/


    private void PlayDynamicFootstep(int foot_number)
    {
        if (AudioSettings.dspTime < time + filterTime)
        {
            return;
        }

        switch (colliderType) // Att switcha olika ljud för olika terrian
        {
            case "House":
                m_AudioSource[0].PlayOneShot(houseSound);
                break;
            case "Plattform":
                m_AudioSource[0].PlayOneShot(plattformSound);
                break;
            default:
                m_AudioSource[0].PlayOneShot(defaultSound);
                break;
        }

        time = AudioSettings.dspTime;
    }

    private void PlayJumpSound()
    {
        m_AudioSource[0].PlayOneShot(jumpSound);
    }

    private void PlayDashSound()
    {
        m_AudioSource[1].PlayOneShot(dashSound);
    }

    private void PlayDieSound()
    {
        m_AudioSource[0].PlayOneShot(dieSound);
    }
}
