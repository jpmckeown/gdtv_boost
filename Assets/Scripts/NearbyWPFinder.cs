using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NearbyWPFinder : MonoBehaviour
{
    private const float neighborRadius = 50.0f; // could expose and edit per object, but makes level designer placement harder to eyeball

    public List<EachWP> FindLOSNeighbors()
    {
        List<EachWP> nearbyNodesWithLineOfSight = new List<EachWP>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius, LayerMask.GetMask("Waypoint"));
        int waypointLayer = LayerMask.NameToLayer("Waypoint");
        RaycastHit rhInfo;

        for (int i = 0; i < colliders.Length; i++) {
            Vector3 directionToNextNearbyNode = colliders[i].transform.position - transform.position;
            bool comparingToSelf = (directionToNextNearbyNode.magnitude == 0.0f);

            // note: could add a mask in this raycast if there are objects which shouldn't block the ray, such as openable doors
            if (comparingToSelf == false && Physics.Raycast(transform.position, directionToNextNearbyNode, out rhInfo, neighborRadius)) {
                if (rhInfo.collider.gameObject.layer == waypointLayer) {
                    nearbyNodesWithLineOfSight.Add(rhInfo.collider.gameObject.GetComponent<EachWP>());
                }
            }
        }
        return nearbyNodesWithLineOfSight;
    }

    // similar to above, except instead of returning a list of waypoint nodes, it searches for a single transform nearby with line of sight

    public Transform ReturnNearbyTransformWithLOS(string layerName) {
        List<EachWP> nearbyNodesWithLineOfSight = new List<EachWP>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius, LayerMask.GetMask(layerName));
        int waypointLayer = LayerMask.NameToLayer(layerName);
        RaycastHit rhInfo;

        for (int i = 0; i < colliders.Length; i++) {
            Vector3 directionToNextNearbyNode = colliders[i].transform.position - transform.position;

            if (Physics.Raycast(transform.position, directionToNextNearbyNode, out rhInfo, neighborRadius)) {
                if (rhInfo.collider.gameObject.layer == waypointLayer) {
                    return rhInfo.collider.gameObject.transform;
                }
            }
        }
        return null;
    }
}
