// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using UnityEngine.Assertions;

namespace View
{
    /// <summary>
    /// ドラッグ中オブジェクトのステータス監視
    /// ※ 同時にドラッグできるオブジェクトは1つのみ
    /// </summary>
    public sealed class DraggableObjectStatus : IDisposable, IDraggableObjectStatusListener
    {
        /// <summary>
        /// ドラッグの開始
        /// </summary>
        public event Action<DraggableObject> OnBeginDragEvent = delegate { };
        /// <summary>
        /// ドラッグ中
        /// </summary>
        public event Action<DraggableObject> OnDragEvent = delegate { };
        /// <summary>
        /// ドラッグの終了
        /// </summary>
        public event Action<DraggableObject> OnEndDragEvent = delegate { };
     
        public static DraggableObjectStatus Current { get; private set; }

        public bool IsActive { get; private set; } = true;
        public bool IsDragging { get; private set; }
        public DraggableObject DraggableObject { get; private set; }
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 初期化 ゲーム開始時に必ず呼ぶ
        /// </summary>
        public static void Initialize()
        {
            if (Current == default)
            {
                Current = new DraggableObjectStatus();
            }
        }

        /// <summary>
        /// 破棄 ゲーム終了時に必ず呼ぶ
        /// </summary>
        public static void Destroy()
        {
            Current?.Dispose();
            Current = default;
        }

        /// <summary>
        /// 有効/無効の切り替え
        /// </summary>
        public static void SetActive(bool isActive) => Current.IsActive = isActive;
        
        bool IDraggableObjectStatusListener.CanIStartDrag() => IsActive && !IsDragging;

        void IDraggableObjectStatusListener.OnBeginDrag(DraggableObject obj)
        {
            Assert.IsTrue(IsActive && !IsDragging, "1度に2つはドラッグできません");
            DraggableObject = obj;
            IsDragging = true;
            OnBeginDragEvent(obj);
        }
        
        void IDraggableObjectStatusListener.OnDrag(DraggableObject obj) => OnDragEvent(obj);

        void IDraggableObjectStatusListener.OnEndDrag(DraggableObject obj)
        {
            IsDragging = false;
            DraggableObject = default;
            OnEndDragEvent(obj);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            Current = default;
            DraggableObject = default;
        }
    }
}