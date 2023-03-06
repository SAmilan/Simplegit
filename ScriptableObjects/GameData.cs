using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StartingPiecelength
{
    public int SideLength = 4;
    public int Normal = 1;
    public int Few = 1;
    public int Many = 1;
}
[System.Serializable]
public class StartingPieceIndexs
{
    public int StartingPieceIndex;
    public List<int> StartingPieceCounters = new List<int>();
}

[System.Serializable]
public class Obstacles
{
    public int ObstacleType;
    public List<Sprite> ObstaclesPieces = new List<Sprite>();
    public List<Vector2> ObstaclePiecePosition = new List<Vector2>();
}
[System.Serializable]
public class ObstacleCount
{
    public int SideLength;
    //here in this list of counts 0 => normal, 1=> few, 2 => many, 3 => none counts for respective obstacles
    public List<int> RoundObstacleCount = new List<int>();
    public List<int> HorizontalObstacleCount = new List<int>();
    public List<int> VerticalObstacleCount = new List<int>();
}
[System.Serializable]
public class StartingPieceFlowVector
{
    public int Angle;
    public Vector2 flowVector;
}
[System.Serializable]
public class ThrowObstacles
{
    public int MinLoopLength;
    public int MaxLoopLength;
    public int ObstaceleCount;
}
[System.Serializable]
public class StartingPieceAnglesVariations
{
    public int Angle;
    public Sprite StartingPieceSprite;
}
[System.Serializable]
public class FlowFilledImages
{
    public int Type;
    public Sprite fillImages;
}
[System.Serializable]
public class FlowAnimationFlowVector
{
    
    public Vector2 FlowVector;
    public Image.FillMethod fillMethod;
    public int fillOrigin;
    public bool Clockwise = true;

}
[System.Serializable]
public class FlowAnimationAngle
{
    public int Angle;
    public List<FlowAnimationFlowVector> FlowAnimationFlowVector = new List<FlowAnimationFlowVector>();
}

[System.Serializable]
public class FlowTypeAnimation
{
    public int Type;
    public List<FlowAnimationAngle> FlowAnimationAngle = new List<FlowAnimationAngle>();
}
[System.Serializable]
public class CrossTypeAnimation
{
    public Vector2 FlowVector;
    public int Angle;
    public Image.FillMethod fillMethod;
    public int fillOrigin;
}

[System.Serializable]
public class CrossTypeFlowFillImage
{
    public int CrossTypeAngle;
    public List<CrossTypeAnimation> FlowAnimationAngle = new List<CrossTypeAnimation>();
}

[System.Serializable]
public class BrokenPieceSurroundingVectors
{
    public List<Vector2> SurroundingVectors = new List<Vector2>();
}

[System.Serializable]
public class PiecesPercentages
{
    public int LTYPEPIECE_1;
    public int LTYPEPIECE_2;
    public int STRAIGHTTYPEPIECE;
    public int CROSSTYPEPIECE;
    public int BOMBTYPEPIECE;
    public string WARNING = "THE PIECE POOL IS OF 14 PIECES,THUS THE TOTAL OF PIECES YOU ENTER SHOULD EQUAL TO 14 FOR EG LTYPE1: 4, LTYP2:4, STRAIGHTTYPE: 2, CROSSTYPE: 2, BOMBTYPE: 2";
}



[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : SingletonScriptableObject<GameData>
{
    public List<StartingPiecelength> StartingPieceLength = new List<StartingPiecelength>();
    public List<StartingPieceIndexs> StartingPiecesIndex = new List<StartingPieceIndexs>();
    public List<Obstacles> Obstacle = new List<Obstacles>();
    public List<Vector2> StartingPieceSurroundingVectors = new List<Vector2>();
    public List<ObstacleCount> ObstacleCountsValues = new List<ObstacleCount>();
    public List<StartingPieceFlowVector> StartingPieceFlowVectors = new List<StartingPieceFlowVector>();
    public int TestValue;
    public Vector2 LEFTSTICKUPVECTOR;
    public Vector2 LEFTSTICKDOWNVECTOR;
    public Vector2 LEFTSTICKLEFTVECTOR;
    public Vector2 LEFTSTICKRIGHTVECTOR;
    public OptionTemplate GameOptions;
    public List<ThrowObstacles> ThrownObstacles = new List<ThrowObstacles>();
    public List<StartingPieceAnglesVariations> StartingPieceAnglesVariations = new List<StartingPieceAnglesVariations>();
    public List<Vector2> StartingPieceSafeAreaVectors = new List<Vector2>();
    public List<FlowFilledImages> FillImages = new List<FlowFilledImages>();
    public List<FlowTypeAnimation> FlowTypeAnimation = new List<FlowTypeAnimation>();
    public List<CrossTypeFlowFillImage> CrossTypeAnimation = new List<CrossTypeFlowFillImage>();
    public List<StartingPieceAnglesVariations> StartingPieceFlowFillImages = new List<StartingPieceAnglesVariations>();
    public List<Vector2> StartingPieceVerticalSafeAreaVectors = new List<Vector2>();
    public List<Vector2> StartingPieceHorizontalSafeAreaVectors = new List<Vector2>();
    public BrokenPieceSurroundingVectors BrokenPieceVectors;
    public PiecesPercentages PiecePercaentages;

}
