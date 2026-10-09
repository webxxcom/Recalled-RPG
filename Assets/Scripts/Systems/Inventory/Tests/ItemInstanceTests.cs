using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Recalled.Systems.Inventory.Tests
{
    public class ItemInstanceTests
    {
        private const int StackableMax = 999;
        private const int NonStackableMax = 1;

        private readonly List<ItemDefinition> _createdDefinitions = new();

        private ItemDefinition _potion;
        private ItemDefinition _sword;

        [SetUp]
        public void SetUp()
        {
            _potion = CreateDefinition(StackableMax);
            _sword = CreateDefinition(NonStackableMax);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var definition in _createdDefinitions)
            {
                if (definition != null)
                    UnityEngine.Object.DestroyImmediate(definition);
            }
            _createdDefinitions.Clear();
        }

        private ItemDefinition CreateDefinition(int maxStockSize)
        {
            var definition = ScriptableObject.CreateInstance<ItemDefinition>();
            definition.MaxStockSize = maxStockSize;
            _createdDefinitions.Add(definition);
            return definition;
        }

        // ---------------------------------------------------------------- valid construction

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

        // ---------------------------------------------------------------- invalid construction

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
        // The constructor currently discards AppendStock's overflow (`out var _`); if AppendStock
        // clamps, `new ItemInstance(potion, 5000)` silently becomes 999 and 4001 items vanish.
        // This test fails until the constructor rejects it.
        [Test]
        public void Ctor_StackableAboveMaxStock_Throws()
        {
            Assert.That(() => new ItemInstance(_potion, StackableMax + 1), Throws.InstanceOf<ArgumentException>());
        }
    }
}
