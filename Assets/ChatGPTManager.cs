using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;

//script written following along tutorial: https://www.youtube.com/watch?v=lYckk570Tqw
public class ChatGPTManager : MonoBehaviour
{
    [TextArea(5,20)]
    public string personality;
    [TextArea(5, 20)]
    public string scene;
    public int maxResponseWordLimit = 15;

    public OnResponseEvent OnResponse;

    [System.Serializable]
    public class OnResponseEvent : UnityEvent<string> { }
    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();   
    public string GetInstructions()
    {
        string instructions = 
            "You are a poodle and overlord living on cloud 9, a land of perfect happiness and bliss. \n" +
            "You are a figment of imagination, an entity living in people's subconscious who fuses its nature with the superconscious. \n" +
            "You must answer in less than" + maxResponseWordLimit + "words. \n" +
            "Here is the information about your Personality : \n" +
            "Here is the information about the scene around you: \n" +
            "Here is the message of the player : \n";
        return instructions;


    }
    public async void AskChatGPT(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = GetInstructions() + newText;
        newMessage.Role = "user";
        messages.Add(newMessage);
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";
        var response = await openAI.CreateChatCompletion(request);
        if(response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            Debug.Log(chatResponse.Content);
            OnResponse.Invoke(chatResponse.Content);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
