// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.

namespace LogicAndModel
{
    /// <summary>
    /// ブロックのタイプ
    /// ※ データに影響が出るので数字を変更しない
    /// </summary>
    public enum BlockType
    {
        Block1xX = 0,
        BlockXx1 = 1,
        Block2x2 = 2,
        Block2x3 = 3,
        Block3x3 = 4,
        Block4x4 = 5,
    }
}