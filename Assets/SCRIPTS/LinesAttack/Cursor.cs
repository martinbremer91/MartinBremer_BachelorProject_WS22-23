using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    private Vector2Int _coords;

    private Vector2Int coords
    {
        set
        {
            _coords = value;
            activeSquare = gridManager.squares[value.x, value.y];

            Vector3 position = activeSquare.transform.position;
            transform.position = 
                new Vector3(position.x, position.y, -1);
        }
        get => _coords;
    }

    private Square activeSquare;
    
    private void Update() => HandleInput();

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && coords.y < gridManager.sqGridSizes.y - 1)
            coords = new Vector2Int(coords.x, coords.y + 1);
        if (Input.GetKeyDown(KeyCode.A) && coords.x > 0)
            coords = new Vector2Int(coords.x - 1, coords.y);
        if (Input.GetKeyDown(KeyCode.S) && coords.y > 0)
            coords = new Vector2Int(coords.x, coords.y - 1);
        if (Input.GetKeyDown(KeyCode.D) && coords.x < gridManager.sqGridSizes.x - 1)
            coords = new Vector2Int(coords.x + 1, coords.y);
        
        if (Input.GetKeyDown(KeyCode.K))
            activeSquare.TurnTile(false);
        if (Input.GetKeyDown(KeyCode.L))
            activeSquare.TurnTile(true);
    }

    public void SetInitialCoords(Vector2Int initCoords) => coords = initCoords;
}
