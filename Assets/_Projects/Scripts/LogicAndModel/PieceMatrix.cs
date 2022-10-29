// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

namespace LogicAndModel
{
    public sealed class PieceMatrix : Matrix<Piece>, IReadOnlyPieceMatrix
    {
        public PieceMatrix(Piece[,] objects) : base(objects) {}
        public PieceMatrix(MatrixSize size) : base(size) {}
    }
}