using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Node = Recalled.Systems.Dialogue.DialogueGraph.Node;

namespace Recalled.Systems.Dialogue
{
    public static class Parser
    {
        static EmotionSO[] AllEmotions;

        static Dictionary<int, Node> ParseNodes(DialogueDTO.Line[] lines)
        {
            Dictionary<int, Node> res = new(); // Create map of shallow nodes which are to be initialized
            foreach (var line in lines)
            {
                Line modelLine = new(line.text, AllEmotions.FirstOrDefault(e => e.Name == line.emotion));
                var node = new Node(modelLine);

                res.Add(line.id, node);
            }
            return res;
        }

        static Node.Choice[] ParseChoices(DialogueDTO.Choice[] choices, Dictionary<int, Node> nodes)
        {
            var res = new Node.Choice[choices.Length];
            for (int i = 0; i < choices.Length; ++i)
            {
                if (nodes.TryGetValue(choices[i].next, out var val))
                    res[i] = new(choices[i].text, val);
            }
            return res;
        }

        public static DialogueGraph Parse(string text)
        {
            AllEmotions = Resources.FindObjectsOfTypeAll<EmotionSO>();
            DialogueDTO dto = JsonUtility.FromJson<DialogueDTO>(text);

            SpeakerSO speaker = Resources.FindObjectsOfTypeAll<SpeakerSO>().FirstOrDefault(ds => ds.Name.Equals(dto.speaker, StringComparison.OrdinalIgnoreCase));
            if (speaker == null)
            {
                //speaker is absent
            }

            EmotionSO startingEmotion = AllEmotions.FirstOrDefault(e => e.Name == dto.emotion);
            if (startingEmotion == null)
            {
                // invalid emotion name
            }

            Dictionary<int, Node> nodeMap = ParseNodes(dto.lines);
            if (nodeMap.Count == 0)
            {
                // dialogue is empty?
            }

            Node startNode = nodeMap.Values.First();
            foreach (var line in dto.lines)
            {
                if (line.choices != null) nodeMap[line.id].AddChoices(ParseChoices(line.choices, nodeMap));
                else if (line.next != 0) nodeMap[line.id].LinkNext(nodeMap[line.next]);
                else if (line.end == 1) nodeMap[line.id].EndNode();
                else { /* Invalid line state */}
            }

            return new(speaker, startingEmotion, startNode, nodeMap.Values.ToList());
        }
    }
}
