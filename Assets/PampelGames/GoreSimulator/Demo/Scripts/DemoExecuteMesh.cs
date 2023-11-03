using UnityEngine;

namespace PampelGames.GoreSimulator.Demo
{
    
    /// <summary>
    ///     A simple script to demonstrate cut, execution and reset of a character.
    /// </summary>
    public class DemoExecuteMesh : MonoBehaviour
    {
        
        public KeyCode ResetKey = KeyCode.Space;

        private void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (!hit.collider.transform.TryGetComponent<IGoreObject>(out var goreObject)) return;
                    goreObject.ExecuteCut(hit.point);
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (!hit.collider.transform.TryGetComponent<IGoreObject>(out var goreObject)) return;
                    goreObject.ExecuteExplosion(250);
                }
            }
            
            if (Input.GetKeyDown(ResetKey))
            {
                FindObjectOfType<GoreSimulator>().ResetCharacter();
            }
        }
    }
}
