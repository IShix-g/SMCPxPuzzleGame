// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class SlotObject : MonoBehaviour
    {
        public Slot Slot{ get; private set; } 
        public BlockObject Block { get; private set; }

        BlockCreator _blockCreator;
        
        public void Initialize(BlockCreator blockCreator, Slot slot)
        {
            OnDestroy();
            _blockCreator = blockCreator;
            Slot = slot;
            Slot.OnPushBlock += OnPushBlock;

            if (slot.HasBlock())
            {
                SetBlock(slot.Block);
            }
        }

        void OnDestroy()
        {
            if (Slot != default)
            {
                Slot.OnPushBlock -= OnPushBlock;
            }
        }

        /// <summary>
        /// ブロックが設置された
        /// </summary>
        void OnPushBlock(Slot obj) => SetBlock(obj.Block);
        
        /// <summary>
        /// ブロックの設置
        /// </summary>
        void SetBlock(Block block)
        {
            Block = _blockCreator.CreateBlock(block);
            Block.transform.position = transform.position;
        }
        
        /// <summary>
        /// ブロックを取り外して取得
        /// </summary>
        public BlockObject PullBlock()
        {
            var block = Block;
            Block = default;
            Slot.PullBlock();
            return block;
        }

        /// <summary>
        /// ブロックを初期位置に戻す
        /// </summary>
        public void ResetPos() => Block.transform.position = transform.position;
    }
}