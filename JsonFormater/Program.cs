using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string json = @"
        {
           ""type"":""Container"",
           ""items"":[
              {
                 ""type"":""TextBlock"",
                 ""text"":""text""
              },
              {
                 ""type"":""Container"",
                 ""items"":[
                    {
                       ""type"":""TextBlock"",
                       ""text"":""${tex1}""
                    },
                    {
                       ""type"":""Container"",
                       ""items"":[
                          {
                             ""type"":""TextBlock"",
                             ""text"":""${text2}""
                          }
                       ]
                    }
                 ]
              },
              {
                 ""type"":""Container"",
                 ""specific"":""true"",
                 ""items"":[
                    {
                       ""type"":""TextBlock"",
                       ""text"":""${text3}""
                    },
                    {
                       ""type"":""Container"",
                       ""items"":[
                          {
                             ""type"":""TextBlock"",
                             ""text"":""${text4}""
                          }
                       ]
                    }
                 ]
              }
           ]
        }";

        var jsonObject = JObject.Parse(json);
        ProcessContainer(jsonObject);
        Console.WriteLine(jsonObject.ToString());
    }

    static void ProcessContainer(JObject container)
    {
        bool isSpecific = container.ContainsKey("specific") && container["specific"]?.ToString() == "true";

        if (container.ContainsKey("items") && container["items"] is JArray items)
        {
            foreach (var item in items)
            {
                if (item is JObject itemObject)
                {
                    if (itemObject["type"]?.ToString() == "TextBlock" && isSpecific)
                    {
                        var text = itemObject["text"]?.ToString();
                        if (!string.IsNullOrEmpty(text) && text.Contains("${"))
                        {
                            itemObject["text"] = "REPLACED VALUE";
                        }
                    }
                    else if (itemObject["type"]?.ToString() == "Container")
                    {
                        ProcessContainer(itemObject);
                    }
                }
            }
        }
    }
}
