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

            // Light background
            Vector4 bgColor = new Vector4(245 / 255f, 245 / 255f, 245 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(230 / 255f, 230 / 255f, 230 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(230 / 255f, 230 / 255f, 230 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(220 / 255f, 220 / 255f, 220 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(255 / 255f, 255 / 255f, 255 / 255f, 1.0f);
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
        }

        // Backward compatibility wrapper
        public static void VSDarkTheme()
        {
            ApplyTheme(ETheme.Dark);
        }
    }
}
