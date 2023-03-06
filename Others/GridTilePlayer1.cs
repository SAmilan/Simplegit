using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class GridTilePlayer1 : TilePiece
{
    #region Private_Variables
    private SinglePlayerInputs m_TwoPlayerInputs;
    private InputAction m_RotatePieceClockwise;
    private InputAction m_RotatePieceAntiClockwise;
    [SerializeField]
    List<double> m_TileCode = new List<double>();
    [SerializeField]
    protected int p_StartingPieceCounter = 0;
    [SerializeField]
    protected TextMeshProUGUI p_StartingPieceCounterText;
    private int m_StartingPieceIndex;
    [SerializeField]
    private Vector2 m_StartingPieceFlowVector;
    [SerializeField]
    private List<GridTilePlayer1> m_AttachedPiece = new List<GridTilePlayer1>();
    [SerializeField]
    private List<GridTilePlayer1> m_StartingPieceLoopPool = new List<GridTilePlayer1>();
    private Coroutine m_NextPieceAnimationCoroutine;
    public Vector2 StartingPieceFlowVector => m_StartingPieceFlowVector;
    #endregion
    #region Public_Variables
    public List<GridTilePlayer1> ContiniousFlowPieces = new List<GridTilePlayer1>();
    public int StartingPieceCounter => p_StartingPieceCounter;
    public List<GridTilePlayer1> StartingPieceLoopPool => m_StartingPieceLoopPool;
    [SerializeField]
    private List<GridTilePlayer1> m_PieceLoop = new List<GridTilePlayer1>();
    public List<GridTilePlayer1> PieceLoop => m_PieceLoop;
    private bool m_IsLoopCompleted;
    private bool m_StartedPieceCounted = false;
    [SerializeField]
    private GameObject m_Right1BrokenImage;
    [SerializeField]
    private GameObject m_Right2BrokenImage;
    [SerializeField]
    private GameObject m_Left1BrokenImage;
    [SerializeField]
    private GameObject m_Left2BrokenImage;
    [SerializeField]
    private GameObject m_UP1BrokenImage;
    [SerializeField]
    private GameObject m_UP2BrokenImage;
    [SerializeField]
    private GameObject m_Down1BrokenImage;
    [SerializeField]
    private GameObject m_Down2BrokenImage;
    [SerializeField]
    private List<BrokenPieceImage> m_BrokenPieceImages = new List<BrokenPieceImage>();
    public List<GridTilePlayer1> AttachedPiece => m_AttachedPiece;
    public List<double> TileCode => m_TileCode;
    public static Action<Vector2> s_ResetTopCopnnectionStatus;
    public static Action<Vector2> s_ResetBottomCopnnectionStatus;
    public static Action<Vector2> s_ResetLeftCopnnectionStatus;
    public static Action<Vector2> s_ResetRightCopnnectionStatus;
    #endregion
    #region Unity_CallBacks
    private void Awake()
    {
        m_TwoPlayerInputs = new SinglePlayerInputs();
    }
    private void OnEnable()
    {
        ContiniousFlowPieces.Clear();
        MakePlayerInputsActive();
        StartListeningEvents();
        StartCoroutine(GetWidthHeight());
    }
    private void OnDisable()
    {
        MakePlayerInputsDeActive();
        StopListeningEvents();
    }
    #endregion
    #region Private_Methods
    private void StartListeningEvents()
    {
        ViewTwoPlayerGame.s_SetGridTilePosition_Player1 += SetGridPiecePosition;
        ViewTwoPlayerGame.s_SetUpcomingBombPositon_Player1 += SetUpcomingBombPosition;
        ViewTwoPlayerGame.s_ActiveTilePiece_Player1 += ActiveTilePiece;
        ViewTwoPlayerGame.s_DeActiveTilePiece_Player1 += DeActiveTilePiece;
        ViewTwoPlayerGame.s_PlaceTilePiece_Player1 += PlaceTilePiece;
        ViewTwoPlayerGame.s_ActiveBomb_Player1 += ActiveBomb;
        ViewTwoPlayerGame.s_PlaceBomb_Player1 += PlaceBombTwoPlayer;
        ViewTwoPlayerGame.s_CheckTileLeftVectorStatus_Player1 += CheckTileLeftStatus;
        ViewTwoPlayerGame.s_CheckTileRightVectorStatus_Player1 += CheckTileRightStatus;
        ViewTwoPlayerGame.s_CheckTileUPVectorStatus_Player1 += CheckTileUpStatus;
        ViewTwoPlayerGame.s_CheckTileDownVectorStatus_Player1 += CheckTileDownStatus;
        ViewTwoPlayerGame.s_SetStartingPositon_Player1 += SetStartingPiecePosition;
        ViewTwoPlayerGame.s_OnStartingPieceCounterChanges_Player1 += OnStartingPieceCounterChanges;
        ViewTwoPlayerGame.s_OnRoundObstacleSpawn_Player1 += CreateRoundObstacle;
        ViewTwoPlayerGame.s_OnHorizontalObstacleSpawn_Player1 += CreateHorizontalObstacle;
        ViewTwoPlayerGame.s_OnVerticalObstacleSpawn_Player1 += CreateVerticalObstacle;
        ViewTwoPlayerGame.s_CheckStartingPieceLoopStatus_Player1 += UpdateStartingPieceLoopStatus;
        ViewTwoPlayerGame.s_MidCounterLoopStatus_Player1 += CheckPieceLoopStatus;
        ViewTwoPlayerGame.s_CalculateDetachPieces_Player1 += CalculateDetachPieces;
        ViewTwoPlayerGame.s_OnGameOvers_Player1 += OnGameOvers;
        ViewTwoPlayerGame.s_StartingPieceCounted_Player1 += StartedPieceCounted;
        ViewTwoPlayerGame.s_SpawnNextStartingPiece_Player1 += CheckStartingPieceSafeArea;
        ViewTwoPlayerGame.s_SpawnStartingPiecePlayer1_Horizontal += CheckStartingPieceHorizontalSafeArea;
        ViewTwoPlayerGame.s_SpawnStartingPiecePlayer1_Vertical += CheckStartingPieceVerticalSafeArea;
        ViewTwoPlayerGame.s_CheckSafeAreaStatus_Player1 += StartingPieceSafeAreaStatus;
        ViewTwoPlayerGame.s_CheckStartingPieceHorizontalSafeAreaStatus_Player1 += StartingPieceHorizontalSafeAreaStatus;
        ViewTwoPlayerGame.s_CheckStartingPieceVerticalSafeAreaStatus_Player1 += StartingPieceVerticalSafeAreaStatus;
        ViewTwoPlayerGame.s_SpawnStartingPiece_Player1 += SpawnStartingPiece;
        ViewTwoPlayerGame.s_StartFlowAnimation_Player1 += StartFlowAnimation;
        ViewTwoPlayerGame.s_SetCurrentPiece_Player1 += SetPieceActive;
        ViewTwoPlayerGame.s_NextTileValidPosition_Player1 += ValidNextPosition;
        ViewTwoPlayerGame.s_LockLoopPiece_Player1 += LockLoopPiece;
        ViewTwoPlayerGame.s_ResetStartingPiecePositions_Player1 += ResetStartingPiecePosition;
        ViewTwoPlayerGame.s_TriggerBrokenAnimation_Player1 += TriggerBrokenPieceAnimation;
        HoldTilePlayer1.s_StorePiece += HoldCurrentActivePiece;
        ViewTwoPlayerGame.s_StorePieceTriggered_Player1 += ResetTilePiece;
        ViewTwoPlayerGame.s_ResetCurrentPosition_Player1 += ResetCurrentPosition;
        HoldTilePlayer1.s_ExtractPiece += ExtractHoldPiece;
        HoldTilePlayer1.s_CheckActiveTileStatus += CheckTileActiveStatus;
        ViewTwoPlayerGame.s_FlashStartingPiece_Player1 += FlashStartingPiece;
        ViewTwoPlayerGame.s_HandlContinuousFlowAnimation_Player1 += HandleContinousTileFlow;
        s_AssignTileCode += AssignTilePieceCode;
        s_DeAttachPieces += DeAttachPiece;
        s_ResetTopCopnnectionStatus += ResetTopCopnnectionStatus;
        s_ResetBottomCopnnectionStatus += ResetBottomCopnnectionStatus;
        s_ResetLeftCopnnectionStatus += ResetLeftCopnnectionStatus;
        s_ResetRightCopnnectionStatus += ResetRightCopnnectionStatus;
    }
    private void StopListeningEvents()
    {
        ViewTwoPlayerGame.s_SetGridTilePosition_Player1 -= SetGridPiecePosition;
        ViewTwoPlayerGame.s_SetUpcomingBombPositon_Player1 -= SetUpcomingBombPosition;
        ViewTwoPlayerGame.s_ActiveTilePiece_Player1 -= ActiveTilePiece;
        ViewTwoPlayerGame.s_DeActiveTilePiece_Player1 -= DeActiveTilePiece;
        ViewTwoPlayerGame.s_PlaceTilePiece_Player1 -= PlaceTilePiece;
        ViewTwoPlayerGame.s_ActiveBomb_Player1 -= ActiveBomb;
        ViewTwoPlayerGame.s_PlaceBomb_Player1 -= PlaceBombTwoPlayer;
        ViewTwoPlayerGame.s_CheckTileLeftVectorStatus_Player1 -= CheckTileLeftStatus;
        ViewTwoPlayerGame.s_CheckTileRightVectorStatus_Player1 -= CheckTileRightStatus;
        ViewTwoPlayerGame.s_CheckTileUPVectorStatus_Player1 -= CheckTileUpStatus;
        ViewTwoPlayerGame.s_CheckTileDownVectorStatus_Player1 -= CheckTileDownStatus;
        ViewTwoPlayerGame.s_SetStartingPositon_Player1 -= SetStartingPiecePosition;
        ViewTwoPlayerGame.s_OnStartingPieceCounterChanges_Player1 -= OnStartingPieceCounterChanges;
        ViewTwoPlayerGame.s_OnRoundObstacleSpawn_Player1 -= CreateRoundObstacle;
        ViewTwoPlayerGame.s_OnHorizontalObstacleSpawn_Player1 -= CreateHorizontalObstacle;
        ViewTwoPlayerGame.s_OnVerticalObstacleSpawn_Player1 -= CreateVerticalObstacle;
        ViewTwoPlayerGame.s_CheckStartingPieceLoopStatus_Player1 -= UpdateStartingPieceLoopStatus;
        ViewTwoPlayerGame.s_MidCounterLoopStatus_Player1 -= CheckPieceLoopStatus;
        ViewTwoPlayerGame.s_CalculateDetachPieces_Player1 -= CalculateDetachPieces;
        ViewTwoPlayerGame.s_OnGameOvers_Player1 -= OnGameOvers;
        ViewTwoPlayerGame.s_StartingPieceCounted_Player1 -= StartedPieceCounted;
        ViewTwoPlayerGame.s_SpawnNextStartingPiece_Player1 -= CheckStartingPieceSafeArea;
        ViewTwoPlayerGame.s_SpawnStartingPiecePlayer1_Horizontal -= CheckStartingPieceHorizontalSafeArea;
        ViewTwoPlayerGame.s_SpawnStartingPiecePlayer1_Vertical -= CheckStartingPieceVerticalSafeArea;
        ViewTwoPlayerGame.s_SpawnStartingPiece_Player1 -= SpawnStartingPiece;
        ViewTwoPlayerGame.s_CheckSafeAreaStatus_Player1 -= StartingPieceSafeAreaStatus;
        ViewTwoPlayerGame.s_CheckStartingPieceHorizontalSafeAreaStatus_Player1 -= StartingPieceHorizontalSafeAreaStatus;
        ViewTwoPlayerGame.s_CheckStartingPieceVerticalSafeAreaStatus_Player1 -= StartingPieceVerticalSafeAreaStatus;
        ViewTwoPlayerGame.s_StartFlowAnimation_Player1 -= StartFlowAnimation;
        ViewTwoPlayerGame.s_SetCurrentPiece_Player1 -= SetPieceActive;
        ViewTwoPlayerGame.s_NextTileValidPosition_Player1 -= ValidNextPosition;
        ViewTwoPlayerGame.s_LockLoopPiece_Player1 -= LockLoopPiece;
        ViewTwoPlayerGame.s_ResetStartingPiecePositions_Player1 -= ResetStartingPiecePosition;
        ViewTwoPlayerGame.s_TriggerBrokenAnimation_Player1 -= TriggerBrokenPieceAnimation;
        HoldTilePlayer1.s_StorePiece -= HoldCurrentActivePiece;
        ViewTwoPlayerGame.s_StorePieceTriggered_Player1 -= ResetTilePiece;
        ViewTwoPlayerGame.s_ResetCurrentPosition_Player1 -= ResetCurrentPosition;
        HoldTilePlayer1.s_ExtractPiece -= ExtractHoldPiece;
        HoldTilePlayer1.s_CheckActiveTileStatus -= CheckTileActiveStatus;
        ViewTwoPlayerGame.s_FlashStartingPiece_Player1 -= FlashStartingPiece;
        ViewTwoPlayerGame.s_HandlContinuousFlowAnimation_Player1 -= HandleContinousTileFlow;
        s_AssignTileCode -= AssignTilePieceCode;
        s_DeAttachPieces -= DeAttachPiece;
        s_ResetTopCopnnectionStatus -= ResetTopCopnnectionStatus;
        s_ResetBottomCopnnectionStatus -= ResetBottomCopnnectionStatus;
        s_ResetLeftCopnnectionStatus -= ResetLeftCopnnectionStatus;
        s_ResetRightCopnnectionStatus -= ResetRightCopnnectionStatus;
    }

    private void Update()
    {
        HandleTileClockwiseRotation();
        HandleAntiClockRotation();
    }

    private void PlaceBombTwoPlayer(Vector2 gridPosition)
    {
        if (p_GridPosition == gridPosition)
        {
            PlaceBomb(gridPosition);
            s_ResetTopCopnnectionStatus?.Invoke(gridPosition + Vector2.down);
            s_ResetBottomCopnnectionStatus?.Invoke(gridPosition + Vector2.up);
            s_ResetLeftCopnnectionStatus?.Invoke(gridPosition + Vector2.right);
            s_ResetRightCopnnectionStatus?.Invoke(gridPosition + Vector2.left);
            StartCoroutine(GameManager.instace.ShakeTwoPlayerCamera(0.6f, 4f));
        }
    }

    private void HoldCurrentActivePiece(TilePiece currentHoldTile, Action<TilePiece> activeTilePiece, Action<Vector2> resetHoldTile)
    {
        if (m_isSelected || m_IsBomb)
        {
            TilePiece currentActiveTile = currentHoldTile;
            activeTilePiece?.Invoke(this);
            if (currentHoldTile != null)
            {
                ExtractHoldPiece(SwapTilePlayer1.Instance);
                return;
            }
            else
            {
                resetHoldTile?.Invoke(this.GridPosition);
            }
        }
    }

    private void TriggerBrokenPieceAnimation(Vector2 targetPiece, float time, Vector2 flowVector)
    {
        if (p_GridPosition == targetPiece)
        {
            StartCoroutine(StartBrokenEndAnimations(time, flowVector));
        }
    }

    private void ValidNextPosition(Vector2 gridPosition, Action<GridTilePlayer1> validTile)
    {
        if (!m_IsObstacle && !m_IsStartingPiece && (p_GridPosition == gridPosition) && !isLoopPiece)
        {
            validTile?.Invoke(this);
        }
    }

    private void ResetStartingPiecePosition(Vector2 gridPosition)
    {

        if (p_GridPosition == gridPosition)
        {
            m_isLoopPiece = false;
            m_IsStartingPiece = false;
            m_PieceLoop.Clear();
            ContiniousFlowPieces.Clear();
            m_StartingPieceLoopPool.Clear();
            m_IsStartingPiece = false;
            m_IsObstacle = false;
            m_IsLoopCompleted = false;
            m_StartingPieceImage.gameObject.SetActive(false);
            p_StartingPieceCounterText.gameObject.SetActive(false);
            m_IsStartingPieceCounted = false;
            m_StartedPieceCounted = false;
            PlaceBomb(gridPosition);
            if (this.transform.parent.name == Constants.STARTINGPIECEAREAPARENTTWOPLAYER)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void StartFlowAnimation(Vector2 GridPosition, float time, Action<Vector2> startingPiecePosition, bool brokernLoopStatus)
    {
        if (p_GridPosition == GridPosition)
        {
            Action normalPieceAction = () =>
            {
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_FlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    if (!brokernLoopStatus)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                    }
                    else
                    {
                        ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                    }
                    StartCoroutine(StartFillingFlowImage(time));
                }
            };
            Action<Action<Vector2>> startingPieceAction = (Action<Vector2> startingPiecePosition) =>
            {
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    p_StaringPieceFlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_StaringPieceFlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_StaringPieceFlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    if (!brokernLoopStatus)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.ClosedLoopPoints);
                    }
                    else
                    {
                        ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                    }
                    StartCoroutine(StartStartingPieceFillingFlowImage(time));
                    StartCoroutine(ExplodeStarttingPiece(this, time, startingPiecePosition));

                }
            };
            Action CrossPieceAction = () =>
            {
                CrossTypeAnimation currentFillImage = null;
                if (!p_IsDualFlow)
                {
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe1Vector);
                }
                else
                {
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe2Vector);

                }

                if (currentFillImage != null)
                {
                    if (!p_IsDualFlow)
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);

                        p_FlowFillImage.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                        StartCoroutine(StartFillingFlowImage(time));
                        p_IsDualFlow = true;
                    }
                    else
                    {
                        ShowPositiveScore(GameData.Instance.GameOptions.CrossPoints);

                        p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImageDualFlow.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImageDualFlow.fillOrigin = currentFillImage.fillOrigin;
                        StartCoroutine(StartFillingDualImage(time));
                    }
                }
            };

            if (m_Type != Constants.CROSS_TYPEPIECE)
                normalPieceAction?.Invoke();
            if (m_Type == Constants.CROSS_TYPEPIECE)
                CrossPieceAction?.Invoke();
            if (m_Type == Constants.STARTING_TYPEPIECE)
                startingPieceAction?.Invoke(startingPiecePosition);
        }
    }


    private void CalculateDetachPieces(Action<int> pieceCount, bool isLoopBreaked)
    {
        int pieces = 0;
        if (!m_isLoopPiece && !m_Canplace && !m_IsStartingPiece && !m_IsObstacle)
        {
            Debug.Log("Tile Position: " + p_GridPosition);
            pieces++;
            pieceCount?.Invoke(pieces);
            if (!isLoopBreaked)
            {
                ShowNegativeScore(GameData.Instance.GameOptions.UnusedTilePenalty);
                StartCoroutine(ExplodeUnusedPiece());//recentchanges
            }
        }
    }
    private void SetStartingPiecePosition(Vector2 positon, int startingPieceIndex, int angle)
    {
        if (p_GridPosition == positon)
        {
            CreateStartingPiece(startingPieceIndex, angle);
        }
    }
    private void DeAttachPiece(Vector2 targetPiece, bool Player1, bool Player2)
    {
        if (targetPiece == p_GridPosition && Player1)
        {
            for (int PieceCount = (int)Constants.ZERO; PieceCount < m_AttachedPiece.Count; PieceCount++)
            {
                int index = m_AttachedPiece[PieceCount].m_AttachedPiece.FindIndex(element => element.GridPosition == targetPiece);
                if (index != Constants.NULLINDEX)
                    m_AttachedPiece[PieceCount].m_AttachedPiece.RemoveAt(index);
            }
            m_AttachedPiece.Clear();
        }
    }

    private void OnStartingPieceCounterChanges(Action<GridTilePlayer1> startingPiece, Action<GridTilePlayer1> loopCompletecallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTilePlayer1> FlowAnimationCallback)
    {
        Action startingPieceAction = () =>
        {
            DecrementStartingPieceCounter(startingPiece, loopCompletecallback, increaseTimer, remainingTime, FlowAnimationCallback);
            m_StartedPieceCounted = true;
        };
        if (IsStartingPiece && !m_StartedPieceCounted)
            startingPieceAction?.Invoke();
    }

    private void DecrementStartingPieceCounter(Action<GridTilePlayer1> startingPiece, Action<GridTilePlayer1> loopCompleteCallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTilePlayer1> FlowAnimationCallback)
    {
        StartCoroutine(DecrementCounter(startingPiece, loopCompleteCallback, increaseTimer, remainingTime, FlowAnimationCallback));
    }

    private void SpawnNewStartingPiece(int timer, bool horizontalStatus, bool verticalStatus)
    {
        m_Canplace = false;
        m_IsStartingPiece = true;
        p_BombImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        float currentWidth = GetComponent<RectTransform>().sizeDelta.x;
        RectTransform pieceImage = m_StartingPieceImage.GetComponent<RectTransform>();
        m_Type = Constants.STARTING_TYPEPIECE;
        var startingPieceCounterValues = GameData.Instance.StartingPiecesIndex.Find(element => element.StartingPieceIndex == m_StartingPieceIndex);
        p_StartingPieceCounter = timer;
        p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
        p_StartingPieceCounterText.gameObject.SetActive(true);
        List<float> angleList;
        if (horizontalStatus)
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE90,
                 Constants.PIECE_ANGLE270,
            };
        }
        else if (verticalStatus)
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE0,
                 Constants.PIECE_ANGLE180,
            };
        }
        else
        {
            angleList = new List<float>()
            {
                 Constants.PIECE_ANGLE0,
                 Constants.PIECE_ANGLE90,
                 Constants.PIECE_ANGLE180,
                 Constants.PIECE_ANGLE270,
            };

        }
        int randomIndex = (int)Random.Range(Constants.ZERO, (angleList.Count - Constants.INT_ONE));
        m_Angle = angleList[randomIndex];
        var FlowVector = GameData.Instance.StartingPieceFlowVectors.Find(angle => angle.Angle == m_Angle);
        var StartingPieceSprite = GameData.Instance.StartingPieceAnglesVariations.Find(element => element.Angle == m_Angle);
        var StartingPieceFillImages = GameData.Instance.StartingPieceFlowFillImages.Find(element => element.Angle == m_Angle);
        m_StartingPieceImage.sprite = StartingPieceSprite.StartingPieceSprite;
        p_StaringPieceFlowFillImage.sprite = StartingPieceFillImages.StartingPieceSprite;
        p_StaringPieceFlowFillImage.gameObject.SetActive(true);
        pieceImage.sizeDelta = (StartingPieceSprite.StartingPieceSprite.rect.width < StartingPieceSprite.StartingPieceSprite.rect.height) ? new Vector2(currentWidth, currentWidth * Constants.STARTTINGPIECEBALANCERATIO) : new Vector2(currentWidth * Constants.STARTTINGPIECEBALANCERATIO, currentWidth);
        p_StaringPieceFlowFillImage.GetComponent<RectTransform>().sizeDelta = pieceImage.sizeDelta;
        p_StaringPieceFlowFillImage.fillAmount = Constants.ZERO;
        m_StartingPieceFlowVector = FlowVector.flowVector;
        m_StartingPieceImage.gameObject.SetActive(true);
        GetCurrentTileCode();
    }

    private void SpawnStartingPiece(Vector2 positon, int timer, bool horizontalStatus, bool verticalStatus)
    {
        if (p_GridPosition == positon && m_Canplace)
        {
            SpawnNewStartingPiece(timer, horizontalStatus, verticalStatus);
        }
    }

    private void HandleContinousTileFlow(Vector2 previousTile, Vector2 currentTile, Action<Vector2, Vector2> nextPieceCallback, Action<GridTilePlayer1, float> lastPieceCallback, Action<int, bool> incrementScoreCallback, GridTilePlayer1 startingPiece, Action<Vector2> resetStartingPosition, Action<GridTilePlayer1> loopCompleteCallback, float flowSpeed, Action onFlowStarts)
    {
        if (p_GridPosition == currentTile)
        {
           // float speed = GameData.Instance.GameOptions.FlowSpeed;
            float speed = flowSpeed;
            float time = 1f / speed;

            Action normalPieceAction = () =>
            {
                m_StartingPieceFlowVector = currentTile - previousTile;
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    LockLoopPiece(this.p_GridPosition);
                    p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_FlowFillImage.fillClockwise = currentFillImage.Clockwise;
                    startingPiece.ContiniousFlowPieces.Add(this);
                    ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                    incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.LengthPoints, false);
                    StartCoroutine(StartFillingContinousFlowForNextPosition(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onFlowStarts));
                }
            };
            Action crossPieceAction = () =>
            {
                LockLoopPiece(this.p_GridPosition);
                m_StartingPieceFlowVector = currentTile - previousTile;
                CrossTypeAnimation currentFillImage = null;
                if (!p_IsDualFlow)
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe1Vector);
                }
                else
                {
                    p_CrossPiecePipe2Vector = m_StartingPieceFlowVector;
                    currentFillImage = GameData.Instance.CrossTypeAnimation.Find(element => element.CrossTypeAngle == m_Angle).FlowAnimationAngle.Find(element => element.FlowVector == p_CrossPiecePipe2Vector);
                }

                if (currentFillImage != null)
                {
                    if (!p_IsDualFlow)
                    {
                        p_FlowFillImage.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImage.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImage.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                        ShowPositiveScore(GameData.Instance.GameOptions.LengthPoints);
                        startingPiece.ContiniousFlowPieces.Add(this);
                        incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.LengthPoints, false);
                        StartCoroutine(StartFillingContinousFlowForNextPosition(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onFlowStarts));
                        p_IsDualFlow = true;
                    }
                    else
                    {
                        p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles = new Vector3(p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.x,
                                                                                      p_FlowFillImageDualFlow.GetComponent<RectTransform>().eulerAngles.y,
                                                                                      currentFillImage.Angle);
                        p_FlowFillImageDualFlow.fillMethod = currentFillImage.fillMethod;
                        p_FlowFillImageDualFlow.fillOrigin = currentFillImage.fillOrigin;
                        ShowPositiveScore(GameData.Instance.GameOptions.CrossPoints);
                        startingPiece.ContiniousFlowPieces.Add(this);
                        incrementScoreCallback?.Invoke(GameData.Instance.GameOptions.CrossPoints, false);
                        StartCoroutine(StartFillingContinousFlowForDualImage(nextPieceCallback, lastPieceCallback, time, startingPiece, loopCompleteCallback, onFlowStarts));
                    }
                }
            };

            Action startingPieceAction = () =>
            {
                m_StartingPieceFlowVector = currentTile - previousTile;
                FlowAnimationFlowVector currentFillImage = GameData.Instance.FlowTypeAnimation.Find(element => element.Type == m_Type).FlowAnimationAngle.Find(element => element.Angle == m_Angle).FlowAnimationFlowVector.Find(element => element.FlowVector == StartingPieceFlowVector);
                if (currentFillImage != null)
                {
                    startingPiece.ContiniousFlowPieces.Add(this);
                    p_StaringPieceFlowFillImage.fillMethod = currentFillImage.fillMethod;
                    p_StaringPieceFlowFillImage.fillOrigin = currentFillImage.fillOrigin;
                    p_StaringPieceFlowFillImage.fillClockwise = currentFillImage.Clockwise;
                   // ShowPositiveScore(startingPiece.ContiniousFlowPieces.Count * GameData.Instance.GameOptions.ClosedLoopPoints);
                    ShowPositiveScore(GameData.Instance.GameOptions.ClosedLoopPoints);
                    if (transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
                    {
                        incrementScoreCallback?.Invoke(startingPiece.ContiniousFlowPieces.Count * GameData.Instance.GameOptions.ClosedLoopPoints, true);
                    }
                    StartCoroutine(StartStartingPieceFillingFlowImageContiniousFlow(time, startingPiece, loopCompleteCallback));
                    StartCoroutine(ExplodeContiniousFlowPiece(startingPiece, time, resetStartingPosition));
                }
            };
            if (IsStartingPiece)
            {
                startingPieceAction?.Invoke();
                return;
            }
            if (m_Type == Constants.CROSS_TYPEPIECE)
                crossPieceAction?.Invoke();
            else
                normalPieceAction?.Invoke();
        }
    }



    private void StartingPieceSafeAreaStatus(Vector2 currentPosition, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition && m_Canplace)
        {
            pieceStatus?.Invoke(true);
        }
    }

    private void StartingPieceHorizontalSafeAreaStatus(Vector2 currentPosition, List<GridTilePlayer1> loopPieces, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition)
        {
            int tileIndex = loopPieces.FindIndex(element => element.GridPosition == p_GridPosition);
            if (tileIndex == Constants.NULLINDEX)
                pieceStatus?.Invoke(true);
        }
    }
    private void StartingPieceVerticalSafeAreaStatus(Vector2 currentPosition, List<GridTilePlayer1> loopPieces, Action<bool> pieceStatus)
    {
        if (p_GridPosition == currentPosition)
        {
            int tileIndex = loopPieces.FindIndex(element => element.GridPosition == p_GridPosition);
            if (tileIndex == Constants.NULLINDEX)
                pieceStatus?.Invoke(true);
        }
    }

    private void CheckStartingPieceSafeArea(Action<Vector2, Action<bool>> safeAreaStatusCallback, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
            {
                pieceCount++;
            };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }

    }

    private void CheckStartingPieceHorizontalSafeArea(Action<Vector2, List<GridTilePlayer1>, Action<bool>> safeAreaStatusCallback, List<GridTilePlayer1> loopPieces, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
            {
                pieceCount++;
            };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceHorizontalSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, loopPieces, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceHorizontalSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }
    }

    private void CheckStartingPieceVerticalSafeArea(Action<Vector2, List<GridTilePlayer1>, Action<bool>> safeAreaStatusCallback, List<GridTilePlayer1> loopPieces, List<Vector2> spawnSpots)
    {
        if (m_Canplace)
        {
            int pieceCount = 0;
            Action<bool> statusAction = (bool status) =>
            {
                pieceCount++;
            };

            foreach (Vector2 safeAreaPosition in GameData.Instance.StartingPieceVerticalSafeAreaVectors)
            {
                safeAreaStatusCallback(safeAreaPosition + p_GridPosition, loopPieces, statusAction);
            }
            if (pieceCount == GameData.Instance.StartingPieceVerticalSafeAreaVectors.Count)
            {
                spawnSpots.Add(p_GridPosition);
            }
        }
    }

    private void CheckPieceLoopStatus(GridTilePlayer1 startingPiece, Vector2 NextPiecePosition, Vector2 flowVector, Action<GridTilePlayer1, Vector2, Vector2> SendNextAttachedPiece)
    {

        if (p_GridPosition == NextPiecePosition && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            m_IsAttached = true;
            GridTilePlayer1 nextPiece = null;
            int startingPieceIndex = m_AttachedPiece.FindIndex(element => element.IsStartingPiece);
            int startingPieceIndexinLoop = startingPiece.m_PieceLoop.FindIndex(element => element.IsStartingPiece);
            if (startingPieceIndex == Constants.NULLINDEX && startingPieceIndexinLoop == Constants.NULLINDEX)
            {
                return;
            }
            startingPiece.m_PieceLoop.Add(this);
            m_StartingPieceFlowVector = flowVector;
            if (m_Type == Constants.CROSS_TYPEPIECE)
            {
                if (!p_SingleFlowVector)
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    p_SingleFlowVector = true;
                }
                else
                {
                    p_CrossPiecePipe2Vector = m_StartingPieceFlowVector;
                }
                int crossTypeIndex = m_AttachedPiece.FindIndex(element => element.GridPosition == (p_GridPosition + flowVector));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece && (element.GridPosition == (p_GridPosition + flowVector) || element.GridPosition == (p_GridPosition - flowVector))); /*&& element.GridPosition == (p_GridPosition)*/
                if (crossTypeIndex != Constants.NULLINDEX)
                {
                    if (!m_AttachedPiece[crossTypeIndex].IsStartingPiece)
                    {
                        nextPiece = m_AttachedPiece[crossTypeIndex];
                        if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                        {
                            if (!nextPiece.p_SingleFlowVector)
                            {
                                nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                                nextPiece.p_SingleFlowVector = true;
                            }
                            else
                            {
                                nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                            }
                        }
                        flowVector = nextPiece.GridPosition - GridPosition;

                    }
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_PieceLoop.Add(m_AttachedPiece[startPiece]);
                }
            }
            else
            {
                int pieceIndex = m_AttachedPiece.FindIndex(element => element.GridPosition != (p_GridPosition - flowVector) && (!element.IsStartingPiece));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece); /*&& element.GridPosition == (p_GridPosition)*/
                if (pieceIndex != Constants.NULLINDEX)
                {
                    nextPiece = m_AttachedPiece[pieceIndex];
                    if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                    {
                        if (!nextPiece.p_SingleFlowVector)
                        {
                            nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                            nextPiece.p_SingleFlowVector = true;
                        }
                        else
                        {
                            nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                        }
                    }
                    flowVector = nextPiece.GridPosition - GridPosition;
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_PieceLoop.Add(m_AttachedPiece[startPiece]);
                }

            }

            if (nextPiece != null)
                SendNextAttachedPiece?.Invoke(startingPiece, nextPiece.GridPosition, flowVector);

        }
    }

    private void UpdateStartingPieceLoopStatus(GridTilePlayer1 startingPiece, Vector2 NextPiecePosition, Vector2 flowVector, Action<GridTilePlayer1, Vector2, Vector2> SendNextAttachedPiece)
    {

        if (p_GridPosition == NextPiecePosition && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            m_IsAttached = true;
            GridTilePlayer1 nextPiece = null;
            int startingPieceIndex = m_AttachedPiece.FindIndex(element => element.IsStartingPiece);
            int startingPieceIndexinLoop = startingPiece.m_StartingPieceLoopPool.FindIndex(element => element.IsStartingPiece);
            if (startingPieceIndex == Constants.NULLINDEX && startingPieceIndexinLoop == Constants.NULLINDEX)
            {
                return;
            }
            startingPiece.m_StartingPieceLoopPool.Add(this);
            m_StartingPieceFlowVector = flowVector;
            if (m_Type == Constants.CROSS_TYPEPIECE)
            {
                if (!p_SingleFlowVector)
                {
                    p_CrossPiecePipe1Vector = m_StartingPieceFlowVector;
                    p_SingleFlowVector = true;
                }
                int crossTypeIndex = m_AttachedPiece.FindIndex(element => element.GridPosition == (p_GridPosition + flowVector));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece && (element.GridPosition == (p_GridPosition + flowVector) || element.GridPosition == (p_GridPosition - flowVector)));
                if (crossTypeIndex != Constants.NULLINDEX)
                {
                    if (!m_AttachedPiece[crossTypeIndex].IsStartingPiece)
                    {
                        nextPiece = m_AttachedPiece[crossTypeIndex];
                        if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                        {
                            if (!nextPiece.p_SingleFlowVector)
                            {
                                nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                                nextPiece.p_SingleFlowVector = true;
                            }
                            else
                            {
                                nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                            }
                        }
                        flowVector = nextPiece.GridPosition - GridPosition;
                    }
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_StartingPieceLoopPool.Add(m_AttachedPiece[startPiece]);
                }
            }
            else
            {
                int pieceIndex = m_AttachedPiece.FindIndex(element => element.GridPosition != (p_GridPosition - flowVector) && (!element.IsStartingPiece));
                int startPiece = m_AttachedPiece.FindIndex(element => element.IsStartingPiece); /*&& element.GridPosition == (p_GridPosition)*/
                if (pieceIndex != Constants.NULLINDEX)
                {
                    nextPiece = m_AttachedPiece[pieceIndex];
                    if (nextPiece.Type == Constants.CROSS_TYPEPIECE)
                    {
                        if (!nextPiece.p_SingleFlowVector)
                        {
                            nextPiece.p_CrossPiecePipe1Vector = nextPiece.GridPosition - GridPosition;
                            nextPiece.p_SingleFlowVector = true;
                        }
                        else
                        {
                            nextPiece.p_CrossPiecePipe2Vector = nextPiece.GridPosition - GridPosition;
                        }
                    }
                    flowVector = nextPiece.GridPosition - GridPosition;
                }
                if (startPiece != Constants.NULLINDEX)
                {
                    startingPiece.m_StartingPieceLoopPool.Add(m_AttachedPiece[startPiece]);
                }
            }

            if (nextPiece != null)
                SendNextAttachedPiece?.Invoke(startingPiece, nextPiece.GridPosition, flowVector);
        }
    }
   
    private void CreateStartingPiece(int startingPieceIndex, int anglee)
    {
        m_Canplace = false;
        m_IsStartingPiece = true;
        p_BombImage.gameObject.SetActive(false);
        m_SelectedImage.gameObject.SetActive(false);
        m_StartingPieceIndex = startingPieceIndex;
        float currentWidth = GetComponent<RectTransform>().sizeDelta.x;
        RectTransform pieceImage = m_StartingPieceImage.GetComponent<RectTransform>();
        m_Type = Constants.STARTING_TYPEPIECE;
        var startingPieceCounterValues = GameData.Instance.StartingPiecesIndex.Find(element => element.StartingPieceIndex == m_StartingPieceIndex);
        p_StartingPieceCounter = GameData.Instance.GameOptions.StartingPieceTimer;
        p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
        p_StartingPieceCounterText.gameObject.SetActive(true);
        m_Angle = anglee;
        var FlowVector = GameData.Instance.StartingPieceFlowVectors.Find(angle => angle.Angle == m_Angle);
        var StartingPieceSprite = GameData.Instance.StartingPieceAnglesVariations.Find(element => element.Angle == m_Angle);
        var StartingPieceFillImages = GameData.Instance.StartingPieceFlowFillImages.Find(element => element.Angle == m_Angle);
        m_StartingPieceImage.sprite = StartingPieceSprite.StartingPieceSprite;
        p_StaringPieceFlowFillImage.sprite = StartingPieceFillImages.StartingPieceSprite;
        p_StaringPieceFlowFillImage.gameObject.SetActive(true);
        pieceImage.sizeDelta = (StartingPieceSprite.StartingPieceSprite.rect.width < StartingPieceSprite.StartingPieceSprite.rect.height) ? new Vector2(currentWidth, currentWidth * Constants.STARTTINGPIECEBALANCERATIO) : new Vector2(currentWidth * Constants.STARTTINGPIECEBALANCERATIO, currentWidth);
        p_StaringPieceFlowFillImage.GetComponent<RectTransform>().sizeDelta = pieceImage.sizeDelta;
        p_StaringPieceFlowFillImage.fillAmount = Constants.ZERO;
        m_StartingPieceImage.gameObject.SetActive(true);
        m_StartingPieceFlowVector = FlowVector.flowVector;
        GetCurrentTileCode();
    }

    private void MakePlayerInputsActive()
    {
        //keyboards
        m_RotatePieceClockwise = m_TwoPlayerInputs.TwoPlayers.RotatePieceClockwisePlayer1;
        m_RotatePieceAntiClockwise = m_TwoPlayerInputs.TwoPlayers.RotatePieceAntiClockwisePlayer1;
     
        m_RotatePieceClockwise.performed += RotatePieceClockwise;
        m_RotatePieceAntiClockwise.performed += RotatePieceAntiClockwise;
        m_RotatePieceClockwise.Enable();
        m_RotatePieceAntiClockwise.Enable();
        
    }
    private void MakePlayerInputsDeActive()
    {
        //keyboards
        m_RotatePieceClockwise.performed -= RotatePieceClockwise;
        m_RotatePieceAntiClockwise.performed -= RotatePieceAntiClockwise;
       
        m_RotatePieceClockwise.Disable();
        m_RotatePieceAntiClockwise.Disable();
       
    }

    private void RotatePieceClockwise(InputAction.CallbackContext context)
    {
       /* Action RotateAction = () =>
        {
            SoundManager.Instance.PlayRotateTilePieceSoundPlayer1();
            RotateTilePiece(-Constants.ANGLE_DIFF);
        };

        if (!m_isGameOver)
            RotateAction?.Invoke();*/
        
    }
    private void RotatePieceAntiClockwise(InputAction.CallbackContext context)
    {
        Action RotateAction = () =>
        {
            SoundManager.Instance.PlayRotateTilePieceSoundPlayer1();
            RotateTilePiece(Constants.ANGLE_DIFF);
        };
        if (!m_isGameOver && m_isSelected)
            RotateAction?.Invoke();
    }
    private void RotatePieceClockwiseGamepads(InputAction.CallbackContext context)
    {
        /* if (!m_isGameOver)
         {
             if(GamepadManager.Instance.m_Gamepad1 != null)
             {
                 if(GamepadManager.Instance.m_Gamepad1.rightTrigger.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.rightShoulder.wasPressedThisFrame)
                 {
                     Debug.Log("Rotation Gamepad Check");
                     RotateTilePiece(-Constants.ANGLE_DIFF);
                 }
             }
         }*/
    }

    private void HandleTileClockwiseRotation()
    {
        Action RotateAction = () =>
        {
            SoundManager.Instance.PlayRotateTilePieceSoundPlayer1();
            RotateTilePiece(-Constants.ANGLE_DIFF);
        };

        if (!m_isGameOver && m_isSelected)
        {
            if (GamepadManager.Instance.m_Gamepad1 != null)
            {
                if (GamepadManager.Instance.m_Gamepad1.rightTrigger.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.rightShoulder.wasPressedThisFrame)
                {
                    RotateAction?.Invoke();
                }
            }
        }
    }

    private void HandleAntiClockRotation()
    {
        Action RotateAction = () =>
        {
            SoundManager.Instance.PlayRotateTilePieceSoundPlayer1();
            RotateTilePiece(Constants.ANGLE_DIFF);
        };
        if (!m_isGameOver && m_isSelected)
        {
            if (GamepadManager.Instance.m_Gamepad1 != null)
            {
                if (GamepadManager.Instance.m_Gamepad1.bButton.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftShoulder.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftTrigger.wasPressedThisFrame)
                {
                    RotateAction?.Invoke();
                }
            }
        }
    }

    private void RotatePieceAntiClockwiseGamepads(InputAction.CallbackContext context)
    {
        /* if (!m_isGameOver)
         {
             if(GamepadManager.Instance.m_Gamepad1 != null)
             {
                 if(GamepadManager.Instance.m_Gamepad1.leftTrigger.wasPressedThisFrame || GamepadManager.Instance.m_Gamepad1.leftShoulder.wasPressedThisFrame)
                 {
                     RotateTilePiece(Constants.ANGLE_DIFF);
                 }
             }
         }*/
    }
    private void AssignTilePieceCode(Vector2 currentPosition, double[] tileCodes, bool isPlayer1, bool isPlayer2)
    {
        Action tileAction = () =>
        {
            m_TileCode.Clear();
            for (int tileIndex = 0; tileIndex < tileCodes.Length; tileIndex++)
            {
                m_TileCode.Add(tileCodes[tileIndex]);
            }

        };

        if (currentPosition == GridPosition && isPlayer1)
            tileAction?.Invoke();
    }
    protected void CheckTileUpStatus(GridTilePlayer1 currentTile, Vector2 UpTilePosition)
    {
        if (UpTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] == m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                currentTile.ValidConnection(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
                ValidConnection(p_BottomConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_TopConnectionStatus);
                InvalidConnection(p_BottomConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
            }
        }

        // FOR DUMMY STARTING PIECE
        if (UpTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] == m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                currentTile.ValidConnection(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
                ValidConnection(p_BottomConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != m_TileCode[Constants.TILECODEINDEX_3] && (currentTile.m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_TopConnectionStatus);
                InvalidConnection(p_BottomConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_TopConnectionStatus);
                DisableConnections(p_BottomConnectionStatus);
            }
        }

    }
    protected void CheckTileDownStatus(GridTilePlayer1 currentTile, Vector2 DownTilePosition)
    {
        if (DownTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] == m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                currentTile.ValidConnection(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
                ValidConnection(p_TopConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_BottomConnectionStatus);
                InvalidConnection(p_TopConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
            }
        }

        // FOR DUMMY STARTING PIECE
        if (DownTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] == m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                currentTile.ValidConnection(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
                ValidConnection(p_TopConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != m_TileCode[Constants.TILECODEINDEX_1] && (currentTile.m_TileCode[Constants.TILECODEINDEX_3] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_1] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_BottomConnectionStatus);
                InvalidConnection(p_TopConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_BottomConnectionStatus);
                DisableConnections(p_TopConnectionStatus);
            }
        }
    }

    protected void CheckTileLeftStatus(GridTilePlayer1 currentTile, Vector2 leftTilePosition)
    {
        if (leftTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] == m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                currentTile.ValidConnection(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
                ValidConnection(p_RightConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_LeftConnectionStatus);
                InvalidConnection(p_RightConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
            }
        }
        //FOR DUMMY STARTING PIECE
        if (leftTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] == m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {

                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                currentTile.ValidConnection(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
                ValidConnection(p_RightConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != m_TileCode[Constants.TILECODEINDEX_2] && (currentTile.m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_LeftConnectionStatus);
                InvalidConnection(p_RightConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_LeftConnectionStatus);
                DisableConnections(p_RightConnectionStatus);
            }
        }
    }
    protected void CheckTileRightStatus(GridTilePlayer1 currentTile, Vector2 rightTilePosition)
    {
        if (rightTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] == m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                m_PieceMask.enabled = false;
                currentTile.m_PieceMask.enabled = false;
                m_AttachedPiece.Add(currentTile);
                currentTile.m_AttachedPiece.Add(this);
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                currentTile.ValidConnection(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
                ValidConnection(p_LeftConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_RightConnectionStatus);
                InvalidConnection(p_LeftConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
            }
        }
        //FOR DUMMY STARTING PIECE
        if (rightTilePosition == p_GridPosition && canPlace == false && !m_IsObstacle && transform.parent.name != Constants.MAINGAMEAREAPARENTTWOPLAYERS)
        {
            if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] == m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO || m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                currentTile.ValidConnection(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
                ValidConnection(p_LeftConnectionStatus);
            }
            else if (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != m_TileCode[Constants.TILECODEINDEX_4] && (currentTile.m_TileCode[Constants.TILECODEINDEX_2] != Constants.ZERO && m_TileCode[Constants.TILECODEINDEX_4] != Constants.ZERO))
            {
                currentTile.InvalidConnection(currentTile.p_RightConnectionStatus);
                InvalidConnection(p_LeftConnectionStatus);
            }
            else
            {
                currentTile.DisableConnections(currentTile.p_RightConnectionStatus);
                DisableConnections(p_LeftConnectionStatus);
            }
        }
    }

    private void CheckLoopPieces(List<GridTilePlayer1> loopPieces)
    {
        GridTilePlayer1 startingPiece = loopPieces.Find(element => element.IsStartingPiece);
        int startingPieceCount = 0;
        foreach (GridTilePlayer1 tile in loopPieces)
        {
            if (tile.IsStartingPiece && tile.GridPosition == startingPiece.GridPosition)
            {
                startingPieceCount++;
            }
        }
        m_IsLoopCompleted = (startingPieceCount >= Constants.STARTINGPIECEPAIR) ? true : false;
    }

    #endregion

    #region COROUTINES
    private IEnumerator ExplodeStarttingPiece(GridTilePlayer1 startingPiece, float time, Action<Vector2> startingPiecePosition)
    {
        yield return new WaitForSeconds(time + 0.3f);
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < startingPiece.m_PieceLoop.Count; loopPieceIndex++)
        {
            if(startingPiece.m_PieceLoop[loopPieceIndex].IsBomb || startingPiece.m_PieceLoop[loopPieceIndex].isSelected)
            {
                startingPiece.m_PieceLoop[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.m_PieceLoop[loopPieceIndex].IgnoreSelectionTile(startingPiece.m_PieceLoop[loopPieceIndex].GridPosition);
            }
            if (!startingPiece.m_PieceLoop[loopPieceIndex].IsStartingPiece && !startingPiece.m_PieceLoop[loopPieceIndex].IsBomb && !startingPiece.m_PieceLoop[loopPieceIndex].isSelected)
            {
                startingPiece.m_PieceLoop[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.m_PieceLoop[loopPieceIndex].PlaceBomb(startingPiece.m_PieceLoop[loopPieceIndex].GridPosition);
            }
        }
        startingPiecePosition?.Invoke(startingPiece.GridPosition);
    }
    IEnumerator ExplodeUnusedPiece()
    {
        yield return new WaitForSeconds(1f);
        PlaceBomb(p_GridPosition);
    }

    private IEnumerator StartFillingFlowImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImage.fillAmount = valueToLerp;

    }

    private IEnumerator StartStartingPieceFillingFlowImageContiniousFlow(float time, GridTilePlayer1 startingPiece, Action<GridTilePlayer1> loopCompleteCallback)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();

        time = (startingPiece.m_IsLoopCompleted) ? Constants.INT_ONE / Constants.MAX_FLOWSPEED : time;
        while (timeElapsed < time)
        {
            if (!startingPiece.m_IsLoopCompleted)
            {
                startingPiece.m_PieceLoop.Clear();
                loopCompleteCallback?.Invoke(startingPiece);
                startingPiece.CheckLoopPieces(startingPiece.m_PieceLoop);
            }
            if (startingPiece.m_IsLoopCompleted)
            {
                time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
            }
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_StaringPieceFlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_StaringPieceFlowFillImage.fillAmount = valueToLerp;

    }

    private IEnumerator StartStartingPieceFillingFlowImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_StaringPieceFlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_StaringPieceFlowFillImage.fillAmount = valueToLerp;

    }

    private IEnumerator StartFillingDualImage(float time)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        while (timeElapsed < time)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImageDualFlow.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImageDualFlow.fillAmount = valueToLerp;

    }

    private IEnumerator DecrementCounter(Action<GridTilePlayer1> startingPiece, Action<GridTilePlayer1> loopCompleteCallback, Action<bool> increaseTimer, int[] remainingTime, Action<GridTilePlayer1> FlowAnimationCallback)
    {
        Action loopCompleteAction = () =>
        {
            remainingTime[Constants.INT_ZERO] = p_StartingPieceCounter;
            increaseTimer?.Invoke(false);
        };
        while (p_StartingPieceCounter > (int)(Constants.ZERO) && !m_IsLoopCompleted)
        {
            m_PieceLoop.Clear();
            if (transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
                loopCompleteCallback?.Invoke(this);
            CheckLoopPieces(m_PieceLoop);
            if (m_IsLoopCompleted)
            {
                loopCompleteAction?.Invoke();
                FlowAnimationCallback?.Invoke(this);
                StopCoroutine("DecrementCounter");
            }
            p_StartingPieceCounter--;
            p_StartingPieceCounterText.color = (p_StartingPieceCounter < Constants.COUNTERWARNING) ? Color.red : Color.white;
            if (p_StartingPieceCounter < Constants.COUNTERWARNING && transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS)
                SoundManager.Instance.PlayCountDownSound();
            p_StartingPieceCounterText.text = p_StartingPieceCounter.ToString();
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForEndOfFrame();
        if (transform.parent.name == Constants.MAINGAMEAREAPARENTTWOPLAYERS && !m_IsLoopCompleted)
        {
            yield return new WaitForSeconds(p_StartingPieceCounter);
            startingPiece?.Invoke(this);
        }
    }

    private IEnumerator StartFillingContinousFlowForNextPosition(Action<Vector2, Vector2> nextPieceCallback, Action<GridTilePlayer1, float> lastPieceCallback, float time, GridTilePlayer1 startingPiece, Action<GridTilePlayer1> loopCompleteCallback, Action onFlowStarts)
    {
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        if (startingPiece.m_IsLoopCompleted)
        {
            time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
            onFlowStarts?.Invoke();
        }
        while (timeElapsed < time)
        {
            if (!startingPiece.m_IsLoopCompleted)
            {
                startingPiece.m_PieceLoop.Clear();
                loopCompleteCallback?.Invoke(startingPiece);
                startingPiece.CheckLoopPieces(startingPiece.m_PieceLoop);
            }
            if (startingPiece.m_IsLoopCompleted)
            {
                time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
                onFlowStarts?.Invoke();
            }
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImage.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImage.fillAmount = valueToLerp;
        if (m_Type != Constants.CROSS_TYPEPIECE)
        {
            m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatus(nextPieceCallback, lastPieceCallback, time));
        }
        else
        {
            m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatusForCrossPiece(nextPieceCallback, lastPieceCallback, time));
        }
    }

    private IEnumerator NextPieceAnimationAttachedPieceStatus(Action<Vector2, Vector2> nextPieceCallback, Action<GridTilePlayer1, float> lastPieceCallback, float time)
    {
        yield return new WaitForSeconds(Constants.ZERO);
        GridTilePlayer1 nextTilePosition = m_AttachedPiece.Find(tile => tile.GridPosition != (p_GridPosition - m_StartingPieceFlowVector));
        if (nextTilePosition != null)
        {
            nextPieceCallback?.Invoke(p_GridPosition, nextTilePosition.GridPosition);
        }
        else
        {
            lastPieceCallback?.Invoke(this, time);
        }
    }

    private IEnumerator StartFillingContinousFlowForDualImage(Action<Vector2, Vector2> nextPieceCallback, Action<GridTilePlayer1, float> lastPieceCallback, float time, GridTilePlayer1 startingPiece, Action<GridTilePlayer1> loopCompleteCallback, Action onFlowStarts)
    {
        // float lerpDuration = 3;
        float startValue = Constants.INT_ZERO;
        float endValue = Constants.INT_ONE;
        float valueToLerp;
        float timeElapsed = 0;
        SoundManager.Instance.PlayLiquidFilledSound();
        if (startingPiece.m_IsLoopCompleted)
        {
            time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
            onFlowStarts?.Invoke();
        }
        while (timeElapsed < time)
        {
            if (!startingPiece.m_IsLoopCompleted)
            {
                startingPiece.m_PieceLoop.Clear();
                loopCompleteCallback?.Invoke(startingPiece);
                startingPiece.CheckLoopPieces(startingPiece.m_PieceLoop);
            }
            if (startingPiece.m_IsLoopCompleted)
            {
                time = Constants.INT_ONE / Constants.MAX_FLOWSPEED;
                onFlowStarts?.Invoke();

            }
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / time);
            p_FlowFillImageDualFlow.fillAmount = valueToLerp;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        p_FlowFillImageDualFlow.fillAmount = valueToLerp;
        m_NextPieceAnimationCoroutine = StartCoroutine(NextPieceAnimationAttachedPieceStatusForCrossPiece(nextPieceCallback, lastPieceCallback, time));
    }

    private IEnumerator NextPieceAnimationAttachedPieceStatusForCrossPiece(Action<Vector2, Vector2> nextPieceCallback, Action<GridTilePlayer1, float> lastPieceCallback, float time)
    {
        yield return new WaitForSeconds(Constants.ZERO);
        Vector2 crossPieceFlowVector = p_CrossPiecePipe1Vector;
        if (!p_IsDualContiniousFlow)
        {
            crossPieceFlowVector = p_CrossPiecePipe1Vector;
            p_IsDualContiniousFlow = true;
        }
        else
        {
            crossPieceFlowVector = p_CrossPiecePipe2Vector;

        }
        GridTilePlayer1 nextTilePosition = m_AttachedPiece.Find(tile => tile.GridPosition == (p_GridPosition + crossPieceFlowVector));
        if (nextTilePosition != null)
        {
            nextPieceCallback?.Invoke(p_GridPosition, nextTilePosition.GridPosition);
        }
        else
        {
            lastPieceCallback?.Invoke(this, time);
        }
    }


    IEnumerator GetWidthHeight()
    {
        yield return new WaitForSeconds(2f);
        Vector2 tileSize = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y);
        RectTransform Right1 = m_Right1BrokenImage.GetComponent<RectTransform>();
        RectTransform Right2 = m_Right2BrokenImage.GetComponent<RectTransform>();
        RectTransform Left1 = m_Left1BrokenImage.GetComponent<RectTransform>();
        RectTransform Left2 = m_Left2BrokenImage.GetComponent<RectTransform>();
        RectTransform UP1 = m_UP1BrokenImage.GetComponent<RectTransform>();
        RectTransform UP2 = m_UP2BrokenImage.GetComponent<RectTransform>();
        RectTransform Down1 = m_Down1BrokenImage.GetComponent<RectTransform>();
        RectTransform Down2 = m_Down2BrokenImage.GetComponent<RectTransform>();
        Right1.sizeDelta = Constants.INT_TWO * tileSize;
        Right1.anchoredPosition = new Vector2(tileSize.x, Right1.anchoredPosition.y);
        Right2.sizeDelta = Constants.INT_TWO * tileSize;
        Right2.anchoredPosition = new Vector2(tileSize.x, Right2.anchoredPosition.y);
        Left1.sizeDelta = Constants.INT_TWO * tileSize;
        Left1.anchoredPosition = new Vector2(-tileSize.x, Left1.anchoredPosition.y);
        Left2.sizeDelta = Constants.INT_TWO * tileSize;
        Left2.anchoredPosition = new Vector2(-tileSize.x, Left2.anchoredPosition.y);
        UP1.sizeDelta = Constants.INT_TWO * tileSize;
        UP1.anchoredPosition = new Vector2(UP1.anchoredPosition.x, tileSize.y);
        UP2.sizeDelta = Constants.INT_TWO * tileSize;
        UP2.anchoredPosition = new Vector2(UP2.anchoredPosition.x, tileSize.y);
        Down1.sizeDelta = Constants.INT_TWO * tileSize;
        Down1.anchoredPosition = new Vector2(Down1.anchoredPosition.x, -tileSize.y);
        Down2.sizeDelta = Constants.INT_TWO * tileSize;
        Down2.anchoredPosition = new Vector2(Down2.anchoredPosition.x, -tileSize.y);
    }

    private IEnumerator StartBrokenEndAnimations(float time, Vector2 targetVector)
    {
        yield return new WaitForSeconds(time);
        int tileCodeIndex = GameData.Instance.BrokenPieceVectors.SurroundingVectors.FindIndex(element => element == targetVector);
        if (tileCodeIndex != Constants.NULLINDEX)
        {
            GameObject targetBrokenPiece = m_BrokenPieceImages.Find(brokenpiece => brokenpiece.FlowVector == targetVector).targetImages.Find(element => element.tileCode == m_TileCode[tileCodeIndex]).BrokenFlowImage;
            if (targetBrokenPiece != null)
            {
                targetBrokenPiece.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator ExplodeContiniousFlowPiece(GridTilePlayer1 startingPiece, float time, Action<Vector2> startingPiecePosition)
    {
        time = (startingPiece.m_IsLoopCompleted) ? Constants.INT_ONE / Constants.MAX_FLOWSPEED : time;
        yield return new WaitForSeconds(time + 0.3f);
        for (int loopPieceIndex = Constants.INT_ZERO; loopPieceIndex < startingPiece.ContiniousFlowPieces.Count; loopPieceIndex++)
        {
            if (!startingPiece.ContiniousFlowPieces[loopPieceIndex].IsStartingPiece)
            {
                startingPiece.ContiniousFlowPieces[loopPieceIndex].m_isLoopPiece = false;
                startingPiece.ContiniousFlowPieces[loopPieceIndex].PlaceBomb(startingPiece.ContiniousFlowPieces[loopPieceIndex].GridPosition);
            }
        }
        startingPiecePosition?.Invoke(startingPiece.GridPosition);
    }

    #endregion
}
