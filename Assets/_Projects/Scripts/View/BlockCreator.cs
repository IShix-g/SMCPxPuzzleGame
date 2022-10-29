// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using LogicAndModel;
using UnityEngine;

namespace View
{
    public sealed class BlockCreator : MonoBehaviour
    {
        [SerializeField] BlockObject _blockPrefab;
        [SerializeField] PieceObject _piecePrefab;
        [SerializeField] PieceColors _pieceColors;

        public Vector3 PieceSize => _piecePrefab.Size;

        readonly Stack<PieceObject> _piecePool = new ();

        public BlockObject CreateBlock(Block block)
        {
            var obj = Instantiate<BlockObject>(_blockPrefab);
            obj.Initialize(this, block, CreatePieces(obj.gameObject.transform, block.Pieces));
            return obj;
        }

        public PieceObject CreatePiece(Piece piece)
        {
            PieceObject obj;
            if (_piecePool.Count > 0)
            {
                obj = _piecePool.Pop();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate<PieceObject>(_piecePrefab);
            }
            obj.Initialize(this, piece, _pieceColors.GetColor(piece.Color));
            return obj;
        }
        
        BlockPieceMatrix CreatePieces(Transform parent, IReadOnlyPieceMatrix pieces)
        {
            var result = new PieceObject[pieces.Size.Column, pieces.Size.Row];
            foreach (var point in pieces.GetAllPoints())
            {
                var piece = pieces.Get(point.Column, point.Row);
                var obj = CreatePiece(piece);
                obj.Initialize(this, piece, _pieceColors.GetColor(piece.Color));
                result[point.Column, point.Row] = obj;
            }
            return new BlockPieceMatrix(result, parent);
        }

        public void Release(BlockObject obj) => Destroy(obj.gameObject);
        
        public void Release(PieceObject obj)
        {
            obj.gameObject.SetActive(false);
            _piecePool.Push(obj);
        }
    }
}