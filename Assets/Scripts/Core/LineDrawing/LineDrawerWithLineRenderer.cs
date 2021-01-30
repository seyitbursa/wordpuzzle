using System.Collections.Generic;
using UnityEngine;

namespace Core.LineDrawing
{

    public class LineDrawerWithLineRenderer : MonoBehaviour, ILineDrawer
    {
        [SerializeField] private GameObject lineDrawerPrefab;

        private LineRenderer lineRenderer;
        private List<Vector3> linePoints;
        private bool hasTempLine;

        private void Awake()
        {
            GameObject lineDrawerObject = Instantiate(lineDrawerPrefab, Vector3.zero, Quaternion.identity, transform);
            lineRenderer = lineDrawerObject.GetComponent<LineRenderer>();
            linePoints = new List<Vector3>();
        }

        public void AddLinePoint(Vector3 linePoint,bool isTemp)
        {
            RemoveTempLine();
            linePoints.Add(linePoint);
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
            hasTempLine = isTemp;
        }

        public void RemoveLastLine()
        {
            if (linePoints.Count > 1)
            {
                linePoints.RemoveAt(linePoints.Count - 1);
                lineRenderer.positionCount = linePoints.Count;
                lineRenderer.SetPositions(linePoints.ToArray());
            }
        }

        public void RemoveTempLine()
        {
            if (linePoints.Count > 1 && hasTempLine)
            {
                linePoints.RemoveAt(linePoints.Count - 1);
                hasTempLine = false;
            }
        }

        public void ClearLinePoints()
        {
            linePoints.Clear();
            lineRenderer.positionCount = linePoints.Count;
        }
    }
}