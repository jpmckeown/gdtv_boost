using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Debug.Log("Quitting the game app");
            Application.Quit();
        }
    }
}
