// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using Others;
using NUnit.Framework;
using UnityEngine;

public class LogicAndModelTest
{
    [Test]
    public void PieceMatrix()
    {
        var matrix = new PieceMatrix(new MatrixSize(10, 10));
        var matrix2 = new PieceMatrix(new Piece[,]
        {
            { new (PieceColor.Black), default },
            { new (PieceColor.Black), new (PieceColor.Black)}
        });
        var matrix3 = new PieceMatrix(new Piece[,]
        {
            { new (PieceColor.Black), new (PieceColor.Black) },
            { default, new (PieceColor.Black)}
        });
        Debug.Log(matrix2);
        Debug.Log(matrix3);
        
        if (matrix.CanIMerge(matrix2, 8, 0))
        {
            matrix.Merge(matrix2, 8, 0);
        }

        if (matrix.CanIMerge(matrix3, 0, 1))
        {
            matrix.Merge(matrix3, 0, 1);
        }
        
        Debug.Log(matrix);
    }
    
    [Test]
    public void MatrixLine()
    {
        Debug.Log(new MatrixLine(MatrixLineDirection.Vertical, 3, new MatrixSize(10, 10)));
    }
    
    [Test]
    public void Board()
    {
        var board = new Board();
        var factory = new BlockFactory();

        {
            var block = factory.Create(BlockType.Block1xX, 5, PieceColor.Black);
            Debug.Log(block.Pieces);
            if (board.CanISetBlock(block, 0, 0))
            {
                board.SetBlock(block, 0, 0);
            }
        }

        {
            var block = factory.Create(BlockType.Block2x2, 5, PieceColor.Black);
            Debug.Log(block.Pieces);
            if (board.CanISetBlock(block, 1, 0))
            {
                board.SetBlock(block, 1, 0);
            }
        }
        
        Debug.Log(board.Pieces);
    }

    [Test]
    public void Game()
    {
        var game = new Game(new DummyGameProgressRepository(), new SimpleBlockOrder());
        var point = (Column: 0, Row: 0);
        
        foreach (var slot in game.Slots.Contents)
        {
            if (!slot.HasBlock())
            {
                continue;
            }

            Debug.Log(slot.Block.Pieces);
            
            for (var column = point.Column; column < game.Board.Pieces.Size.Column; column++)
            {
                for (var row = point.Row; row < game.Board.Pieces.Size.Row; row++)
                {
                    if (game.Board.CanISetBlock(slot.Block, point.Column, point.Row))
                    {
                        game.Board.SetBlock(slot.PullBlock(), point.Column, point.Row);
                        goto LOOP_END;
                    }
                    point.Row++;
                }
                point.Column++;
            }
            
            LOOP_END: ;
        }
        Debug.Log(game.Board.Pieces);
    }
}
