using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RTS
{
    public class StairTile : FloorTile
    {
        [SerializeField]
        [Min(0)]
        private int upperFloor = 1;
        public int UpperFloor => upperFloor;

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Real-time strategy/Terrain/Stair tile")]
        private static void CreateAsset()
        {
            AssetManager.CreateWithPanel<StairTile>("Save terrain stair tile", "New stair tile");
        }
#endif
    }
}
