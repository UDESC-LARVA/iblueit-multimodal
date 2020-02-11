using System;
using System.Windows.Forms;

namespace Ibit.Core.Util
{
    public class SysMessage
    {
        public static DialogResult Show(string msg, string title = "") => MessageBox.Show(msg, title);

        public static DialogResult Warning(string msg) =>
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static DialogResult Error(Exception e) => MessageBox.Show($"{e.Message}\n\nStacktrace: {e.StackTrace}",
            e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static DialogResult Error(string msg, string title) =>
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static DialogResult Error(string msg) =>
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static DialogResult Info(string msg) =>
            MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static DialogResult YesNoQuestion(string question) =>
            MessageBox.Show(question, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        public static DialogResult OkCancelQuestion(string question) =>
            MessageBox.Show(question, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
    }
}