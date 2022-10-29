// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using JetBrains.Annotations;

namespace LogicAndModel
{
    [Serializable]
    public sealed class GameProgressDataModel
    {
        public int CurrentPoint;
        [CanBeNull] public BoardDataModel Board;
        [CanBeNull] public SlotsDataModel Slots;

        public GameProgressDataModel(){}
        
        public GameProgressDataModel(int currentPoint, Board board, Slots slots)
        {
            CurrentPoint = currentPoint;
            Board = new BoardDataModel(board);
            Slots = new SlotsDataModel(slots);
        }
    }
}