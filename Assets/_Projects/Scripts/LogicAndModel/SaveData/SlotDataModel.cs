// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using JetBrains.Annotations;

namespace LogicAndModel
{
    [Serializable]
    public sealed class SlotsDataModel
    {
        public SlotDataModel[] Contents;

        public SlotsDataModel(Slots slots)
        {
            Contents = new SlotDataModel[slots.Contents.Count];

            for (var i = 0; i < slots.Contents.Count; i++)
            {
                var slot = slots.Contents[i];
                if (slot.HasBlock())
                {
                    Contents[i] = new SlotDataModel(slot.Block.BlockType, slot.Block.BlockID);
                }
            }
        }
        
        [CanBeNull]
        public Block[] GetBlocks(Func<BlockType, int, Block> createBlock)
        {
            if (Contents == default)
            {
                return default;
            }

            var blocks = new Block[Contents.Length];
            for (var i = 0; i < blocks.Length; i++)
            {
                if (Contents[i] != default
                    && Contents[i].BlockID > 0)
                {
                    blocks[i] = createBlock(Contents[i].Type, Contents[i].BlockID);
                }
            }
            return blocks;
        }
    }
    
    [Serializable]
    public sealed class SlotDataModel
    {
        public BlockType Type;
        public int BlockID;

        public SlotDataModel(BlockType type, int blockID)
        {
            Type = type;
            BlockID = blockID;
        }
    }
}