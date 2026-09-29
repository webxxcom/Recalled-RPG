using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Dialogue
{
    internal static class Parser
    {
        static EmotionSO[] AllEmotions;

        static Dictionary<int, DialogueModel.Line> ParseLines(DialogueDTO.Line[] lines)
        {
            Dictionary<int, DialogueModel.Line> res = new();
            foreach (var line in lines)
            {
                DialogueModel.Line modelLine =
                    new(line.id, line.text, AllEmotions.FirstOrDefault(e => e.Name == line.emotion));

                res.Add(modelLine.Id, modelLine);
            }
            return res;
        }

        static DialogueModel.Choice[] ParseChoices(DialogueDTO.Choice[] choices, Dictionary<int, DialogueModel.Line> lines)
        {
            List<DialogueModel.Choice> res = new();
            foreach (var choice in choices)
            {
                if (lines.TryGetValue(choice.next, out var val))
                    res.Add(new(choice.text, val));
                else throw new AbsentLineIdException();
            }
            return res.ToArray();
        }

        public static DialogueModel Parse(DialogueDefinition definition)
        {
            AllEmotions = Resources.FindObjectsOfTypeAll<EmotionSO>();
            DialogueModel model = new();
            DialogueDTO dto = JsonUtility.FromJson<DialogueDTO>(definition.TextData);

            model.speaker = Resources.FindObjectsOfTypeAll<DialogueSpeaker>().FirstOrDefault(ds => ds.Name.Equals(dto.speaker, StringComparison.OrdinalIgnoreCase));
            if (model.speaker == null)
                throw new AbsentSpeakerException($"Speaker with name {dto.speaker} was not found");

            model.startingEmotion = AllEmotions.FirstOrDefault(e => e.Name == dto.emotion);
            if (model.startingEmotion == null)
                throw new InvalidEmotionException($"Emotion with name {dto.emotion} was not found");

            Dictionary<int, DialogueModel.Line> lineMap = ParseLines(dto.lines);

            model.StartLine = lineMap.Values.First();
            for (int i = 0; i < dto.lines.Length; ++i)
            {
                var line = dto.lines[i];

                if (line.Type == DialogueDTO.Line.Types.Choices)
                    lineMap[line.id].ChoiceLine(ParseChoices(line.choices, lineMap));
                else if (line.Type == DialogueDTO.Line.Types.Continue)
                    lineMap[line.id].ContinueLine(lineMap[line.next]);
                else if (line.Type == DialogueDTO.Line.Types.End)
                    lineMap[line.id].EndLine();
                else throw new InvalidLineStateException($"Unexpected error occured in {nameof(DialogueDTO.Line)}");
            }
            model.Lines = lineMap.Values.ToList();

            return model;
        }
    }
}
