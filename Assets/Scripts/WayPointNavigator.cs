using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WayPointNavigator : MonoBehaviour
{
    EachWP gotoTarget;

    private float closeEnoughToWaypointToChange = 5.0f; // meters, more generous for car than walking with tight pivots
    private float enemySpeedMin = 10.0f;
    private float enemySpeedMax = 20.0f;
    private float enemySpeed = 0.0f;
    private NearbyWPFinder wpScanner;
    private TMP_Text displayAIState;

    private float offsetFromRoadCenter = 0.0f;

    void Start()
    {
        displayAIState = gameObject.GetComponentInChildren<TMP_Text>();

        wpScanner = gameObject.GetComponent<NearbyWPFinder>();
        if (wpScanner == null) {
            Debug.LogError("NearbyWPFinder missing on " + gameObject.name);
        }

        gotoTarget = ManagerForWP.instance.firstNode;
        StartCoroutine(ReconsiderAIMode());
    }

    void Update() {
        if (gotoTarget != null) {
            Vector3 destinationWithOffset = gotoTarget.PositionOffsetFromCenter(offsetFromRoadCenter);
            transform.LookAt(destinationWithOffset);
            transform.position += transform.forward * Time.deltaTime * enemySpeed;

            float distanceToTarget = Vector3.Distance(destinationWithOffset, transform.position);
            float trackProgressAtNextNode = gotoTarget.GetTotalScoreUpToThisNode();

            // approximate track progress as distance from start to next node minus how far we are from that node
            float trackProgress = Mathf.FloorToInt(trackProgressAtNextNode - distanceToTarget);
            int trackProgressPerc = Mathf.FloorToInt(100.0f * trackProgress / ManagerForWP.instance.TotalTrackLengthBest());

            displayAIState.text = "" + trackProgress+"\n"+ trackProgressPerc + "%";
            if (distanceToTarget <= closeEnoughToWaypointToChange) { // near enough to turn to next
                gotoTarget = gotoTarget.ReturnRandomNextNode();
            }
        }

        Vector3 heightChange = transform.position;
        float terrainHeightHere = Terrain.activeTerrain.SampleHeight(transform.position);
        heightChange.y = terrainHeightHere;
        transform.position = heightChange;
    }

    // cars here are implemented in a cheaty, direct position update rather than driving forces, steering/gas is an "exercise for the reader" (todo) but once it does, it will be possible for the enemy to get caught on obstacles, or pushed away from having line of sight to the waypoint it previously was targeting. The purpose of a function like the next one is to find the nearest waypoint is can drive towards without cheating  cutting past part of the course

    // reminder: unlike stealth, "best" in sight isn't good, because that could cause us to skip parts
    // find the highest candidate which is either supposed to be next or already passed
    // remember waypoints know distance from start. this could be used!
    
    void FindNearbyNotCheatingWaypoint() {
        /* List<EachWP> nearbyDirectNode;

        if (gotoTarget == null) { // no idea where to go? this would pick highest one with line of sight - but that might be cheating!
            nearbyDirectNode = wpScanner.FindLOSNeighbors();
            EachWP highestNode = null;
            float highestScoreSeen = -1;
            foreach (EachWP wp in nearbyDirectNode) {
                float scoreToThisNode = wp.GetTotalScoreUpToThisNode();
                if (highestNode == null || scoreToThisNode > highestScoreSeen) {
                    highestNode = wp;
                    highestScoreSeen = scoreToThisNode;
                }
            }
            gotoTarget = highestNode;
        }*/
    }

    IEnumerator ReconsiderAIMode()
    {
        while(true) {
            enemySpeed = Random.Range(enemySpeedMin, enemySpeedMax);
            offsetFromRoadCenter = Random.Range(-1.0f, 1.0f);

            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
        }
    }
}
