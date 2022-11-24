// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using TMPro;
using UnityEngine;

namespace View
{
    public sealed class PointText : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] TextMeshProUGUI _text;

        void Start()
        {
            _text.text = _gameManager.Current.CurrentPoint.ToString();
            _gameManager.Current.OnCollectedPoint += OnCollectedPoint;
        }

        void OnDestroy()
        {
            if (_gameManager != default
                && _gameManager.Current != default)
            {
                _gameManager.Current.OnCollectedPoint -= OnCollectedPoint;
            }
        }

        void OnCollectedPoint((int Total,int Add) point) => _text.text = point.Total.ToString();

        void Reset()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _text = GetComponent<TextMeshProUGUI>();
        }
    }
}