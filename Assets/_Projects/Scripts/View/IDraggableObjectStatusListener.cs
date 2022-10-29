// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

namespace View
{
    /// <summary>
    /// ドラッグ中オブジェクトから呼ばれる情報
    /// </summary>
    public interface IDraggableObjectStatusListener
    {
        bool CanIStartDrag();
        void OnBeginDrag(DraggableObject obj);
        void OnDrag(DraggableObject obj);
        void OnEndDrag(DraggableObject obj);
    }
}