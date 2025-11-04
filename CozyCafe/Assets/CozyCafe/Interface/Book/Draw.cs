using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Draw : MonoBehaviour
{
    private LineRenderer line;
    private List<Vector2> points;
    private EdgeCollider2D edgeCollider;
    private CircleCollider2D circleCollider;

    private float minDistance = 0f;
    private float eraseRadius = 0.5f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }
        if (Vector2.Distance(points.Last(), position) > minDistance && points.Count > 1)
        {
            SetPoint(position);
            UpdateCollider();
        }

        else if (points.Count == 1)
        {
            Vector2 tinyOffset = position + Vector2.one * 0.001f;
            SetPoint(tinyOffset);
            UpdateCollider();
        }
    }

    public void EraseLine(Vector3 eraserPosition)
    {
        if (line == null) return;

        Vector3[] currentPositions = new Vector3[line.positionCount];
        line.GetPositions(currentPositions);

        List<Vector3> newPositions = new List<Vector3>();

        for (int i = 0; i < currentPositions.Length; i++)
        {
            if (Vector3.Distance(currentPositions[i], eraserPosition) > eraseRadius)
            {
                newPositions.Add(currentPositions[i]);
            }
        }

        line.positionCount = newPositions.Count;
        line.SetPositions(newPositions.ToArray());
    }

    private void SetPoint(Vector2 point)
    {
        points.Add(point);
        line.positionCount = points.Count;
        line.SetPosition(points.Count - 1, point);
    }

    private void UpdateCollider()
    {
        if (points.Count < 4)
        {
            edgeCollider.enabled = false;
            circleCollider.enabled = true;

            circleCollider.offset = transform.InverseTransformPoint(points[0]);
            circleCollider.radius = 0.05f; // adjust for your scale
        }
        else if (points.Count >= 4)
        {
            circleCollider.enabled = false;
            edgeCollider.enabled = true;

            Vector2[] localPoints = new Vector2[points.Count];
            for (int i = 0; i < points.Count; i++)
                localPoints[i] = transform.InverseTransformPoint(points[i]);

            edgeCollider.points = localPoints;
        }
    }
}
