// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    public sealed class Slot : IDisposable
    {
        public event Action<Slot> OnPushBlock = delegate { };
        public event Action<Slot> OnPullBlock = delegate { };

        public Block Block { get; private set; }
        public bool IsDisposed { get; private set; }

        public Slot(Block block = default) => Block = block;

        /// <summary>
        /// ブロックを設定
        /// </summary>
        public void PushBlock(Block block)
        {
            Assert.IsNull(Block, "既にブロック設置済みです、一度削除して設置してください");
            Block = block;
            OnPushBlock(this);
        }

        /// <summary>
        /// ブロックを取り外す
        /// </summary>
        public Block PullBlock()
        {
            if (Block != default)
            {
                var block = Block;
                Block = default;
                OnPullBlock(this);
                return block;
            }

            return default;
        }

        public bool HasBlock() => Block != default;
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            Block = default;
        }
    }
}