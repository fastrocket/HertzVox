using System;
using UnityEngine;

namespace Hertzole.HertzVox
{
    [Serializable]
    public struct BlockPos
    {
        public int x, y, z;

        public BlockPos(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //Overriding GetHashCode and Equals gives us a faster way to
        //compare two positions and we have to do that a lot
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 47;

                hash = hash * 227 + x.GetHashCode();
                hash = hash * 227 + y.GetHashCode();
                hash = hash * 227 + z.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (GetHashCode() == obj.GetHashCode())
                return true;

            return false;
        }

        //returns the position of the chunk containing this block
        public BlockPos ContainingChunkCoordinates()
        {
            int x = Mathf.FloorToInt(this.x / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;
            int y = Mathf.FloorToInt(this.y / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;
            int z = Mathf.FloorToInt(this.z / (float)Chunk.CHUNK_SIZE) * Chunk.CHUNK_SIZE;

            return new BlockPos(x, y, z);
        }

        public BlockPos Add(int x, int y, int z)
        {
            return new BlockPos(this.x + x, this.y + y, this.z + z);
        }

        public BlockPos Add(BlockPos pos)
        {
            return new BlockPos(this.x + pos.x, this.y + pos.y, this.z + pos.z);
        }

        public BlockPos Subtract(BlockPos pos)
        {
            return new BlockPos(this.x - pos.x, this.y - pos.y, this.z - pos.z);
        }

        //BlockPos and Vector3 can be substituted for one another
        public static implicit operator BlockPos(Vector3 v)
        {
            BlockPos blockPos = new BlockPos(
                Mathf.RoundToInt(v.x / HertzVoxConfig.BlockSize),
                Mathf.RoundToInt(v.y / HertzVoxConfig.BlockSize),
                Mathf.RoundToInt(v.z / HertzVoxConfig.BlockSize)
                );

            return blockPos;
        }

        public static implicit operator Vector3(BlockPos pos)
        {
            return new Vector3(pos.x, pos.y, pos.z) * HertzVoxConfig.BlockSize;
        }

        public static implicit operator BlockPos(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return new BlockPos(0, 1, 0);
                case Direction.Down:
                    return new BlockPos(0, -1, 0);
                case Direction.North:
                    return new BlockPos(0, 0, 1);
                case Direction.East:
                    return new BlockPos(1, 0, 0);
                case Direction.South:
                    return new BlockPos(0, 0, -1);
                case Direction.West:
                    return new BlockPos(-1, 0, 0);
                default:
                    return new BlockPos();
            }
        }

        //These operators let you add and subtract BlockPos from each other
        //or check equality with == and !=
        public static BlockPos operator -(BlockPos pos1, BlockPos pos2)
        {
            return pos1.Subtract(pos2);
        }

        public static BlockPos operator +(BlockPos pos1, BlockPos pos2)
        {
            return pos1.Add(pos2);
        }

        public static bool operator ==(BlockPos pos1, BlockPos pos2)
        {
            return Equals(pos1, pos2);
        }

        public static bool operator !=(BlockPos pos1, BlockPos pos2)
        {
            return !Equals(pos1, pos2);
        }

        //You can safely use BlockPos as part of a string like this:
        //"block at " + BlockPos + " is broken."
        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }
    }
}
