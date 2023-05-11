using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTS
{
    public class FloorTile : TerrainTile
    {
        [SerializeField]
        [Min(0)]
        private int floor = 0;
        public override int Floor => floor;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Real-time strategy/Terrain/Floor tile")]
        private static void CreateAsset()
        {
            AssetManager.CreateWithPanel<FloorTile>("Save terrain floor tile", "New floor tile");
        }
#endif
    }
}
