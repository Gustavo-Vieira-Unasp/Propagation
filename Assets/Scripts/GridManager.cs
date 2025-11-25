using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _gapSize = 0.05f;
    private const float _sideLength = 1.0f;
    private float _tileHeight;
    private float _yCorrection;
    private Dictionary<Vector2, Tile> _tiles;
    private Tile.SoilType[] _availableSoilTypes;
    private const int MinLevel = 0;
    private const int MaxLevel = 9;

    void Awake()
    {
        _tileHeight = (Mathf.Sqrt(3f) / 2f) * _sideLength;
        _yCorrection = _tileHeight / 3.0f;

        _tiles = new Dictionary<Vector2, Tile>();

        _availableSoilTypes = new Tile.SoilType[]
        {
            Tile.SoilType.Paved,
            Tile.SoilType.PureDryClay, 
            Tile.SoilType.PureSand,
            Tile.SoilType.PureCompactedSiltide, 
            Tile.SoilType.SandyLoam,
            Tile.SoilType.SiltyLoam,
            Tile.SoilType.PureLoam,
            Tile.SoilType.HumusLoam,
            Tile.SoilType.ClayLoam,
            Tile.SoilType.ClayHumusLoam
        };
    }

    void Start()
    {
        StartGrid();
    }

    void StartGrid()
    {
        float xStep = (_sideLength / 2.0f) + _gapSize;
        float yStep = _tileHeight + _gapSize;
        float totalWidthOffset = 0f;

        for(int y = 0; y < _height; y++)
        {
            float yBase = y * yStep;

            for(int x = 0; x < _width; x++)
            {
                bool isUpsideDown = ((x + y) % 2 != 0);

                float xPos = x * xStep;
                float yPos;
                Quaternion rotation = Quaternion.identity;
                
                if (isUpsideDown)
                {                    
                    yPos = yBase + _yCorrection;
                    rotation = Quaternion.Euler(0, 0, 180f);
                } 
                else 
                {
                    yPos = yBase;
                }

                Vector2 position = new Vector2(xPos, yPos);
                Vector2 gridPos = new Vector2(x, y);
                
                var spawnedTile = Instantiate(_tilePrefab, position, rotation);
                spawnedTile.name = $"Tile {x} {y}"; 
                spawnedTile.transform.SetParent(transform);

                Tile tileComponent = spawnedTile.GetComponent<Tile>();
                if (tileComponent == null) continue; 

                _tiles[gridPos] = tileComponent;

                Tile.SoilType selectedSoil;
                int selectedVegetation;
                int selectedHuman;
                
                do
                {
                    selectedSoil = GetRandomSoilType();
                    selectedVegetation = GetRandomLevel(); 
                    selectedHuman = GetRandomLevel();
                }
                while (CheckImpossibility(gridPos, selectedSoil, selectedVegetation, selectedHuman));

                tileComponent.Initialize(selectedSoil, selectedVegetation, selectedHuman);

                if (x == _width - 1)
                {
                    totalWidthOffset = xPos;
                }
            }
        }
        
        float camX = totalWidthOffset / 2.0f;
        float camY = ((_height - 1) * yStep) / 2.0f; 
        
        _cameraTransform.position = new Vector3(camX, camY, -10);
    }

    private Tile.SoilType GetRandomSoilType()
    {
        int randomIndex = Random.Range(0, _availableSoilTypes.Length);
        return _availableSoilTypes[randomIndex];
    }

    private int GetRandomLevel()
    {
        return Random.Range(MinLevel, MaxLevel + 1); 
    }

    private bool CheckImpossibility(Vector2 gridPos, Tile.SoilType newSoil, int newVeg, int newHuman)
    {
        Vector2 leftNeighborPos = gridPos + Vector2.left; 

        if (_tiles.TryGetValue(leftNeighborPos, out Tile leftNeighbor))
        {
            if (newSoil == Tile.SoilType.Paved && leftNeighbor.Vegetation >= 7)
            {
                return true;
            }
             if (newSoil == Tile.SoilType.ClayLoam && newHuman <= 2)
            {
                return true;
            }
            
            if (Mathf.Abs(newHuman - leftNeighbor.HumanPresence) > 5)
            {
                return true;
            }
        }
        
        return false;
    }
}