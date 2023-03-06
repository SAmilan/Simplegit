using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Private_Variables
    [SerializeField]
    private UIView m_TwoPlayerLoopCompletedGameOver;
    [SerializeField]
    private UIView m_SinglePlayerNamePanel;
    [SerializeField]
    private UIView m_TwoPlayerNamePanel;
    [SerializeField]
    private UIView m_HomePage;
    [SerializeField]
    private UIView m_SinglePlayerScreen;
    [SerializeField]
    private UIView m_TwoPlayerScreen;
    [SerializeField]
    private UIView m_OptionPanel;
    [SerializeField]
    private UIView m_TwoPlayerGameOverPanel;
    [SerializeField]
    private UIView m_SinglePlayerGameOverPanel;
    [SerializeField]
    private int m_GameSideLengthSinglePlayer;
    [SerializeField]
    private int m_GameSideLengthTwoplayers;
    [Header("Side Panel Pieces")]
    [SerializeField]
    [Range(0f, 1f)]
    private float m_LtypePiece_1;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_LtypePiece_2;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_StraighttypePiece;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_CrosstypePiece;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_BombtypePiece;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_InversiontypePiece;
    [SerializeField]
    private int m_StartingPieceCount = 1;
    [SerializeField]
    private List<Vector2> m_StartingPiecePoolSinglePlayer = new List<Vector2>(4);
    [SerializeField]
    private List<Vector2> m_StartingPiecePoolTwoPlayer = new List<Vector2>(4);
    [SerializeField]
    private int m_RoundObstacleCount_SinglePlayer = 0;
    [SerializeField]
    private int m_RoundObstacleCount_TwoPlayer = 0;
    [SerializeField]
    private int m_HorizontalObstacleCount = 0;
    [SerializeField]
    private int m_VerticalObstacleCount = 0;
    private int m_StartingPieceStatus;
    [SerializeField]
    private int m_ObstacleStatus;

    [SerializeField]
    private string m_TwoPlayerModeLoopCompletedWinner;
    [SerializeField]
    private string m_TwoPlayerModePlayer1_Name;
    [SerializeField]
    private string m_TwoPlayerModePlayer2_Name;
    [SerializeField]
    private int m_TwoPlayerModePlayer1_Score;
    [SerializeField]
    private int m_TwoPlayerModePlayer2_Score;
    [SerializeField]
    private string m_SinglePlayerModePlayerName;
    [SerializeField]
    private int m_SinglePlayerModePlayerScore;
    [SerializeField]
    private GameObject m_Loader;
    private bool m_UpcomingTileStatus;
    [SerializeField]
    private int m_SinglePlayerRow;
    [SerializeField]
    private int m_SinglePlayerColumn;
    [SerializeField]
    private int m_TwoPlayerRow;
    [SerializeField]
    private int m_TwoPlayerColumn;
    [SerializeField]
    private int m_StartingPieceTimerValue;
    [SerializeField]
    private int m_LengthPoints;
    [SerializeField]
    private int m_CrossPiecePoints;
    [SerializeField]
    private int m_CloseLoopPoints;
    [SerializeField]
    private int m_UnusedTilePenalty;
    private float m_TileReplacementPenaltyValue;
    [SerializeField]
    private bool m_SinglePlayerGamePauseStatus = false;
    [SerializeField]
    private bool m_TwoPlayerGamePauseStatus = false;
    [SerializeField]
    private GameObject m_SinglePlayerCamera;
    [SerializeField]
    private GameObject m_TwoPlayerCamera;
    private bool m_FirstPlayerPaused = false;
    private bool m_SecondPlayerPaused = false;
   



    #endregion
    #region Public_Variables
    public float LtypePiece_1 => m_LtypePiece_1;
    public float LtypePiece_2 => m_LtypePiece_2;
    public float StraighttypePiece => m_StraighttypePiece;
    public float CrosstypePiece => m_CrosstypePiece;
    public float BombtypePiece => m_BombtypePiece;
    public float InversiontypePiece => m_InversiontypePiece;
    public int SidelengthSinglePlayers => m_GameSideLengthSinglePlayer;
    public int SidelengthTwoPlayers => m_GameSideLengthTwoplayers;
    public int StartingPieceCount => m_StartingPieceCount;
    public int RoundObstacleCount_SinglePlayer => m_RoundObstacleCount_SinglePlayer;
    public int RoundObstacleCount_TwoPlayer => m_RoundObstacleCount_TwoPlayer;
    public int HorizontalObstacleCount => m_HorizontalObstacleCount;
    public int VerticalObstacleCount => m_VerticalObstacleCount;
    public int ObstacleStatus => m_ObstacleStatus;
    public string TwoPlayerModePlayer1_Name => m_TwoPlayerModePlayer1_Name;
    public string TwoPlayerModePlayer2_Name => m_TwoPlayerModePlayer2_Name;
    public int TwoPlayerModePlayer1_Score => m_TwoPlayerModePlayer1_Score;
    public int TwoPlayerModePlayer2_Score => m_TwoPlayerModePlayer2_Score;
    public string SinglePlayerModePlayerName => m_SinglePlayerModePlayerName;
    public int SinglePlayerModePlayerScore => m_SinglePlayerModePlayerScore;
    public bool UpcomingTileStatus => m_UpcomingTileStatus;
    public int SinglePlayerRow => m_SinglePlayerRow;
    public int SinglePlayerColumn => m_SinglePlayerColumn;
    public int TwoPlayerRow => m_TwoPlayerRow;
    public int TwoPlayerColumn => m_TwoPlayerColumn;
    public float TileReplacementPenaltyValue => m_TileReplacementPenaltyValue;
    public int StartingPieceTimerValue => m_StartingPieceTimerValue;
    public int LengthPoints => m_LengthPoints;
    public int CrossPiecePoints => m_CrossPiecePoints;
    public int CloseLoopPoints => m_CloseLoopPoints;
    public int UnusedTilePenalty => m_UnusedTilePenalty;
    public string TwoPlayerModeLoopCompletedWinner => m_TwoPlayerModeLoopCompletedWinner;
    [SerializeField]
    public int CurrentGame = 0;


    public List<Vector2> StartingPiecePoolSinglePlayer => m_StartingPiecePoolSinglePlayer;
    public List<Vector2> StartingPiecePoolTwoPlayer => m_StartingPiecePoolTwoPlayer;
    public bool SinglePlayerGamePauseStatus => m_SinglePlayerGamePauseStatus;
    public bool TwoPlayerGamePauseStatus => m_TwoPlayerGamePauseStatus;
    public bool FirstPlayerPauseStatus => m_FirstPlayerPaused;
    public bool SecondPlayerPauseStatus => m_SecondPlayerPaused;
    public static GameManager instace;

    #endregion

    #region Unity_Callbacks

    private void Awake()
    {
        instace = this;
        File.Delete(Application.persistentDataPath + "/LeaderBoardData.txt");
        m_StartingPieceStatus = (int)Constants.ZERO;
    }
    private void OnEnable()
    {
        GameData.Instance.GameOptions = OptionsReader.instance.LoadFile();
        m_TileReplacementPenaltyValue = GameData.Instance.GameOptions.TileReplacementPenalty;
        m_SinglePlayerRow = GameData.Instance.GameOptions.SinglePlayerGameRows;
        m_SinglePlayerColumn = GameData.Instance.GameOptions.SinglePlayerGameColumns;
        m_TwoPlayerRow = GameData.Instance.GameOptions.TwoPlayerGameRows;
        m_TwoPlayerColumn = GameData.Instance.GameOptions.TwoPlayerGameColumn;
        m_UpcomingTileStatus = GameData.Instance.GameOptions.UpcomingTileStatus;
        m_TwoPlayerModePlayer1_Name = null;
        m_TwoPlayerModePlayer2_Name = null;
        m_TwoPlayerModePlayer1_Score = (int)Constants.ZERO;
        m_TwoPlayerModePlayer2_Score = (int)Constants.ZERO;
        m_SinglePlayerModePlayerName = null;
        m_SinglePlayerModePlayerScore = (int)Constants.ZERO;
        //skchnagesm_BombtypePiece = Constants.BOMB_NORMALPROBABILITY;
        OnBombProbabiltyChanges();
       // m_BombtypePiece = (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_NORMAL) ? Constants.BOMB_NORMALPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_FEW) ? Constants.BOMB_FEWPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_MANY) ? Constants.BOMB_MANYPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.NONEBOMB) ? Constants.NOBOMB : Constants.BOMB_NORMALPROBABILITY;
        m_StartingPieceTimerValue = GameData.Instance.GameOptions.StartingPieceTimer;
        m_LengthPoints = GameData.Instance.GameOptions.LengthPoints;
        m_CrossPiecePoints = GameData.Instance.GameOptions.CrossPoints;
        m_CloseLoopPoints = GameData.Instance.GameOptions.ClosedLoopPoints;
        m_UnusedTilePenalty = GameData.Instance.GameOptions.UnusedTilePenalty;
        //m_GameSideLengthSinglePlayer = (m_SinglePlayerRow < m_SinglePlayerColumn) ? m_SinglePlayerRow : m_SinglePlayerColumn;
        //m_GameSideLengthTwoplayers = (m_TwoPlayerRow < m_TwoPlayerColumn) ? m_TwoPlayerRow : m_TwoPlayerColumn;
        AllocateStartingPiecePool(GameData.Instance.GameOptions.SinglePlayerGameRows, GameData.Instance.GameOptions.SinglePlayerGameColumns, GameData.Instance.GameOptions.TwoPlayerGameRows, GameData.Instance.GameOptions.TwoPlayerGameColumn);
        AssignStartingPieceValues();
        CalculateRoundObstacles_SinglePlayer();
        CalculateRoundObstacles_TwoPlayers();
       //skchanges AssignCurrentObstaclesValues();
       //skchnages ViewOptionPanel.s_SideLength += GetSideLengthCallBack;
        ViewOptionPanel.s_StartingPieceStatusChanges += GetStartingPieceCallBack;
        ViewLevelOptionsPanel.s_OnOptionPanelCloses += OnOptionPanelCloses;
       // ViewOptionPanel.s_OnBombProbabityChanges += OnBombProbabiltyChanges;
        ViewOptionPanel.s_ObstacleStatusChanges += GetObstacleCount;
        //ViewOptionPanel.s_UpcomingTileStatusChanges += OnUpcomingStatusChanges;
        ViewSinglePlayerGame.s_OnGameEnds += OnGameEnds;
        ViewTwoPlayerGame.s_OnGameEnds += OnGameEnds;
        ViewTwoPlayerGame.s_OnGameOverCallback += OnTwoPlayerGameOvers;
        ViewSinglePlayerGame.s_OnGameManagerCallbacks += OnSinglePlayerGameOvers;
        ViewTwoPlayerGameOver.s_OnGameEnds += OnGameEnds;
        ViewSinglePlayerGameOvers.s_OnGameEnds += OnGameEnds;
        ViewSinglePlayerGameOvers.s_OnSinglePlayerScoreSubmitted += OnSinglePlayerScoreSubmitted;
        ViewSinglePlayerNamePanel.s_OnSinglePlayerNameChanges += OnSinglePlayerNameChanges;
        ViewTwoPlayerGameOver.s_OnTwoPlayerGameSubmitted += OnTwoPlayerScoreSubmitted;
        ViewTwoPlayerNamePanel.s_OnTwoPlayerNameChanges += OnTwoPlayerNameChanges;
        ViewLevelOptionsPanel.s_OnStartingPiecePoolChanges += AllocateStartingPiecePool;
        ViewLevelOptionsPanel.s_OnObstacleCoverageChanges += CalculateRoundObstacles_SinglePlayer;
        ViewLevelOptionsPanel.s_OnObstacleCoverageChanges += CalculateRoundObstacles_TwoPlayers;
        ViewLevelOptionsPanel.s_OnBombProbabiltyChanges += OnBombProbabiltyChanges;
        ViewLevelOptionsPanel.s_OnUpcomingStatusChanges += OnUpcomingStatusChanges;
        ViewTwoPlayerGame.s_OnLoopCompleteWinnerTriggered += OnTwoPlayerLoopCompleted;
        ViewSinglePlayerGame.s_SinglePlayerGamePauseStatus += OnSinglePlayerPauseStatusChanges;
        ViewTwoPlayerGame.s_TwoPlayerGamePauseStatus += OnTwoPlayerPauseStatusChanges;
        ViewTwoPlayerGame.s_OnFirstPlayerPaused += OnFirstPlayerPaused;
        ViewTwoPlayerGame.s_OnSecondPlayerPaused += OnSecondPlayerPaused;
    }
    private void OnDisable()
    {
        //skchnagesViewOptionPanel.s_SideLength -= GetSideLengthCallBack;
        ViewOptionPanel.s_StartingPieceStatusChanges -= GetStartingPieceCallBack;
        //ViewOptionPanel.s_OnBombProbabityChanges -= OnBombProbabiltyChanges;
        ViewOptionPanel.s_ObstacleStatusChanges -= GetObstacleCount;
      //  ViewOptionPanel.s_UpcomingTileStatusChanges -= OnUpcomingStatusChanges;
        ViewSinglePlayerGame.s_OnGameEnds -= OnGameEnds;
        ViewTwoPlayerGame.s_OnGameEnds -= OnGameEnds;
        ViewTwoPlayerGame.s_OnGameOverCallback -= OnTwoPlayerGameOvers;
        ViewSinglePlayerGame.s_OnGameManagerCallbacks -= OnSinglePlayerGameOvers;
        ViewTwoPlayerGameOver.s_OnGameEnds -= OnGameEnds;
        ViewSinglePlayerGameOvers.s_OnGameEnds -= OnGameEnds;
        ViewSinglePlayerGameOvers.s_OnSinglePlayerScoreSubmitted -= OnSinglePlayerScoreSubmitted;
        ViewSinglePlayerNamePanel.s_OnSinglePlayerNameChanges -= OnSinglePlayerNameChanges;
        ViewTwoPlayerGameOver.s_OnTwoPlayerGameSubmitted -= OnTwoPlayerScoreSubmitted;
        ViewTwoPlayerNamePanel.s_OnTwoPlayerNameChanges -= OnTwoPlayerNameChanges;
        ViewLevelOptionsPanel.s_OnOptionPanelCloses -= OnOptionPanelCloses;
        ViewLevelOptionsPanel.s_OnStartingPiecePoolChanges -= AllocateStartingPiecePool;
        ViewLevelOptionsPanel.s_OnObstacleCoverageChanges -= CalculateRoundObstacles_SinglePlayer;
        ViewLevelOptionsPanel.s_OnObstacleCoverageChanges -= CalculateRoundObstacles_TwoPlayers;
        ViewLevelOptionsPanel.s_OnBombProbabiltyChanges -= OnBombProbabiltyChanges;
        ViewLevelOptionsPanel.s_OnUpcomingStatusChanges -= OnUpcomingStatusChanges;
        ViewTwoPlayerGame.s_OnLoopCompleteWinnerTriggered -= OnTwoPlayerLoopCompleted;
        ViewSinglePlayerGame.s_SinglePlayerGamePauseStatus -= OnSinglePlayerPauseStatusChanges;
        ViewTwoPlayerGame.s_TwoPlayerGamePauseStatus -= OnTwoPlayerPauseStatusChanges;
        ViewTwoPlayerGame.s_OnFirstPlayerPaused -= OnFirstPlayerPaused;
        ViewTwoPlayerGame.s_OnSecondPlayerPaused -= OnSecondPlayerPaused;
    }
    #endregion



    #region Public_Methods
    public void ShowLoader()
    {
        Instantiate(m_Loader);
    }
    public void HideLoader()
    {
        Destroy(m_Loader.gameObject);
    }

    #endregion

    #region Private_Methods
    private void OnSinglePlayerPauseStatusChanges(bool pauseStatus)
    {
        m_SinglePlayerGamePauseStatus = pauseStatus;
    }
    private void OnTwoPlayerPauseStatusChanges(bool pauseStatus)
    {
        m_TwoPlayerGamePauseStatus = pauseStatus;
    }

    private void OnFirstPlayerPaused(bool status)
    {
        m_FirstPlayerPaused = status;
    }
    private void OnSecondPlayerPaused(bool status)
    {
        m_SecondPlayerPaused = status;
    }
    private void OnSinglePlayerNameChanges(string singlePlayerName)
    {
        m_SinglePlayerModePlayerName = singlePlayerName;
        LeaderBoardData singlePlayerData = new LeaderBoardData(m_SinglePlayerModePlayerName, m_SinglePlayerModePlayerScore, Constants.INT_ZERO);
        List<LeaderBoardData> leaderBoardDataContainer = new List<LeaderBoardData>();
        leaderBoardDataContainer.Add(singlePlayerData);
        LeaderboardManager.instance.SaveFile(leaderBoardDataContainer);
        ViewController.instance.ChangeView(m_HomePage);
    }
    private void OnTwoPlayerNameChanges(string firstPlayerName, string secondPlayerName)
    {
        m_TwoPlayerModePlayer1_Name = firstPlayerName;
        m_TwoPlayerModePlayer2_Name = secondPlayerName;
        int firstPlayerIndex = LeaderboardManager.instance.LoadFile().FindIndex(leaderBoardelement => (leaderBoardelement.playerScore < GameManager.instace.TwoPlayerModePlayer1_Score) && (leaderBoardelement.playerRank <= Constants.MAX_LEADERBOARDENTRIES));
        int secondPlayerIndex = LeaderboardManager.instance.LoadFile().FindIndex(leaderBoardelement => (leaderBoardelement.playerScore < GameManager.instace.TwoPlayerModePlayer2_Score) && (leaderBoardelement.playerRank <= Constants.MAX_LEADERBOARDENTRIES));
        List<LeaderBoardData> leaderBoardDataContainer = new List<LeaderBoardData>();
        if(firstPlayerIndex != Constants.NULLINDEX || (LeaderboardManager.instance.LoadFile().Count < Constants.MAX_LEADERBOARDENTRIES && m_TwoPlayerModePlayer1_Score > Constants.INT_ZERO))
        {
            LeaderBoardData firstPlayerData = new LeaderBoardData(m_TwoPlayerModePlayer1_Name, m_TwoPlayerModePlayer1_Score, Constants.INT_ZERO);
            leaderBoardDataContainer.Add(firstPlayerData);
        }
        if(secondPlayerIndex != Constants.NULLINDEX || (LeaderboardManager.instance.LoadFile().Count < Constants.MAX_LEADERBOARDENTRIES && m_TwoPlayerModePlayer2_Score > Constants.INT_ZERO))
        {
            LeaderBoardData secondPlayerData = new LeaderBoardData(m_TwoPlayerModePlayer2_Name, m_TwoPlayerModePlayer2_Score, Constants.INT_ZERO);
            leaderBoardDataContainer.Add(secondPlayerData);

        }
        LeaderboardManager.instance.SaveFile(leaderBoardDataContainer);
        ViewController.instance.ChangeView(m_HomePage);
    }
    private void OnSinglePlayerScoreSubmitted()
    {
        ViewController.instance.ChangeView(m_SinglePlayerNamePanel);
    }
    private void OnTwoPlayerScoreSubmitted()
    {
        ViewController.instance.ChangeView(m_TwoPlayerNamePanel);
    }
    private void OnUpcomingStatusChanges()
    {
        m_UpcomingTileStatus = GameData.Instance.GameOptions.UpcomingTileStatus;
    }

    private void AssignStartingPieceValues()
    {
        var startingPieceElement = GameData.Instance.StartingPieceLength.Find(startingPiece => (startingPiece.SideLength == m_GameSideLengthSinglePlayer));
        m_StartingPieceCount = (m_StartingPieceStatus == Constants.STARTINGPIECE_SINGLE) ? Constants.STARTINGPIECESINGLECOUNT : (m_StartingPieceStatus == Constants.STARTINGPIECE_FEW) ? startingPieceElement.Few : (m_StartingPieceStatus == Constants.STARTINGPIECE_NORMAL) ? startingPieceElement.Normal : (m_StartingPieceStatus == Constants.STARTINGPIECE_MANY) ? startingPieceElement.Many : m_StartingPieceCount;
    }
    private void OnGameEnds()
    {
        ViewController.instance.ChangeView(m_HomePage);
    }
    private void OnOptionPanelCloses()
    {
        ViewController.instance.ChangeView(m_HomePage);
    }

    private void OnTwoPlayerLoopCompleted(string playerName)
    {
        m_TwoPlayerModeLoopCompletedWinner = playerName;
        ViewController.instance.ChangeView(m_TwoPlayerLoopCompletedGameOver);
    }
    
    private void OnTwoPlayerGameOvers(string player1_Name, string player2_Name, int player1_Score, int player2_Score)
    {
        m_TwoPlayerModePlayer1_Name = player1_Name;
        m_TwoPlayerModePlayer2_Name = player2_Name;
        m_TwoPlayerModePlayer1_Score = player1_Score;
        m_TwoPlayerModePlayer2_Score = player2_Score;
        ViewController.instance.ChangeView(m_TwoPlayerGameOverPanel);
    }

    private void OnSinglePlayerGameOvers(string playerName, int playerScore)
    {
        m_SinglePlayerModePlayerName = playerName;
        m_SinglePlayerModePlayerScore = playerScore;
        ViewController.instance.ChangeView(m_SinglePlayerGameOverPanel);
    }
    private void OnBombProbabiltyChanges()
    {
        Action normalBombProbability = () =>
        {
            m_BombtypePiece = 0.07142f;
            m_LtypePiece_1 = 0.3571f;
            m_LtypePiece_2 = 0.3571f;
            m_CrosstypePiece = 0.07142f;
            //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
            m_InversiontypePiece = Constants.ZERO;
            m_StraighttypePiece = 0.1428f;
        };
        Action fewBombProbability = () =>
        {
            m_BombtypePiece = 0.07142f;
            m_LtypePiece_1 = 0.3571f;
            m_LtypePiece_2 = 0.3571f;
            m_CrosstypePiece = 0.07142f;
            //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
            m_InversiontypePiece = Constants.ZERO;
            m_StraighttypePiece = 0.1428f;
        };
        Action manyBombProbability = () =>
        {
            m_BombtypePiece = 0.07142f;
            m_LtypePiece_1 = 0.3571f;
            m_LtypePiece_2 = 0.3571f;
            m_CrosstypePiece = 0.07142f;
            //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
            m_InversiontypePiece = Constants.ZERO;
            m_StraighttypePiece = 0.1428f;
        };
        Action noBombProbability = () =>
        {
            m_BombtypePiece = 0.07142f;
            m_LtypePiece_1 = 0.3571f;
            m_LtypePiece_2 = 0.3571f;
            m_CrosstypePiece = 0.07142f;
            //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
            m_InversiontypePiece = Constants.ZERO;
            m_StraighttypePiece = 0.1428f;
        };
        /*changeson 14-06-22 Action normalBombProbability = () =>
         {
             m_BombtypePiece = 0.125f;
             m_LtypePiece_1 = 0.3125f;
             m_LtypePiece_2 = 0.3125f;
             m_CrosstypePiece = 0.125f;
             //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = 0.125f;
         };
         Action fewBombProbability = () =>
         {
             m_BombtypePiece = 0.125f;
             m_LtypePiece_1 = 0.3125f;
             m_LtypePiece_2 = 0.3125f;
             m_CrosstypePiece = 0.125f;
             //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = 0.125f;
         };
         Action manyBombProbability = () =>
         {
             m_BombtypePiece = 0.1875f;
             m_LtypePiece_1 = 0.3125f;
             m_LtypePiece_2 = 0.3125f;
             m_CrosstypePiece = 0.0625f;
             // m_InversiontypePiece = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = 0.125f;
         };
         Action noBombProbability = () =>
         {
             m_BombtypePiece = Constants.NOBOMB;
             m_LtypePiece_1 = 0.3125f;
             m_LtypePiece_2 = 0.3125f;
             m_CrosstypePiece = 0.1875f;
             // m_InversiontypePiece = Constants.OTHERPIECE_NOBOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = 0.1875f;
         };*/

        /* Action normalBombProbability = () =>
         {
             m_BombtypePiece = Constants.BOMB_NORMALPROBABILITY;
             m_LtypePiece_1 = Constants.LTYPE1_FEW_BOMBPROBABILITY;
             m_LtypePiece_2 = Constants.LTYPE2_FEW_BOMBPROBABILITY;
             m_CrosstypePiece = Constants.CROSSTYPE_FEW_BOMBPROBABILITY;
             //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = Constants.STRAIGHTTYPE_FEW_BOMBPROBABILITY;
         };
         Action fewBombProbability = () =>
         {
             m_BombtypePiece = Constants.BOMB_FEWPROBABILITY;
             m_LtypePiece_1 = Constants.LTYPE1_FEW_BOMBPROBABILITY;
             m_LtypePiece_2 = Constants.LTYPE2_FEW_BOMBPROBABILITY;
             m_CrosstypePiece = Constants.CROSSTYPE_FEW_BOMBPROBABILITY;
             //m_InversiontypePiece = Constants.INVERSION_FEW_BOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = Constants.STRAIGHTTYPE_FEW_BOMBPROBABILITY;
         };
         Action manyBombProbability = () =>
         {
             m_BombtypePiece = Constants.BOMB_MANYPROBABILITY;
             m_LtypePiece_1 = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
             m_LtypePiece_2 = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
             m_CrosstypePiece = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
            // m_InversiontypePiece = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = Constants.OTHERPIECE_MANYBOMBPROBABILITY;
         };
         Action noBombProbability = () =>
         {
             m_BombtypePiece = Constants.NOBOMB;
             m_LtypePiece_1 = Constants.OTHERPIECE_NOBOMBPROBABILITY;
             m_LtypePiece_2 = Constants.OTHERPIECE_NOBOMBPROBABILITY;
             m_CrosstypePiece = Constants.OTHERPIECE_NOBOMBPROBABILITY;
             // m_InversiontypePiece = Constants.OTHERPIECE_NOBOMBPROBABILITY;
             m_InversiontypePiece = Constants.ZERO;
             m_StraighttypePiece = Constants.OTHERPIECE_NOBOMBPROBABILITY;
         };*/

        if (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_NORMAL)
            normalBombProbability?.Invoke();
        else if (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_FEW)
            fewBombProbability?.Invoke();
        else if (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_MANY)
            manyBombProbability?.Invoke();
        else if (GameData.Instance.GameOptions.BombProbability == Constants.NONEBOMB)
            noBombProbability?.Invoke();
        else
            normalBombProbability?.Invoke();
        //skchnagesm_BombtypePiece = (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_NORMAL) ? Constants.BOMB_NORMALPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_FEW) ? Constants.BOMB_FEWPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.BOMB_MANY) ? Constants.BOMB_MANYPROBABILITY : (GameData.Instance.GameOptions.BombProbability == Constants.NONEBOMB) ? Constants.NOBOMB : Constants.BOMB_NORMALPROBABILITY;
    }
    private void GetSideLengthCallBack(int length)
    {
        m_GameSideLengthSinglePlayer = length;
        //AllocateStartingPiecePool();
        AssignStartingPieceValues();
      //skchanges  AssignCurrentObstaclesValues();
    }
    private void GetStartingPieceCallBack(int startingPieceStatus)
    {
        m_StartingPieceStatus = startingPieceStatus;
        AssignStartingPieceValues();
    }
    private void CalculateRoundObstacles_SinglePlayer()
    {
        int roundObstacles = Mathf.CeilToInt(GameData.Instance.GameOptions.ObstacleCoverage * (GameData.Instance.GameOptions.SinglePlayerGameRows * GameData.Instance.GameOptions.SinglePlayerGameColumns) / 100);
        m_RoundObstacleCount_SinglePlayer = roundObstacles;
    }
    private void CalculateRoundObstacles_TwoPlayers()
    {
        int roundObstacles = Mathf.CeilToInt(GameData.Instance.GameOptions.ObstacleCoverage * (GameData.Instance.GameOptions.TwoPlayerGameRows * GameData.Instance.GameOptions.TwoPlayerGameColumn) / 100);
        m_RoundObstacleCount_TwoPlayer = roundObstacles;
    }
    private void AssignCurrentObstaclesValues()
    {
        Action<ObstacleCount> obstacleAction = (ObstacleCount obstacleCount) =>
        {
            m_RoundObstacleCount_SinglePlayer = obstacleCount.RoundObstacleCount[m_ObstacleStatus];
            m_HorizontalObstacleCount = obstacleCount.HorizontalObstacleCount[m_ObstacleStatus];
            m_VerticalObstacleCount = obstacleCount.VerticalObstacleCount[m_ObstacleStatus];
        };
        ObstacleCount obstacleElement = GameData.Instance.ObstacleCountsValues.Find(obstacle => (obstacle.SideLength == m_GameSideLengthSinglePlayer));
        obstacleAction?.Invoke(obstacleElement);
    }

    private void GetObstacleCount(int optionIndex)
    {
        m_ObstacleStatus = optionIndex;
       //skchanges AssignCurrentObstaclesValues();
    }
    private void AllocateStartingPiecePool(int singlePlayerRow, int singlePlayerColumn, int multiiPlayerRow, int multiPlayerColumn)
    {
        m_GameSideLengthSinglePlayer = (singlePlayerRow < singlePlayerColumn) ? singlePlayerRow : singlePlayerColumn;
        m_GameSideLengthTwoplayers = (multiiPlayerRow < multiPlayerColumn) ? multiiPlayerRow : multiPlayerColumn;
        m_StartingPiecePoolSinglePlayer.Clear();
        m_StartingPiecePoolSinglePlayer.Add(new Vector2(1f, 1f));
        m_StartingPiecePoolSinglePlayer.Add(new Vector2(m_GameSideLengthSinglePlayer - 2, 1f));
        m_StartingPiecePoolSinglePlayer.Add(new Vector2(1f, m_GameSideLengthSinglePlayer - 2f));
        m_StartingPiecePoolSinglePlayer.Add(new Vector2(m_GameSideLengthSinglePlayer - 2f, m_GameSideLengthSinglePlayer - 2f));
        m_StartingPiecePoolTwoPlayer.Clear();
        m_StartingPiecePoolTwoPlayer.Add(new Vector2(1f, 1f));
        m_StartingPiecePoolTwoPlayer.Add(new Vector2(m_GameSideLengthTwoplayers - 2, 1f));
        m_StartingPiecePoolTwoPlayer.Add(new Vector2(1f, m_GameSideLengthTwoplayers - 2f));
        m_StartingPiecePoolTwoPlayer.Add(new Vector2(m_GameSideLengthTwoplayers - 2f, m_GameSideLengthTwoplayers - 2f));
    }

    #endregion

    #region Coroutines

    public IEnumerator ShakeSinglePlayerCamera(float duration, float magnitude)
    {
        Vector3 orignalPosition = m_SinglePlayerCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            m_SinglePlayerCamera.transform.localPosition = new Vector3(x, y, orignalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        m_SinglePlayerCamera.transform.localPosition = orignalPosition;
    }

    public IEnumerator ShakeTwoPlayerCamera(float duration, float magnitude)
    {
        Vector3 orignalPosition = m_TwoPlayerCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            m_TwoPlayerCamera.transform.localPosition = new Vector3(x, y, orignalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        m_TwoPlayerCamera.transform.localPosition = orignalPosition;
    }



    


    #endregion
}
