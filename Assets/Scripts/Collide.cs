using UnityEngine;
using UnityEngine.SceneManagement;

public class Collide : MonoBehaviour
{
    void Start(){
    }

    void OnCollisionEnter(Collision other) {
        switch(other.gameObject.tag){
            case "Finish":
                Debug.Log("landing success!");
                break;
            case "Friendly":
                Debug.Log("back at launchpad");
                break;
            default:
                Debug.Log("exploding rocket");
                ReloadLevel();
                break;
        }

        void ReloadLevel(){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}