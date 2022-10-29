// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using LogicAndModel;
using UnityEngine;

namespace View
{
    [CreateAssetMenu(fileName = "PieceColors", menuName = "ScriptableObject/PieceColors", order = 0)]
    public class PieceColors : ScriptableObject
    {
        [SerializeField] ColorData[] _colors;
        
        [Serializable]
        public sealed class ColorData
        {
            public PieceColor ColorType;
            public Color Color;
        }

        public Color GetColor(PieceColor colorType)
        {
            foreach (var color in _colors)
            {
                if (color.ColorType == colorType)
                {
                    return color.Color;
                }
            }
            
#if DEBUG
            Debug.LogError($"存在しないカラー {colorType}");
#endif
            return default;
        }

        void Reset()
        {
            Color ConvertToColor(PieceColor colorType)
                => colorType switch
                {
                    PieceColor.White => Color.white,
                    PieceColor.Gray => Color.gray,
                    PieceColor.Black => Color.black,
                    PieceColor.Red => Color.red,
                    PieceColor.Green => Color.green,
                    PieceColor.Yellow => Color.yellow,
                    PieceColor.Blue => Color.blue,
                    PieceColor.Pink => Color.magenta,
                    PieceColor.Orange => Color.red,
                    PieceColor.Purple => Color.blue,
                    PieceColor.Brown => Color.red,
                    _ => Color.white
                };
            
            var list = new List<ColorData>();
            foreach (PieceColor type in Enum.GetValues(typeof(PieceColor)))
            {
                if (type != PieceColor.None)
                {
                    list.Add(new ColorData(){ ColorType = type, Color = ConvertToColor(type)});
                }
            }
            _colors = list.ToArray();
        }
    }
}