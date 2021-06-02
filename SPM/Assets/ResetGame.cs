using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour {


    public void Reset() {
        SceneManager.LoadScene("MainMenu");
    }

}
