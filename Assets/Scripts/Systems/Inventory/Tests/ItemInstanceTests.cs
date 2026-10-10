using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Recalled.Systems.Inventory.Tests
{
    public abstract class ItemTestBase
    {
        protected const int StackableMax = 999;
        protected const int NonStackableMax = 1;

        private readonly List<ItemDefinition> _createdDefinitions = new();

        [TearDown]
        public void DestroyCreatedDefinitions()
        {
            foreach (var definition in _createdDefinitions)
            {
                if (definition != null)
                    UnityEngine.Object.DestroyImmediate(definition);
            }
            _createdDefinitions.Clear();
        }

        protected ItemDefinition CreateDefinition(int maxStockSize)
        {
            var definition = ScriptableObject.CreateInstance<ItemDefinition>();
            definition.MaxStockSize = maxStockSize;
            _createdDefinitions.Add(definition);
            return definition;
        }
    }

    public class ItemInstanceConstructionTests : ItemTestBase
    {
        private ItemDefinition _potion;
        private ItemDefinition _sword;

        [SetUp]
        public void SetUp()
        {
            _potion = CreateDefinition(StackableMax);
            _sword = CreateDefinition(NonStackableMax);
        }

        [TestCase(1)]
        [TestCase(500)]
        [TestCase(StackableMax)]
        public void Ctor_StackableWithValidCount_StoresDefinitionAndCount(int count)
        {
            var instance = new ItemInstance(_potion, count);

            Assert.That(instance.Definition, Is.SameAs(_potion));
            Assert.That(instance.Count, Is.EqualTo(count));
        }

        [Test]
        public void Ctor_NonStackableWithCountOne_StoresDefinitionAndCount()
        {
            var instance = new ItemInstance(_sword, 1);

            Assert.That(instance.Definition, Is.SameAs(_sword));
            Assert.That(instance.Count, Is.EqualTo(1));
        }

        [Test]
        public void Ctor_NullDefinition_ThrowsArgumentNull()
        {
            Assert.That(() => new ItemInstance(null, 1), Throws.InstanceOf<ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Ctor_NonPositiveCount_Throws(int count)
        {
            Assert.That(() => new ItemInstance(_potion, count), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void Ctor_NonStackableWithCountAboveOne_Throws()
        {
            Assert.That(() => new ItemInstance(_sword, 2), Throws.InstanceOf<ArgumentException>());
        }

        // ASSUMPTION: an instance IS one stack, so a count above MaxStockSize is invalid and throws.
        // If AppendStock clamps and the constructor discards its overflow, items silently vanish.
        [Test]
        public void Ctor_StackableAboveMaxStock_Throws()
        {
            Assert.That(() => new ItemInstance(_potion, StackableMax + 1), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void ChangingCopy_DoesNotChangeOriginal()
        {
            var original = new ItemInstance(_potion, 5);
            var copy = original.Copy();

            var slots = new ItemSlotsArray(1);
            slots.Add(copy);
            slots.Add(_potion, 10);

            Assert.That(copy.Count, Is.EqualTo(15));     // sanity: the copy really was mutated
            Assert.That(original.Count, Is.EqualTo(5));
        }

        [Test]
        public void ChangingOriginal_DoesNotChangeCopy()
        {
            var original = new ItemInstance(_potion, 5);
            var copy = original.Copy();

            var slots = new ItemSlotsArray(1);
            slots.Add(original);
            slots.Add(_potion, 10);

            Assert.That(original.Count, Is.EqualTo(15)); // sanity: the original really was mutated
            Assert.That(copy.Count, Is.EqualTo(5));
        }

        // Only scans the assembly that defines ItemInstance. Subclasses declared in OTHER
        // assemblies are not covered — keep subclasses next to ItemInstance, or add their
        // assemblies to this list.
        private static IEnumerable<Type> ConcreteSubclasses() =>
            typeof(ItemInstance).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ItemInstance)));

        [Test]
        public void EveryConcreteSubclass_OverridesCopy()
        {
            var missing = ConcreteSubclasses()
                .Where(t =>
                {
                    var copy = t.GetMethod(nameof(ItemInstance.Copy),
                        BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    return copy == null || copy.DeclaringType != t;
                })
                .Select(t => t.FullName)
                .ToList();

            Assert.That(missing, Is.Empty,
                "These ItemInstance subclasses don't override Copy() and would be sliced: "
                + string.Join(", ", missing));
        }

        [Test]
        public void Copy_IsVirtual()
        {
            var copy = typeof(ItemInstance).GetMethod(nameof(ItemInstance.Copy),
                BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

            Assert.That(copy, Is.Not.Null, "ItemInstance.Copy() not found");
            Assert.That(copy.IsVirtual && !copy.IsFinal, Is.True, "Copy() must be overridable");
        }
    }

    // =====================================================================================
    // Copy() CONTRACT — every ItemInstance type must satisfy it.
    //
    // To cover a new subclass, add one fixture that inherits this class and overrides
    // CreateOriginal(). All contract tests then run against that type automatically.
    // =====================================================================================

    public abstract class ItemInstanceCopyContract : ItemTestBase
    {
        /// <summary>Build a valid, representative instance of the type under test.</summary>
        protected abstract ItemInstance CreateOriginal();

        [Test]
        public void Copy_ReturnsNewInstance()
        {
            var original = CreateOriginal();

            var copy = original.Copy();

            Assert.That(copy, Is.Not.Null);
            Assert.That(copy, Is.Not.SameAs(original));
        }

        // The slicing guard: a subclass that doesn't override Copy() would return a plain
        // ItemInstance and silently lose its own data.
        [Test]
        public void Copy_PreservesRuntimeType()
        {
            var original = CreateOriginal();

            var copy = original.Copy();

            Assert.That(copy.GetType(), Is.EqualTo(original.GetType()));
        }

        [Test]
        public void Copy_SharesDefinition_AndCopiesCount()
        {
            var original = CreateOriginal();

            var copy = original.Copy();

            Assert.That(copy.Definition, Is.SameAs(original.Definition)); // shared config, not copied
            Assert.That(copy.Count, Is.EqualTo(original.Count));
        }

        [Test]
        public void Copy_CalledTwice_ReturnsTwoDistinctInstances()
        {
            var original = CreateOriginal();

            var first = original.Copy();
            var second = original.Copy();

            Assert.That(first, Is.Not.SameAs(second));
        }

        [Test]
        public void Copy_OfCopy_IsStillCorrectType()
        {
            var original = CreateOriginal();

            var copyOfCopy = original.Copy().Copy();

            Assert.That(copyOfCopy.GetType(), Is.EqualTo(original.GetType()));
            Assert.That(copyOfCopy.Count, Is.EqualTo(original.Count));
        }
    }

    [TestFixture]
    public class StackableItemInstanceCopyTests : ItemInstanceCopyContract
    {
        protected override ItemInstance CreateOriginal() =>
            new ItemInstance(CreateDefinition(StackableMax), 42);
    }

    [TestFixture]
    public class NonStackableItemInstanceCopyTests : ItemInstanceCopyContract
    {
        protected override ItemInstance CreateOriginal() =>
            new ItemInstance(CreateDefinition(NonStackableMax), 1);
    }
}