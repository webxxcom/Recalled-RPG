using Recalled.Systems.Inventory;

namespace Recalled.Gameplay
{
    public interface IItemProcessor
    {
        bool Consume(ItemDefinition item);
        bool Consume(ItemInstance item);
        bool TryDiscard(ItemInstance item);
    }
}
