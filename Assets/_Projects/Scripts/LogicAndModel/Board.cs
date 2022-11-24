// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    public sealed class Board : IDisposable
    {
        /// <summary>
        /// ブロックが設置された
        /// </summary>
        public event Action<(int Column, int Row)> OnSetBlock = delegate {};
        /// <summary>
        /// 列または行が削除された
        /// </summary>
        public event Action<List<MatrixLine>> OnDeletedLines = delegate {};
        
        /// <summary>
        /// ボードのサイズ
        /// </summary>
        public static readonly MatrixSize Size = new (10, 10);
        /// <summary>
        /// Pieces
        /// </summary>
        public IReadOnlyPieceMatrix Pieces => _pieces;
        public bool IsDisposed { get; private set; }
        
        readonly PieceMatrix _pieces;

        public Board(PieceMatrix pieces = default)
        {
            _pieces = pieces ?? new PieceMatrix(Size);

            Assert.IsTrue(_pieces.Size.Column == Size.Column && _pieces.Size.Row == Size.Row,
                $"縦、横のサイズが合ってません。Column:{_pieces.Size.Column}/{Size.Column} Row:{_pieces.Size.Row}/{Size.Row}");
        }

        /// <summary>
        /// ブロックの設置
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlock(Block block, int startColumn, int startRow)
        {
            _pieces.Merge(block.PullPieces(), startColumn, startRow);
            block.Dispose();
            OnSetBlock((startColumn, startRow));
        }

        /// <summary>
        /// 対象のブロックを削除
        /// </summary>
        public void DeleteBlocks(List<MatrixLine> lines)
        {
            foreach (var line in lines)
            {
                foreach (var point in line)
                {
                    if (_pieces.Has(point.Column, point.Row))
                    {
                        _pieces.Delete(point.Column, point.Row);
                    }
                }
            }
            OnDeletedLines(lines);
        }
        
        /// <summary>
        /// 対象のブロックが設置可能か？
        /// </summary>
        public bool CanISetBlock(Block block, int startColumn, int startRow) => _pieces.CanIMerge(block.Pieces, startColumn, startRow);
        
        /// <summary>
        /// 対象のブロックが設置可能か？
        /// </summary>
        public bool CanISetBlock(Block block)
        {
            for (var column = 0; column < _pieces.Size.Column; column++)
            {
                for (var row = 0; row < _pieces.Size.Row; row++)
                {
                    if (CanISetBlock(block, column, row))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            _pieces.Dispose();
        }
    }
}