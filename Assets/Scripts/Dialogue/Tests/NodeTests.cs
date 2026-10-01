using NUnit.Framework;
using System;
using Choice = Recalled.Dialogue.DialogueGraph.Node.Choice;

namespace Recalled.Dialogue.Tests
{
    public class NodeTests
    {
        [Test]
        public void Type_BeforeLinking_Throws()
        {
            var node = TestGraphs.NewNode("A");

            Assert.That(() => node.Type, Throws.InvalidOperationException);
        }

        [Test]
        public void Type_AfterEndNode_IsEnd()
        {
            var node = TestGraphs.NewNode("A").EndNode();

            Assert.That(node.Type, Is.EqualTo(Line.Types.End));
        }

        [Test]
        public void Type_AfterLinkNext_IsContinue()
        {
            var node = TestGraphs.NewNode("A").LinkNext(TestGraphs.Ending("B"));

            Assert.That(node.Type, Is.EqualTo(Line.Types.Continue));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Type_AfterAddChoices_IsChoicesRegardlessOfCount(int count)
        {
            var choices = new Choice[count];
            for (int i = 0; i < count; i++)
                choices[i] = new Choice($"Option {i}", TestGraphs.Ending($"Target {i}"));

            var node = TestGraphs.NewNode("Question").AddChoices(choices);

            Assert.That(node.Type, Is.EqualTo(Line.Types.Choices));
        }

        [Test]
        public void Choices_OnNonChoiceNode_IsEmptyNotNull()
        {
            var node = TestGraphs.NewNode("A").EndNode();

            Assert.That(node.Choices, Is.Not.Null.And.Empty);
        }

        [Test]
        public void LinkNext_AfterEndNode_Throws()
        {
            var node = TestGraphs.NewNode("A").EndNode();

            Assert.That(() => node.LinkNext(TestGraphs.Ending("B")), Throws.InvalidOperationException);
        }

        [Test]
        public void AddChoices_AfterLinkNext_Throws()
        {
            var node = TestGraphs.NewNode("A").LinkNext(TestGraphs.Ending("B"));
            var choices = new[] { new Choice("Yes", TestGraphs.Ending("C")) };

            Assert.That(() => node.AddChoices(choices), Throws.InvalidOperationException);
        }

        [Test]
        public void EndNode_AfterAddChoices_Throws()
        {
            var node = TestGraphs.NewNode("A")
                .AddChoices(new[] { new Choice("Yes", TestGraphs.Ending("B")) });

            Assert.That(() => node.EndNode(), Throws.InvalidOperationException);
        }

        [Test]
        public void LinkNext_Null_ThrowsArgumentNull()
        {
            var node = TestGraphs.NewNode("A");

            Assert.That(() => node.LinkNext(null), Throws.ArgumentNullException);
        }

        [Test]
        public void LinkNext_Null_ReportsParameterName()
        {
            var node = TestGraphs.NewNode("A");

            var exception = Assert.Throws<ArgumentNullException>(() => node.LinkNext(null));

            Assert.That(exception.ParamName, Is.EqualTo("next"));
        }

        [Test]
        public void AddChoices_Null_ThrowsArgumentException()
        {
            var node = TestGraphs.NewNode("A");

            Assert.That(() => node.AddChoices(null), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void AddChoices_Empty_ThrowsArgumentException()
        {
            var node = TestGraphs.NewNode("A");

            Assert.That(() => node.AddChoices(Array.Empty<Choice>()), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void AddChoices_ChoiceWithNullTarget_ThrowsArgumentException()
        {
            var node = TestGraphs.NewNode("A");
            var choices = new[] { new Choice("Leave", null) };

            Assert.That(() => node.AddChoices(choices), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void LinkNext_RejectedNull_LeavesNodeLinkable()
        {
            var node = TestGraphs.NewNode("A");

            Assert.Catch(() => node.LinkNext(null));

            Assert.That(() => node.EndNode(), Throws.Nothing);
        }

        [Test]
        public void AddChoices_RejectedEmpty_LeavesNodeLinkable()
        {
            var node = TestGraphs.NewNode("A");

            Assert.Catch(() => node.AddChoices(Array.Empty<Choice>()));

            Assert.That(() => node.EndNode(), Throws.Nothing);
        }

        [Test]
        public void AddChoices_SourceArrayChangedAfterwards_ChoicesUnaffected()
        {
            var original = TestGraphs.Ending("Original target");
            var replacement = TestGraphs.Ending("Replacement target");
            var choices = new[] { new Choice("Yes", original) };
            var node = TestGraphs.NewNode("Question").AddChoices(choices);

            choices[0] = new Choice("Hacked", replacement);

            Assert.That(node.Choices[0].Text, Is.EqualTo("Yes"));
            Assert.That(node.Choices[0].Next, Is.SameAs(original));
        }
    }
}
