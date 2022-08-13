using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class UITextTrigger : MonoBehaviour
{
    [SerializeField]
    private bool destroyAfterUse;
    [SerializeField]
    private string textToDisplay;
    [SerializeField]
    private int displayDuration;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ui text. player trigger enter");


            EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage(textToDisplay, displayDuration, true));
            //then check the bool to see if it should be destroyed
            if(destroyAfterUse)
            {
                Destroy(gameObject);
            }

        }
    }
}
