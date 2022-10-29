// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public sealed class GameOverUI : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] RectTransform _parent;
        [SerializeField] Button _button;
        
        void Start()
        {
            _parent.gameObject.SetActive(false);
            _button.onClick.AddListener(_gameManager.RetryGame);
            _gameManager.Current.OnGameOver += OnGameOver;
        }

        void OnDestroy()
        {
            if (_gameManager != default
                && _gameManager.Current != default)
            {
                _gameManager.Current.OnGameOver -= OnGameOver;
            }
        }

        void OnGameOver() => _parent.gameObject.SetActive(true);

        void Reset() => _gameManager = FindObjectOfType<GameManager>();
    }
}