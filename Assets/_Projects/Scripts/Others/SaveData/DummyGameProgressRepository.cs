// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

#if DEBUG
using LogicAndModel;
using UnityEngine;

namespace Others
{
    /// <summary>
    /// 一切保存しないダミー
    /// ※ デバッグモードでしか動かないので注意
    /// </summary>
    public sealed class DummyGameProgressRepository : IGameProgressRepository
    {
        public DummyGameProgressRepository()
            => Debug.LogWarning($"{nameof(DummyGameProgressRepository)}が使われています。本番で使わないように注意してください。");
        
        public void Save(GameProgressDataModel data) {}

        public GameProgressDataModel Load() => new ();

        public void Delete() {}
    }
}
#endif