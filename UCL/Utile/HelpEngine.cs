using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
internal class HelpEngine
{
    public static void MSB_Error(string text) => System.Windows.Forms.MessageBox.Show(text, "FC-UI", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    public static Font GetDefaultFont(
        string familyName = "Arial",
        float emSize = 11.0F,
        FontStyle fontStyle = FontStyle.Regular) => new Font(familyName, emSize, fontStyle);

    public static Graphics GetGraphics(ref Bitmap bitmap, SmoothingMode SmoothingMode, TextRenderingHint TextRenderingHint)
    {
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.SmoothingMode = SmoothingMode;
        graphics.TextRenderingHint = TextRenderingHint;

        return graphics;
    }


    public class GetRandom
    {
        private readonly System.Random random = new System.Random(System.Environment.TickCount);

        public Color ColorArgb(int alpha = 255) => Color.FromArgb(alpha, Int(0, 255), Int(0, 255), Int(0, 255));

        public int Int(int min, int max) => random.Next(min, max);

        public float Float(int min, int max) => random.Next(min * 100, max * 100) / 100;

        public bool Bool() => Int(0, 2) == 1;
    }
}