using NUnit.Framework;
using System;

namespace Recalled.Systems.Dialogue.Tests
{
    public class ConversationTests
    {
        // ---------- Construction ----------

        [Test]
        public void Constructor_DoesNotRaiseLineStartedOrStart()
        {
            var conversation = new Conversation(TestGraphs.Linear("A"));
            var recorder = new ConversationRecorder(conversation);

            Assert.That(recorder.Lines, Is.Empty);
            Assert.That(conversation.HasStarted, Is.False);
        }

        [Test]
        public void Constructor_NullGraph_ThrowsArgumentNull()
        {
            Assert.That(() => new Conversation(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Begin_RaisesLineStartedForStartLine()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            var recorder = new ConversationRecorder(conversation);

            conversation.Begin();

            Assert.That(recorder.Lines, Has.Count.EqualTo(1));
            Assert.That(recorder.LastText, Is.EqualTo("A"));
            Assert.That(conversation.HasStarted, Is.True);
        }

        [Test]
        public void Begin_CalledTwice_Throws()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            conversation.Begin();

            Assert.That(() => conversation.Begin(), Throws.InvalidOperationException);
        }

        [Test]
        public void Begin_CalledTwice_DoesNotRaiseStartLineAgain()
        {
            // The throw must happen BEFORE the event, not after it.
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            Assert.Catch<InvalidOperationException>(() => conversation.Begin());

            Assert.That(recorder.Lines, Has.Count.EqualTo(1));
        }

        [Test]
        public void Advance_BeforeBegin_Throws()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));

            Assert.That(() => conversation.Advance(), Throws.InvalidOperationException);
        }

        [Test]
        public void ChooseOption_BeforeBegin_Throws()
        {
            var conversation = new Conversation(TestGraphs.WithChoice());

            Assert.That(() => conversation.ChooseOption(0), Throws.InvalidOperationException);
        }

        [Test]
        public void Advance_OnContinueLine_RaisesLineStartedForNextLine()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            conversation.Advance();

            Assert.That(recorder.LastText, Is.EqualTo("B"));
        }

        [Test]
        public void Payload_OnContinueLine_HasContinueTypeAndNoChoices()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            var recorder = new ConversationRecorder(conversation);

            conversation.Begin();

            Assert.That(recorder.LastLine.Type, Is.EqualTo(Line.Types.Continue));
            Assert.That(recorder.LastLine.Choices, Is.Empty);
        }

        [Test]
        public void Advance_OnChoiceLine_Throws()
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            conversation.Begin();

            Assert.That(() => conversation.Advance(), Throws.InvalidOperationException);
        }

        [Test]
        public void Advance_OnChoiceLine_LeavesConversationUsable()
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            Assert.Catch<InvalidOperationException>(() => conversation.Advance());
            conversation.ChooseOption(0);

            Assert.That(recorder.LastText, Is.EqualTo("Said yes"));
        }

        [Test]
        public void Payload_OnChoiceLine_HasChoicesTypeAndOptionTextsInOrder()
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            var recorder = new ConversationRecorder(conversation);

            conversation.Begin();

            Assert.That(recorder.LastLine.Type, Is.EqualTo(Line.Types.Choices));
            Assert.That(recorder.LastLine.Choices, Is.EqualTo(new[] { "Yes", "No" }));
        }

        [Test]
        public void Payload_OnSingleOptionChoiceLine_HasChoicesType()
        {
            var conversation = new Conversation(TestGraphs.WithSingleChoice());
            var recorder = new ConversationRecorder(conversation);

            conversation.Begin();

            Assert.That(recorder.LastLine.Type, Is.EqualTo(Line.Types.Choices));
            Assert.That(recorder.LastLine.Choices, Is.EqualTo(new[] { "Okay" }));
        }

        [TestCase(0, "Said yes")]
        [TestCase(1, "Said no")]
        public void ChooseOption_ValidIndex_RaisesLineStartedForThatChoicesTarget(int index, string expected)
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            conversation.ChooseOption(index);

            Assert.That(recorder.LastText, Is.EqualTo(expected));
        }

        [Test]
        public void ChooseOption_OnContinueLine_Throws()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            conversation.Begin();

            Assert.That(() => conversation.ChooseOption(0), Throws.InvalidOperationException);
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void ChooseOption_IndexOutOfRange_ThrowsArgumentOutOfRange(int index)
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            conversation.Begin();

            Assert.That(() => conversation.ChooseOption(index),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ChooseOption_IndexOutOfRange_LeavesConversationUsable()
        {
            var conversation = new Conversation(TestGraphs.WithChoice());
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            Assert.Catch(() => conversation.ChooseOption(5));
            conversation.ChooseOption(1);

            Assert.That(recorder.LastText, Is.EqualTo("Said no"));
        }

        [Test]
        public void Payload_OnFinalLine_HasEndType()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();

            conversation.Advance();

            Assert.That(recorder.LastLine.Type, Is.EqualTo(Line.Types.End));
        }

        [Test]
        public void Advance_OnFinalLine_Throws()
        {
            var conversation = new Conversation(TestGraphs.Linear("A", "B"));
            conversation.Begin();
            conversation.Advance();

            Assert.That(() => conversation.Advance(), Throws.InvalidOperationException);
        }

        [Test]
        public void ChooseOption_LeadingBackToEarlierLine_RaisesLineStartedForThatLine()
        {
            var conversation = new Conversation(TestGraphs.WithLoop());
            var recorder = new ConversationRecorder(conversation);
            conversation.Begin();
            conversation.Advance();

            conversation.ChooseOption(0);

            Assert.That(recorder.LastText, Is.EqualTo("Start"));
        }
    }
}
