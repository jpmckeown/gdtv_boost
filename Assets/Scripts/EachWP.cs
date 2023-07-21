using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// note: Waypoints use Physics to find locally nearby waypoints, and raycasts for line of sight test; the GameObject should be set on a layer called "Waypoint" then in Unity's menus Edit->Project Settings->Physics scroll to the bottom and uncheck anything else from colliding with the Waypoint to prevent physical things from bumping it. Note that raycasts like guns might also need to mask it out. If being mindful of avoiding the layer sounds like a pain this could be brute forced instead with comprehensive distance checks by a manager layer that finds and operates on all waypoints. Despite "physics" sounding like a costly answer it's actually very well optimized for detecting local objects that aren't moving and rigidbody forces aren't being applied to.

public class EachWP : MonoBehaviour
{
    public TMP_Text scoreLabel;
    public List<EachWP> nextNodes = new List<EachWP>();
    float scoreFromPreviousNode = 0.0f;
    float scoreFromTrackStart = 0.0f; // helps to identify shortest path

    // todo: not yet calculated
    // combined with distance to next node, can be used to estimate progress during a lap
    float scoreToTrackEnd = 0.0f; // helps gauge track progress during a branch, given best remaining choices

    public float trackWidthHere = 5.0f;

    public bool SetScore(float newScore, EachWP fromNode) {
        bool hadNoScoreYet = (scoreFromPreviousNode == 0.0f);
        bool scoredChanged = false;

        float newDistanceToConsider = scoreFromPreviousNode + fromNode.GetTotalScoreUpToThisNode();

        // either not scored yet, or this is a shorter path than previously considered
        if(hadNoScoreYet || scoreFromTrackStart > newDistanceToConsider) {
            scoreFromPreviousNode = Mathf.Floor(newScore); // note: floor is not necessary, just so math in example adds up cleaner
            scoreFromTrackStart = scoreFromPreviousNode + fromNode.GetTotalScoreUpToThisNode();
            scoredChanged = true; // a clue to continue updating from this point forward, new shorter way found
            UpdateScoreDisplay();
        }
        return hadNoScoreYet || scoredChanged; // either way, continue processing
    }

    public float GetScoreToNext() {
        return scoreFromPreviousNode;
    }

    public float GetTotalScoreUpToThisNode() {
        return scoreFromTrackStart;
    }

    public void ForgetScore() {
        scoreFromPreviousNode = 0.0f;
        scoreFromTrackStart = 0.0f;
        scoreToTrackEnd = 0.0f;
    }

    void Start() {
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay() {
        scoreLabel.text = "+" + Mathf.Floor(scoreFromPreviousNode) + "\n=" + Mathf.Floor(scoreFromTrackStart);
    }

    // -1.0 as far left edge, 1.0 as right edge
    public Vector3 PositionOffsetFromCenter(float offsetAmount) { 
        Vector3 leftEdgeHere = transform.position - transform.right * trackWidthHere;
        Vector3 rightEdgeHere = transform.position + transform.right * trackWidthHere;
        float normalizedOffset = (offsetAmount * 0.5f) + 0.5f; // from -1.0 to 1.0 scale to 0.0-1.0 so we can use complement

        // because normalizedOffset is 0.0f-1.0, taking 1.0f-normalizedOffset gives us the opposite percentage, so
        // if normalizedOffset is 0.5f will be 50% balance between the two points
        // if normalizedOffset is 0.3f will be 30% to one side, 70% toward other side
        return normalizedOffset * leftEdgeHere + (1.0f - normalizedOffset) * rightEdgeHere;
    }

    public EachWP ReturnRandomNextNode() {
        int randomNodeIndex = Random.Range(0, nextNodes.Count);
        return nextNodes[randomNodeIndex];
    }

    public EachWP ReturnNextNodeForShortestPathToEnd() {
        int nodeCount = nextNodes.Count;
        int bestIndex = 0;
        for(int i=1;i<nodeCount;i++) { // skipping index 0, since it's default as the one to beat
            if(nextNodes[i].GetTotalScoreUpToThisNode() < nextNodes[bestIndex].GetTotalScoreUpToThisNode()) {
                bestIndex = i;
            }
        }
        return nextNodes[bestIndex];
    }

    public int nextNodeCount() {
        return nextNodes.Count;
    }

    public EachWP ReturnNodeByIndex(int index) {
        if(index < 0 || index >= nextNodes.Count) {
            Debug.LogError("invalid node requested, index "+ index + " for waypoint: " + gameObject.name);
            return null;
        }
        return nextNodes[index];
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < nextNodes.Count; i++) {
            if(nextNodes[i] == null) {
                continue; // can't draw what is missing! skip this connection
            }
            Transform nextTransform = nextNodes[i].gameObject.transform;
            Vector3 leftEdgeHere = transform.position - transform.right * trackWidthHere;
            Vector3 rightEdgeHere = transform.position + transform.right * trackWidthHere;
            Vector3 leftEdgeNext = nextTransform.position - nextTransform.right * nextNodes[i].trackWidthHere;
            Vector3 rightEdgeNext = nextTransform.position + nextTransform.right * nextNodes[i].trackWidthHere;

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.75f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(leftEdgeHere, leftEdgeNext);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rightEdgeHere, rightEdgeNext);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, nextTransform.position); // center line
        }
    }
}
