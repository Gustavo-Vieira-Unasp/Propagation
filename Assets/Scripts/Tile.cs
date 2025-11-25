using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public enum SoilType { Paved, PureDryClay, PureSand, 
                        PureCompactedSiltide, SandyLoam, SiltyLoam, 
                        PureLoam, ClayLoam, HumusLoam, ClayHumusLoam }
    public SoilType CurrentSoil { get; private set; }
    public int Vegetation { get; private set; }
    public int HumanPresence { get; private set; }
    public Color CurrentColor { get; private set; }

    private static readonly string[] VegetationLevelNames = new string[] 
    { "Wasteland", "Mown Grass", "Grassland", "Scattered Shrubs", "Hedge", "Orchard", "Pasture", "Woodland Edge", "Forest", "Dense Forest" };

    private static readonly string[] HumanPresenceLevelNames = new string[] 
    { "Untouched", "Natural", "Refuge", "Rural", "Shared Habitat", "Urban Park", "Urban Fringe", "Conflict Zone", "Concrete Jungle", "Fully Artificial" };
    
    private static readonly Dictionary<SoilType, Color> SoilColors = new Dictionary<SoilType, Color>()
    {
        { SoilType.Paved, new Color(0.392f, 0.392f, 0.392f) },     
        { SoilType.PureDryClay, new Color(0.745f, 0.588f, 0.471f) },
        { SoilType.PureSand, new Color(0.941f, 0.902f, 0.745f) },
        { SoilType.PureCompactedSiltide, new Color(0.706f, 0.667f, 0.588f) },
        { SoilType.SandyLoam, new Color(0.824f, 0.725f, 0.549f) },
        { SoilType.SiltyLoam, new Color(0.667f, 0.627f, 0.549f) },
        { SoilType.PureLoam, new Color(0.627f, 0.549f, 0.353f) },
        { SoilType.HumusLoam, new Color(0.588f, 0.471f, 0.314f) },
        { SoilType.ClayLoam, new Color(0.314f, 0.235f, 0.157f) },
        { SoilType.ClayHumusLoam, new Color(0.275f, 0.196f, 0.118f) }
    };

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("Tile prefab needs a Renderer component!");
        }
    }

    public void Initialize(SoilType soil, int vegetation, int human)
    {
        CurrentSoil = soil;
        Vegetation = vegetation;
        HumanPresence = human;

        Color baseColor = SoilColors[soil];
        
        float h, s, v;
        Color.RGBToHSV(baseColor, out h, out s, out v);

        float maxSaturation = 1.0f;
        s = Mathf.Lerp(s, maxSaturation, Vegetation / 9f);

        float minHumanValue = 0.3f;
        v = Mathf.Lerp(v, minHumanValue, HumanPresence / 9f);

        CurrentColor = Color.HSVToRGB(h, s, v);
        _renderer.material.color = CurrentColor;
    }
    
    public string GetVegetationName()
    {
        int index = Mathf.Clamp(Vegetation, 0, 9);
        return VegetationLevelNames[index];
    }
    
    public string GetHumanPresenceName()
    {
        int index = Mathf.Clamp(HumanPresence, 0, 9);
        return HumanPresenceLevelNames[index];
    }
}
