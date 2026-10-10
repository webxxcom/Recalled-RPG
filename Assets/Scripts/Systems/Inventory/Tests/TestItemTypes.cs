namespace Recalled.Systems.Inventory.Tests
{
    /// <summary>
    /// Test-only ItemInstance subclass. Lets ItemSlotsArray tests verify that the inventory
    /// copies and creates items POLYMORPHICALLY (via Copy() / CreateInstance) instead of
    /// constructing plain ItemInstances, which would slice subclasses.
    /// </summary>
    public class TestItemInstance : ItemInstance
    {
        public TestItemInstance(ItemDefinition definition, int count) : base(definition, count) { }

        internal override ItemInstance Copy() => new TestItemInstance(Definition, Count);
    }
}
