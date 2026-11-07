using Recalled.Systems.Inventory;

namespace Recalled.UI
{
    internal interface IInventoryItemView
    {
        ItemInstance Displayed { get; }
        void ShowItem(ItemInstance item, bool showDisabled);
        void ShowEmpty();
    }
}
