﻿using System;
using System.Text;
using System.IO;
using Ink;
using Ink.Parsed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace InkPlugin
{
    // Finds all choices within the story, and produce a JSON file with their contents,
    // including in which file they were defined.
    internal class ChoiceListPlugin : Ink.IPlugin
    {
        public ChoiceListPlugin ()
        {
        }

        public void PostParse(Ink.Parsed.Story parsedStory)
        {
            // Nothing
        }

        public void PostExport(Ink.Parsed.Story parsedStory, Ink.Runtime.Story runtimeStory)
        {
            var choiceJsonArray = new JArray ();

            var allChoices = parsedStory.FindAll<Choice>();
            foreach (Ink.Parsed.Choice choice in allChoices) {

                var sb = new StringBuilder ();

                if (choice.startContent != null) {
                    sb.Append (choice.startContent.ToString ());
                }

                if (choice.choiceOnlyContent) {
                    sb.Append (choice.choiceOnlyContent.ToString ());
                }

                // Note that this choice text is an approximation since
                // it can be dynamically generated at runtime. We are therefore
                // making the assumption that the startContent and choiceOnlyContent
                // lists contain only literal (string) content.
                var choiceTextApproximation = sb.ToString ();
                var filename = choice.debugMetadata.fileName;

                var jsonObj = new JObject ();
                jsonObj ["filename"] = filename;
                jsonObj ["choiceText"] = choiceTextApproximation.ToString ();

                choiceJsonArray.Add (jsonObj);
            }

            var jsonString = choiceJsonArray.ToString ();

            File.WriteAllText ("choiceList.json", jsonString, System.Text.Encoding.UTF8);
        }

    }
}


