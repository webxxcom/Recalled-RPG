namespace Recalled.Systems.Inventory.Tests
{
    /// <summary>
    /// Test-only definition whose factory produces TestItemInstance.
    /// Kept in its own file named after the class: Unity expects that for ScriptableObject types.
    /// </summary>
    public class TestItemDefinition : ItemDefinition
    {
        public override ItemInstance CreateInstance(int count = 1) => new TestItemInstance(this, count);
    }
}
