using System;
using System.Collections.Generic;

namespace Recalled.Systems.Inventory
{
    public interface IBagView : IReadOnlyList<ItemInstance>
    {
        int TotalCountOf(ItemDefinition def);
        void RequestDiscardItem(ItemInstance item);
        event Action Changed;
    }
}
