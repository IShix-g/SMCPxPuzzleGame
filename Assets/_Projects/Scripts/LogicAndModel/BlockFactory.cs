// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using Random = UnityEngine.Random;

namespace LogicAndModel
{
    /// <summary>
    /// ブロックの生成
    /// </summary>
    public sealed class BlockFactory
    {
        public Block Create1xXRandom(PieceColor color) => Create(BlockType.Block1xX, Random.Range(1,6), color);
        public Block CreateXx1Random(PieceColor color) => Create(BlockType.BlockXx1, Random.Range(1,5), color);
        public Block Create2x2Random(PieceColor color) => Create(BlockType.Block2x2, Random.Range(1,6), color);
        public Block Create2x3Random(PieceColor color) => Create(BlockType.Block2x3, Random.Range(1,3), color);
        public Block Create3x3Random(PieceColor color) => Create(BlockType.Block3x3, Random.Range(1,6), color);
        public Block Create4x4Random(PieceColor color) => Create(BlockType.Block4x4, Random.Range(1,5), color);
        
        public Block Create(BlockType type, int id, PieceColor color)
        {
            switch (type)
            {
                // 1 x length のブロック生成
                // 例) 1 x 3の場合
                // ■ ■ ■ 
                case BlockType.Block1xX:
                    return id switch
                    {
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color)} })),
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color), new(color)} })),
                        3 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color), new(color), new(color)} })),
                        4 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color), new(color), new(color), new(color)} })),
                        5 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color), new(color), new(color), new(color), new(color)} })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                // length x 1 のブロック生成
                // 例) 3 x 1の場合
                // ■
                // ■
                // ■
                case BlockType.BlockXx1:
                    return id switch
                    {
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color)} })),
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color)}, {new(color)} })),
                        3 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color)}, {new(color)}, {new(color)} })),
                        4 => new Block(type, id, new PieceMatrix(new Piece[,]{ {new(color)}, {new(color)}, {new(color)}, {new(color)} })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                // 2 x 2 のブロック生成
                case BlockType.Block2x2:
                    return id switch
                    {
                        //■ ■
                        //■ ■
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color)},
                            {new(color), new(color)}
                        })),
                        //■
                        //■ ■
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), default},
                            {new(color), new(color)}
                        })),
                        //■ ■
                        //■
                        3 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color)},
                            {new(color), default}
                        })),
                        //■ ■
                        //  ■
                        4 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color)},
                            {default, new(color)}
                        })),
                        //  ■
                        //■ ■
                        5 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {default, new(color)},
                            {new(color), new(color)}
                        })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                // 2 x 3 のブロック生成
                case BlockType.Block2x3:
                    return id switch
                    {
                        //  ■
                        //■ ■ ■
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {default, new(color), default},
                            {new(color), new(color), new(color)}
                        })),
                        //■ ■ ■
                        //  ■
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color)},
                            {default, new(color), default}
                        })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                // 3 x 3 のブロック生成
                case BlockType.Block3x3:
                    return id switch
                    {
                        //■ ■ ■
                        //■ ■ ■
                        //■ ■ ■
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color)},
                            {new(color), new(color), new(color)},
                            {new(color), new(color), new(color)}
                        })),
                        //■
                        //■
                        //■ ■ ■
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), default, default},
                            {new(color), default, default},
                            {new(color), new(color), new(color)}
                        })),
                        //■ ■ ■
                        //■
                        //■
                        3 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color)},
                            {new(color), default, default},
                            {new(color), default, default}
                        })),
                        //■ ■ ■
                        //    ■
                        //    ■
                        4 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color)},
                            {default, default, new(color)},
                            {default, default, new(color)}
                        })),
                        //    ■
                        //    ■
                        //■ ■ ■
                        5 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {default, default, new(color)},
                            {default, default, new(color)},
                            {new(color), new(color), new(color)}
                        })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                // 4 x 4 のブロック生成
                case BlockType.Block4x4:
                    return id switch
                    {
                        //■
                        //■
                        //■
                        //■ ■ ■ ■
                        1 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), default, default, default},
                            {new(color), default, default, default},
                            {new(color), default, default, default},
                            {new(color), new(color), new(color), new(color)}
                        })),
                        //■ ■ ■ ■
                        //■
                        //■
                        //■
                        2 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color), new(color)},
                            {new(color), default, default, default},
                            {new(color), default, default, default},
                            {new(color), default, default, default}
                        })),
                        //■ ■ ■ ■
                        //      ■
                        //      ■
                        //      ■
                        3 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {new(color), new(color), new(color), new(color)},
                            {default, default, default, new(color)},
                            {default, default, default, new(color)},
                            {default, default, default, new(color)}
                        })),
                        //      ■
                        //      ■
                        //      ■
                        //■ ■ ■ ■
                        4 => new Block(type, id, new PieceMatrix(new Piece[,]
                        {
                            {default, default, default, new(color)},
                            {default, default, default, new(color)},
                            {default, default, default, new(color)},
                            {new(color), new(color), new(color), new(color)}
                        })),
                        _ =>  throw new ArgumentOutOfRangeException(nameof(id), id, default)
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, default);
            }
        }
    }
}