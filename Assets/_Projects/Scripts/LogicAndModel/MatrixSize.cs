// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    /// <summary>
    /// 行列のサイズ
    /// </summary>
    public readonly struct MatrixSize : IEquatable<MatrixSize>
    {
        public readonly int Column;
        public readonly int Row;

        public MatrixSize(int column, int row)
        {
            Assert.IsTrue(column >= 1 && row >= 1, $"1以上を指定 column:{column} row:{row}");
            Column = column;
            Row = row;
        }

        public bool Equals(MatrixSize other) => Column == other.Column && Row == other.Row;
        public override bool Equals(object obj) => obj is MatrixSize other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Column, Row);

        public static bool operator ==(MatrixSize lhs, MatrixSize rhs) => lhs.Column == rhs.Column && lhs.Row == rhs.Row;
        public static bool operator !=(MatrixSize lhs, MatrixSize rhs) => !(lhs == rhs);
        
        public override string ToString() => $"({Column},{Row})";
    }
}