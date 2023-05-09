#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTS
{
    public class WallTile : TerrainTile
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Real-time strategy/Terrain/Wall tile")]
        private static void CreateAsset()
        {
            AssetManager.CreateWithPanel<WallTile>("Save terrain wall tile", "New wall tile");
        }
#endif
    }
}
