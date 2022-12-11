using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ARAstar;
using Visualisation;
using System.Threading;

public class RunARA : MonoBehaviour
{

    private AraStar arastar;
    private Thread planningThread;
    [SerializeField] private Material pathMaterial;
    [SerializeField] private Material pathPointsMaterial;
    [SerializeField] private Material visitedMaterial;
    [SerializeField] private Material obstMaterial;
    private int prevPlotPathCounter;

    void Start()
    {
        planningThread = new Thread(main);
        planningThread.Start();
    }

    void Update()
    {
        if (arastar != null && arastar.getPath() != null && prevPlotPathCounter != arastar.getPlotPathCounter())
        {
            plotObstacles();
            plotPath();
            prevPlotPathCounter = arastar.getPlotPathCounter();
        }
    }

    public void main()
    {
        var s_start = Tuple.Create(5, 5);
        var s_goal = Tuple.Create(45, 25);
        arastar = new AraStar(s_start, s_goal, 2.5, "euclidean");
        var _tup_1 = arastar.searching();
        var path = _tup_1.Item1;
        var visited = _tup_1.Item2;
    }

    private void plotPath()
    {
        deletePreviousPath();
        List<Tuple<int, int>> path = arastar.getPath();
        HashSet<Tuple<int, int>> visited = arastar.getVisited();
        RenderPath renderPath = new RenderPath();
        renderPath.Draw(path, visited, transform, pathMaterial, pathPointsMaterial, visitedMaterial);
    }

    private void deletePreviousPath()
    {
        if (transform.Find("PathLine") != null)
        {
            Transform previousPath = transform.Find("PathLine");
            Destroy(previousPath.gameObject);
        }
        if (transform.Find("PathPoints") != null)
        {
            Transform previousPoints = transform.Find("PathPoints");
            Destroy(previousPoints.gameObject);
        }
        if (transform.Find("VisitedPoints") != null)
        {
            Transform previousVisited = transform.Find("VisitedPoints");
            Destroy(previousVisited.gameObject);
        }
    }

    private void plotObstacles()
    {
        HashSet<Tuple<int, int>> obs = arastar.getObs();
        RenderObstacles renderObstacles = new RenderObstacles();
        renderObstacles.Draw(obs, transform, obstMaterial);
    }

}
