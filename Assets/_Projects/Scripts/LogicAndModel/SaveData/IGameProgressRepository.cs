// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

namespace LogicAndModel
{
    /// <summary>
    /// ゲームの進捗データ
    /// </summary>
    public interface IGameProgressRepository
    {
        void Save(GameProgressDataModel data);
        GameProgressDataModel Load();
        void Delete();
    }
}