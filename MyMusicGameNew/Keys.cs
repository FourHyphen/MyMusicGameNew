using System.Windows.Input;

namespace MyMusicGameNew
{
    public class Keys
    {
        /// <summary>
        /// アプリケーションで有効な、キーによる操作内容
        /// </summary>
        public enum EnableKeys
        {
            JudgeLine1,
            JudgeLine2,
            Suspend,
            Restart,    // TODO: キー入力に対応(現在Suspend画面のOKボタンでのみRestartできる)
            Else
        }

        /// <summary>
        /// キー入力内容をキーイベントから抜き出す
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static EnableKeys ToEnableKeys(System.Windows.Input.Key key, System.Windows.Input.KeyboardDevice keyboard)
        {
            EnableKeys keyConbination = ToEnableKeysConbination(key, keyboard);
            if (keyConbination != EnableKeys.Else)
            {
                return keyConbination;
            }

            return ToEnableKeysOneKey(key);
        }

        private static EnableKeys ToEnableKeysConbination(System.Windows.Input.Key key, System.Windows.Input.KeyboardDevice keyboard)
        {
            // Ctrl + Shift + 何か
            if (keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                // nothing
            }

            // Ctrl + 何か
            if (keyboard.Modifiers == ModifierKeys.Control)
            {
                // nothing
            }

            // Shift + 何か
            if (keyboard.Modifiers == ModifierKeys.Shift)
            {
                // nothing
            }

            return EnableKeys.Else;
        }

        private static EnableKeys ToEnableKeysOneKey(System.Windows.Input.Key key)
        {
            if (key == Key.F)
            {
                return EnableKeys.JudgeLine1;
            }
            else if (key == Key.J)
            {
                return EnableKeys.JudgeLine2;
            }
            else if (key == Key.Enter)
            {
                return EnableKeys.Suspend;
            }

            return EnableKeys.Else;
        }
    }
}
