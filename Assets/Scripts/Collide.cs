using UnityEngine;
using UnityEngine.SceneManagement;

public class Collide : MonoBehaviour
{
    public float delayRestartLevel = 3f;
    public float delayNextLevel = 3f;

    void Start(){
    }

    void OnCollisionEnter(Collision other) {
        switch(other.gameObject.tag){
            case "Finish":
                LevelDone();
                break;
            case "Friendly":
                Debug.Log("back at launchpad");
                break;
            default:
                CrashSequence();
                break;
        }

        void CrashSequence(){
            Debug.Log("exploding rocket");
            GetComponent<Movement>().enabled = false;
            Invoke("ReloadLevel", DelayRestartLevel);
        }

        void LevelDone(){
            Debug.Log("landing success!");
            LoadNextLevel();
        }

        void ReloadLevel(){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        void LoadNextLevel(){
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int NextSceneIndex = currentSceneIndex + 1;
            if(NextSceneIndex == SceneManager.sceneCountInBuildSettings){
                NextSceneIndex = 0;
            }
            SceneManager.LoadScene(NextSceneIndex);
        }
    }
}
