namespace TheDeveloper.HAAoE
{
    using UnityEngine;

    public class HAAOE_Example : MonoBehaviour
    {
        private int currentEffect = 0;

        private void Start()
        {
            this.SetEffect(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                this.SetEffect(this.currentEffect - 1);
            if (Input.GetKeyDown(KeyCode.D))
                this.SetEffect(this.currentEffect + 1);
            if (Input.GetKeyDown(KeyCode.R))
                this.ReloadEffect();
        }

        private void SetEffect(int newIndex)
        {
            if (newIndex < 0 ||
                newIndex >= transform.childCount)
                return;

            transform.GetChild(currentEffect).gameObject.SetActive(false);
            currentEffect = newIndex;
            transform.GetChild(currentEffect).gameObject.SetActive(true);
        }

        private void ReloadEffect()
        {
            // ParticleSystem.Stop() doesn't turn off all child particle systems
            var g = transform.GetChild(currentEffect).gameObject;
            g.SetActive(false);
            g.SetActive(true);
        }
    }
}