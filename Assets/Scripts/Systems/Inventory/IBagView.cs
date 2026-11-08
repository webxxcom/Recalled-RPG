using System;
using System.Collections.Generic;

namespace Recalled.Systems.Inventory
{
    public interface IBagView : IReadOnlyList<ItemInstance>
    {
        int TotalCountOf(ItemDefinition def);
        event Action Changed;
    }
}
