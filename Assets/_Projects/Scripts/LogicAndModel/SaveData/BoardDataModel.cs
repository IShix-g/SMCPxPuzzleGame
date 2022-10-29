// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LogicAndModel
{
    [Serializable]
    public sealed class BoardDataModel
    {
        public int ColumnLength;
        public int RowLength;
        public List<PieceDataModel> Pieces;

        public BoardDataModel(Board board)
        {
            ColumnLength = board.Pieces.Size.Column;
            RowLength = board.Pieces.Size.Row;
            
            Pieces = new List<PieceDataModel>();
            foreach (var point in board.Pieces.GetAllPoints())
            {
                var piece = board.Pieces.Get(point.Column, point.Row);
                var data = new PieceDataModel(point.Column, point.Row, piece);
                Pieces.Add(data);
            }
        }
        
        [CanBeNull]
        public PieceMatrix GetPieceMatrix()
        {
            if (Pieces == default)
            {
                return default;
            }

            var pieces = new Piece[ColumnLength, RowLength];
            foreach (var piece in Pieces)
            {
                pieces[piece.Column, piece.Row] = new Piece(piece.Color);
            }
            return new PieceMatrix(pieces);
        }
    }
    
    [Serializable]
    public sealed class PieceDataModel
    {
        public int Column;
        public int Row;
        public PieceColor Color;

        public PieceDataModel(int column, int row, Piece piece)
        {
            Column = column;
            Row = row;
            Color = piece.Color;
        }
    }
}