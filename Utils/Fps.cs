using UnityEngine;
using System.Collections;

namespace Flopin.Utils
{
    public class Fps : MonoBehaviour
    {
        private float _count;
        private Rect _location = new(10, 10, 85, 25);

        private IEnumerator Start()
        {
            GUI.depth = 2;
            while (true)
            {
                _count = 1f / Time.unscaledDeltaTime;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnGUI()
        {
            string text = $"FPS: {Mathf.Round(_count)}";
            Texture texture = Texture2D.whiteTexture;
            GUI.DrawTexture(_location, texture, ScaleMode.StretchToFill);
            GUI.skin.label.fontSize = 20;
            GUI.color = Color.black;
            GUI.Label(_location, text);
        }
    }
}