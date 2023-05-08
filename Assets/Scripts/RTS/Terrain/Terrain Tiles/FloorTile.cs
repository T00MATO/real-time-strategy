using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTS
{
    public class FloorTile : TerrainTile
    {
        [SerializeField]
        [Min(1)]
        private int floor = 1;
        public int Floor => floor;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Real-time strategy/Terrain/Floor tile")]
        private static void CreateAsset()
        {
            var path = EditorHelper.GetAssetPath("Save terrain floor tile", "New floor tile");

            AssetDatabase.CreateAsset(CreateInstance<FloorTile>(), path);
        }
#endif
    }
}
