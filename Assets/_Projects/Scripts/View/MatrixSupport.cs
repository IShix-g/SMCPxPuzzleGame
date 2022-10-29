// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using UnityEngine;

namespace View
{
    public static class MatrixSupport
    {
        /// <summary>
        /// Matrix上の中心座標を返す
        /// z座標は常に0
        /// </summary>
        public static Vector3 GetMatrixCenter(this Bounds bounds, int column, int row, int columnLength, int rowLength)
            => bounds.size.GetMatrixCenter(column, row, columnLength, rowLength);
        
        /// <summary>
        /// Matrix上の中心座標を返す
        /// z座標は常に0
        /// </summary>
        public static Vector3 GetMatrixCenter(this Vector3 size, int column, int row, int columnLength, int rowLength)
            => new Vector3(row * size.x, column * size.y) - (new Vector3((rowLength - 1) * size.x, (columnLength - 1) * size.y) / 2);
    }
}