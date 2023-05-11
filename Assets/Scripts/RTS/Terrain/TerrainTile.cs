using UnityEngine.Tilemaps;

namespace RTS
{
    public abstract class TerrainTile : Tile
    {
        public virtual int Floor => 0;
    }
}