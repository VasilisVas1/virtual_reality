using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class PlayerInteract : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.E)){
                float interactRange=2f;
                Collider[] colliderArray=Physics.OverlapSphere(transform.position, interactRange);
                foreach(Collider collider in colliderArray){
                    if(collider.TryGetComponent(out NPCInteractable npcInteractable)){
                        npcInteractable.Interact();
                    }

                }
            }
        
        }
    }
}
