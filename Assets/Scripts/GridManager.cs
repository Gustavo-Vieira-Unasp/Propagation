using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cameraTransform; 
    private const float _sideLength = 1.0f;
    private float _tileHeight;
    private float _yCorrection;

    void Awake()
    {
        _tileHeight = (Mathf.Sqrt(3f) / 2f) * _sideLength;
        
        _yCorrection = _tileHeight / 3.0f;
    }

    void Start()
    {
        StartGrid();
    }

    void StartGrid()
    {
        float xStep = _sideLength / 2.0f;
        float yStep = _tileHeight;
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
                
                var spawnedTile = Instantiate(_tilePrefab, position, rotation);
                spawnedTile.name = $"Tile {x} {y}"; 
                spawnedTile.transform.SetParent(transform);

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
}