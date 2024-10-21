using UnityEngine;

namespace Meangpu.Skybox
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Light Preset", menuName = "Meangpu/Light Preset", order = 1)]
    public class SkyboxLightPreset : ScriptableObject
    {
        public Gradient AmbientCol;
        public Gradient DirectionalCol;
        public Gradient FogCol;

        public Gradient upperCol;
        public Gradient lowerCol;
    }
}