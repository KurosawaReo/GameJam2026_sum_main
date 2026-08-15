/*
   汎用的に使う機能.
*/
namespace Common
{
    /// <summary>
    /// ノーツをタップした結果.
    /// </summary>
    public enum Result
    {
        Perfect,
        Good,
        Bad,    //ミスも含む.
    }

    /// <summary>
    /// 体の部位.
    /// </summary>
    public enum BodyParts
    {
        Head,   //頭.
        ArmL,   //左腕.
        ArmR,   //右腕.
        LegL,   //左脚.
        LegR    //右脚.
    }
}