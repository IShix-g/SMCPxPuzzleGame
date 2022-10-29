// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

namespace LogicAndModel
{
    /// <summary>
    /// ブロックを選定して生成
    /// </summary>
    public interface IBlockOrder
    {
        public Block Create(BlockType type, int id);
        public Block Create(Board board, int slotNumber, int slotLength);
    }
}