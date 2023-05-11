using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class TerrainNaviNode
    {
        public TerrainNaviNode ParentNode;
        public TerrainTile Tile;
        public Vector3Int Position;
        public int FromStart;
        public int FromEnd;
        public int TotalCost;
    }

    public class TerrainNavigator : MonoBehaviour
    {
        private const int STRAIGHT_COST = 10;
        private const int DIAGONAL_COST = 14;

        private static readonly (Vector3Int value, int cost)[] SEARCH_DIRECTIONS =
        {
            (new Vector3Int(-1, -1), DIAGONAL_COST),
            (new Vector3Int(0, -1), STRAIGHT_COST),
            (new Vector3Int(1, -1), DIAGONAL_COST),
            (new Vector3Int(-1, 0), STRAIGHT_COST),
            (new Vector3Int(1, 0), STRAIGHT_COST),
            (new Vector3Int(-1, 1),DIAGONAL_COST),
            (new Vector3Int(0, 1), STRAIGHT_COST),
            (new Vector3Int(1, 1), DIAGONAL_COST),
        };

        private Dictionary<Vector3Int, TerrainTile> cachedTiles;
        private Dictionary<Vector3Int, TerrainNaviNode> openedNodes;
        private Dictionary<Vector3Int, TerrainNaviNode> closedNodes;
        private Vector3Int endPosition;

        public void Setup(Dictionary<Vector3Int, TerrainTile> cache)
        {
            cachedTiles = cache;
        }

        public List<Vector3Int> FindPathes(Vector3Int start, Vector3Int end)
        {
            if (cachedTiles.TryGetValue(start, out var startTile))
            {
                if (startTile is not FloorTile)
                    throw new UnityException("Start tile should be floor tile or stair tile.");

                openedNodes = new Dictionary<Vector3Int, TerrainNaviNode>();
                closedNodes = new Dictionary<Vector3Int, TerrainNaviNode>();
                endPosition = end;

                var currentNode = CreateStartNode(startTile, start, end);
                openedNodes.Add(start, currentNode);

                while (currentNode.Position != end)
                {
                    openedNodes.Remove(currentNode.Position);
                    closedNodes.Add(currentNode.Position, currentNode);

                    var newNodes = SearchBorderNodes(start, currentNode);

                    if (newNodes.Count == 0)
                        break;

                    currentNode = GetBestNodes(currentNode, newNodes);
                }

                var pathes = new List<Vector3Int>();
                while (currentNode.ParentNode != null)
                {
                    pathes.Insert(0, currentNode.Position);
                    currentNode = currentNode.ParentNode;
                }

                return pathes;
            }

            throw new UnityException("Start tile is not exist.");
        }

        private TerrainNaviNode CreateStartNode(TerrainTile startTile, Vector3Int start, Vector3Int end)
        {
            var fromEnd = (end.x - start.x + end.y - start.y) * STRAIGHT_COST;

            return new TerrainNaviNode
            {
                ParentNode = null,
                Tile = startTile,
                Position = start,
                FromStart = 0,
                FromEnd = fromEnd,
                TotalCost = fromEnd,
            };
        }

        private List<TerrainNaviNode> SearchBorderNodes(Vector3Int position, TerrainNaviNode parentNode)
        {
            var newNodes = new List<TerrainNaviNode>();
            foreach (var direction in SEARCH_DIRECTIONS)
            {
                var searchPosition = position + direction.value;

                if (openedNodes.ContainsKey(searchPosition))
                    continue;

                if (closedNodes.ContainsKey(searchPosition))
                    continue;

                if (!cachedTiles.ContainsKey(searchPosition))
                    continue;

                var naviNode = CreateNaviNode(parentNode, searchPosition, direction.cost);
                AddNaviNode(naviNode, out var opened);
                if (opened)
                    newNodes.Add(naviNode);
            }

            return newNodes;
        }

        private TerrainNaviNode CreateNaviNode(TerrainNaviNode parentNode, Vector3Int position, int appendCost)
        {
            var terrainTile = cachedTiles[position];
            var fromStart = parentNode.FromStart + appendCost;
            var fromEnd = (endPosition.x - position.x + endPosition.y - position.y) * STRAIGHT_COST;

            return new TerrainNaviNode
            {
                ParentNode = parentNode,
                Tile = terrainTile,
                Position = position,
                FromStart = fromStart,
                FromEnd = fromEnd,
                TotalCost = fromStart + fromEnd,
            };
        }

        private void AddNaviNode(TerrainNaviNode naviNode, out bool opened)
        {
            opened = false;

            if (naviNode.Tile is FloorTile floorTile)
            {
                var parentNode = naviNode.ParentNode;

                if (floorTile is StairTile stairTile)
                {
                    if (stairTile.Floor == parentNode.Tile.Floor)
                    {
                        var upperPosition = new Vector3Int(naviNode.Position.x, naviNode.Position.y, stairTile.UpperFloor);
                        openedNodes.Add(upperPosition, naviNode);
                        opened = true;
                    }
                    else if (stairTile.UpperFloor == parentNode.Tile.Floor)
                    {
                        var lowerPosition = new Vector3Int(naviNode.Position.x, naviNode.Position.y, stairTile.Floor);
                        openedNodes.Add(lowerPosition, naviNode);
                        opened = true;
                    }
                    else
                    {
                        var blockedPosition = new Vector3Int(naviNode.Position.x, naviNode.Position.y, parentNode.Tile.Floor);
                        closedNodes.Add(blockedPosition, naviNode);
                    }
                }
                else
                {
                    if (floorTile.Floor == parentNode.Tile.Floor)
                    {
                        openedNodes.Add(naviNode.Position, naviNode);
                        opened = true;
                    }
                    else
                    {
                        closedNodes.Add(naviNode.Position, naviNode);
                    }
                }
            }
            else if (naviNode.Tile is WallTile)
            {
                closedNodes.Add(naviNode.Position, naviNode);
            }
        }

        private TerrainNaviNode GetBestNodes(TerrainNaviNode currentNode, List<TerrainNaviNode> newNodes)
        {
            foreach (var naviNode in newNodes)
            {
                if (currentNode.TotalCost > naviNode.TotalCost)
                {
                    currentNode = naviNode;
                }
                else if (currentNode.TotalCost == naviNode.TotalCost)
                {
                    if (currentNode.FromStart > naviNode.FromStart)
                        currentNode = naviNode;
                }
            }
            return currentNode;
        }
    }
}
