// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace LogicAndModel
{
    public interface IReadOnlyMatrix<T> where T : class, IDisposable
    {
        MatrixSize Size { get; }
        int ObjectCount { get; }
        T Get(int column, int row);
        IEnumerable<T> GetAll();
        IEnumerable<(int Column, int Row)> GetAllPoints();
        bool Has(int column, int row);
        bool CanISet(int column, int row);
        bool IsInRange(int column, int row);
        float GetOccupancyRate();
        string ToString();
    }
}