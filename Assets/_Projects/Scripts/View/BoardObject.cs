// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class BoardObject : MonoBehaviour
    {
        [SerializeField] BlockCreator _blockCreator;

        Board _board;
        BoardPieceMatrix _pieces;

        void OnDestroy()
        {
            if (_board != default)
            {
                _board.OnDeletedLines -= OnDeletedLines;
            }
        }

        public void SetBoard(Board board)
        {
            OnDestroy();
            
            _board = board;

            var objects = new PieceObject[board.Pieces.Size.Column, board.Pieces.Size.Row];
            foreach (var point in board.Pieces.GetAllPoints())
            {
                var piece = board.Pieces.Get(point.Column, point.Row);
                objects[point.Column, point.Row] = _blockCreator.CreatePiece(piece);
            }
            _pieces = new BoardPieceMatrix(objects, transform.position, _blockCreator.PieceSize);
            
            var objIndex = 0;
            for (var column = 0; column < board.Pieces.Size.Column; column++)
            {
                for (var row = 0; row < board.Pieces.Size.Row; row++)
                {
                    transform.GetChild(objIndex).transform.position = _pieces.GetBoardBounds(column, row).center;
                    objIndex++;
                }
            }
            
            board.OnDeletedLines += OnDeletedLines;
        }

        /// <summary>
        /// ラインが削除された
        /// </summary>
        void OnDeletedLines(List<MatrixLine> lines)
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
        }

        /// <summary>
        /// ブロックの設置
        /// </summary>
        public void SetBlock(BlockObject block, int startColumn, int startRow)
        {
            _pieces.Merge(block.PullPieces(), startColumn, startRow);
            _board.SetBlock(block.Block, startColumn, startRow);
            block.Dispose();
        }

        /// <summary>
        /// 対象のブロックが設置可能か？
        /// 設置不可の場合 (-1,-1)が返る
        /// </summary>
        public (int Column, int Row) GetSettableBlockPoint(BlockObject block)
        {
            var point = GetBlockPointOnMatrix(block);
            return point.Column >= 0
                   && point.Row >= 0
                   && _pieces.CanIMerge(block.Pieces, point.Column, point.Row) ? point : (-1,-1);
        }

        /// <summary>
        /// ブロックの位置からMatrix上の座標を返す
        /// </summary>
        (int Column, int Row) GetBlockPointOnMatrix(BlockObject block)
        {
            var leftTopCenter = block.transform.position + block.PieceSize.GetMatrixCenter(block.Pieces.Size.Column - 1, 0, block.Pieces.Size.Column, block.Pieces.Size.Row);
            
            for (var column = 0; column < _board.Pieces.Size.Column; column++)
            {
                for (var row = 0; row < _board.Pieces.Size.Row; row++)
                {
                    if (_pieces.GetBoardBounds(column, row).Contains(leftTopCenter))
                    {
                        return (column, row);
                    }
                }
            }
            return (-1, -1);
        }

        void OnDrawGizmosSelected()
        {
            if (_blockCreator == default)
            {
                return;
            }
            
            Gizmos.color = Color.blue;
            var parentPos = transform.position;
            for (var column = 0; column < Board.Size.Column; column++)
            {
                for (var row = 0; row < Board.Size.Row; row++)
                {
                    var center = _blockCreator.PieceSize.GetMatrixCenter(column, row, Board.Size.Column, Board.Size.Row);
                    Gizmos.DrawWireCube(parentPos + center, _blockCreator.PieceSize);
                }
            }
        }
    }
}