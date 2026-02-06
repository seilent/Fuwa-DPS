using BPSR_ZDPS.DataTypes;
using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_ZDPS
{
    public static class Theme
    {
        private static void Apply(Dictionary<ImGuiCol, Vector4> NewThemeColors)
        {
            var colors = ImGui.GetStyle().Colors;
            foreach (var color in NewThemeColors)
            {
                colors[(int)color.Key] = color.Value;
            }
        }

        private static void ApplyThemeStyles(ETheme theme)
        {
            var style = ImGui.GetStyle();

            style.PopupRounding = 3;
            style.WindowPadding = new Vector2(4, 4);
            style.FramePadding = new Vector2(6, 4);
            style.ItemSpacing = new Vector2(6, 2);
            style.ScrollbarSize = 18;
            style.WindowBorderSize = 1;
            style.ChildBorderSize = 1;
            style.PopupBorderSize = 1;
            style.FrameBorderSize = 0;
            style.WindowRounding = 3;
            style.ChildRounding = 3;
            style.FrameRounding = 3;
            style.ScrollbarRounding = 2;
            style.GrabRounding = 3;
            style.TabRounding = 3;

            // This makes it easier to grab the window borders for resizing
            style.WindowBorderHoverPadding = 8;
        }

        private static Dictionary<ImGuiCol, Vector4> GetDarkThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            Vector4 bgColor = new Vector4(37 / 255f, 37 / 255f, 38 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(82 / 255f, 82 / 255f, 85 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(82 / 255f, 82 / 255f, 85 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(90 / 255f, 90 / 255f, 95 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(51 / 255f, 51 / 255f, 51 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(29 / 255f, 151 / 255f, 236 / 255f, 1.0f);
            Vector4 panelActiveColor = new Vector4(0 / 255f, 119 / 255f, 200 / 255f, 1.0f);

            Vector4 textColor = new Vector4(255 / 255f, 255 / 255f, 255 / 255f, 1.0f);
            Vector4 textDisabledColor = new Vector4(151 / 255f, 151 / 255f, 151 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(78 / 255f, 78 / 255f, 78 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(153 / 255f, 153 / 255f, 153 / 255f, 1.0f);

            colors[ImGuiCol.Text] = textColor;
            colors[ImGuiCol.TextDisabled] = textDisabledColor;
            colors[ImGuiCol.TextSelectedBg] = panelActiveColor;
            colors[ImGuiCol.WindowBg] = bgColor;
            colors[ImGuiCol.ChildBg] = bgColor;
            colors[ImGuiCol.PopupBg] = bgColor;
            colors[ImGuiCol.Border] = borderColor;
            colors[ImGuiCol.BorderShadow] = borderColor;
            colors[ImGuiCol.FrameBg] = panelColor;
            colors[ImGuiCol.FrameBgHovered] = panelHoverColor;
            colors[ImGuiCol.FrameBgActive] = panelActiveColor;
            colors[ImGuiCol.TitleBg] = lightBgColor;
            colors[ImGuiCol.TitleBgActive] = bgColor;
            colors[ImGuiCol.TitleBgCollapsed] = bgColor;
            colors[ImGuiCol.MenuBarBg] = panelColor;
            colors[ImGuiCol.ScrollbarBg] = panelColor;
            colors[ImGuiCol.ScrollbarGrab] = lightBgColor;
            colors[ImGuiCol.ScrollbarGrabHovered] = veryLightBgColor;
            colors[ImGuiCol.ScrollbarGrabActive] = veryLightBgColor;
            colors[ImGuiCol.CheckMark] = panelActiveColor;
            colors[ImGuiCol.SliderGrab] = panelHoverColor;
            colors[ImGuiCol.SliderGrabActive] = panelActiveColor;
            colors[ImGuiCol.Button] = panelColor;
            colors[ImGuiCol.ButtonHovered] = panelHoverColor;
            colors[ImGuiCol.ButtonActive] = panelHoverColor;
            colors[ImGuiCol.Header] = panelColor;
            colors[ImGuiCol.HeaderHovered] = panelHoverColor;
            colors[ImGuiCol.HeaderActive] = panelActiveColor;
            colors[ImGuiCol.Separator] = separatorColor;
            colors[ImGuiCol.SeparatorHovered] = separatorColor;
            colors[ImGuiCol.SeparatorActive] = separatorColor;
            colors[ImGuiCol.ResizeGrip] = bgColor;
            colors[ImGuiCol.ResizeGripHovered] = panelColor;
            colors[ImGuiCol.ResizeGripActive] = lightBgColor;
            colors[ImGuiCol.PlotLines] = panelActiveColor;
            colors[ImGuiCol.PlotLinesHovered] = panelHoverColor;
            colors[ImGuiCol.PlotHistogram] = panelActiveColor;
            colors[ImGuiCol.PlotHistogramHovered] = panelHoverColor;

            colors[ImGuiCol.ModalWindowDimBg] = lightBgColor_fade;
            colors[ImGuiCol.DragDropTarget] = bgColor;
            colors[ImGuiCol.NavCursor] = bgColor;
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            //colors[ImGuiCol.DockingEmptyBg] = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;
            //colors[ImGuiCol.DockingPreview] = panelActiveColor;

            return colors;
        }

        private static Dictionary<ImGuiCol, Vector4> GetBlackThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Pure black background
            Vector4 bgColor = new Vector4(0 / 255f, 0 / 255f, 0 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(25 / 255f, 25 / 255f, 25 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(25 / 255f, 25 / 255f, 25 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(35 / 255f, 35 / 255f, 35 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(20 / 255f, 20 / 255f, 20 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(40 / 255f, 120 / 255f, 200 / 255f, 1.0f);
            Vector4 panelActiveColor = new Vector4(20 / 255f, 100 / 255f, 180 / 255f, 1.0f);

            Vector4 textColor = new Vector4(240 / 255f, 240 / 255f, 240 / 255f, 1.0f);
            Vector4 textDisabledColor = new Vector4(120 / 255f, 120 / 255f, 120 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(50 / 255f, 50 / 255f, 50 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(80 / 255f, 80 / 255f, 80 / 255f, 1.0f);

            colors[ImGuiCol.Text] = textColor;
            colors[ImGuiCol.TextDisabled] = textDisabledColor;
            colors[ImGuiCol.TextSelectedBg] = panelActiveColor;
            colors[ImGuiCol.WindowBg] = bgColor;
            colors[ImGuiCol.ChildBg] = bgColor;
            colors[ImGuiCol.PopupBg] = bgColor;
            colors[ImGuiCol.Border] = borderColor;
            colors[ImGuiCol.BorderShadow] = borderColor;
            colors[ImGuiCol.FrameBg] = panelColor;
            colors[ImGuiCol.FrameBgHovered] = panelHoverColor;
            colors[ImGuiCol.FrameBgActive] = panelActiveColor;
            colors[ImGuiCol.TitleBg] = lightBgColor;
            colors[ImGuiCol.TitleBgActive] = bgColor;
            colors[ImGuiCol.TitleBgCollapsed] = bgColor;
            colors[ImGuiCol.MenuBarBg] = panelColor;
            colors[ImGuiCol.ScrollbarBg] = panelColor;
            colors[ImGuiCol.ScrollbarGrab] = lightBgColor;
            colors[ImGuiCol.ScrollbarGrabHovered] = veryLightBgColor;
            colors[ImGuiCol.ScrollbarGrabActive] = veryLightBgColor;
            colors[ImGuiCol.CheckMark] = panelActiveColor;
            colors[ImGuiCol.SliderGrab] = panelHoverColor;
            colors[ImGuiCol.SliderGrabActive] = panelActiveColor;
            colors[ImGuiCol.Button] = panelColor;
            colors[ImGuiCol.ButtonHovered] = panelHoverColor;
            colors[ImGuiCol.ButtonActive] = panelHoverColor;
            colors[ImGuiCol.Header] = panelColor;
            colors[ImGuiCol.HeaderHovered] = panelHoverColor;
            colors[ImGuiCol.HeaderActive] = panelActiveColor;
            colors[ImGuiCol.Separator] = separatorColor;
            colors[ImGuiCol.SeparatorHovered] = separatorColor;
            colors[ImGuiCol.SeparatorActive] = separatorColor;
            colors[ImGuiCol.ResizeGrip] = bgColor;
            colors[ImGuiCol.ResizeGripHovered] = panelColor;
            colors[ImGuiCol.ResizeGripActive] = lightBgColor;
            colors[ImGuiCol.PlotLines] = panelActiveColor;
            colors[ImGuiCol.PlotLinesHovered] = panelHoverColor;
            colors[ImGuiCol.PlotHistogram] = panelActiveColor;
            colors[ImGuiCol.PlotHistogramHovered] = panelHoverColor;

            colors[ImGuiCol.ModalWindowDimBg] = lightBgColor_fade;
            colors[ImGuiCol.DragDropTarget] = bgColor;
            colors[ImGuiCol.NavCursor] = bgColor;
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;

            return colors;
        }

        private static Dictionary<ImGuiCol, Vector4> GetLightThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Light background - fixed contrast: darker bg, lighter panels
            Vector4 bgColor = new Vector4(235 / 255f, 235 / 255f, 235 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(220 / 255f, 220 / 255f, 220 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(220 / 255f, 220 / 255f, 220 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(210 / 255f, 210 / 255f, 210 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(245 / 255f, 245 / 255f, 245 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(0 / 255f, 120 / 255f, 215 / 255f, 1.0f);
            Vector4 panelActiveColor = new Vector4(0 / 255f, 80 / 255f, 180 / 255f, 1.0f);

            Vector4 textColor = new Vector4(20 / 255f, 20 / 255f, 20 / 255f, 1.0f);
            Vector4 textDisabledColor = new Vector4(140 / 255f, 140 / 255f, 140 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(180 / 255f, 180 / 255f, 180 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(160 / 255f, 160 / 255f, 160 / 255f, 1.0f);

            colors[ImGuiCol.Text] = textColor;
            colors[ImGuiCol.TextDisabled] = textDisabledColor;
            colors[ImGuiCol.TextSelectedBg] = panelActiveColor;
            colors[ImGuiCol.WindowBg] = bgColor;
            colors[ImGuiCol.ChildBg] = bgColor;
            colors[ImGuiCol.PopupBg] = bgColor;
            colors[ImGuiCol.Border] = borderColor;
            colors[ImGuiCol.BorderShadow] = borderColor;
            colors[ImGuiCol.FrameBg] = panelColor;
            colors[ImGuiCol.FrameBgHovered] = panelHoverColor;
            colors[ImGuiCol.FrameBgActive] = panelActiveColor;
            colors[ImGuiCol.TitleBg] = lightBgColor;
            colors[ImGuiCol.TitleBgActive] = bgColor;
            colors[ImGuiCol.TitleBgCollapsed] = bgColor;
            colors[ImGuiCol.MenuBarBg] = panelColor;
            colors[ImGuiCol.ScrollbarBg] = panelColor;
            colors[ImGuiCol.ScrollbarGrab] = lightBgColor;
            colors[ImGuiCol.ScrollbarGrabHovered] = veryLightBgColor;
            colors[ImGuiCol.ScrollbarGrabActive] = veryLightBgColor;
            colors[ImGuiCol.CheckMark] = panelActiveColor;
            colors[ImGuiCol.SliderGrab] = panelHoverColor;
            colors[ImGuiCol.SliderGrabActive] = panelActiveColor;
            colors[ImGuiCol.Button] = panelColor;
            colors[ImGuiCol.ButtonHovered] = panelHoverColor;
            colors[ImGuiCol.ButtonActive] = panelHoverColor;
            colors[ImGuiCol.Header] = panelColor;
            colors[ImGuiCol.HeaderHovered] = panelHoverColor;
            colors[ImGuiCol.HeaderActive] = panelActiveColor;
            colors[ImGuiCol.Separator] = separatorColor;
            colors[ImGuiCol.SeparatorHovered] = separatorColor;
            colors[ImGuiCol.SeparatorActive] = separatorColor;
            colors[ImGuiCol.ResizeGrip] = bgColor;
            colors[ImGuiCol.ResizeGripHovered] = panelColor;
            colors[ImGuiCol.ResizeGripActive] = lightBgColor;
            colors[ImGuiCol.PlotLines] = panelActiveColor;
            colors[ImGuiCol.PlotLinesHovered] = panelHoverColor;
            colors[ImGuiCol.PlotHistogram] = panelActiveColor;
            colors[ImGuiCol.PlotHistogramHovered] = panelHoverColor;

            colors[ImGuiCol.ModalWindowDimBg] = lightBgColor_fade;
            colors[ImGuiCol.DragDropTarget] = bgColor;
            colors[ImGuiCol.NavCursor] = bgColor;
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(0.00f, 0.00f, 0.00f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.20f, 0.20f, 0.20f, 0.20f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;

            return colors;
        }

        // Semantic color storage for theme-aware UI colors
        private static Dictionary<ThemeColor, Vector4> _semanticColors = new();

        private static void LoadSemanticColors(ETheme theme)
        {
            _semanticColors = theme switch
            {
                ETheme.Light => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors
                    { ThemeColor.PrimaryText, new Vector4(20/255f, 20/255f, 20/255f, 1.0f) },
                    { ThemeColor.WarningText, new Vector4(0.8f, 0.0f, 0.0f, 0.75f) },
                    { ThemeColor.SuccessText, new Vector4(0.0f, 0.5f, 0.0f, 0.75f) },

                    // Button colors - more distinct selected color for light theme
                    { ThemeColor.SelectedButton, new Vector4(0.55f, 0.70f, 0.90f, 1.0f) },
                    { ThemeColor.DestructiveButton, new Vector4(0.9f, 0.3f, 0.3f, 1.0f) },
                    { ThemeColor.ButtonRed, new Vector4(0.8f, 0.2f, 0.2f, 1.0f) },

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, new Vector4(0.7f, 0.7f, 0.7f, 1.0f) },
                    { ThemeColor.BuffHeader_Debuff, new Vector4(1.0f, 0.6f, 0.6f, 0.75f) },
                    { ThemeColor.BuffHeader_Buff, new Vector4(0.6f, 0.9f, 0.6f, 0.75f) },
                    { ThemeColor.BuffHeader_Unknown, new Vector4(0.9f, 0.8f, 0.4f, 0.75f) },

                    // Status colors
                    { ThemeColor.ConnectionConnected, new Vector4(0.0f, 0.5f, 0.0f, 1.0f) },
                    { ThemeColor.ConnectionDisconnected, new Vector4(0.8f, 0.0f, 0.0f, 1.0f) }
                },
                ETheme.Dark => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors
                    { ThemeColor.PrimaryText, new Vector4(1.0f, 1.0f, 1.0f, 1.0f) },
                    { ThemeColor.WarningText, Colors.Red_Transparent },
                    { ThemeColor.SuccessText, Colors.Green_Transparent },

                    // Button colors
                    { ThemeColor.SelectedButton, Colors.DimGray },
                    { ThemeColor.DestructiveButton, Colors.Red },
                    { ThemeColor.ButtonRed, Colors.DarkRed },

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, Colors.DimGray },
                    { ThemeColor.BuffHeader_Debuff, Colors.DarkRed_Transparent },
                    { ThemeColor.BuffHeader_Buff, Colors.LightGreen_Transparent },
                    { ThemeColor.BuffHeader_Unknown, Colors.Goldenrod_Transparent },

                    // Status colors
                    { ThemeColor.ConnectionConnected, Colors.Green },
                    { ThemeColor.ConnectionDisconnected, Colors.Red }
                },
                ETheme.Black => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors - same as Dark
                    { ThemeColor.PrimaryText, new Vector4(240/255f, 240/255f, 240/255f, 1.0f) },
                    { ThemeColor.WarningText, Colors.Red_Transparent },
                    { ThemeColor.SuccessText, Colors.Green_Transparent },

                    // Button colors - same as Dark
                    { ThemeColor.SelectedButton, Colors.DimGray },
                    { ThemeColor.DestructiveButton, Colors.Red },
                    { ThemeColor.ButtonRed, Colors.DarkRed },

                    // Buff header colors - same as Dark
                    { ThemeColor.BuffHeader_Default, Colors.DimGray },
                    { ThemeColor.BuffHeader_Debuff, Colors.DarkRed_Transparent },
                    { ThemeColor.BuffHeader_Buff, Colors.LightGreen_Transparent },
                    { ThemeColor.BuffHeader_Unknown, Colors.Goldenrod_Transparent },

                    // Status colors - same as Dark
                    { ThemeColor.ConnectionConnected, Colors.Green },
                    { ThemeColor.ConnectionDisconnected, Colors.Red }
                }
            };
        }

        public static void ApplyTheme(ETheme theme)
        {
            Dictionary<ImGuiCol, Vector4> colors = theme switch
            {
                ETheme.Dark => GetDarkThemeColors(),
                ETheme.Black => GetBlackThemeColors(),
                ETheme.Light => GetLightThemeColors(),
                _ => GetDarkThemeColors() // Default fallback
            };

            Apply(colors);
            ApplyThemeStyles(theme);

            // Load semantic colors for the new theme system
            LoadSemanticColors(theme);
        }

        // ========================================================================
        // NEW THEME SYSTEM - Unified color accessor
        // ========================================================================

        /// <summary>
        /// Get a theme-aware semantic color by key.
        /// Returns Dark theme colors as fallback if not yet initialized.
        /// </summary>
        public static Vector4 GetColor(ThemeColor colorKey)
        {
            if (_semanticColors.TryGetValue(colorKey, out var color))
            {
                return color;
            }
            // Fallback to Dark theme colors if not yet loaded
            return colorKey switch
            {
                ThemeColor.PrimaryText => new Vector4(1.0f, 1.0f, 1.0f, 1.0f),  // Dark theme white
                ThemeColor.WarningText => Colors.Red_Transparent,
                ThemeColor.SuccessText => Colors.Green_Transparent,
                _ => Vector4.One
            };
        }

        // ========================================================================
        // BACKWARD COMPATIBILITY WRAPPERS
        // These methods maintain compatibility with existing code.
        // New code should use Theme.GetColor(ThemeColor.X) directly.
        // ========================================================================

        public static Vector4 GetPrimaryTextColor() => GetColor(ThemeColor.PrimaryText);
        public static Vector4 GetSelectedButtonColor() => GetColor(ThemeColor.SelectedButton);
        public static Vector4 GetWarningTextColor() => GetColor(ThemeColor.WarningText);
        public static Vector4 GetSuccessTextColor() => GetColor(ThemeColor.SuccessText);

        public static Vector4 GetBuffHeaderColor(int buffTypeColor)
        {
            ThemeColor key = buffTypeColor switch
            {
                99 => ThemeColor.BuffHeader_Default,
                0  => ThemeColor.BuffHeader_Debuff,
                1  => ThemeColor.BuffHeader_Buff,
                2  => ThemeColor.BuffHeader_Unknown,
                _  => ThemeColor.BuffHeader_Default
            };
            return GetColor(key);
        }

        public static Vector4 GetDestructiveButtonColor() => GetColor(ThemeColor.DestructiveButton);
        public static Vector4 GetButtonRedColor() => GetColor(ThemeColor.ButtonRed);

        public static Vector4 GetConnectionStatusColor(bool isConnected)
            => GetColor(isConnected ? ThemeColor.ConnectionConnected : ThemeColor.ConnectionDisconnected);

        // Backward compatibility wrapper
        public static void VSDarkTheme()
        {
            ApplyTheme(ETheme.Dark);
        }
    }
}
