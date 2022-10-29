// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace View
{
    /// <summary>
    /// オブジェクトのドラッグ
    /// ※ 同時にドラッグできるオブジェクトは1つのみ
    /// </summary>
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// ドラッグ中のオブジェクトのY座標に加算する値 (ドラッグ中に指で隠れる為)
        /// </summary>
        const float _dragYOffset = 1;
        
        /// <summary>
        /// ドラッグ中か？
        /// </summary>
        public bool IsDragging { get; private set; }
        /// <summary>
        /// 現在のポジション
        /// </summary>
        public Vector3 CurrentPos { get; private set; }
        
        public Transform Transform { get; private set; }

        IDraggableObjectStatusListener _listener;
        Camera _mainCamera;

        /// <summary>
        /// ドラッグの開始
        /// </summary>
        protected virtual void OnBeginDrag(){}
        /// <summary>
        /// ドラッグ中
        /// </summary>
        protected virtual void OnDrag(){}
        /// <summary>
        /// ドラッグの終了
        /// </summary>
        protected virtual void OnEndDrag(){}
        
        protected virtual void Awake()
        {
            _mainCamera = Camera.main;
            Transform = transform;
        }
        
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _listener = DraggableObjectStatus.Current;
            Assert.IsNotNull(_listener, "DraggableObjectStatusがありません。DraggableObjectStatus.Initialize()をManagerなどで読んでください。");
            
            IsDragging = _listener.CanIStartDrag();
            if (IsDragging)
            {
                CurrentPos = Transform.position;
                OnBeginDrag();
                _listener.OnBeginDrag(this);
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                MoveTo(eventData);
                OnDrag();
                _listener.OnDrag(this);
            }
        }
        
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (IsDragging)
            {
                IsDragging = false;
                MoveTo(eventData);
                OnEndDrag();
                _listener.OnEndDrag(this);
            }
        }

        void MoveTo(PointerEventData eventData)
        {
            var pos = _mainCamera.ScreenToWorldPoint(eventData.position);
            if (pos != Vector3.zero)
            {
                pos.y += _dragYOffset;
                pos.z = 0;
                Transform.position = CurrentPos = Vector3.Lerp(Transform.position, pos, 0.5f);
            }
        }
    }
}