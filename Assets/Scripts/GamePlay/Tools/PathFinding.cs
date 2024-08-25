using System.Collections.Generic;
using GamePlay.Sys;
using UnityEngine;

namespace Tools
{
    public class PathfindingAStar
    {
        private TileSystem tileSystem;

        public PathfindingAStar(TileSystem _tileSystem)
        {
            tileSystem = _tileSystem;
        }

        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target, bool IsConsideringObstacle)
        {
            List<Vector3Int> path = new List<Vector3Int>();

            HashSet<Vector3Int> openSet = new HashSet<Vector3Int>(); // case à explorer
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>(); // case déjà exploré

            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
            Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

            openSet.Add(start); // Débuté l'exploration à la position de départ

            gScore[start] = 0;
            fScore[start] = Heuristic(start, target);

            // generation des path
            while (openSet.Count > 0)
            {
                Vector3Int current = GetLowestFScore(openSet, fScore);

                if (current == target)
                {
                    path = ReconstructPath(cameFrom, current);
                    break;
                }

                openSet.Remove(current);  closedSet.Add(current); // passer les tiles exploser dans closedSet
               
                foreach (Vector3Int neighbor in GetNeighbors(current, IsConsideringObstacle))
                {   
                    // check si on a déjà passer sur ce point
                    if (closedSet.Contains(neighbor))
                        continue; // pass à l'iteration suivante

                    float tentativeGScore = gScore[current] + Heuristic(current, neighbor);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
                }
            }

            return path;
        }

        private List<Vector3Int> GetNeighbors(Vector3Int current, bool IsConsideringObstacle)
        {
            List<Vector3Int> neighbors = new List<Vector3Int>();
            Vector3Int[] directions = new Vector3Int[] {
                Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down
            };

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;
                if (IsConsideringObstacle)
                {
                    Debug.Log("Considère obstacle");
                    if (IsTileActive(neighbor) && IsTileWalkable(neighbor))
                    {
                        neighbors.Add(neighbor);
                    }
                }
                else
                {
                    Debug.Log("Considère pas les obstacles");
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool IsTileActive(Vector3Int position)
        {
            return tileSystem.TileMap.GetTile(position) != null;
        }

        private bool IsTileWalkable(Vector3Int position)
        {
            Vector3 targetPosition = tileSystem.ConvertCellToWorldPosition(position);
            return Ground.VerifieWalkability(targetPosition);
        }
        private float Heuristic(Vector3Int a, Vector3Int b)
        {
            // Use Manhattan distance for heuristic
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private Vector3Int GetLowestFScore(HashSet<Vector3Int> openSet, Dictionary<Vector3Int, float> fScore)
        {
            Vector3Int lowest = new Vector3Int();
            float lowestScore = float.MaxValue;

            foreach (Vector3Int pos in openSet)
            {
                float score = fScore.GetValueOrDefault(pos, float.MaxValue);
                if (score < lowestScore)
                {
                    lowestScore = score;
                    lowest = pos;
                }
            }

            return lowest;
        }

        private List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            List<Vector3Int> totalPath = new List<Vector3Int> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }

            totalPath.Reverse();
            return totalPath;
        }
    }
}
