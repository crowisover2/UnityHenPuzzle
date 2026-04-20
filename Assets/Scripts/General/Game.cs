using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

namespace PuzzleHen.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Text txtID;

        [Header("Board Game")]
        [SerializeField] private RectTransform grid;
        [SerializeField] private RectTransform placeHolder;

        [Header("End Game")]
        [SerializeField] private GameObject whenGameIsOver;
        [SerializeField] private Button btnSkip;

        [Header("Other")]
        [SerializeField] private Button btnRetry;
        [SerializeField] private Button btnExit;

        private Board board;

        private List<RectTransform> holder;

        private Rect preview;
        private Rect smallPrev;

        private Matrix shuffledCoor;

        private int numSlices => PuzzleDataMiner.Current.NumSlices;
        private int maxSizes;
        private int MaxSizes
        {
            get { 
                if(maxSizes == 0)
                {
                    maxSizes = numSlices * numSlices;
                }

                return maxSizes;
            }
        }

        #region Unity
        // Use this for initialization
        private void Start()
        {
            //txtID.text = PuzzleDataMiner.Current.ID;

            //btnRetry.onClick.AddListener(() => {

            //});
            //btnExit.onClick.AddListener(() => {

            //});

            board = new(numSlices);

            SetHolder();
            StartShuffling();
            StartSettingGameObject();
        }
        #endregion

        private void SetHolder()
        {
            holder = new List<RectTransform>();

            preview = PuzzleDataMiner.Current.Preview.rect;
            smallPrev = PuzzleDataMiner.Current.Pieces[0].rect;

            grid.rect.Set(0, 0, preview.width, preview.height);

            GridLayoutGroup glg = grid.GetComponent<GridLayoutGroup>();
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = numSlices;
            glg.cellSize = new Vector2(smallPrev.width, smallPrev.height);

            // Calculate total pieces (Columns * Rows)
            int totalPieces = glg.constraintCount * glg.constraintCount;

            for (int i = 0; i < totalPieces; i++)
            {
                // 1. Create GameObject with RectTransform component immediately
                GameObject newPiece = new GameObject($"Piece_{i}", typeof(RectTransform));

                // 2. Set Parent (false ensures it doesn't try to keep world position)
                newPiece.transform.SetParent(grid, false);

                // 3. Cache the RectTransform
                RectTransform rt = newPiece.GetComponent<RectTransform>();
                holder.Add(rt);
            }
        }

        private void StartShuffling()
        {
            //gameState = GameEnums.GameState.SHUFFLING;

            shuffledCoor = new();

            ShufflePuzzle();
            EnsureSolvable();
        }
        private void ShufflePuzzle()
        {
            for (int i = 0; i < 1000; i++)
            {
                PerformRandomMove();
            }
        }
        private void PerformRandomMove()
        {
            int emptyTileX = -1;
            int emptyTileY = -1;

            int emptyTileValue = maxSizes - 1;

            //shuffle
            for (int row = 0; row < numSlices; row++)
            {
                for (int column = 0; column < numSlices; column++)
                {
                    if (shuffledCoor.GetID(row, column) == emptyTileValue)
                    {
                        emptyTileX = column;
                        emptyTileY = row;
                        break;
                    }
                }
            }

            int direction = Random.Range(0, 4);

            // Swap the empty tile with the adjacent tile in the chosen direction
            switch (direction)
            {
                case 0:  // Up
                    if (emptyTileX > 0)
                        SwapTiles(emptyTileX, emptyTileY, emptyTileX - 1, emptyTileY);
                    break;
                case 1:  // Down
                    if (emptyTileX < 3)
                        SwapTiles(emptyTileX, emptyTileY, emptyTileX + 1, emptyTileY);
                    break;
                case 2:  // Left
                    if (emptyTileY > 0)
                        SwapTiles(emptyTileX, emptyTileY, emptyTileX, emptyTileY - 1);
                    break;
                case 3:  // Right
                    if (emptyTileY < 3)
                        SwapTiles(emptyTileX, emptyTileY, emptyTileX, emptyTileY + 1);
                    break;
            }
        }
        private void SwapTiles(int x1, int y1, int x2, int y2)
        {
            int temp = shuffledCoor.GetID(y1, x1);
            shuffledCoor.SetID(y1, x1, shuffledCoor.GetID(y2, x2));
            shuffledCoor.SetID(y2, x2, temp);
        }
        private void EnsureSolvable()
        {
            // Check if the initial state is solvable, if not, perform additional shuffle
            if (!IsSolvable())
            {
                //Debug.Log("Damn");
                ShufflePuzzle(); // Recursive call until a solvable state is achieved
                EnsureSolvable();
            }
        }
        private bool IsSolvable()
        {
            // Flatten the puzzle into a 1D array for easier inversion counting

            int[] flatPuzzle = new int[MaxSizes];
            int index = 0;
            for (int row = 0; row < numSlices; row++)
            {
                for (int column = 0; column < numSlices; column++)
                {
                    flatPuzzle[index++] = shuffledCoor.GetID(row, column);
                }
            }

            // Count the number of inversions
            int inversions = 0;
            int emptyTileValue = maxSizes - 1;

            for (int i = 0; i < 15; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    if (flatPuzzle[i] > flatPuzzle[j] && flatPuzzle[i] != emptyTileValue && flatPuzzle[j] != emptyTileValue)
                    {
                        inversions++;
                    }
                }
            }

            // Check if the number of inversions is even
            return inversions % 2 == 0;
        }

        private void StartSettingGameObject()
        {
            //gameState = GameEnums.GameState.START;

            ScalePieces();
            PreDeterminedPoints();

            for (int row = 0; row < Constants.MaxRows; row++)
            {
                for (int column = 0; column < Constants.MaxColumns; column++)
                {
                    int index = row * Constants.MaxRows + column;
                    placeHolder[index].transform.position = GetScreenCoordinates(row, column);

                    board.SetGameObject(row, column, placeHolder[index]);
                    board.SetOriginal(row, column);

                    if (shuffledCoor.GetID(row, column).Equals(emptyTileValue))
                    {
                        board.SetEmpty(row, column);
                    }
                }
            }
        }

        private void ScalePieces()
        {
            wholeImage = PuzzleDataMiner.GetPreview();

            placeHolder[0].GetComponent<SpriteRenderer>().sprite = PuzzleDataMiner.GetPiecesAt(0);
            Bounds bounds = placeHolder[0].GetComponent<SpriteRenderer>().sprite.bounds;

            if (wholeImage.texture.width > wholeImage.texture.height)
            {
                whichImageIsLonger = "w";
            }
            else if (wholeImage.texture.width == wholeImage.texture.height)
            {
                whichImageIsLonger = "e";
                ratioImage = (float)wholeImage.texture.width / wholeImage.texture.height;
                ratioScreen = (float)Screen.width / Screen.height;
            }
            else
            {
                whichImageIsLonger = "h";
            }

            if (Screen.width > Screen.height)
            {
                whichScreenIsLonger = "w";
            }
            else
            {
                whichScreenIsLonger = "h";
            }

            screenHeight = Camera.main.orthographicSize * 2f;
            screenWidth = screenHeight / Screen.height * Screen.width;

            if (whichImageIsLonger.Equals("e"))
            {
                float temp = Mathf.Min((float)screenHeight, (float)screenWidth);
                if (whichScreenIsLonger.Equals("w"))
                {
                    screenHeight = temp;
                    screenWidth = temp * ratioImage;
                }
                else if (whichScreenIsLonger.Equals("h"))
                {
                    screenWidth = temp;
                    screenHeight = temp / ratioImage;
                }
            }

            Debug.Log(whichImageIsLonger + " " + whichScreenIsLonger);

            float width = screenWidth / bounds.size.x / Constants.MaxColumns;
            float height = screenHeight / bounds.size.y / Constants.MaxRows;

            placeHolder[0].transform.localScale = new Vector3(width, height, 1f);
            placeHolder[0].AddComponent<BoxCollider2D>();

            for (int c = 1; c < placeHolder.Length; c++)
            {
                placeHolder[c].GetComponent<SpriteRenderer>().sprite = PuzzleDataMiner.GetPiecesAt(c);
                placeHolder[c].transform.localScale = new Vector3(width, height, 1f);
                placeHolder[c].AddComponent<BoxCollider2D>();
            }
        }

    }
}
/*
//Array Coor
//Constants.MaxRows, Constants.MaxColumns

private Matrix preDeterminedCoor;

//Game state
// [SerializeField] private GameEnums.GameState gameState;



Vector3 screenPositionToAnimate;
private Piece pieceToAnimate;
private Piece theNullPlace;

private float ratioImage;
private float ratioScreen;
private float screenHeight;
private float screenWidth;

private string whichImageIsLonger = "";
private string whichScreenIsLonger = "";

//Const
private const float animSpeed = 10f;
private const int emptyTileValue = 16;


#region Setting GameObject
private void PreDeterminedPoints()
{
preDeterminedCoor = new();

float ratio = 1;
if (!ratioScreen.Equals(ratioImage))
{
    if (Screen.width > Screen.height)
    {
        ratio = (float) Screen.height / Screen.width;
    }
    else
    {
        ratio = ratioScreen;
    }
}

for (int row = 0; row < Constants.MaxRows; row++)
{
    float trueY = row * 0.25f;

    for (int column = 0; column < Constants.MaxColumns; column++)
    {
        float trueX = column * 0.25f;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3((trueX + 0.125f), 1f - (trueY + 0.125f), 0f));

        if (Screen.width > Screen.height)
        {
            Vector2 point = new(ray.origin.x * ratio, ray.origin.y);
            preDeterminedCoor.SetVector(row, column, point);
        }
        else
        {
            Vector2 point = new(ray.origin.x, ray.origin.y * ratio);
            preDeterminedCoor.SetVector(row, column, point);
        }
    }
}
}

#endregion

#region Game
private Vector3 GetScreenCoordinates(int _row, int _column)
{
Vector3 point = preDeterminedCoor.GetVector(_row, _column);
return point;
}

private Vector3 GetScreenCoordinates(int _id)
{
Vector3 point = preDeterminedCoor.GetVector(_id);
return point;
}

private void PlacedGameObjectBasedOnShuffledCoor()
{
for (int row = 0; row < Constants.MaxRows; row++)
{
    for (int column = 0; column < Constants.MaxColumns; column++)
    {
        int id = shuffledCoor.GetID(row, column);
        if (id.Equals(emptyTileValue))
        {
            theNullPlace = board.GetPiece(row, column);
        }
        else 
        {
            board.SetPosition(row, column, GetScreenCoordinates(id));
        }
        board.SetCurrent(row, column, id);
    }
}
}

private void Swap()
{
theNullPlace = board.Swap(theNullPlace, pieceToAnimate);
}

public void CheckPieceInput()
{
if (Input.GetMouseButtonUp(0))
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    if (hit.collider != null)
    {
        Piece tempPiece = board.GetPiece(hit.collider.gameObject);

        //check for the null piece, taking into account the game bounds
        bool pieceFound = false;
        List<int[]> neighbors = new();

        // Check up
        if ((tempPiece.CurrRow - 1) >= 0)
            neighbors.Add(new int[] { tempPiece.CurrRow - 1, tempPiece.CurrColumn });

        // Check down
        if ((tempPiece.CurrRow + 1) < Constants.MaxRows)
            neighbors.Add(new int[] { tempPiece.CurrRow + 1, tempPiece.CurrColumn });

        // Check left
        if ((tempPiece.CurrColumn - 1) >= 0)
            neighbors.Add(new int[] { tempPiece.CurrRow, tempPiece.CurrColumn - 1 });

        // Check right
        if ((tempPiece.CurrColumn + 1) < Constants.MaxColumns)
            neighbors.Add(new int[] { tempPiece.CurrRow, tempPiece.CurrColumn + 1 });

        foreach (int[] item in neighbors)
        {
            if (item[0].Equals(theNullPlace.CurrRow))
            {
                if (item[1].Equals(theNullPlace.CurrColumn))
                {
                    pieceFound = true;
                    break;
                }
            }
        }

        if (pieceFound)
        {
            //get the coordinates of the empty object
            screenPositionToAnimate = GetScreenCoordinates(theNullPlace.CurrRow, theNullPlace.CurrColumn);
            pieceToAnimate = tempPiece;
            //gameState = GameEnums.GameState.ANIMATION;
        }

    }
}
}

private void AnimateMovement(Piece toMove, float time)
{
//animate it
//Lerp could also be used, but I prefer the MoveTowards approach :)
toMove.GameObject.transform.position = Vector2.MoveTowards(toMove.GameObject.transform.position, screenPositionToAnimate, time * animSpeed);
}

private void CheckIfAnimationEnded()
{
if (Vector2.Distance(pieceToAnimate.GameObject.transform.position, screenPositionToAnimate)<0.01f)
{
    pieceToAnimate.GameObject.transform.position = screenPositionToAnimate;
    //make sure they swap, exchange positions and stuff
    Swap();
    //check if the use has won
    CheckForVictory();
}
}

private void CheckForVictory()
{
//dual loop to check the object's properties
//gameState = GameEnums.GameState.CHECKING;
int nonEmptyCount = 0;
for (int column = 0; column < Constants.MaxColumns; column++)
{
    for (int row = 0; row < Constants.MaxRows; row++)
    {
        if (board.IsTheSame(row, column)) nonEmptyCount++;
    }
}

if(nonEmptyCount.Equals(Constants.MaxSize))
{
    //gameState = GameEnums.GameState.END;
    return;
}
//else gameState = GameEnums.GameState.PLAYING;
}
#endregion

#region When Game End
private void DisplayVN()
{
//SceneManager.LoadScene((int)GameEnums.SceneOrder.VN);
}

private void DisplayPanel() => whenGameIsOver.SetActive(true);
#endregion

// Update is called once per frame
//private void Update()
//{
//    switch (gameState)
//    {
//        case GameEnums.GameState.START:
//            if (Input.GetMouseButtonUp(0))
//            {
//                PlacedGameObjectBasedOnShuffledCoor();
//                gameState = GameEnums.GameState.PLAYING;
//            }
//            break;
//        case GameEnums.GameState.PLAYING:
//            CheckPieceInput();
//            break;
//        case GameEnums.GameState.ANIMATION:
//            AnimateMovement(pieceToAnimate, Time.deltaTime);
//            CheckIfAnimationEnded();
//            break;
//        case GameEnums.GameState.END:
//            DisplayPanel();
//            break;
//    }
//}

/// <summary>
/// boring UI, waiting for uGUI framework :)
/// </summary>
//private void OnGUI()
//{
//    switch (gameState)
//    {
//        case GameState.Start:
//            GUI.Label(new Rect(0, 0, 100, 100), "Tap to start!");
//            break;
//        case GameState.End:
//            GUI.Label(new Rect(0, 0, 100, 100), "Congrats, tap to start over!");
//            break;
//        default:
//            break;
//    }
//}
*/

