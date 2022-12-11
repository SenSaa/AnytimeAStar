using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Visualisation
{
    public class RenderPath
    {
        public void Draw(List<Tuple<int, int>> points, HashSet<Tuple<int, int>> visitedStates, Transform transform, Material pathMaterial, Material pathPointsMaterial, Material visitedMaterial)
        {
            List<Vector3> path = processPoints(points);
            renderPathLine(path, transform, pathMaterial);
            renderPathPoints(path, transform, pathPointsMaterial);
            List<Vector3> visitedPositions = processVisitedPosSet(visitedStates);
            renderVisitedStates(visitedPositions, transform, visitedMaterial);
        }


        private List<Vector3> processPoints(List<Tuple<int, int>> points)
        {
            List<Vector3> pathPoints = new List<Vector3>();
            foreach (var point in points)
            {
                pathPoints.Add(new Vector3(point.Item1, 0, point.Item2));
            }
            return pathPoints;
        }

        private void renderPathLine(List<Vector3> points, Transform transform, Material material)
        {
            GameObject pathLineGO = new GameObject("PathLine");
            pathLineGO.transform.parent = transform;
            LineRenderer pathLineRender = pathLineGO.AddComponent<LineRenderer>();
            pathLineRender.positionCount = points.Count;
            pathLineRender.SetPositions(points.ToArray());
            pathLineRender.widthMultiplier = 0.25f;
            pathLineRender.material = material;
        }
        private void renderPathPoints(List<Vector3> points, Transform transform, Material material)
        {
            GameObject pathPtsGO = new GameObject("PathPoints");
            pathPtsGO.transform.parent = transform;
            foreach (var pt in points)
            {
                GameObject pointRenderGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pointRenderGo.transform.position = pt;
                pointRenderGo.transform.parent = pathPtsGO.transform;
                pointRenderGo.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                pointRenderGo.name = "State";
                pointRenderGo.GetComponent<MeshRenderer>().material = material;
            }
        }

        private List<Vector3> processVisitedPosSet(HashSet<Tuple<int, int>> points)
        {
            List<Vector3> visitedPosList = new List<Vector3>();
            foreach (var point in points)
            {
                visitedPosList.Add(new Vector3(point.Item1, 0, point.Item2));
            }
            return visitedPosList;
        }

        private void renderVisitedStates(List<Vector3> visitedPosList, Transform transform, Material material)
        {
            GameObject visitedPtsGO = new GameObject("VisitedPositions");
            visitedPtsGO.transform.parent = transform;
            foreach (var pt in visitedPosList)
            {
                GameObject visitedPosRenderGo = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                visitedPosRenderGo.transform.position = pt;
                visitedPosRenderGo.transform.parent = visitedPtsGO.transform;
                visitedPosRenderGo.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                visitedPosRenderGo.name = "VisitedPos";
                visitedPosRenderGo.GetComponent<MeshRenderer>().material = material;
            }
        }
    }
}
