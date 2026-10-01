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
                var node = new DialogueGraph.Node(modelLine).EndNode();

                res.Add(line.id, node);
            }
            return res;
        }

        static DialogueGraph.Node.Choice[] ParseChoices(DialogueDTO.Choice[] choices, Dictionary<int, DialogueGraph.Node> nodes)
        {
            var res = new DialogueGraph.Node.Choice[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                if (nodes.TryGetValue(choices[i].next, out var val))
                    res[i] = new(choices[i].text, val);
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

            model.StartNode = nodeMap.Values.First();
            foreach (var line in dto.lines)
            {
                if (line.choices != null) nodeMap[line.id].AddChoices(ParseChoices(line.choices, nodeMap));
                else if (line.next != 0) nodeMap[line.id].LinkNext(nodeMap[line.next]);
                else if (line.end == 1) nodeMap[line.id].EndNode();
                else { /* Invalid line state */}
            }
            model.Lines = nodeMap.Values.ToList();

            return model;
        }
    }
}
