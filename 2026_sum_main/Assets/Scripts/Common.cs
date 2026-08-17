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
        Main,   //全身.
        Head,   //頭.
        ArmL,   //左腕.
        ArmR,   //右腕.
        LegL,   //左脚.
        LegR    //右脚.
    }

    /// <summary>
    /// 使用する譜面の種類.
    /// </summary>
    public enum NoteChartType
    {
        Normal, //通常譜面.
        Extra   //裏譜面.
    }
}