// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace LogicAndModel
{
    public sealed class Game : IDisposable
    {
        /// <summary>
        /// 1ラインを消すと獲得できる得点
        /// </summary>
        const int _deleteLineReward = 1;
        
        /// <summary>
        /// 得点獲得
        /// </summary>
        public event Action<(int Total, int Add)> OnCollectedPoint = delegate {};
        /// <summary>
        /// ゲームオーバー
        /// </summary>
        public event Action OnGameOver = delegate {};

        /// <summary>
        /// 現在の点数
        /// </summary>
        public int CurrentPoint { get; private set; }
        /// <summary>
        /// ボード
        /// </summary>
        public readonly Board Board;
        /// <summary>
        /// スロット
        /// </summary>
        public readonly Slots Slots;
        public bool IsDisposed { get; private set; }
        
        readonly IBlockOrder _blockOrder;
        readonly IGameProgressRepository _repository;

        public Game(IGameProgressRepository repository, IBlockOrder blockOrder)
        {
            _repository = repository;
            _blockOrder = blockOrder;

            var dataModel = repository.Load();
            Board = new Board(dataModel.Board?.GetPieceMatrix());
            Slots = new Slots(dataModel.Slots?.GetBlocks(blockOrder.Create));
            CurrentPoint = dataModel.CurrentPoint;

            Board.OnSetBlock += OnSetBlock;
            Slots.OnFullSlots += OnFullSlots;
            Slots.OnEmptySlots += CreateBlocksOnSlots;
            
            if (Slots.IsAllSlotEmpty())
            {
                CreateBlocksOnSlots();
            }
        }

        /// <summary>
        /// ボードにブロックを設置
        /// </summary>
        void OnSetBlock((int Column, int Row) point)
        {
            var deletableLines = GetDeletableLines(Board);
            // 削除可能な列または行がある場合
            if (deletableLines.Count > 0)
            {
                // 削除依頼
                Board.DeleteBlocks(deletableLines);
                // 得点の加算
                var reward = ConvertToPoint(deletableLines);
                CurrentPoint += reward;
                OnCollectedPoint((CurrentPoint, reward));
            }

            if (deletableLines.Count == 0
                && IsGameOver(Slots, Board))
            {
                OnGameOver();
                _repository.Delete();
            }
        }

        /// <summary>
        /// すべてのスロットにブロックが設置された
        /// </summary>
        void OnFullSlots()
        {
            if (IsGameOver(Slots, Board))
            {
                OnGameOver();
                _repository.Delete();
            }
        }

        /// <summary>
        /// すべてのスロットにブロックを生成
        /// </summary>
        void CreateBlocksOnSlots()
        {
            for (var i = 0; i < Slots.Contents.Count; i++)
            {
                var block = _blockOrder.Create(Board, i + 1, Slots.Contents.Count);
                Slots.Contents[i].PushBlock(block);
            }
        }

        /// <summary>
        /// ゲームオーバーかを判定
        /// </summary>
        bool IsGameOver(Slots slots, Board board)
        {
            // スロットのブロックがボードに置けなくなったらゲームオーバー
            foreach (var slot in slots.Contents)
            {
                if (slot.HasBlock()
                    && board.CanISetBlock(slot.Block))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ポイントに変換
        /// </summary>
        int ConvertToPoint(IList<MatrixLine> lines) => _deleteLineReward * lines.Count;
        
        /// <summary>
        /// 削除可能な行または列を取得
        /// </summary>
        List<MatrixLine> GetDeletableLines(Board board)
        {
            var lines = new List<MatrixLine>();

            for (var column = 0; column < board.Pieces.Size.Column; column++)
            {
                if (IsFullLine(board.Pieces, MatrixLineDirection.Vertical, column))
                {
                    lines.Add(new MatrixLine(MatrixLineDirection.Vertical, column, board.Pieces.Size));
                }
            }

            for (var row = 0; row < board.Pieces.Size.Row; row++)
            {
                if (IsFullLine(board.Pieces, MatrixLineDirection.Horizontal, row))
                {
                    lines.Add(new MatrixLine(MatrixLineDirection.Horizontal, row, board.Pieces.Size));
                }
            }
            
            return lines;
        }
    
        /// <summary>
        /// 行または列が1列埋まったか？
        /// </summary>
        bool IsFullLine(IReadOnlyPieceMatrix pieces, MatrixLineDirection direction, int targetColumnOrRow)
        {
            if (direction == MatrixLineDirection.Vertical)
            {
                for (var row = 0; row < pieces.Size.Row; row++)
                {
                    if (!pieces.Has(targetColumnOrRow, row))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (var column = 0; column < pieces.Size.Column; column++)
                {
                    if (!pieces.Has(column, targetColumnOrRow))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 進捗データの保存
        /// </summary>
        public void SaveProgressData()
        {
            if (!IsGameOver(Slots, Board))
            {
                var data = new GameProgressDataModel(CurrentPoint, Board, Slots);
                _repository.Save(data);
            }
        }
        
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;

            Board.OnSetBlock -= OnSetBlock;
            Slots.OnFullSlots -= OnFullSlots;
            Slots.OnEmptySlots -= CreateBlocksOnSlots;
            Board.Dispose();
            Slots.Dispose();
        }
    }
}