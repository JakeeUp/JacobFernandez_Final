using UnityEngine;

namespace PampelGames.GoreSimulator.Demo
{
    
    /// <summary>
    ///     A simple script to demonstrate cut and rigidbody force together.
    /// </summary>
    public class DemoExecuteRagdollCut : MonoBehaviour
    {
        
        public KeyCode ResetKey = KeyCode.Space;
        public float force = 1000f;

        private void Update()
        {
            
            // Note that here we are getting the GoreBone, since we need an intact body.
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (!hit.collider.transform.TryGetComponent<GoreBone>(out var goreBone)) return;
                    
                    goreBone.ExecuteRagdollCut(hit.point, force);
                }
            }
            
            if (Input.GetKeyDown(ResetKey))
            {
                FindObjectOfType<GoreSimulator>().ResetCharacter();
            }
        }
    }
}