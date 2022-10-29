// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class BoardPieceMatrix : Matrix<PieceObject>, IReadOnlyPieceObjectMatrix
    {
        readonly Bounds[,] _boardBounds;

        public BoardPieceMatrix(PieceObject[,] objects, Vector3 center, Vector3 pieceSize) : base(objects)
        {
            _boardBounds = CreateBoardBounds(Size, center, pieceSize);

            foreach (var point in GetAllPoints())
            {
                Get(point.Column, point.Row).transform.position = GetBoardBounds(point.Column, point.Row).center;
            }
        }

        public BoardPieceMatrix(MatrixSize size, Vector3 center, Vector3 pieceSize) : base(size)
            => _boardBounds = CreateBoardBounds(size, center, pieceSize);

        protected override void OnSet(PieceObject obj, int column, int row)
            => obj.transform.position = GetBoardBounds(column, row).center;

        public Bounds GetBoardBounds(int column, int row) => _boardBounds[column, row];

        Bounds[,] CreateBoardBounds(MatrixSize size, Vector3 center, Vector3 pieceSize)
        {
            var result = new Bounds[size.Column, size.Row];
            for (var column = 0; column < Size.Column; column++)
            {
                for (var row = 0; row < Size.Row; row++)
                {
                    var centerPos = center + pieceSize.GetMatrixCenter((Size.Column - 1) - column, row, Size.Column, Size.Row);
                    result[column, row] = new Bounds(centerPos, pieceSize);
                }
            }
            return result;
        }
    }
}