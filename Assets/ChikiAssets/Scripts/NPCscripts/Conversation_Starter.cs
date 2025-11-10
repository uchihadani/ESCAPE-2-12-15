using UnityEngine;
using DialogueEditor;
public class Conversation_Starter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                ConversationManager.Instance.StartConversation(myConversation);
            }
        }
    }

}
