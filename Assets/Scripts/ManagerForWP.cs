using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerForWP : MonoBehaviour
{
    public static ManagerForWP instance; // for vehicles (WayPointNavigators) to easily grab common starting point

    public EachWP firstNode; // helps to start or detect a loop, and tells navigators which to pick at start
    private EachWP[] allWP; // gathered automatically from its children in the hierarchy

    private float trackDistanceBest = 0.0f; // calculates after starting

    void Awake() // note: Awake so it'll be reached before Start initializes in AI car WayPointNavigator
    {
        instance = this;

        allWP = gameObject.GetComponentsInChildren<EachWP>(); // note: likely not in order! may branch anyway
        foreach (EachWP wp in allWP) {
            wp.ForgetScore();
        }

        // switch which of two below is commented out based on if you want distances populated rapidly or (for illustration) gradually
        StartCoroutine(SlowlyMeasureNextNodesFrom(firstNode)); // gradual version to be able to observe what it's doing
        // MeasureNodeBranchesStartingFrom(firstNode); // "instant"/immediate (within 1 frame) version
    }

    // "instant"/immediate (within 1 frame) version
    void MeasureNodeBranchesStartingFrom(EachWP startNode) {
        int nextNodes = startNode.nextNodeCount(); // usually 1, except where there's a branch
        for (int i = 0; i < nextNodes; i++) {
            EachWP nextNode = startNode.ReturnNodeByIndex(i);
            float distFromPrevious = Vector3.Distance(nextNode.transform.position,
                                                        startNode.transform.position);
            bool foundNewOrShorterPath = nextNode.SetScore(distFromPrevious, startNode);
            if (foundNewOrShorterPath) {
                MeasureNodeBranchesStartingFrom(nextNode);
            }
        }

        trackDistanceBest = 0.0f;
        EachWP measureNode = firstNode;
        EachWP measureNextNode = measureNode.ReturnNextNodeForShortestPathToEnd();
        do {
            // note: math.floor is only here for clearer debugging, so measure matches no-decimal debug display
            trackDistanceBest += Mathf.Floor(Vector3.Distance(measureNextNode.transform.position,
                                                        measureNode.transform.position));
            measureNode = measureNextNode;
            measureNextNode = measureNode.ReturnNextNodeForShortestPathToEnd();
        } while (measureNode != firstNode);

    }

    public float TotalTrackLengthBest() {
        return trackDistanceBest;
    }

    // same as above MeasureNodeBranchesStartingFro(), but through a coroutine so its changes are gradual
    IEnumerator SlowlyMeasureNextNodesFrom(EachWP startNode) {
        int nextNodes = startNode.nextNodeCount(); // usually 1, except where there's a branch
        for (int i = 0; i < nextNodes; i++) {
            EachWP nextNode = startNode.ReturnNodeByIndex(i);
            float distFromPrevious = Vector3.Distance(nextNode.transform.position, startNode.transform.position);
            yield return new WaitForSeconds(1.0f+0.5f*i); // pause before next update, stagger if multiple branches
            bool foundNewOrShorterPath = nextNode.SetScore(distFromPrevious, startNode);
            if (foundNewOrShorterPath) {
                StartCoroutine(SlowlyMeasureNextNodesFrom(nextNode));
            }
        }
    }
}
