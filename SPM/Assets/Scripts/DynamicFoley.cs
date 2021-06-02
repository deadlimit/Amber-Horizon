using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFoley : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private double time;
    private float filterTime;
    
    public AudioClip defaultSound;
    public AudioClip houseSound;
    public AudioClip plattformSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip dieSound;

    private float audioVolume1;
    private float audioVolume2;

    private string colliderType;

    
    // Start is called before the first frame update
    void Start()
    {
        audioVolume1 = 0.2f;
        audioVolume2 = 1f;
        m_AudioSource = GetComponent<AudioSource>();
        time = AudioSettings.dspTime;
        filterTime = 0.2f;
        
    }

    void Update()
    {
        RaycastHit hit;
        
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
    
    private void PlayDynamicFootstep(int foot_number)
    {
        if (AudioSettings.dspTime < time + filterTime)
        {
            return;
        }

        switch (colliderType) // Att switcha olika ljud för olika terrian
        {
            case "House":
                m_AudioSource.volume = audioVolume2;
                m_AudioSource.PlayOneShot(houseSound);
                break;
            case "Plattform":
                m_AudioSource.volume = audioVolume2;
                m_AudioSource.PlayOneShot(plattformSound);
                break;
            default:
                m_AudioSource.volume = audioVolume2;
                m_AudioSource.PlayOneShot(defaultSound);
                break;
        }

        time = AudioSettings.dspTime;
    }

    private void PlayJumpSound()
    {
        m_AudioSource.volume = audioVolume2;
        m_AudioSource.PlayOneShot(jumpSound);
    }

    private void PlayDashSound()
    {
        m_AudioSource.volume = audioVolume1;
        m_AudioSource.PlayOneShot(dashSound);
    }

    private void PlayDieSound()
    {
        m_AudioSource.volume = audioVolume2;
        m_AudioSource.PlayOneShot(dieSound);
    }
}
