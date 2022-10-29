// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class PieceObject : MonoBehaviour, IDisposable
    {
        [SerializeField] SpriteRenderer _render;
        
        public Piece Piece { get; private set; }
        public Vector3 Size => _render.bounds.size;
        public bool IsDisposed { get; private set; }

        BlockCreator _creator;
        int _startOrderInLayer;
        
        public void Initialize(BlockCreator creator, Piece piece, Color color)
        {
            IsDisposed = false;
            _creator = creator;
            Piece = piece;
            _render.color = color;
            _startOrderInLayer = _render.sortingOrder;
        }

        public void SetOrderInLayer(int layer) => _render.sortingOrder = layer;
        
        public void ResetOrderInLayer() => _render.sortingOrder = _startOrderInLayer;

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            Piece = default;
            ResetOrderInLayer();
            _creator.Release(this);
        }
    }
}