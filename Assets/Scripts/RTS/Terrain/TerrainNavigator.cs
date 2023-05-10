using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public struct TerrainNaviNode
    {
        public float FromStart;
        public float FromEnd;
        public float TotalCost;

        public TerrainNaviNode(float fromStart, float fromEnd)
        {
            FromStart = fromStart;
            FromEnd = fromEnd;
            TotalCost = fromStart + fromEnd;
        }
    }

    public class TerrainNavigator : MonoBehaviour
    {
        private TerrainTileCache terrainTileCache;

        public void Setup(TerrainTileCache cache)
        {
            terrainTileCache = cache;
        }

        public Queue<Vector3Int> FindPathes(Vector3Int start, Vector3Int end)
        {
            throw new NotImplementedException();
        }
    }
}
