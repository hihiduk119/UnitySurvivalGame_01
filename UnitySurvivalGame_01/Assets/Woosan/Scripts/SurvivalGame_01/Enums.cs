using UnityEngine;

public enum MemoryState {
    DefaultSmooth = 0,
    Clamp,
}

public enum ButtonType {
    ControlLeft = 0,
    ControlRight,
}

public enum MapType {
    Forest = 0,
    WinterForest,
}

public enum LiftType {
    ski_lift = 0,

}

public enum XML_LOAD_DATA {
    MapData = 0,
}

//생성되는 젬의 위치
public enum GemPosType {
    Left_90 = 0,
    Left_45,
    Center,
    Right_45,
    Right_90,
}

//커브드 월드 변경 타입
namespace WoosanStudio.SpeedGame {
    public enum MoveTypeOnCurvedWorld
    {
        Center = 0,
        LeftShort,
        LeftLong,
        RightShort,
        RightLong,
        TopShort,
        TopLong,
        BottomShort,
        BottomLong,
    }    
}


//map type 와 순서는 같아야 한.
namespace WoosanStudio.SpeedGame {
    public class MapColor {
        static public Color[] land = new Color[]    { new Color32(165, 221, 138, 255), new Color32(255, 255, 255, 255)};
        static public Color[] fog = new Color[]     { new Color32(165, 221, 138, 255), new Color32(255, 255, 255, 255)};
        static public Color[] camera = new Color[]  { new Color32(165, 221, 138, 255), new Color32(255, 255, 255, 255)};
        static public Color[] shadow = new Color[]  { new Color32(152, 194, 119, 255), new Color32(150, 150, 150, 255)};

        //static public Color[] land = new Color[] { new Color32(165, 221, 138, 255), new Color32(255, 255, 255, 255) };
        //static public Color[] fog = new Color[] { new Color32(255, 0, 0, 255), new Color32(255, 255, 255, 255) };
        //static public Color[] camera = new Color[] { new Color32(255, 0, 0, 255), new Color32(255, 255, 255, 255) };
        //static public Color[] shadow = new Color[] { new Color32(152, 194, 119, 255), new Color32(150, 150, 150, 255) };
    }   
}


namespace WoosanStudio.RhythmGame {
    //TargetController , CamController 에서 사용.
    public enum PositionState
    {
        Left,
        Middle,
        Right,
    }

    public enum OnTriggerState
    {
        Enter,
        Exit,
    }
}
