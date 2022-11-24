// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

#if DEBUG
using LogicAndModel;
using UnityEngine;

namespace Others
{
    /// <summary>
    /// PlayerPrefsは本番で使わない
    /// 重い、そもそもPlayerPrefsの用途にそぐわない
    /// ※ デバッグモードでしか動かないので注意
    /// </summary>
    public sealed class PlayerPrefsGameProgressRepository : IGameProgressRepository
    {
        const string _playerPrefsKey = "PlayerPrefsGameProgressRepository_SaveDataString";

        GameProgressDataModel _data;

        public PlayerPrefsGameProgressRepository()
        {
            var json = PlayerPrefs.GetString(_playerPrefsKey, string.Empty);
            _data = !string.IsNullOrEmpty(json)
                  ? JsonUtility.FromJson<GameProgressDataModel>(json)
                  : new GameProgressDataModel();
            
            Debug.LogWarning($"{nameof(PlayerPrefsGameProgressRepository)}が使われています。本番で使わないように注意してください。");
        }

        public void Save(GameProgressDataModel data)
        {
            _data = data;
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(_playerPrefsKey, json);
        }

        public GameProgressDataModel Load() => _data;

        public void Delete()
        {
            _data = new GameProgressDataModel();
            PlayerPrefs.DeleteKey(_playerPrefsKey);
        }
    }
}
#endif