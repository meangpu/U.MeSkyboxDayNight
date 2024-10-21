using UnityEngine;
using System;
using VInspector;

namespace Meangpu.Skybox
{
    [ExecuteAlways]
    public class SkyboxLightByTime : MonoBehaviour
    {
        [SerializeField] private Light DirectionalLight;
        [SerializeField] private SkyboxLightPreset Preset;

        [SerializeField, Range(0, 24)] private float TimeOfDay;
        [SerializeField] bool _useRealTime;

        [SerializeField] float _updateIntervalSecond = 60f;
        private float _nextUpdateTime;
        float _timePercentCache;

        private void Update()
        {
            if (Application.IsPlaying(gameObject))
            {
                if (_useRealTime)
                {
                    if (Time.time >= _nextUpdateTime)
                    {
                        TimeOfDay = GetCurrentHour();
                        _nextUpdateTime = Time.time + _updateIntervalSecond;
                    }
                }
                else
                {
                    TimeOfDay += Time.deltaTime;
                    TimeOfDay %= 24;
                }
                UpdateLight(TimeOfDay / 24f);
            }
            else
            {
                UpdateLight(TimeOfDay / 24f);
            }
        }

        [Button]
        void UpdateLight(float timePercent)
        {
            if (timePercent == _timePercentCache) return;
            RenderSettings.ambientLight = Preset.AmbientCol.Evaluate(timePercent);
            RenderSettings.fogColor = Preset.FogCol.Evaluate(timePercent);

            if (RenderSettings.skybox.HasProperty("_Top"))
            {
                RenderSettings.skybox.SetColor("_Top", Preset.upperCol.Evaluate(timePercent));
            }
            if (RenderSettings.skybox.HasProperty("_Bottom"))
            {
                RenderSettings.skybox.SetColor("_Bottom", Preset.lowerCol.Evaluate(timePercent));
            }

            DirectionalLight.color = Preset.DirectionalCol.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            _timePercentCache = timePercent;
        }

        public static float GetCurrentHour()
        {
            return (float)(DateTime.Now.TimeOfDay.TotalHours % 24);
        }
    }

}