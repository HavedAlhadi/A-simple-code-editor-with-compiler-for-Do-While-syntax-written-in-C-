using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace codewhileloop
{
    public class ColouredText
    {
        public string Text;
        public Color Foreground;
        public Color Background;

        public ColouredText(string text, Color foreground, Color background)
        {
            Text = text;
            Foreground = foreground;
            Background = background;
        }

        public ColouredText(string text, Color foreground) : this(text, foreground, Color.Transparent) { }

        public ColouredText(string text) : this(text, Color.Transparent, Color.Transparent) { }
    }

    public static class RichTextBoxHelper
    {
        private static RichTextBox _AppendText(RichTextBox box, string text, Color foreColor, Color backColor)
        {
            if (string.IsNullOrEmpty(text)) return box;

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = foreColor;
            box.SelectionBackColor = backColor;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;

            return box;
        }

        private static void _UpdateText(RichTextBox box, IEnumerable<ColouredText> newTextWithColors)
        {
            //box.Text = "";

            foreach (var text in newTextWithColors)
            {
                var foreColor = text.Foreground; if (foreColor == Color.Transparent) foreColor = box.ForeColor;
                var backColor = text.Background; if (backColor == Color.Transparent) backColor = box.BackColor;

                _AppendText(box, text.Text, foreColor, backColor);
            }
        }

        public static void UpdateText(this RichTextBox richTextbox, IEnumerable<ColouredText> text)
        {
            if (richTextbox.InvokeRequired) richTextbox.Invoke((MethodInvoker)(() => { _UpdateText(richTextbox, text); }));
            else _UpdateText(richTextbox, text);
        }

        public static void UpdateText(this RichTextBox richTextbox, ColouredText text)
        {
            var list = new List<ColouredText>() { text };

            if (richTextbox.InvokeRequired) richTextbox.Invoke((MethodInvoker)(() => { _UpdateText(richTextbox, list); }));
            else _UpdateText(richTextbox, list);
        }
    }
}