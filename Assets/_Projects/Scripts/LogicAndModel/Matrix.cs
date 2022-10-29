// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace LogicAndModel
{
    /// <summary>
    /// 対象のクラスを2次元の配列(マトリックス)で管理
    /// </summary>
    public abstract class Matrix<T> : IReadOnlyMatrix<T>, IDisposable where T : class, IDisposable
    {
        public bool IsDisposed { get; private set; }
        public MatrixSize Size { get; private set; }
        public int ObjectCount { get; private set; }
        T[,] _objects;

        public Matrix(T[,] objects)
        {
            Assert.IsTrue(objects.Rank is >= 1 and <= 2, $"2次元以下の配列を渡してください。length:{objects.Rank}");
            _objects = objects;
            Size = new MatrixSize(_objects.GetLength(0), _objects.GetLength(1));
            ObjectCount = GetObjectLength();
        }

        public Matrix(MatrixSize size)
        {
            Size = size;
            _objects = new T[size.Column, size.Row];
        }

        protected virtual void OnSet(T obj, int column, int row){}
        protected virtual void OnPull(T obj, int column, int row){}
        protected virtual void OnDelete(T obj, int column, int row){}
        protected virtual void OnMerge(IReadOnlyMatrix<T> target, int startColumn, int startRow){}
        protected virtual void OnDispose(){}

        /// <summary>
        /// 設置
        /// </summary>
        public void Set(T obj, int column, int row)
        {
            Assert.IsTrue(IsInRange(column, row), $"指定されたcolumn,rowの値が範囲外です。column:{column},row:{row}");
            Assert.IsNull(_objects[column,row], $"既にオブジェクトが存在します。column:{column},row:{row}");
            ObjectCount++;
            _objects[column, row] = obj;
            OnSet(obj, column, row);
        }

        /// <summary>
        /// 取り外す
        /// </summary>
        public T Pull(int column, int row)
        {
            Assert.IsNotNull(_objects[column,row], $"存在しないオブジェクトに対してPullが実行されました。column:{column},row:{row}");
            var piece = _objects[column, row];
            _objects[column, row] = default;
            OnPull(piece, column, row);
            return piece;
        }
        
        /// <summary>
        /// 取得
        /// </summary>
        public T Get(int column, int row) => _objects[column, row];
        
        /// <summary>
        /// すべてのオブジェクトを取得
        /// </summary>
        public IEnumerable<T> GetAll()
        {
            for (var column = 0; column < Size.Column; column++)
            {
                for (var row = 0; row < Size.Row; row++)
                {
                    if (_objects[column, row] != default)
                    {
                        yield return _objects[column, row];
                    }
                }
            }
        }

        /// <summary>
        /// 存在するすべてのオブジェクトのポイントを返す
        /// </summary>
        public IEnumerable<(int Column, int Row)> GetAllPoints()
        {
            for (var column = 0; column < Size.Column; column++)
            {
                for (var row = 0; row < Size.Row; row++)
                {
                    if (_objects[column, row] != default)
                    {
                        yield return (column, row);
                    }
                }
            }
        }

        /// <summary>
        /// 存在するオブジェクトの数を取得
        /// </summary>
        int GetObjectLength()
        {
            var count = 0;
            for (var column = 0; column < Size.Column; column++)
            {
                for (var row = 0; row < Size.Row; row++)
                {
                    if (_objects[column, row] != default)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        
        /// <summary>
        /// 削除
        /// </summary>
        public void Delete(int column, int row)
        {
            var obj = _objects[column, row];
            _objects[column, row] = default;
            if (obj != default)
            {
                ObjectCount--;
                OnDelete(obj, column, row);
                obj.Dispose();
            }
        }

        /// <summary>
        /// 存在するか？
        /// </summary>
        public bool Has(int column, int row) => Get(column, row) != default;

        /// <summary>
        /// 設置する事ができるか？
        /// </summary>
        public bool CanISet(int column, int row) => !Has(column, row);

        /// <summary>
        /// 渡されたcolumnとrowがマトリックス内に収まるか？
        /// </summary>
        public bool IsInRange(int column, int row)
            => column >= 0 && row >= 0
               && Size.Column > column
               && Size.Row > row;

        /// <summary>
        /// マトリックスのマージ(和集合)
        /// ※ マージの際、すべてのオブジェクトが移動されるので注意
        /// </summary>
        public void Merge(Matrix<T> target, int startColumn, int startRow)
        {
            var targetColumn = 0;
            var targetRow = 0;
            var endColumn = startColumn + target.Size.Column;
            var endRow = startRow + target.Size.Row;
            
            for (var column = startColumn; column < endColumn; column++)
            {
                targetRow = 0;
                for (var row = startRow; row < endRow; row++)
                {
                    if (target.Has(targetColumn, targetRow))
                    {
                        Set(target.Pull(targetColumn, targetRow), column, row);
                    }

                    targetRow++;
                }
                targetColumn++;
            }
            OnMerge(target, startColumn, startRow);
        }
        
        /// <summary>
        /// マトリックスのマージが可能か？
        /// </summary>
        public bool CanIMerge(IReadOnlyMatrix<T> target, int startColumn, int startRow)
        {
            var endColumn = startColumn + target.Size.Column - 1;
            var endRow = startRow + target.Size.Row - 1;
            
            if (!IsInRange(endColumn, endRow))
            {
                return false;
            }
            
            var targetColumn = 0;
            var targetRow = 0;

            for (var column = startColumn; column <= endColumn; column++)
            {
                targetRow = 0;
                for (var row = startRow; row <= endRow; row++)
                {
                    if (target.Has(targetColumn, targetRow)
                        && !CanISet(column, row))
                    {
                        return false;
                    }
                    targetRow++;
                }
                targetColumn++;
            }

            return true;
        }

        /// <summary>
        /// 占有率を0-1の間で返す
        /// 1はすべての埋まった状態
        /// 0は1つも設置されていない状態
        /// </summary>
        public float GetOccupancyRate() => (float)ObjectCount / (Size.Column * Size.Row);

        public override string ToString()
        {
            var msg = $"- {GetType()} -\n";
            for (var column = 0; column < Size.Column; column++)
            {
                for (var row = 0; row < Size.Row; row++)
                {
                    msg += $"|{(Has(column,row) ? "O" : " ")}";
                }
                msg += "|\n";
            }
            return msg;
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            
            OnDispose();
            foreach (var obj in GetAll())
            {
                obj.Dispose();
            }
            _objects = default;
        }
    }
}