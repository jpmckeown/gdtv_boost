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
                ReachLanding();
                break;
            case "Friendly":
                Debug.Log("back at launchpad");
                break;
            default:
                CrashSequence();
                break;
        }
    }

    void CrashSequence(){
        // add soundfx and particlefx
        Debug.Log("exploding rocket");
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayRestartLevel);
        // ReloadLevel();
    }

    void ReachLanding(){
        // add soundfx and particlefx
        Debug.Log("landing success!");
        // LoadNextLevel();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayNextLevel);
    }

    void ReloadLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log("next scene = " + nextSceneIndex);
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
