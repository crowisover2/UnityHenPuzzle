using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PuzzleHen.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Text txtID;

        [Header("End Game")]
        [SerializeField] private GameObject whenGameIsOver;
        [SerializeField] private Text txtWin;
        [SerializeField] private Button btnNext;

        [Header("Other")]
        [SerializeField] private Button btnRetry;
        [SerializeField] private Button btnExit;
        [SerializeField] private Button btnStart;


        [Header("Board Game")]
        [SerializeField] private RectTransform grid;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite barSprite;
        [SerializeField] 
        private Board board;
        //[SerializeField] 
        private Matrix shuffledCoor;

        private List<RectTransform> holder;
        private List<Button> btnPieces;

        private Rect preview;
        private Rect smallPrev;

        private int maxSizes;

        private int emptyTileX, emptyTileY;

        private int emptyTileValue;

        private int prevNum;

        private bool isPlayerWin;

        private int numSlices => PuzzleDataMiner.Current.NumSlices;

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
            isPlayerWin = false;

            txtID.text = PuzzleDataMiner.Current.ID;

            prevNum = Constants.IdIgnore;

            btnNext.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(Constants.SceneGame);
            });

            btnRetry.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(Constants.SceneGame);
            });

            btnExit.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(Constants.SceneChooseGirl);
            });

            btnStart.onClick.AddListener(() => {
                PlacedGameObjectBasedOnShuffledCoor();
                btnStart.gameObject.SetActive(false);
            });

            board = new(() => {
                txtWin.text = "Puzzle Clear \n" + txtID.text + " Scene is Unlocked";
                PuzzleDataMiner.SetupCompleteNextChallenge();
                isPlayerWin = true;
                whenGameIsOver.SetActive(true);
            });

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

        #region ShufflingID
        private void StartShuffling()
        {
            shuffledCoor = new();

            emptyTileX = -1;
            emptyTileY = -1;
            
            emptyTileValue = Random.Range(0, MaxSizes);
            //emptyTileValue = 15;

            SetEmptyTileMatrix();

            ShufflePuzzle();
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
            GetEmptyTileMatrix();
            RandomSwap();
        }
        private void GetEmptyTileMatrix()
        {
            emptyTileValue = shuffledCoor.GetEmptyPos(out emptyTileY, out emptyTileX);
        }
        private void SetEmptyTileMatrix()
        {
            for (int row = 0; row < numSlices; row++)
            {
                for (int column = 0; column < numSlices; column++)
                {
                    var isTrue = shuffledCoor.InitEmpty(row, column, emptyTileValue);
                    if (isTrue)
                    {
                        emptyTileX = column;
                        emptyTileY = row;
                        return;
                    }
                }
            }
        }
        private void RandomSwap()
        {
            int idCurr = emptyTileValue;
            //Debug.Log("id " + idCurr);

            int idUp, idDown, idLeft, idRight;

            // Check up
            idUp = idCurr - numSlices;
            if (idUp < 0) idUp = Constants.IdIgnore;

            // Check down
            idDown = idCurr + numSlices;
            if (idDown >= MaxSizes) idDown = Constants.IdIgnore;

            // Check left
            idLeft = idCurr - 1;
            if (idLeft < 0 || idCurr % numSlices == 0) idLeft  = Constants.IdIgnore;

            // Check right
            idRight = idCurr + 1;
            if (idRight % numSlices == 0 || idRight >= MaxSizes) idRight = Constants.IdIgnore;

            int direction;

            do
            {
                direction = Random.Range(0, 4);
            }
            while (direction == prevNum);

            prevNum = direction;

            // Swap the empty tile with the adjacent tile in the chosen direction
            switch (direction)
            {
                case 0: // up
                    if (idUp != Constants.IdIgnore)
                    { SwapTiles(emptyTileY, emptyTileX, emptyTileY - 1, emptyTileX); }
                    break;
                case 1: // down
                    if (idDown != Constants.IdIgnore)
                    { SwapTiles(emptyTileY, emptyTileX, emptyTileY + 1, emptyTileX); }
                    break;
                case 2: // left
                    if (idLeft != Constants.IdIgnore) { SwapTiles(emptyTileY, emptyTileX, emptyTileY, emptyTileX - 1); }
                    break;
                case 3: // right
                    if (idRight != Constants.IdIgnore)
                    {
                        SwapTiles(emptyTileY, emptyTileX, emptyTileY, emptyTileX + 1);
                    }
                    break;
            }
        }
        private void SwapTiles(int _rowOri, int _columnOri, int _rowTarget, int _columnTarget)
        {
            shuffledCoor.SwapIdCurr(_rowOri, _columnOri, _rowTarget, _columnTarget);
        }
        #endregion

        private void StartSettingGameObject()
        {
            //gameState = GameEnums.GameState.START;

            PlacingImagesToPieces();
            //PreDeterminedPoints();

            for (int row = 0; row < numSlices; row++)
            {
                for (int column = 0; column < numSlices; column++)
                {
                    int index = row * numSlices + column;
                    //placeHolder[index].transform.position = GetScreenCoordinates(row, column);

                    board.InitPerPiece(index, btnPieces[index].GetComponent<RectTransform>());
                }
            }
        }
        private void PlacingImagesToPieces()
        {
            GameObject temp = Instantiate(grid.gameObject, grid.parent);

            var rects1 = temp.GetComponentsInChildren<RectTransform>()
                .Where(r => r.parent == temp.transform)
                .ToArray();

            btnPieces = new List<Button>();

            for (int index = 0; index < MaxSizes; index++)
            {
                rects1[index].gameObject.AddComponent<Image>();
                Button btnTemp = rects1[index].gameObject.AddComponent<Button>();
                btnTemp.image.sprite = PuzzleDataMiner.Current.Pieces[index];

                btnPieces.Add(btnTemp);
            }

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(temp.GetComponent<RectTransform>());

            Destroy(temp.GetComponent<GridLayoutGroup>());

        }
        private void PlacedGameObjectBasedOnShuffledCoor()
        {
            emptyTileValue = shuffledCoor.IdEmpty;

            for (int row = 0; row < numSlices; row++)
            {
                for (int column = 0; column < numSlices; column++)
                {
                    int IdOri, IdCurr;

                    shuffledCoor.GetID(row, column, out IdOri, out IdCurr);

                    if (IdCurr.Equals(emptyTileValue))
                    {
                        Image temp = holder[IdOri].AddComponent<Image>();
                        temp.sprite = btnPieces[IdOri].image.sprite;

                        GameObject newPiece = new GameObject("Bar", typeof(RectTransform));
                        RectTransform tNewPiece = newPiece.GetComponent<RectTransform>();
                        tNewPiece.SetParent(holder[IdOri].transform);
                        tNewPiece.localScale = Vector3.one;
                        tNewPiece.localPosition = Vector3.zero;

                        tNewPiece.anchorMin = Vector2.zero;        // bottom-left (0,0)
                        tNewPiece.anchorMax = Vector2.one;         // top-right (1,1)
                        tNewPiece.pivot = new Vector2(0.5f, 0.5f); // center pivot
                        tNewPiece.offsetMin = Vector2.zero;        // left, bottom offset = 0
                        tNewPiece.offsetMax = Vector2.zero;        // right, top offset = 0

                        Image temp2 = newPiece.AddComponent<Image>();
                        temp2.sprite = barSprite;
                        temp2.color = new Color(1, 1, 1, 0.5f);

                        btnPieces[IdOri].enabled = false;
                        btnPieces[IdOri].image.sprite = emptySprite;
                        
                        board.IdEmpty = IdOri;
                    }
                    else
                    {
                        btnPieces[IdOri].onClick.AddListener(() => CheckPieceInput(IdOri));
                    }

                    board.SetCurrent(IdOri, IdCurr);
                }
            }
        }
        public void CheckPieceInput(int _idOri)
        {
            int idCurr = board.GetPiece(_idOri).IdCurr;
            int idUp, idDown, idLeft, idRight;

            // Check up
            idUp = idCurr - numSlices;
            if (idUp < 0) idUp = Constants.IdIgnore;

            // Check down
            idDown = idCurr + numSlices;
            if (idDown >= MaxSizes) idDown = Constants.IdIgnore;

            // Check left
            idLeft = idCurr - 1;
            if (idLeft < 0 || idCurr % numSlices == 0) idLeft = Constants.IdIgnore;

            // Check right
            idRight = idCurr + 1;
            if (idRight % numSlices == 0 || idRight >= MaxSizes) idRight = Constants.IdIgnore;

            board.Swap(idCurr, new int [] { idUp, idDown, idLeft, idRight });
        }

    }
}

