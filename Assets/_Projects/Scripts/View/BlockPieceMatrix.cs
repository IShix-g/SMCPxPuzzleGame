// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using UnityEngine;

namespace View
{
    public class BlockPieceMatrix : Matrix<PieceObject>, IReadOnlyPieceObjectMatrix
    {
        Transform _parent;

        public BlockPieceMatrix(PieceObject[,] objects, Transform parent) : base(objects)
        {
            _parent = parent;

            foreach (var point in GetAllPoints())
            {
                var piece = Get(point.Column, point.Row);
                SetObject(piece, point.Column, point.Row);
            }
        }
        
        public BlockPieceMatrix(MatrixSize size, Transform parent) : base(size) => _parent = parent;

        protected override void OnSet(PieceObject obj, int column, int row) => SetObject(obj, column, row);

        protected override void OnDispose() => _parent = default;

        void SetObject(PieceObject piece, int column, int row)
        {
            piece.transform.SetParent(_parent);
            var center = piece.Size.GetMatrixCenter((Size.Column - 1) - column, row, Size.Column, Size.Row);
            piece.transform.position = _parent.position + center;
        }
        
        /// <summary>
        /// すべてのピースに親要素を設定
        /// </summary>
        public void SetParentAll(Transform target)
        {
            foreach (var piece in GetAll())
            {
                piece.transform.SetParent(target);
            }
        }

        /// <summary>
        /// すべてのピースのOrderInLayerの設定
        /// </summary>
        public void SetOrderInLayerAll(int layer)
        {
            foreach (var piece in GetAll())
            {
                piece.SetOrderInLayer(layer);
            }
        }
        
        /// <summary>
        /// すべてのピースのOrderInLayerを初期値に戻す
        /// </summary>
        public void ResetOrderInLayerAll()
        {
            foreach (var piece in GetAll())
            {
                piece.ResetOrderInLayer();
            }
        }
        
        /// <summary>
        /// すべてのサイズを取得
        /// </summary>
        public Vector3 GetPieceSize()
        {
            foreach (var piece in GetAll())
            {
                return piece.Size;
            }
            return Vector3.zero;
        }
    }
}