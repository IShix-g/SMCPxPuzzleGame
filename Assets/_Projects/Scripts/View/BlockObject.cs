// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class BlockObject : DraggableObject, IDisposable
    {
        /// <summary>
        /// ドラッグ中の画像のOrderInLayer
        /// </summary>
        const int _dragOrderInLayer = 10;
        
        public Block Block { get; private set; }
        public IReadOnlyPieceObjectMatrix Pieces => _pieces;
        public Vector3 PieceSize => _pieces.GetPieceSize();
        public bool IsDisposed { get; private set; }

        BlockPieceMatrix _pieces;
        BlockCreator _blockCreator;

        public void Initialize(BlockCreator blockCreator, Block block, BlockPieceMatrix pieces)
        {
            IsDisposed = false;
            _blockCreator = blockCreator;
            Block = block;
            _pieces = pieces;
        }
        
        /// <summary>
        /// ピースを取り外して取得
        /// </summary>
        public BlockPieceMatrix PullPieces()
        {
            _pieces.SetParentAll(default);
            var pieces = _pieces;
            _pieces = default;
            return pieces;
        }

        protected override void OnBeginDrag() => _pieces.SetOrderInLayerAll(_dragOrderInLayer);

        protected override void OnEndDrag() => _pieces.ResetOrderInLayerAll();
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            _pieces?.Dispose();
            _pieces = default;
            Block = default;
            _blockCreator.Release(this);
        }
    }
}