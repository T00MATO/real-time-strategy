using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RTS
{
    [RequireComponent(typeof(Grid), typeof(Tilemap))]
    public class TerrainManager : MonoBehaviour
    {
        [SerializeField]
        private Tilemap terrainTilemap;

        public Vector2[] FindPathes(Vector3Int start, Vector3Int end)
        {
            throw new NotImplementedException();
        }
    }
}
