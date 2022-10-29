// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace View
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] BoardObject _board;
        [SerializeField] SlotsObject _slots;
        
        /// <summary>
        /// 現在のゲーム
        /// </summary>
        public Game Current { get; private set; }

        [Inject]
        public void Inject(Game game)
        {
            Current = game;
            _board.SetBoard(Current.Board);
            _slots.SetSlots(Current.Slots);
        }

        void Awake() => DraggableObjectStatus.Initialize();

        void Start() => DraggableObjectStatus.Current.OnEndDragEvent += OnEndDragEvent;

        void OnDestroy()
        {
            Current.Dispose();
            DraggableObjectStatus.Current.OnEndDragEvent -= OnEndDragEvent;
            DraggableObjectStatus.Destroy();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Current.SaveProgressData();
            }
        }
        
        void OnApplicationQuit() => Current.SaveProgressData();

        /// <summary>
        /// ブロックのドラッグ終了
        /// </summary>
        void OnEndDragEvent(DraggableObject obj)
        {
            var block = (BlockObject) obj;
            var slot = _slots.GetSlot(block);
            var point = _board.GetSettableBlockPoint(block);
            if (point.Column >= 0
                && point.Row >= 0)
            {
                _board.SetBlock(slot.PullBlock(), point.Column, point.Row);
            }
            else
            {
                slot.ResetPos();
            }
        }
        
        /// <summary>
        /// ゲームのリトライ
        /// </summary>
        public void RetryGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}