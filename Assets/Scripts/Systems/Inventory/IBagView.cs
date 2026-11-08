using System;
using System.Collections.Generic;

namespace Recalled.Systems.Inventory
{
    public interface IBagView : IReadOnlyList<ItemInstance>, IView
    {
        int TotalCountOf(ItemDefinition def);
    }
}
