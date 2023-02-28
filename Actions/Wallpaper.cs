using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Kairos.Actions
{
    public enum WallpaperStyle
    {
        Fill,
        Fit,
        Stretch,
        Tile,
        Center,
        Span,
    }
    public class Wallpaper : Action //TODO: look into non-static wallpapers
    {
        public override string Type { get; set; } = "Change the Wallpaper";
        public string Path { get; set; } = "";

        private const string DESKTOP_REG_PATH = @"Control Panel\Desktop";
        private const string HISTORY_REG_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers";
        private const string WALLPAPER_STYLE_REG_PATH = "WallpaperStyle";
        private const string TILE_WALLPAPER_REG_PATH = "TileWallpaper";

        private const int HISTORY_MAX_ENTRIES = 5;

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static State? _backupState;
        public static bool _historyRestored;

        public struct Config
        {
            public int Style;
            public bool IsTile;
        }
        public struct State
        {
            public Config Config;
            public string[] History;
            public string Wallpaper;
        }
        private static int GetRegistryValue(RegistryKey key, string name, int defaultValue)
        {
            return int.Parse((string)key.GetValue(name) ?? defaultValue.ToString());
        }
        private static bool GetRegistryValue(RegistryKey key, string name, bool defaultValue)
        {
            return ((string)key.GetValue(name) ?? (defaultValue ? "1" : "0")) == "1";
        }
        private static void SetRegistryValue(RegistryKey key, string name, int value)
        {
            key.SetValue(name, value.ToString());
        }
        private static void SetRegistryValue(RegistryKey key, string name, bool value)
        {
            key.SetValue(name, value ? "1" : "0");
        }
        private static Config GetWallpaperConfig()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(DESKTOP_REG_PATH, true);

            return new Config
            {
                Style = GetRegistryValue(key, WALLPAPER_STYLE_REG_PATH, 0),
                IsTile = GetRegistryValue(key, TILE_WALLPAPER_REG_PATH, false),
            };
        }
        private static void SetWallpaperConfig(Config value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(DESKTOP_REG_PATH, true);
            SetRegistryValue(key, WALLPAPER_STYLE_REG_PATH, value.Style);
            SetRegistryValue(key, TILE_WALLPAPER_REG_PATH, value.IsTile);
        }
        private static void SetStyle(WallpaperStyle style)
        {
            switch (style)
            {
                case WallpaperStyle.Fill:
                    SetWallpaperConfig(new Config { Style = 10, IsTile = false });
                    break;
                case WallpaperStyle.Fit:
                    SetWallpaperConfig(new Config { Style = 6, IsTile = false });
                    break;
                case WallpaperStyle.Stretch:
                    SetWallpaperConfig(new Config { Style = 2, IsTile = false });
                    break;
                case WallpaperStyle.Tile:
                    SetWallpaperConfig(new Config { Style = 0, IsTile = true });
                    break;
                case WallpaperStyle.Center:
                    SetWallpaperConfig(new Config { Style = 0, IsTile = false });
                    break;
                case WallpaperStyle.Span: // Windows 8 or newer only
                    SetWallpaperConfig(new Config { Style = 22, IsTile = false });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style));
            }
        }
        public static void ChangeWallpaper(string filename = null)
        {
            if (filename == null)
            {
                MessageBox.Show("Make sure to select an image file");
            }
            else
            {
                if(SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE) == 0)
                {
                    MessageBox.Show("Image file missing");
                }
            }
        }

        private static void RestoreHistory()
        {
            if (_historyRestored) return;

            if (!_backupState.HasValue)
            {
                MessageBox.Show("Error: You must call BackupState() before.");
                throw new Exception("You must call BackupState() before.");
            }

            var backupState = _backupState.Value;

            using (var key = Registry.CurrentUser.OpenSubKey(HISTORY_REG_PATH, true))
            {
                for (var i = 0; i < HISTORY_MAX_ENTRIES; i++)
                    if (backupState.History[i] != null)
                        key.SetValue($"BackgroundHistoryPath{i}", backupState.History[i], RegistryValueKind.String);
            }

            _historyRestored = true;
        }

        /// <summary>
        /// Backups the current wallpaper state (style and history).
        /// </summary>
        private static void BackupState()
        {
            var history = new string[HISTORY_MAX_ENTRIES];

            using (var key = Registry.CurrentUser.OpenSubKey(HISTORY_REG_PATH, true))
            {
                for (var i = 0; i < history.Length; i++)
                    history[i] = (string)key.GetValue($"BackgroundHistoryPath{i}");
            }

            _backupState = new State
            {
                Config = GetWallpaperConfig(),
                History = history,
                Wallpaper = history[0],
            };

            _historyRestored = false;
        }

        /// <summary>
        /// Restores the state (style, wallpaper and history) before any Set() method.
        /// </summary>
        private static void RestoreState()
        {
            if (!_backupState.HasValue)
            {
                MessageBox.Show("You must call BackupState() before.");
                throw new Exception("You must call BackupState() before.");
            }

            SetWallpaperConfig(_backupState.Value.Config);
            ChangeWallpaper(_backupState.Value.Wallpaper);
            RestoreHistory();

            _backupState = null;
        }

        /// <summary>
        /// Sets the wallpaper without changing its style.
        /// </summary>
        private static void Set(string filename)
        {
            BackupState();
            ChangeWallpaper(filename);
        }

        /// <summary>
        /// Sets the wallpaper with the given style.
        /// </summary>
        private static void Set(string filename, WallpaperStyle style)
        {
            BackupState();
            SetStyle(style);
            ChangeWallpaper(filename);
        }

        /// <summary>
        /// Sets the wallpaper without changing its style nor the history in Windows settings.
        /// </summary>
        private static void SilentSet(string filename)
        {
            Set(filename);
            RestoreHistory();
        }

        /// <summary>
        /// Sets the wallpaper with the given style without changing the history in Windows settings.
        /// </summary>
        private static void SilentSet(string filename, WallpaperStyle style)
        {
            Set(filename, style);
            RestoreHistory();
        }
        public override void Do() //Hide taskbars
        {
            if (Path != "")
            {
                Set(Path);
            }
            else
            {
                MessageBox.Show("Path is null; Make sure to select an image file");
            }
        }
        public override void Undo() //Show taskbars
        {
            if (Path != "")
            {
                RestoreState();
            }
            else
            {
                MessageBox.Show("Path is null; Make sure to select an image file");
            }
        }
    }
}
