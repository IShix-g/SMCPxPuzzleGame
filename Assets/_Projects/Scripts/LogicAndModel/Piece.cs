// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    public sealed class Piece : IDisposable
    {
        public readonly PieceColor Color;

        public Piece(PieceColor color)
        {
            Assert.IsFalse(color == PieceColor.None, "必ず色を設定してください");
            Color = color;
        }

        public void Dispose() {}
    }
}