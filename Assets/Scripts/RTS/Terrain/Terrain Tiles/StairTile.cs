#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTS
{
    public class StairTile : TerrainTile
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Real-time strategy/Terrain/Stair tile")]
        private static void CreateAsset()
        {
            AssetManager.CreateWithPanel<StairTile>("Save terrain stair tile", "New stair tile");
        }
#endif
    }
}
