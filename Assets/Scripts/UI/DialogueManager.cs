using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    //public string FileName;

    //public bool WriteText;

    public Dialogue Conversation;

    public static int i { get; private set; }

    public static int j { get; private set; }

    public static bool InConversation { get; private set; }
    public static bool InSentence { get; private set;}

    public Text NameTxt;
    public Text SentenceTxt;

    public GameObject DialogueUI;

    public static DialogueManager MeInThis { get; private set; }
    void Awake() {
        MeInThis = this;
        //if(WriteText){
        //    JsonLoader<Dialogue>.UpdateData(Conversation, FileName);
        //}
        DialogueUI.SetActive(false);
        InConversation = false;
    }
	    
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.X))
        //    StartDialogue();
        if (InConversation && (i != 0 || j != 0)) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (!InSentence) {
                    NextSentence();
                    print(i + "and" + j);
                }
                else {
                    StopAllCoroutines();
                    SentenceTxt.text = Conversation.Conversation[i].Sentences[j - 1];
                    InSentence = false;
                }

            }
        }
		
	}
    //
    //public void StartDialogue(){
    //    Dialogue Conv = Conversation;
    //    if(Conv != null){
    //        StartConversation();
    //    }else{
    //        print("Invalid Conversation");
    //    }
    //}

    MonoBehaviour AuxObj;

    public void StartConversation(Dialogue dialogue, MonoBehaviour obj) {
        i = 0;
        j = 0;
        InConversation = true;
        InSentence = false;
        Conversation = dialogue;
        AuxObj = obj;
        obj.enabled = false;
        DialogueUI.SetActive(true);
        NextSentence();
        
    }
    public void NextSentence() {
        
        if (j > Conversation.Conversation[i].Sentences.ToArray().Length-1) {
            NextChat();
            return;
        }
        NameTxt.text = Conversation.Conversation[i].Name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(Conversation.Conversation[i].Sentences[j]));
        j++;


    }

    public void NextChat() {
        i++;
        if (i > Conversation.Conversation.ToArray().Length-1) {
            EndConversation();
            return;
        }
        j = 0;
        
        NextSentence();
    }

    public int Speed;

    IEnumerator TypeSentence(string sentence) {
        SentenceTxt.text = "";
        InSentence = true;
        int i = 0;
        foreach (char letter in sentence.ToCharArray()) {
            SentenceTxt.text += letter;
            if (i == Speed) {
                i = 0;
                yield return null;
            }
            else
                i++;
        }
        InSentence = false;
    }

    public void EndConversation() {
        InConversation = false;
        AuxObj.enabled = true;
        DialogueUI.SetActive(false);
    }

}
[System.Serializable]
public class Dialogue {
    public List<Chat> Conversation;
}
[System.Serializable]
public class Chat {
    public string Name;
    public List<string> Sentences;
}