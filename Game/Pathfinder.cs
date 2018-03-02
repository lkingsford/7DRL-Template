using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class Pathfinder
    {
        public static List<XY> FindPath(bool[,] walkableMap, XY start, XY end, int limit = 20, bool failureAllowed = true)
        {
            // Ported from pseudocode on https://en.wikipedia.org/wiki/A*_search_algorithm
            // Readily copy pasted in
            var width = walkableMap.GetLength(0);
            var height = walkableMap.GetLength(1);

            //    // The set of nodes already evaluated.
            //    closedSet := {}
            var closedSet = new bool[width, height];
            //    // The set of currently discovered nodes that are not evaluated yet.
            //    // Initially, only the start node is known.
            //    openSet := {start}
            var openSet = new List<XY>();
            openSet.Add(start);
            //    // For each node, which node it can most efficiently be reached from.
            //    // If a node can be reached from many nodes, cameFrom will eventually contain the
            //    // most efficient previous step.
            //    cameFrom := the empty map
            var cameFrom = new Dictionary<XY, XY>();

            //    // For each node, the cost of getting from the start node to that node.
            //    gScore := map with default value of Infinity
            var gscore = new int[width, height];
            for (var ix = 0; ix < width; ++ix)
            {
                for (var iy = 0; iy < height; ++iy)
                {
                    gscore[ix, iy] = int.MaxValue;
                }
            }
            //    // The cost of going from start to start is zero.
            //    gScore[start] := 0 
            gscore[start.X, start.Y] = 0;
            //    // For each node, the total cost of getting from the start node to the goal
            //    // by passing by that node. That value is partly known, partly heuristic.
            //    fScore := map with default value of Infinity
            var fscore = new int[width, height];
            for (var ix = 0; ix < width; ++ix)
            {
                for (var iy = 0; iy < height; ++iy)
                {
                    fscore[ix, iy] = int.MaxValue;
                }
            }
            //    // For the first node, that value is completely heuristic.
            //    fScore[start] := heuristic_cost_estimate(start, goal)
            fscore[start.X, start.Y] = Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);

            XY nearestPoint = null;
            var nearestDistance = int.MaxValue;

            //    while openSet is not empty
            while (openSet.Count > 0)
            {
                //        current := the node in openSet having the lowest fScore[] value
                var current = openSet.Aggregate((curMin, i) => (fscore[i.X, i.Y] < fscore[curMin.X, curMin.Y]) ? i : curMin);
                //        if current = goal
                // Special case for one off if unwalkable (as most common opt needed if fail)
                if (walkableMap[end.X, end.Y] && current.Equals(end))
                {
                    //            return reconstruct_path(cameFrom, current)
                    //    total_path := [current]
                    var totalPath = new List<XY>();
                    totalPath.Add(current);
                    //    while current in cameFrom.Keys:
                    while (cameFrom.Any(i => i.Key.Equals(current)))
                    {
                        //        current := cameFrom[current]
                        current = cameFrom[current];
                        //        total_path.append(current)
                        totalPath.Insert(0, current);
                    }
                    return totalPath;
                }
                else if (!walkableMap[end.X, end.Y] && nearestDistance <= 2)
                {
                    break;
                }

                //        openSet.Remove(current)
                openSet.Remove(current);
                //        closedSet.Add(current)
                closedSet[current.X, current.Y] = true;
                //        for each neighbor of current
                var neighbours = new List<XY>();
                neighbours.Add(new XY(current.X - 1, current.Y - 1));
                neighbours.Add(new XY(current.X + 1, current.Y - 1));
                neighbours.Add(new XY(current.X, current.Y - 1));
                neighbours.Add(new XY(current.X - 1, current.Y));
                neighbours.Add(new XY(current.X + 1, current.Y));
                neighbours.Add(new XY(current.X - 1, current.Y + 1));
                neighbours.Add(new XY(current.X + 1, current.Y + 1));
                neighbours.Add(new XY(current.X, current.Y + 1));
                // Is this more efficient then branches? I dunno!
                neighbours = neighbours.Where(i =>
                                              (i.X >= 0) &&
                                              (i.X < width) &&
                                              (i.Y >= 0) &&
                                              (i.Y < height) &&
                                              (walkableMap[i.X, i.Y]) &&
                                              (Math.Abs(i.X - end.X) + Math.Abs(i.Y - end.Y) <= limit)
                                              ).ToList();
                foreach (var neighbour in neighbours)
                {
                    //            if neighbor in closedSet
                    if (closedSet[neighbour.X, neighbour.Y])
                    {
                        //                continue		// Ignore the neighbor which is already evaluated.
                        continue;
                    }
                    //            // The distance from start to a neighbor
                    //            tentative_gScore := gScore[current] + dist_between(current, neighbor)
                    var tentativeGScore = gscore[current.X, current.Y] + 1; //1 is always distance for now
                    //            if neighbor not in openSet	// Discover a new node
                    if (!openSet.Any(i => i.Equals(neighbour)))
                    {
                        //                openSet.Add(neighbor)
                        openSet.Add(neighbour);
                    }
                    //            else if tentative_gScore >= gScore[neighbor]
                    else if (tentativeGScore >= gscore[neighbour.X, neighbour.Y])
                    {
                        //                continue		// This is not a better path.
                        continue;
                    }

                    //            // This path is the best until now. Record it!
                    //            cameFrom[neighbor] := current
                    cameFrom[neighbour] = current;
                    //            gScore[neighbor] := tentative_gScore
                    gscore[neighbour.X, neighbour.Y] = tentativeGScore;
                    //            fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)
                    var estimatedDistance = Math.Abs(neighbour.X - end.X) + Math.Abs(neighbour.Y - end.Y);
                    fscore[neighbour.X, neighbour.Y] = estimatedDistance;
                    if (estimatedDistance < nearestDistance)
                    {
                        nearestPoint = neighbour;
                    }
                }
            }

            if (!failureAllowed)
            {
                // Go close as possible
                var totalPath = new List<XY>();
                var current = nearestPoint;
                totalPath.Add(current);
                //    while current in cameFrom.Keys:
                while (cameFrom.Any(i => i.Key.Equals(current)))
                {
                    //        current := cameFrom[current]
                    current = cameFrom[current];
                    //        total_path.append(current)
                    totalPath.Insert(0, current);
                }
                return totalPath;
            }
            else
            {
                return null;
            }
        }
    }
}