using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFoley : MonoBehaviour
{

    private PlayerController m_PlayerController;
    private AudioSource m_AudioSource;
    private PhysicsComponent physics;

    public string colliderType;
    public AudioClip defaultSound;
    public AudioClip houseSound;
    public AudioClip plattformSound;


    // Start is called before the first frame update
    void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        m_AudioSource = GetComponent<AudioSource>();
        physics = GetComponent<PhysicsComponent>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Collider>().gameObject.GetComponent<SurfaceColliderType>())
        {
            colliderType = hit.gameObject.GetComponent<SurfaceColliderType>().GetTerrainType();
            Debug.Log("colliderType = " + colliderType);
        }

        
    }


    private void PlayDynamicFootstep()
    {
        if (physics.isGrounded())
        {
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
        }
    }
}
