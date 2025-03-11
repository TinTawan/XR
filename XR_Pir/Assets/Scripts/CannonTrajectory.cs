using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField, Min(3)] private int lineSegments = 60;     // to smooth the trajectory line

    public void ShowTrajectoryLine(Vector3 startPoint, Vector3 startVelocity)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];
        lineRendererPoints[0] = startPoint;

        for (int i = 1; i < lineSegments; i++)
        {
            float t = (i / (float)lineSegments) * 3f;
            lineRendererPoints[i] = startPoint + startVelocity * t + (Vector3.up * (0.5f * Physics.gravity.y * t * t));     // calculates trajectory of cannon ball when gravity is applied
        }

        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    public void EnableTrajectory(bool enable)
    {
        lineRenderer.enabled = enable;
    }
}