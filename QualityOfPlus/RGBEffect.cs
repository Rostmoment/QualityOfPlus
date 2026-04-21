using UnityEngine;

namespace QualityOfPlus
{
    class RGBEffect : MonoBehaviour
    {
        private float speed = float.NaN;
        private Renderer rend;

        public void Initialize(Renderer rend, float speed)
        {
            this.speed = speed;
            this.rend = rend;
        }

        private void Update()
        {
            if (float.IsNaN(speed) || float.IsInfinity(speed))
                return;

            if (rend.IsNullOrDestroyed())
                return;

            float hue = (Time.realtimeSinceStartup * speed) % 1.0f;
            float saturation = 0.8f + 0.2f * Mathf.Sin(Time.realtimeSinceStartup * 2f);
            Color rgbColor = Color.HSVToRGB(hue, saturation, 1f);
            rend.material.color = rgbColor;
        }
    }
}