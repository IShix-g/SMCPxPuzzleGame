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
        public event Action<DraggableObject> OnBeginDrag = delegate { };
        /// <summary>
        /// ドラッグ中
        /// </summary>
        public event Action<DraggableObject> OnDrag = delegate { };
        /// <summary>
        /// ドラッグの終了
        /// </summary>
        public event Action<DraggableObject> OnEndDrag = delegate { };
     
        public static DraggableObjectStatus Current { get; private set; }

        public bool IsActive { get; private set; } = true;
        public bool IsDragging { get; private set; }
        public DraggableObject DraggableObject { get; private set; }
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 初期化 ゲーム開始時に必ず呼ぶ
        /// </summary>
        public static void Initialize() => Current ??= new DraggableObjectStatus();

        /// <summary>
        /// 破棄 ゲーム終了時に必ず呼ぶ
        /// </summary>
        public static void Release()
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
            OnBeginDrag(obj);
        }
        
        void IDraggableObjectStatusListener.OnDrag(DraggableObject obj) => OnDrag(obj);

        void IDraggableObjectStatusListener.OnEndDrag(DraggableObject obj)
        {
            IsDragging = false;
            DraggableObject = default;
            OnEndDrag(obj);
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