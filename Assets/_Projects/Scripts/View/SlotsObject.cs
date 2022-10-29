// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using LogicAndModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace View
{
    public sealed class SlotsObject : MonoBehaviour
    {
        [SerializeField] BlockCreator _blockCreator;
        
        SlotObject[] _slots;

        public void SetSlots(Slots slots)
        {
            _slots = GetComponentsInChildren<SlotObject>();
            Assert.IsTrue(slots.Contents.Count == _slots.Length, $"数が合いません {slots.Contents.Count}/{_slots.Length}");

            for (var i = 0; i < _slots.Length; i++)
            {
                _slots[i].Initialize(_blockCreator, slots.Contents[i]);
            }
        }

        /// <summary>
        /// 対象のブロックを保持しているスロットを返す
        /// </summary>
        public SlotObject GetSlot(BlockObject obj)
        {
            foreach (var slot in _slots)
            {
                if (slot.Block == obj)
                {
                    return slot;
                }
            }
            return default;
        }
    }
}