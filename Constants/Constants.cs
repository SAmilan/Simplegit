using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    public const int NULLINDEX = -1;
    public const float ZERO = 0f;
    public const float THREE = 3f;
    public const float POSSIBLEANGLE_COUNT = 4f;
    public const int INT_TWO = 2;
    public const int INT_ZERO = 0;
    public const int INT_ONE = 1;

    public const float HOMEPIPEDELTA = 0.1f;
    //public const int SINGLEPLAYERGRIDSIZE = 924;
    public const int SINGLEPLAYERGRIDSIZE = 792;

    //public const int TWOPLAYERGRIDSIZE = 775;
    public const int TWOPLAYERGRIDSIZE = 740;

    public const int MIN_SIDELENGTH = 4;
    public const int TOTAL_SIDEPANELPIECES = 14; //skchnages
    public const float Total_Pieces = 5f;
    public const float PIECE_ANGLE0 = 0f;
    public const float PIECE_ANGLE90 = 90f;
    public const float PIECE_ANGLE180 = 180f;
    public const float PIECE_ANGLE270 = 270f;
    public const int L_TYPE1PIECE = 0;
    public const int L_TYPE2PIECE = 1;
    public const int STRAIGHT_TYPEPIECE = 2;
    public const int CROSS_TYPEPIECE = 3;
    public const int BOMB_TYPEPIECE = 4;
    public const int STARTING_TYPEPIECE = 4;
    public const float INPUT_DELAY = 0.15f;
    public const float TOTAL_ANGLE = 360f;
    public const float ANGLE_DIFF = 90F;
    public const int TILECODEINDEX_1 = 0;
    public const int TILECODEINDEX_2 = 1;
    public const int TILECODEINDEX_3 = 2;
    public const int TILECODEINDEX_4 = 3;
    public const int OPTIONPANEL_SCROLLBUTTON = 160;
    //option scrolling indexs
    public const int OPTIONINDEX_0 = 0;
    public const int OPTIONINDEX_1 = 1;
    public const int OPTIONINDEX_2 = 2;
    public const int OPTIONINDEX_3 = 3;
    public const int OPTIONINDEX_4 = 4;
    public const int OPTIONINDEX_5 = 5;
    public const int OPTIONINDEX_6 = 6;
    public const int OPTIONINDEX_7 = 7;
    public const int OPTIONINDEX_8 = 8;
    //sidelength values
    public const int SIDELENGTH_4 = 4;
    public const int SIDELENGTH_5 = 5;
    public const int SIDELENGTH_6 = 6;
    public const int SIDELENGTH_7 = 7;
    public const int SIDELENGTH_8 = 8;
    public const int SIDELENGTH_9 = 9;
    public const int SIDELENGTH_10 = 10;
    public const int SIDELENGTH_11 = 11;
    public const int SIDELENGTH_12 = 12;

    ///BOMB PROBABITIES
    public const float BOMB_FEWPROBABILITY = 0.25f;
    public const float BOMB_NORMALPROBABILITY = 0.25f;
    public const float BOMB_MANYPROBABILITY = 0.16666f;
    public const float NOBOMB = 0f;

    public const float LTYPE1_FEW_BOMBPROBABILITY = 0.302f;
    public const float LTYPE2_FEW_BOMBPROBABILITY = 0.37f;
    public const float STRAIGHTTYPE_FEW_BOMBPROBABILITY = 0.304f;
    public const float CROSSTYPE_FEW_BOMBPROBABILITY = 0.372f;
    public const float INVERSION_FEW_BOMBPROBABILITY = 0f;

    public const float OTHERPIECE_MANYBOMBPROBABILITY = 0.16666f;
    public const float OTHERPIECE_NOBOMBPROBABILITY = 0.416666f;

    public const string BOMB_NORMAL = "normal";
    public const string BOMB_FEW = "few";
    public const string BOMB_MANY = "many";
    public const string NONEBOMB = "none";

    //StartingPiece Indexes
    public const int STARTINGPIECESINGLECOUNT = 1;
    public const int STARTINGPIECE_SINGLE = 0;
    public const int STARTINGPIECE_FEW = 1;
    public const int STARTINGPIECE_NORMAL = 2;
    public const int STARTINGPIECE_MANY = 3;

    //skchnagespublic const float STARTTINGPIECEBALANCERATIO = 1.33f;
    public const float STARTTINGPIECEBALANCERATIO = 3f;
    public const int STARTINGPIECE_COUNTERUPPERVALUE = 60;
    public const int STARTINGPIECE_COUNTERLOWERTVALUE = 30;
    public const int COUNTERWARNING = 10;

    //Obstacles
    public const int ROUND_OBSTACLE = 0;
    public const int HORIZONTAL_OBSTACLE = 1; 
    public const int VERTICAL_OBSTACLE = 2;

    //OBSTACLES SPRITES INDEXEXS
    public const int ROUND_PIECE1 = 0;
    public const int HORIZONTAL_PIECE1 = 0;
    public const int HORIZONTAL_PIECE2 = 1;
    public const int VERTICAL_PIECE1 = 0;
    public const int VERTICAL_PIECE2 = 1;
    public const int VERTICAL_PIECE3 = 2;
    //Obstacle Option SelectionIndex
    public const int OBSTACLE_NORMAL = 0;
    public const int OBSTACLE_FEW = 1;
    public const int OBSTACLE_MANY = 2;
    public const int OBSTACLE_NONE = 3;

    public const string MAINGAMEAREAPARENTSINGLEPLAYER = "Panel - MainGrid";
    public const string STARTINGPIECEAREAPARENTSINGLEPLAYER = "Panel - StartingPieceGrid";
    public const string MAINGAMEAREAPARENTTWOPLAYERS = "Panel - Gameplay";
    public const string STARTINGPIECEAREAPARENTTWOPLAYER = "Panel - StartingPieceArea";
    public const float SCOREDELAY = 0.1f;
    public const float SCORECALCULATEDELAY = 0.2f;
    public const int SCORE_100 = 100;
    public const int SORE_500 = 500;
    public const int SCORE_1000 = 1000;
    public const float INCREASESCOREDELAY = 0.01f;
    public const int DECREASEDSCORE = -100;
    public const float DETACHDPIECEDELAY = 2f;
    public const float DECREASESCOREDELAY = 5f;
    public const float GAMEOVERSCOREDELAY = 5f;
    public const float SCORE_SPEED = 5f;
    public const float DECREMENT_SCORESPEED = 24f;
    public const int NAME_SHORTERLENGTH = 2;
    public const int NAME_LONGERLENGTH = 10;

    public const string SAMENAME_WARNING = "Player1 and Player 2 should not have the same name.";
    public const string VALIDATION_WARNING = "Name should be in Range of 2 - 10.";
    public const float LEADERBOARD_DELAY = 1.5f;
    public const string SERVER_URL = "http://localhost:80/StripesServer.php";
    public const string FILE_URL = "http://localhost:80/";
    public const string HOLD_ANIMATION = "Hold";

    public const float MIN_VECTORVALUE = -0.72f; 
    public const float MAX_VECTORVALUE = 0.72f;
    public const int MAX_LEADERBOARDENTRIES = 10;
    public const float BG_XDELTA = 58f;
    public const float BG_YDELTA = 72f;
    public const float BG_TWOPLAYER_XDELTA = 40f;
    public const float BG_TWOPLATER_YDELTA = 36f;
    public const float STARTINGPIECEPAIR = 2f;
    public const int STARTINGPIECETIMEDIFFERENCE = 5;
    public const float INITIAL_TIMERVALUE = 75f;
    public const float OPTIONPANEL_CLOSEDELAY = 0.5f;
    public const string POSITIVESCORE_ANIMATION = "Positive";
    public const string NEGATIVESCORE_ANIMATION = "Negative";
    public const string GUEST_NAME = "Guest";
    public const float FLASH_INTERVAL = 6f;
    public const float MAX_FLOWSPEED = 4f;
    public const string MAINGAMESCENE = "StripesMainScene";
    public const float GAMELOAD_DELAY = 11f;
    public const float CONNECTION_DELAY = 1.5f;
    public const int AUXILLARYTIMER_COUNTDOWN = 10;
    public const float AUXILLARYTIMER_DELAY = 0.4f;
}
