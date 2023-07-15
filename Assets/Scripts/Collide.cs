using UnityEngine;

public class Collide : MonoBehaviour
{
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
                break;
        }
    }
}
