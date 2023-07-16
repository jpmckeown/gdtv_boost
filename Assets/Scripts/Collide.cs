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
                loadNextLevel();
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

        void loadNextLevel(){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
