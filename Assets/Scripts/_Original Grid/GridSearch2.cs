  using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Source https://github.com/lordjesus/Packt-Introduction-to-graph-algorithms-for-game-developers
/// </summary>
public class GridSearch2 {

    public struct SearchResult
    {
        public List<ContactPoint2D> Path { get; set; }
    }

    public static List<Point2> AStarSearch(Grid2 grid, Point2 startPosition, Point2 endPosition, bool isAgent = false)
    {
        List<Point2> path = new List<Point2>();

        List<Point2> positionsTocheck = new List<Point2>();
        Dictionary<Point2, float> costDictionary = new Dictionary<Point2, float>();
        Dictionary<Point2, float> priorityDictionary = new Dictionary<Point2, float>();
        Dictionary<Point2, Point2> parentsDictionary = new Dictionary<Point2, Point2>();

        positionsTocheck.Add(startPosition);
        priorityDictionary.Add(startPosition, 0);
        costDictionary.Add(startPosition, 0);
        parentsDictionary.Add(startPosition, null);

        while (positionsTocheck.Count > 0)
        {
            Point2 current = GetClosestVertex(positionsTocheck, priorityDictionary);
            positionsTocheck.Remove(current);
            if (current.Equals(endPosition))
            {
                path = GeneratePath(parentsDictionary, current);
                return path;
            }

            foreach (Point2 neighbour in grid.GetAdjacentCells(current, isAgent))
            {
                float newCost = costDictionary[current] + grid.GetCostOfEnteringCell(neighbour);
                if (!costDictionary.ContainsKey(neighbour) || newCost < costDictionary[neighbour])
                {
                    costDictionary[neighbour] = newCost;

                    float priority = newCost + ManhattanDiscance(endPosition, neighbour);
                    positionsTocheck.Add(neighbour);
                    priorityDictionary[neighbour] = priority;

                    parentsDictionary[neighbour] = current;
                }
            }
        }
        return path;
    }

    private static Point2 GetClosestVertex(List<Point2> list, Dictionary<Point2, float> distanceMap)
    {
        Point2 candidate = list[0];
        foreach (Point2 vertex in list)
        {
            if (distanceMap[vertex] < distanceMap[candidate])
            {
                candidate = vertex;
            }
        }
        return candidate;
    }

    private static float ManhattanDiscance(Point2 endPos, Point2 point)
    {
        return Math.Abs(endPos.X - point.X) + Math.Abs(endPos.Y - point.Y);
    }

    public static List<Point2> GeneratePath(Dictionary<Point2, Point2> parentMap, Point2 endState)
    {
        List<Point2> path = new List<Point2>();
        Point2 parent = endState;
        while (parent != null && parentMap.ContainsKey(parent))
        {
            path.Add(parent);
            parent = parentMap[parent];
        }
        return path;
    }
}
