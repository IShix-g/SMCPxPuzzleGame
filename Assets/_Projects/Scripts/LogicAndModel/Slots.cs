// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    public sealed class Slots : IDisposable
    {
        /// <summary>
        /// スロットの数
        /// </summary>
        public const int SlotsLength = 3;
        
        /// <summary>
        /// すべてのスロットが埋まった
        /// </summary>
        public event Action OnFullSlots = delegate {};
        /// <summary>
        /// すべてのスロットが空になった
        /// </summary>
        public event Action OnEmptySlots = delegate {};

        public IList<Slot> Contents => _contents;
        public bool IsDisposed { get; private set; }

        readonly Slot[] _contents;

        public Slots(IList<Block> blocks = default)
        {
            _contents = new Slot[SlotsLength];
            for (var i = 0; i < _contents.Length; i++)
            {
                var slot = new Slot(blocks?[i]);
                _contents[i] = slot;
                slot.OnPushBlock += OnPushBlock;
                slot.OnPullBlock += OnPullBlock;
            }
            Assert.IsTrue(blocks == default || blocks.Count == SlotsLength, $"blocksの数が合いません。{blocks?.Count}/{SlotsLength}");
        }

        /// <summary>
        /// ブロックが設置された
        /// </summary>
        void OnPushBlock(Slot slot)
        {
            foreach (var content in _contents)
            {
                if (!content.HasBlock())
                {
                    return;
                }
            }
            OnFullSlots();
        }

        /// <summary>
        /// ブロックが外された
        /// </summary>
        void OnPullBlock(Slot slot)
        {
            if (IsAllSlotEmpty())
            {
                OnEmptySlots();
            }
        }
        
        /// <summary>
        /// すべてのスロットが空
        /// </summary>
        public bool IsAllSlotEmpty()
        {
            foreach (var slot in Contents)
            {
                if (slot.HasBlock())
                {
                    return false;
                }
            }
            return true;
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            
            foreach (var content in _contents)
            {
                content.OnPushBlock -= OnPushBlock;
                content.OnPullBlock -= OnPullBlock;
                content.Dispose();
            }
        }
    }
}