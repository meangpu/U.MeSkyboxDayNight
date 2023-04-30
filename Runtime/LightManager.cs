using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class LightManager : MonoBehaviour
{
    // ref
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    // var
    [SerializeField, Range(0, 24)] private float TimeOfDay;


    bool isRealTime = false;

    [SerializeField] Slider sliderTime;

    [SerializeField] Button autoBtn;
    [SerializeField] Button realBtn;


    private void OnValidate() 
    {
        if (DirectionalLight != null)
        {
            return;
        }
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
        
    }

    private void Update() 
    {
        if(Preset == null)
        {
            return;
        }
        if(Application.isPlaying)
        {
            if(isRealTime)
            {
                TimeOfDay = sliderTime.value;
            }
            else
            {
                TimeOfDay += Time.deltaTime;
                TimeOfDay %= 24;
            }

            updateLight(TimeOfDay /24f);
        }
        else
        {
            updateLight(TimeOfDay /24f);
        }
    }

    void updateLight(float timePercent)
    {
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



        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalCol.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent*360f) -90f, 170f, 0));
        }
    } 

    public void lightRealTime()
    {
        sliderTime.gameObject.SetActive(true);
        var today = System.DateTime.Now;
        sliderTime.value = today.Hour;

        isRealTime = true;
        selectNowBtn(realBtn);
    }

    public void lightAuto()
    {
        sliderTime.gameObject.SetActive(false);
        isRealTime = false;
        selectNowBtn(autoBtn);
    }


    void selectNowBtn(Button _sel)
    {
        _sel.Select();
    }


}
