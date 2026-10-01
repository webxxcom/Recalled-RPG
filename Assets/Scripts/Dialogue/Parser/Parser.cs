using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Dialogue
{
    internal static class Parser
    {
        static EmotionSO[] AllEmotions;

        static Dictionary<int, DialogueGraph.Node> ParseNodes(DialogueDTO.Line[] lines)
        {
            Dictionary<int, DialogueGraph.Node> res = new();
            foreach (var line in lines)
            {
                Line modelLine = new(line.text, AllEmotions.FirstOrDefault(e => e.Name == line.emotion));
                DialogueGraph.Node node = new(modelLine, null);

                res.Add(line.id, node);
            }
            return res;
        }

        static DialogueGraph.Node[] ParseChoices(DialogueDTO.Choice[] choices, Dictionary<int, DialogueGraph.Node> nodes)
        {
            DialogueGraph.Node[] res = new DialogueGraph.Node[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                if (nodes.TryGetValue(choices[i].next, out var val))
                    res[i] = new DialogueGraph.Node(new(choices[i].text, null), val);
                // choice.next may have invalid line id
            }
            return res;
        }

        public static DialogueGraph Parse(DialogueDefinition definition)
        {
            AllEmotions = Resources.FindObjectsOfTypeAll<EmotionSO>();
            DialogueGraph model = new();
            DialogueDTO dto = JsonUtility.FromJson<DialogueDTO>(definition.TextData);

            model.speaker = Resources.FindObjectsOfTypeAll<SpeakerSO>().FirstOrDefault(ds => ds.Name.Equals(dto.speaker, StringComparison.OrdinalIgnoreCase));
            if (model.speaker == null)
            {
                //speaker is absent
            }

            model.startingEmotion = AllEmotions.FirstOrDefault(e => e.Name == dto.emotion);
            if (model.startingEmotion == null)
            {
                // invalid emotion name
            }

            Dictionary<int, DialogueGraph.Node> nodeMap = ParseNodes(dto.lines);
            if (nodeMap.Count == 0)
            {
                // dialogue is empty?
            }

            model.StartLine = nodeMap.Values.First();
            foreach (var line in dto.lines)
            {
                switch (line.Type)
                {
                    case DialogueDTO.Line.Types.Choices:
                        nodeMap[line.id].AddNext(ParseChoices(line.choices, nodeMap));
                        break;
                    case DialogueDTO.Line.Types.Continue:
                        nodeMap[line.id].AddNext(nodeMap[line.next]);
                        break;
                    case DialogueDTO.Line.Types.End:
                        nodeMap[line.id].EndNode();
                        break;
                    default:
                        throw new InvalidLineStateException($"Unexpected error occured in {nameof(DialogueDTO.Line)}");
                }
            }
            model.Lines = nodeMap.Values.ToList();

            return model;
        }
    }
}
