// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace LogicAndModel
{
    /// <summary>
    /// 列または行の方向
    /// </summary>
    public enum MatrixLineDirection{ Vertical, Horizontal }
    
    /// <summary>
    /// 特定の1列または1行
    /// </summary>
    public sealed class MatrixLine : IEnumerable<(int Column, int Row)>
    {
        /// <summary>
        /// 方向
        /// </summary>
        public readonly MatrixLineDirection Direction;
        /// <summary>
        /// 対象の列または行
        /// </summary>
        public readonly int TargetColumnOrRow;
        /// <summary>
        /// 対象の列または行の数
        /// </summary>
        public readonly int Length;

        public MatrixLine(MatrixLineDirection direction, int targetColumnOrRow, MatrixSize size)
        {
            Direction = direction;
            TargetColumnOrRow = targetColumnOrRow;
            Length = direction == MatrixLineDirection.Vertical ? size.Row : size.Column;
        }

        public IEnumerator<(int Column, int Row)> GetEnumerator()
        {
            for (var i = 0; i < Length; i++)
            {
                yield return (
                    Direction == MatrixLineDirection.Vertical ? TargetColumnOrRow : i,
                    Direction == MatrixLineDirection.Horizontal ? TargetColumnOrRow : i
                );
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var msg = "-MatrixLine\n";
            foreach (var point in this)
            {
                msg += $"{point} ";
            }
            return msg;
        }
    }
}