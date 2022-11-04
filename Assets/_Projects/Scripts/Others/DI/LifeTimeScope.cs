// SMCPxPuzzleGame:
// Copyright (c) 2022 IShix All rights reserved.
// Licensed under the GNU Affero General Public License v3.0. See LICENSE file in the project root for full license information.
// VContainer:
// Copyright (c) 2020 hadashiA
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using LogicAndModel;
using VContainer;
using VContainer.Unity;

namespace Others
{
    public class LifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IGameProgressRepository, PlayerPrefsGameProgressRepository>(Lifetime.Scoped);
            builder.Register<IBlockOrder, SimpleBlockOrder>(Lifetime.Scoped);
            builder.Register<Game>(Lifetime.Scoped);
        }
    }
}