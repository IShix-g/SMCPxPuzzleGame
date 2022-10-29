// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using Random = UnityEngine.Random;

namespace LogicAndModel
{
    /// <summary>
    /// ブロックを選定して生成
    /// </summary>
    public sealed class SimpleBlockOrder : IBlockOrder
    {
        /// <summary>
        /// 各ブロックサイズに対する色
        /// </summary>
        const PieceColor _color2x2 = PieceColor.Blue;
        const PieceColor _color2x3 = PieceColor.Green;
        const PieceColor _color3x3 = PieceColor.Yellow;
        const PieceColor _color4x4 = PieceColor.Red;
        const PieceColor _color1xX = PieceColor.Orange;
        const PieceColor _colorXx1 = PieceColor.Purple;
        
        readonly BlockFactory _blockFactory = new ();
        
        /// <summary>
        /// ブロックの生成
        /// </summary>
        public Block Create(BlockType type, int id) => _blockFactory.Create(type, id, ConvertToColor(type));

        /// <summary>
        /// ブロックの生成
        /// </summary>
        public Block Create(Board board, int slotNumber, int slotLength)
            => Random.Range(0, 50) switch
            {
                > 5 and <= 12 => _blockFactory.Create2x3Random(_color2x3),
                <= 15 => _blockFactory.Create3x3Random(_color3x3),
                <= 18 => _blockFactory.Create4x4Random(_color4x4),
                <= 22 => _blockFactory.Create1xXRandom(_color1xX),
                <= 25 => _blockFactory.CreateXx1Random(_colorXx1),
                _ => _blockFactory.Create2x2Random(_color2x2)
            };

        /// <summary>
        /// 設置可能なブロックを取得 nullの可能性あり
        /// </summary>
        Block Create(Board board)
        {
            Block block = default;

            var index = 1;
            while (true)
            {
                try
                {
                    block = Create(BlockType.Block2x3, index);
                    index++;
                    if (block != default
                        && board.CanISetBlock(block))
                    {
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }

            if (block == default)
            {
                index = 1;
                while (true)
                {
                    try
                    {
                        block = Create(BlockType.Block2x2, index);
                        index++;
                        if (block != default
                            && board.CanISetBlock(block))
                        {
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            
            if (block == default)
            {
                index = 4;
                while (true)
                {
                    try
                    {
                        block = Create(BlockType.Block1xX, index);
                        index--;
                        if (block != default
                            && board.CanISetBlock(block))
                        {
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }
            }

            return block;
        }
        
        /// <summary>
        /// ブロックタイプからPieceColorに変更
        /// </summary>
        PieceColor ConvertToColor(BlockType type)
            => type switch
            {
                BlockType.Block1xX => _color1xX,
                BlockType.BlockXx1 => _colorXx1,
                BlockType.Block2x2 => _color2x2,
                BlockType.Block2x3 => _color2x3,
                BlockType.Block3x3 => _color3x3,
                BlockType.Block4x4 => _color4x4,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, default)
            };
    }
}