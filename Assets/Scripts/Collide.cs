using UnityEngine;
using UnityEngine.SceneManagement;

public class Collide : MonoBehaviour
{
    // parameters
    public float delayRestartLevel = 6f;
    public float delayNextLevel = 3f;
    public AudioClip landingSuccessSound;
    public AudioClip crashExplosionSound;

    public ParticleSystem successParticles;
    public ParticleSystem crashParticles;

    // caching
    AudioSource audioSource;

    // state
    bool inTransition = false;
    bool collisionDisable = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        DebugKeysResponse();
    }

    void DebugKeysResponse() {
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            collisionDisable = !collisionDisable;
        }
    }

    void OnCollisionEnter(Collision other) {
        if(inTransition || collisionDisable) { return; }

        switch(other.gameObject.tag) {
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
        inTransition = true;
        audioSource.Stop(); // engine noise no more

        // add soundfx and particlefx
        audioSource.PlayOneShot(crashExplosionSound);
        crashParticles.Play();
        // crashExplosionSound.Play();

        Debug.Log("exploding rocket");
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadLevel", delayRestartLevel);
        // ReloadLevel();
    }

    void ReachLanding(){
        inTransition = true;
        audioSource.Stop(); // engine noise no more
                
        // add soundfx and particlefx
        audioSource.PlayOneShot(landingSuccessSound);
        successParticles.Play();
        // landingSuccessSound.Play();

        Debug.Log("landing success!");
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
