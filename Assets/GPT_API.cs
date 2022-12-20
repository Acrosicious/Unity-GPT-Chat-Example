using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GPT_API : MonoBehaviour
{

    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "I am ready to answer your questions!";
    }

    public void EnterText(string input)
    {
        text.text = "Please wait...";

        var prompt = "This AI can answer any question! Next question is: " + input + "?";

        StartCoroutine(GetNLP(prompt, 50, 0.5f, (s) => { text.text = s; }));

    }

    public IEnumerator GetNLP(string prompt, int max_tokens, float temperature, Action<string> callback)
    {
        var dict = new Dictionary<string, string>();
        dict.Add("prompt", prompt);
        dict.Add("max_tokens", max_tokens.ToString());
        dict.Add("temperature", temperature.ToString());

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:5000/nlp", dict))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    var response = JsonConvert.DeserializeObject<NLPResponse>(webRequest.downloadHandler.text, new JsonSerializerSettings
                    {
                        Error = (obj, err) => Debug.LogError(err + " " + webRequest.downloadHandler.text)
                    });
                    callback.Invoke(response.choices[0].text);
                    break;
            }
        }
    }


    // https://json2csharp.com
    public class Choice
    {
        public string finish_reason { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public string text { get; set; }
    }

    public class NLPResponse
    {
        public List<Choice> choices { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string @object { get; set; }
        public Usage usage { get; set; }
    }

    public class Usage
    {
        public int completion_tokens { get; set; }
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }
    }


}
