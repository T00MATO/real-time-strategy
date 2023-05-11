using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RTS
{
    [RequireComponent(typeof(Grid), typeof(Tilemap), typeof(TerrainNavigator))]
    [ExecuteInEditMode]
    public class TerrainManager : MonoBehaviour
    {
        private static readonly Vector3 ISOMETRIC_CELL_SIZE = new Vector3(0.70710678118f, 0.70710678118f, 0f);
        private static readonly Color ISOMETRIC_CELL_OUTLINE_COLOR = new Color(1f, 1f, 1f, 0.5f);

        [SerializeField]
        private Grid terrainGrid;

        [SerializeField]
        private Tilemap terrainTilemap;

        [SerializeField]
        private TerrainNavigator terrainNavigator;

        private Dictionary<Vector3Int, TerrainTile> cachedTiles;

        private void Reset()
        {
            terrainGrid = GetComponent<Grid>();
            terrainTilemap = GetComponent<Tilemap>();
            terrainNavigator = GetComponent<TerrainNavigator>();
        }

        private void OnEnable()
        {
            Tilemap.tilemapTileChanged += OnTilemapChanged;
        }

        private void OnDisable()
        {
            Tilemap.tilemapTileChanged -= OnTilemapChanged;
        }

        private void OnValidate()
        {
            Setup();
        }

        private void Awake()
        {
            Setup();
        }

        private void OnDrawGizmosSelected()
        {
            var anchorMatrix = Matrix4x4.Translate(terrainTilemap.tileAnchor);
            var isoScaleMatrix = Matrix4x4.Scale(ISOMETRIC_CELL_SIZE);
            var rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, 45f));
            var scaleMatrix = Matrix4x4.Scale(terrainGrid.cellSize);

            var isometricMatrix = scaleMatrix * rotMatrix * isoScaleMatrix * anchorMatrix;

            foreach (var cellPosition in cachedTiles.Keys)
            {
                if (cellPosition.z == 0)
                    continue;
                Gizmos.matrix = isometricMatrix;

                Gizmos.color = new Color(0f, 0f, 0f, 0.5f);
                Gizmos.DrawWireCube(cellPosition, Vector3.one);
            }

            foreach (var (cellPosition, terrainTile) in cachedTiles)
            {
                var floorMatrix = Matrix4x4.Translate(new Vector3(0, cellPosition.z * terrainGrid.cellSize.y, 0));
                Gizmos.matrix = floorMatrix * isometricMatrix;

                Gizmos.color = GetTerrainGizmosColor(terrainTile);
                Gizmos.DrawCube(cellPosition, Vector3.one);

                Gizmos.color = ISOMETRIC_CELL_OUTLINE_COLOR;
                Gizmos.DrawWireCube(cellPosition, Vector3.one);
            }
        }

        private void OnTilemapChanged(Tilemap tilemap, Tilemap.SyncTile[] tiles)
        {
            if (terrainTilemap != tilemap)
                return;

            Setup();
        }

        private void Setup()
        {
            SetupTerrain();
            terrainNavigator.Setup(cachedTiles);
        }

        private void SetupTerrain()
        {
            cachedTiles = new Dictionary<Vector3Int, TerrainTile>();

            for (var cellX = terrainTilemap.cellBounds.xMin; cellX < terrainTilemap.cellBounds.xMax; cellX++)
            {
                for (var cellY = terrainTilemap.cellBounds.yMin; cellY < terrainTilemap.cellBounds.yMax; cellY++)
                {
                    var terrainTile = terrainTilemap.GetTile<TerrainTile>(new Vector3Int(cellX, cellY));
                    
                    if (terrainTile == null)
                        continue;

                    cachedTiles.Add(new Vector3Int(cellX, cellY, terrainTile.Floor), terrainTile);
                }
            }
        }

        private static Color GetTerrainGizmosColor(TerrainTile terrainTile)
        {
            if (terrainTile is FloorTile floorTile)
            {
                if (floorTile is StairTile)
                    return new Color(0f, 1f, 0f, 0.5f);

                return new Color(1f, 0f, 0f, 0.5f);
            }

            if (terrainTile is WallTile)
                return new Color(0f, 0f, 1f, 0.5f);

            throw new UnityException("Tile must be TerrainTile in Terrain Manager.");
        }

        //  TODO: 길찾기 메소드 구현 필요
        //public List<Vector3Int> FindPathes(Vector3Int start, Vector3Int end) => terrainNavigator.FindPathes(start, end);
    }
}
