namespace GuitarMan.SoundMenuBehaviour.FileLoadSystem
{
    public class FileLoadConstants
    {
        public const string DefaultFilter = ".wav";

        public const string SoundsFilterName = ".wav";

        public const string ShortcutName = "Desktop";

        public const string ShortcutPath = "C:\\Users\\%USERPROFILE%\\Desktop";

        public static readonly string[] SoundsFilterExtensions = {".wav", ".mp3", ".ogg"};

        public static readonly string[] ExcludedExtensions = {".lnk", ".tmp", ".zip", ".rar", ".exe"};
    }
}