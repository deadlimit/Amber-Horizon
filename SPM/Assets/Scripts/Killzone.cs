using UnityEngine;
using EventCallbacks;
using System.Collections.Generic;
using System.Collections;

public class Killzone : MonoBehaviour {
        
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) 
        {
            PlayerDiedEvent pde = new PlayerDiedEvent(other.gameObject);
            EventSystem<PlayerDiedEvent>.FireEvent(pde);

            //call this is you want to have a delay.
            //StartCoroutine(WaitBeforeDeathEvent(other));
        }           
    }

    private IEnumerator WaitBeforeDeathEvent(Collider other)
    {

       yield return new WaitForSeconds(.15f);

        PlayerDiedEvent pde = new PlayerDiedEvent(other.gameObject);
        EventSystem<PlayerDiedEvent>.FireEvent(pde);
    }

}
