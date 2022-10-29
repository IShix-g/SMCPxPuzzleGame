// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;

namespace LogicAndModel
{
    public sealed class Block : IDisposable
    {
        public IReadOnlyPieceMatrix Pieces => _pieces;
        public int BlockID { get; private set; }
        public BlockType BlockType { get; private set; }
        public bool IsDisposed { get; private set; }

        PieceMatrix _pieces;

        public Block(BlockType blockType, int blockID, PieceMatrix pieces)
        {
            BlockType = blockType;
            BlockID = blockID;
            _pieces = pieces;
        }

        /// <summary>
        /// ピースを取り外して取得
        /// </summary>
        public PieceMatrix PullPieces()
        {
            var pieces = _pieces;
            _pieces = default;
            return pieces;
        }
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            _pieces?.Dispose();
        }
    }
}