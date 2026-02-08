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

        private static Dictionary<ImGuiCol, Vector4> GetPurpleThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Dark purple background with violet accents
            Vector4 bgColor = new Vector4(30 / 255f, 20 / 255f, 40 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(45 / 255f, 30 / 255f, 60 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(45 / 255f, 30 / 255f, 60 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(60 / 255f, 40 / 255f, 75 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(45 / 255f, 30 / 255f, 60 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(120 / 255f, 80 / 255f, 180 / 255f, 1.0f);
            Vector4 panelActiveColor = new Vector4(100 / 255f, 50 / 255f, 150 / 255f, 1.0f);

            Vector4 textColor = new Vector4(245 / 255f, 240 / 255f, 255 / 255f, 1.0f);
            Vector4 textDisabledColor = new Vector4(140 / 255f, 120 / 255f, 160 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(70 / 255f, 50 / 255f, 90 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(100 / 255f, 80 / 255f, 120 / 255f, 1.0f);

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
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 0.90f, 1.00f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.50f, 0.40f, 0.60f, 0.20f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;

            return colors;
        }

        private static Dictionary<ImGuiCol, Vector4> GetCyberpunkThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Nearly black with neon cyan/pink accents
            Vector4 bgColor = new Vector4(10 / 255f, 10 / 255f, 15 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(20 / 255f, 20 / 255f, 30 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(20 / 255f, 20 / 255f, 30 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(30 / 255f, 30 / 255f, 40 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(20 / 255f, 20 / 255f, 30 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(0 / 255f, 255 / 255f, 255 / 255f, 1.0f);  // Cyan neon
            Vector4 panelActiveColor = new Vector4(255 / 255f, 0 / 255f, 128 / 255f, 1.0f);  // Magenta neon

            Vector4 textColor = new Vector4(0 / 255f, 255 / 255f, 255 / 255f, 1.0f);  // Cyan text
            Vector4 textDisabledColor = new Vector4(80 / 255f, 80 / 255f, 120 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(40 / 255f, 40 / 255f, 60 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(60 / 255f, 60 / 255f, 100 / 255f, 1.0f);

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
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(0.00f, 1.00f, 1.00f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.10f, 0.10f, 0.15f, 0.20f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;

            return colors;
        }

        private static Dictionary<ImGuiCol, Vector4> GetSolarizedThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Solarized Dark palette
            Vector4 bgColor = new Vector4(0 / 255f, 43 / 255f, 54 / 255f, 1.0f);       // #002b36
            Vector4 lightBgColor = new Vector4(7 / 255f, 54 / 255f, 66 / 255f, 1.0f);    // #073642
            Vector4 lightBgColor_fade = new Vector4(7 / 255f, 54 / 255f, 66 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(15 / 255f, 80 / 255f, 90 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(7 / 255f, 54 / 255f, 66 / 255f, 1.0f);     // #073642
            Vector4 panelHoverColor = new Vector4(38 / 255f, 139 / 255f, 210 / 255f, 1.0f);  // #268bd2 blue
            Vector4 panelActiveColor = new Vector4(133 / 255f, 153 / 255f, 0 / 255f, 1.0f);   // #859900 green

            Vector4 textColor = new Vector4(147 / 255f, 161 / 255f, 161 / 255f, 1.0f);  // #93a1a1 base0
            Vector4 textDisabledColor = new Vector4(88 / 255f, 110 / 255f, 117 / 255f, 1.0f);  // #586e75 base01
            Vector4 borderColor = new Vector4(7 / 255f, 54 / 255f, 66 / 255f, 1.0f);     // #073642
            Vector4 separatorColor = new Vector4(101 / 255f, 123 / 255f, 131 / 255f, 1.0f);  // #657b83 base1

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
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(0.65f, 0.75f, 0.75f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.15f, 0.30f, 0.35f, 0.20f);
            colors[ImGuiCol.Tab] = bgColor;
            colors[ImGuiCol.TabHovered] = panelHoverColor;
            colors[ImGuiCol.TabSelected] = panelActiveColor;
            colors[ImGuiCol.TabDimmed] = bgColor;
            colors[ImGuiCol.TabDimmedSelected] = panelActiveColor;

            return colors;
        }

        private static Dictionary<ImGuiCol, Vector4> GetForestThemeColors()
        {
            Dictionary<ImGuiCol, Vector4> colors = new();

            // Dark green nature theme
            Vector4 bgColor = new Vector4(20 / 255f, 35 / 255f, 25 / 255f, 1.0f);
            Vector4 lightBgColor = new Vector4(30 / 255f, 50 / 255f, 35 / 255f, 1.0f);
            Vector4 lightBgColor_fade = new Vector4(30 / 255f, 50 / 255f, 35 / 255f, 0.35f);
            Vector4 veryLightBgColor = new Vector4(40 / 255f, 65 / 255f, 45 / 255f, 1.0f);

            Vector4 panelColor = new Vector4(30 / 255f, 50 / 255f, 35 / 255f, 1.0f);
            Vector4 panelHoverColor = new Vector4(80 / 255f, 160 / 255f, 80 / 255f, 1.0f);
            Vector4 panelActiveColor = new Vector4(40 / 255f, 140 / 255f, 40 / 255f, 1.0f);

            Vector4 textColor = new Vector4(220 / 255f, 240 / 255f, 220 / 255f, 1.0f);
            Vector4 textDisabledColor = new Vector4(100 / 255f, 140 / 255f, 100 / 255f, 1.0f);
            Vector4 borderColor = new Vector4(40 / 255f, 70 / 255f, 45 / 255f, 1.0f);
            Vector4 separatorColor = new Vector4(60 / 255f, 100 / 255f, 65 / 255f, 1.0f);

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
            colors[ImGuiCol.NavWindowingHighlight] = new Vector4(0.70f, 0.95f, 0.70f, 0.70f);
            colors[ImGuiCol.NavWindowingDimBg] = new Vector4(0.20f, 0.35f, 0.25f, 0.20f);
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
                    { ThemeColor.ConnectionDisconnected, new Vector4(0.8f, 0.0f, 0.0f, 1.0f) },

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.0f, 0.365f, 0.851f, 1.0f) },     // Blue (0xFF005DD9)
                    { ThemeColor.WarningBanner, new Vector4(0.678f, 0.369f, 0.082f, 1.0f) },  // Brown (0xFFAD5E15)

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.40f, 0.70f, 1.00f, 1.00f) },
                    { ThemeColor.ChatTimestamp, new Vector4(0.60f, 0.65f, 0.75f, 1.00f) },

                    // Chat channel colors - brighter for light theme
                    { ThemeColor.ChatChannel_Null, new Vector4(0.60f, 0.60f, 0.60f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.20f, 0.60f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.30f, 0.80f, 0.30f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.50f, 0.60f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.70f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(1.00f, 0.40f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.40f, 0.70f, 0.80f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.40f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.20f, 0.10f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(224 / 255f, 64 / 255f, 64 / 255f, 0.85f) },
                    { ThemeColor.HpBarWarning, new Vector4(223 / 255f, 171 / 255f, 8 / 255f, 0.85f) },
                    { ThemeColor.HpBarHealthy, new Vector4(28 / 255f, 180 / 255f, 84 / 255f, 0.85f) },

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.25f, 0.85f, 0.35f, 1.0f) },
                    { ThemeColor.DebugUnseen, new Vector4(0.85f, 0.25f, 0.25f, 1.0f) },

                    // Status/state colors - Light theme uses darker variants for contrast
                    { ThemeColor.EncounterPausedBackground, new Vector4(0.7f, 0.2f, 0.2f, 0.75f) },  // Darker red
                    { ThemeColor.EncounterPausedText, new Vector4(20/255f, 20/255f, 20/255f, 1.0f) },
                    { ThemeColor.EncounterResumedButton, new Vector4(0.0f, 0.5f, 0.0f, 1.0f) },  // Dark green

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(0.85f, 0.65f, 0.0f, 1.0f) },  // Darker gold when pinned
                    { ThemeColor.WindowUnpinned, new Vector4(0.4f, 0.4f, 0.4f, 1.0f) },  // Darker gray when unpinned
                    { ThemeColor.WindowCollapsed, new Vector4(0.2f, 0.5f, 0.8f, 1.0f) },  // Darker blue when collapsed
                    { ThemeColor.WindowExpanded, new Vector4(0.35f, 0.35f, 0.35f, 1.0f) },  // Darker gray when expanded

                    // Common UI colors - slightly darker for light theme visibility
                    { ThemeColor.QualityBasic, new Vector4(0.5f, 0.6f, 0.8f, 1.0f) },  // Darker blue
                    { ThemeColor.QualityAdvanced, new Vector4(0.55f, 0.45f, 0.75f, 1.0f) },  // Darker purple
                    { ThemeColor.QualityExcellent, new Vector4(0.8f, 0.7f, 0.1f, 1.0f) },  // Darker gold

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(180 / 255f, 180 / 255f, 180 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(140 / 255f, 140 / 255f, 140 / 255f, 1.0f) }
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
                    { ThemeColor.ConnectionDisconnected, Colors.Red },

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.0f, 0.365f, 0.851f, 1.0f) },     // Blue (0xFF005DD9)
                    { ThemeColor.WarningBanner, new Vector4(0.678f, 0.369f, 0.082f, 1.0f) },  // Brown (0xFFAD5E15)

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.40f, 0.70f, 1.00f, 1.00f) },
                    { ThemeColor.ChatTimestamp, new Vector4(0.60f, 0.65f, 0.75f, 1.00f) },

                    // Chat channel colors
                    { ThemeColor.ChatChannel_Null, new Vector4(0.50f, 0.50f, 0.50f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.39f, 0.78f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.56f, 0.93f, 0.56f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.71f, 0.76f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.84f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(1.00f, 0.63f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.68f, 0.85f, 0.90f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.55f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.39f, 0.28f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(224 / 255f, 64 / 255f, 64 / 255f, 0.85f) },
                    { ThemeColor.HpBarWarning, new Vector4(223 / 255f, 171 / 255f, 8 / 255f, 0.85f) },
                    { ThemeColor.HpBarHealthy, new Vector4(28 / 255f, 180 / 255f, 84 / 255f, 0.85f) },

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.25f, 0.85f, 0.35f, 1.0f) },
                    { ThemeColor.DebugUnseen, new Vector4(0.85f, 0.25f, 0.25f, 1.0f) },

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, Colors.DarkRed_Transparent },
                    { ThemeColor.EncounterPausedText, Colors.White },
                    { ThemeColor.EncounterResumedButton, Colors.DarkGreen },

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(1.0f, 0.85f, 0.0f, 1.0f) },  // Gold when pinned
                    { ThemeColor.WindowUnpinned, new Vector4(0.6f, 0.6f, 0.6f, 1.0f) },  // Gray when unpinned
                    { ThemeColor.WindowCollapsed, new Vector4(0.4f, 0.7f, 1.0f, 1.0f) },  // Blue when collapsed
                    { ThemeColor.WindowExpanded, new Vector4(0.5f, 0.5f, 0.5f, 1.0f) },  // Gray when expanded

                    // Common UI colors
                    { ThemeColor.QualityBasic, Colors.QualityBasic },
                    { ThemeColor.QualityAdvanced, Colors.QualityAdvanced },
                    { ThemeColor.QualityExcellent, Colors.QualityExcellent },

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(78 / 255f, 78 / 255f, 78 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(120 / 255f, 120 / 255f, 120 / 255f, 1.0f) }
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
                    { ThemeColor.ConnectionDisconnected, Colors.Red },

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.0f, 0.365f, 0.851f, 1.0f) },     // Blue (0xFF005DD9)
                    { ThemeColor.WarningBanner, new Vector4(0.678f, 0.369f, 0.082f, 1.0f) },  // Brown (0xFFAD5E15)

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.40f, 0.70f, 1.00f, 1.00f) },
                    { ThemeColor.ChatTimestamp, new Vector4(0.60f, 0.65f, 0.75f, 1.00f) },

                    // Chat channel colors
                    { ThemeColor.ChatChannel_Null, new Vector4(0.50f, 0.50f, 0.50f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.39f, 0.78f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.56f, 0.93f, 0.56f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.71f, 0.76f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.84f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(1.00f, 0.63f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.68f, 0.85f, 0.90f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.55f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.39f, 0.28f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(224 / 255f, 64 / 255f, 64 / 255f, 0.85f) },
                    { ThemeColor.HpBarWarning, new Vector4(223 / 255f, 171 / 255f, 8 / 255f, 0.85f) },
                    { ThemeColor.HpBarHealthy, new Vector4(28 / 255f, 180 / 255f, 84 / 255f, 0.85f) },

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.25f, 0.85f, 0.35f, 1.0f) },
                    { ThemeColor.DebugUnseen, new Vector4(0.85f, 0.25f, 0.25f, 1.0f) },

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, Colors.DarkRed_Transparent },
                    { ThemeColor.EncounterPausedText, new Vector4(240/255f, 240/255f, 240/255f, 1.0f) },
                    { ThemeColor.EncounterResumedButton, Colors.DarkGreen },

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(1.0f, 0.85f, 0.0f, 1.0f) },  // Gold when pinned
                    { ThemeColor.WindowUnpinned, new Vector4(0.5f, 0.5f, 0.5f, 1.0f) },  // Dim gray when unpinned
                    { ThemeColor.WindowCollapsed, new Vector4(0.3f, 0.6f, 0.9f, 1.0f) },  // Blue when collapsed
                    { ThemeColor.WindowExpanded, new Vector4(0.4f, 0.4f, 0.4f, 1.0f) },  // Gray when expanded

                    // Common UI colors
                    { ThemeColor.QualityBasic, Colors.QualityBasic },
                    { ThemeColor.QualityAdvanced, Colors.QualityAdvanced },
                    { ThemeColor.QualityExcellent, Colors.QualityExcellent },

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(50 / 255f, 50 / 255f, 50 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(80 / 255f, 80 / 255f, 80 / 255f, 1.0f) }
                },
                ETheme.Purple => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors - purple theme adjusted
                    { ThemeColor.PrimaryText, new Vector4(245/255f, 240/255f, 255/255f, 1.0f) },
                    { ThemeColor.WarningText, new Vector4(1.0f, 0.5f, 0.5f, 0.75f) },
                    { ThemeColor.SuccessText, new Vector4(0.5f, 1.0f, 0.7f, 0.75f) },

                    // Button colors
                    { ThemeColor.SelectedButton, new Vector4(80/255f, 50/255f, 120/255f, 1.0f) },
                    { ThemeColor.DestructiveButton, new Vector4(1.0f, 0.4f, 0.4f, 1.0f) },
                    { ThemeColor.ButtonRed, new Vector4(0.8f, 0.3f, 0.3f, 1.0f) },

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, new Vector4(60/255f, 40/255f, 80/255f, 1.0f) },
                    { ThemeColor.BuffHeader_Debuff, new Vector4(1.0f, 0.5f, 0.5f, 0.75f) },
                    { ThemeColor.BuffHeader_Buff, new Vector4(0.6f, 1.0f, 0.6f, 0.75f) },
                    { ThemeColor.BuffHeader_Unknown, new Vector4(1.0f, 0.8f, 0.3f, 0.75f) },

                    // Status colors
                    { ThemeColor.ConnectionConnected, new Vector4(0.5f, 1.0f, 0.7f, 1.0f) },
                    { ThemeColor.ConnectionDisconnected, new Vector4(1.0f, 0.4f, 0.4f, 1.0f) },

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.5f, 0.3f, 0.9f, 1.0f) },
                    { ThemeColor.WarningBanner, new Vector4(0.9f, 0.5f, 0.2f, 1.0f) },

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.60f, 0.50f, 1.00f, 1.00f) },
                    { ThemeColor.ChatTimestamp, new Vector4(0.65f, 0.55f, 0.85f, 1.00f) },

                    // Chat channel colors - purple tinted
                    { ThemeColor.ChatChannel_Null, new Vector4(0.50f, 0.50f, 0.55f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.50f, 0.70f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.50f, 0.90f, 0.60f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.65f, 0.75f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.80f, 0.20f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(1.00f, 0.55f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.60f, 0.80f, 0.90f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.60f, 0.10f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.45f, 0.40f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(224 / 255f, 64 / 255f, 64 / 255f, 0.85f) },
                    { ThemeColor.HpBarWarning, new Vector4(223 / 255f, 171 / 255f, 8 / 255f, 0.85f) },
                    { ThemeColor.HpBarHealthy, new Vector4(28 / 255f, 180 / 255f, 84 / 255f, 0.85f) },

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.35f, 0.95f, 0.45f, 1.0f) },
                    { ThemeColor.DebugUnseen, new Vector4(0.95f, 0.35f, 0.35f, 1.0f) },

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, new Vector4(0.7f, 0.2f, 0.4f, 0.75f) },  // Purple-red
                    { ThemeColor.EncounterPausedText, new Vector4(245/255f, 240/255f, 255/255f, 1.0f) },
                    { ThemeColor.EncounterResumedButton, new Vector4(0.5f, 1.0f, 0.7f, 1.0f) },  // Green-mint

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(1.0f, 0.8f, 0.2f, 1.0f) },  // Gold-yellow
                    { ThemeColor.WindowUnpinned, new Vector4(0.5f, 0.4f, 0.7f, 1.0f) },  // Dim purple
                    { ThemeColor.WindowCollapsed, new Vector4(0.5f, 0.3f, 0.9f, 1.0f) },  // Violet
                    { ThemeColor.WindowExpanded, new Vector4(0.4f, 0.35f, 0.5f, 1.0f) },  // Gray-purple

                    // Common UI colors
                    { ThemeColor.QualityBasic, new Vector4(0.6f, 0.7f, 0.9f, 1.0f) },
                    { ThemeColor.QualityAdvanced, new Vector4(0.75f, 0.6f, 0.9f, 1.0f) },
                    { ThemeColor.QualityExcellent, new Vector4(1.0f, 0.85f, 0.2f, 1.0f) },

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(60 / 255f, 40 / 255f, 80 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(100 / 255f, 70 / 255f, 130 / 255f, 1.0f) }
                },
                ETheme.Cyberpunk => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors - cyan for cyberpunk
                    { ThemeColor.PrimaryText, new Vector4(0/255f, 255/255f, 255/255f, 1.0f) },
                    { ThemeColor.WarningText, new Vector4(1.0f, 0.2f, 0.5f, 0.75f) },  // Pink warning
                    { ThemeColor.SuccessText, new Vector4(0.0f, 1.0f, 0.8f, 0.75f) },  // Cyan-green success

                    // Button colors
                    { ThemeColor.SelectedButton, new Vector4(30/255f, 30/255f, 50/255f, 1.0f) },
                    { ThemeColor.DestructiveButton, new Vector4(1.0f, 0.0f, 0.5f, 1.0f) },  // Neon pink
                    { ThemeColor.ButtonRed, new Vector4(1.0f, 0.0f, 0.6f, 1.0f) },  // Hot pink

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, new Vector4(20/255f, 20/255f, 40/255f, 1.0f) },
                    { ThemeColor.BuffHeader_Debuff, new Vector4(1.0f, 0.3f, 0.6f, 0.75f) },
                    { ThemeColor.BuffHeader_Buff, new Vector4(0.3f, 1.0f, 0.8f, 0.75f) },
                    { ThemeColor.BuffHeader_Unknown, new Vector4(1.0f, 0.8f, 0.0f, 0.75f) },

                    // Status colors
                    { ThemeColor.ConnectionConnected, new Vector4(0.0f, 1.0f, 0.8f, 1.0f) },  // Cyan-green
                    { ThemeColor.ConnectionDisconnected, new Vector4(1.0f, 0.0f, 0.5f, 1.0f) },  // Neon pink

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.0f, 0.8f, 1.0f, 1.0f) },  // Cyan
                    { ThemeColor.WarningBanner, new Vector4(1.0f, 0.5f, 0.0f, 1.0f) },  // Orange

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.00f, 1.00f, 1.00f, 1.00f) },  // Bright cyan
                    { ThemeColor.ChatTimestamp, new Vector4(0.40f, 0.60f, 0.80f, 1.00f) },

                    // Chat channel colors - neon cyberpunk
                    { ThemeColor.ChatChannel_Null, new Vector4(0.40f, 0.40f, 0.50f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.00f, 0.90f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.00f, 1.00f, 0.70f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.60f, 0.80f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.90f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(1.00f, 0.40f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.40f, 0.80f, 1.00f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.70f, 0.00f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.20f, 0.50f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(1.0f, 0.0f, 0.6f, 0.85f) },  // Neon pink
                    { ThemeColor.HpBarWarning, new Vector4(1.0f, 0.8f, 0.0f, 0.85f) },  // Yellow
                    { ThemeColor.HpBarHealthy, new Vector4(0.0f, 1.0f, 0.8f, 0.85f) },  // Cyan-green

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.0f, 1.0f, 0.5f, 1.0f) },  // Neon green
                    { ThemeColor.DebugUnseen, new Vector4(1.0f, 0.2f, 0.5f, 1.0f) },  // Neon pink

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, new Vector4(1.0f, 0.0f, 0.5f, 0.75f) },  // Neon pink transparent
                    { ThemeColor.EncounterPausedText, new Vector4(0/255f, 255/255f, 255/255f, 1.0f) },  // Cyan
                    { ThemeColor.EncounterResumedButton, new Vector4(0.0f, 1.0f, 0.8f, 1.0f) },  // Cyan-green

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(1.0f, 1.0f, 0.0f, 1.0f) },  // Neon yellow
                    { ThemeColor.WindowUnpinned, new Vector4(0.3f, 0.3f, 0.4f, 1.0f) },  // Dark gray
                    { ThemeColor.WindowCollapsed, new Vector4(0.0f, 1.0f, 1.0f, 1.0f) },  // Cyan
                    { ThemeColor.WindowExpanded, new Vector4(0.25f, 0.25f, 0.35f, 1.0f) },  // Dim gray

                    // Common UI colors
                    { ThemeColor.QualityBasic, new Vector4(0.4f, 0.7f, 1.0f, 1.0f) },  // Cyan-blue
                    { ThemeColor.QualityAdvanced, new Vector4(0.8f, 0.4f, 1.0f, 1.0f) },  // Purple
                    { ThemeColor.QualityExcellent, new Vector4(1.0f, 0.9f, 0.2f, 1.0f) },  // Gold

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(30 / 255f, 30 / 255f, 50 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(0 / 255f, 255 / 255f, 255 / 255f, 0.8f) }  // Cyan
                },
                ETheme.Solarized => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors - Solarized palette
                    { ThemeColor.PrimaryText, new Vector4(147/255f, 161/255f, 161/255f, 1.0f) },  // #93a1a1
                    { ThemeColor.WarningText, new Vector4(220/255f, 50/255f, 47/255f, 0.75f) },  // Solarized red
                    { ThemeColor.SuccessText, new Vector4(133/255f, 153/255f, 0/255f, 0.75f) },  // Solarized green

                    // Button colors
                    { ThemeColor.SelectedButton, new Vector4(88/255f, 110/255f, 117/255f, 1.0f) },  // #586e75
                    { ThemeColor.DestructiveButton, new Vector4(220/255f, 50/255f, 47/255f, 1.0f) },
                    { ThemeColor.ButtonRed, new Vector4(203/255f, 75/255f, 22/255f, 1.0f) },  // Solarized orange

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, new Vector4(88/255f, 110/255f, 117/255f, 1.0f) },
                    { ThemeColor.BuffHeader_Debuff, new Vector4(220/255f, 50/255f, 47/255f, 0.75f) },
                    { ThemeColor.BuffHeader_Buff, new Vector4(133/255f, 153/255f, 0/255f, 0.75f) },
                    { ThemeColor.BuffHeader_Unknown, new Vector4(181/255f, 137/255f, 0/255f, 0.75f) },  // Solarized yellow

                    // Status colors
                    { ThemeColor.ConnectionConnected, new Vector4(133/255f, 153/255f, 0/255f, 1.0f) },  // Green
                    { ThemeColor.ConnectionDisconnected, new Vector4(220/255f, 50/255f, 47/255f, 1.0f) },  // Red

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(38/255f, 139/255f, 210/255f, 1.0f) },  // Solarized blue
                    { ThemeColor.WarningBanner, new Vector4(203/255f, 75/255f, 22/255f, 1.0f) },  // Orange

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(38/255f, 139/255f, 210/255f, 1.00f) },
                    { ThemeColor.ChatTimestamp, new Vector4(101/255f, 123/255f, 131/255f, 1.00f) },

                    // Chat channel colors - Solarized palette
                    { ThemeColor.ChatChannel_Null, new Vector4(88/255f, 110/255f, 117/255f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(38/255f, 139/255f, 210/255f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(133/255f, 153/255f, 0/255f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(211/255f, 54/255f, 130/255f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(181/255f, 137/255f, 0/255f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(108/255f, 113/255f, 196/255f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(42/255f, 161/255f, 152/255f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(203/255f, 75/255f, 22/255f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(220/255f, 50/255f, 47/255f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(220 / 255f, 50 / 255f, 47 / 255f, 0.85f) },
                    { ThemeColor.HpBarWarning, new Vector4(181 / 255f, 137 / 255f, 0 / 255f, 0.85f) },
                    { ThemeColor.HpBarHealthy, new Vector4(133 / 255f, 153 / 255f, 0 / 255f, 0.85f) },

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(133/255f, 153/255f, 0/255f, 1.0f) },
                    { ThemeColor.DebugUnseen, new Vector4(220/255f, 50/255f, 47/255f, 1.0f) },

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, new Vector4(220/255f, 50/255f, 47/255f, 0.75f) },
                    { ThemeColor.EncounterPausedText, new Vector4(147/255f, 161/255f, 161/255f, 1.0f) },
                    { ThemeColor.EncounterResumedButton, new Vector4(133/255f, 153/255f, 0/255f, 1.0f) },

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(181/255f, 137/255f, 0/255f, 1.0f) },  // Yellow
                    { ThemeColor.WindowUnpinned, new Vector4(88/255f, 110/255f, 117/255f, 1.0f) },  // Base01
                    { ThemeColor.WindowCollapsed, new Vector4(38/255f, 139/255f, 210/255f, 1.0f) },  // Blue
                    { ThemeColor.WindowExpanded, new Vector4(101/255f, 123/255f, 131/255f, 1.0f) },  // Base1

                    // Common UI colors
                    { ThemeColor.QualityBasic, new Vector4(38/255f, 139/255f, 210/255f, 1.0f) },  // Blue
                    { ThemeColor.QualityAdvanced, new Vector4(108/255f, 113/255f, 196/255f, 1.0f) },  // Violet
                    { ThemeColor.QualityExcellent, new Vector4(181/255f, 137/255f, 0/255f, 1.0f) },  // Yellow

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(7 / 255f, 54 / 255f, 66 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(101/255f, 123/255f, 131/255f, 1.0f) }
                },
                ETheme.Forest => new Dictionary<ThemeColor, Vector4>
                {
                    // Text colors - forest theme adjusted
                    { ThemeColor.PrimaryText, new Vector4(220/255f, 240/255f, 220/255f, 1.0f) },
                    { ThemeColor.WarningText, new Vector4(1.0f, 0.5f, 0.2f, 0.75f) },  // Orange warning
                    { ThemeColor.SuccessText, new Vector4(0.3f, 0.9f, 0.5f, 0.75f) },  // Forest green

                    // Button colors
                    { ThemeColor.SelectedButton, new Vector4(30/255f, 70/255f, 40/255f, 1.0f) },
                    { ThemeColor.DestructiveButton, new Vector4(1.0f, 0.4f, 0.2f, 1.0f) },  // Orange-red
                    { ThemeColor.ButtonRed, new Vector4(0.9f, 0.3f, 0.2f, 1.0f) },

                    // Buff header colors
                    { ThemeColor.BuffHeader_Default, new Vector4(35/255f, 60/255f, 40/255f, 1.0f) },
                    { ThemeColor.BuffHeader_Debuff, new Vector4(1.0f, 0.5f, 0.3f, 0.75f) },  // Orange-red
                    { ThemeColor.BuffHeader_Buff, new Vector4(0.4f, 0.9f, 0.5f, 0.75f) },  // Green
                    { ThemeColor.BuffHeader_Unknown, new Vector4(1.0f, 0.85f, 0.3f, 0.75f) },  // Gold

                    // Status colors
                    { ThemeColor.ConnectionConnected, new Vector4(0.3f, 0.9f, 0.5f, 1.0f) },  // Forest green
                    { ThemeColor.ConnectionDisconnected, new Vector4(1.0f, 0.4f, 0.2f, 1.0f) },  // Orange-red

                    // Banner colors (ModuleSolver)
                    { ThemeColor.InfoBanner, new Vector4(0.2f, 0.6f, 0.4f, 1.0f) },  // Forest green
                    { ThemeColor.WarningBanner, new Vector4(0.9f, 0.6f, 0.2f, 1.0f) },  // Brown-orange

                    // Chat colors
                    { ThemeColor.ChatLink, new Vector4(0.30f, 0.90f, 0.60f, 1.00f) },  // Light green
                    { ThemeColor.ChatTimestamp, new Vector4(0.50f, 0.80f, 0.60f, 1.00f) },

                    // Chat channel colors - forest nature theme
                    { ThemeColor.ChatChannel_Null, new Vector4(0.45f, 0.50f, 0.45f, 1.0f) },
                    { ThemeColor.ChatChannel_World, new Vector4(0.30f, 0.70f, 0.80f, 1.0f) },
                    { ThemeColor.ChatChannel_Scene, new Vector4(0.35f, 0.80f, 0.40f, 1.0f) },
                    { ThemeColor.ChatChannel_Team, new Vector4(1.00f, 0.65f, 0.65f, 1.0f) },
                    { ThemeColor.ChatChannel_Union, new Vector4(1.00f, 0.80f, 0.30f, 1.0f) },
                    { ThemeColor.ChatChannel_Private, new Vector4(0.90f, 0.50f, 0.90f, 1.0f) },
                    { ThemeColor.ChatChannel_Group, new Vector4(0.40f, 0.70f, 0.80f, 1.0f) },
                    { ThemeColor.ChatChannel_TopNotice, new Vector4(1.00f, 0.60f, 0.20f, 1.0f) },
                    { ThemeColor.ChatChannel_System, new Vector4(1.00f, 0.35f, 0.20f, 1.0f) },

                    // HP bar colors (SpawnTracker)
                    { ThemeColor.HpBarCritical, new Vector4(1.0f, 0.3f, 0.2f, 0.85f) },  // Orange-red
                    { ThemeColor.HpBarWarning, new Vector4(1.0f, 0.75f, 0.2f, 0.85f) },  // Orange
                    { ThemeColor.HpBarHealthy, new Vector4(0.3f, 0.9f, 0.5f, 0.85f) },  // Forest green

                    // Debug colors (NetDebug)
                    { ThemeColor.DebugSeen, new Vector4(0.3f, 0.9f, 0.5f, 1.0f) },  // Forest green
                    { ThemeColor.DebugUnseen, new Vector4(1.0f, 0.4f, 0.2f, 1.0f) },  // Orange-red

                    // Status/state colors
                    { ThemeColor.EncounterPausedBackground, new Vector4(0.9f, 0.3f, 0.2f, 0.75f) },  // Orange-red transparent
                    { ThemeColor.EncounterPausedText, new Vector4(220/255f, 240/255f, 220/255f, 1.0f) },
                    { ThemeColor.EncounterResumedButton, new Vector4(0.3f, 0.9f, 0.5f, 1.0f) },  // Forest green

                    // Window state colors
                    { ThemeColor.WindowPinned, new Vector4(1.0f, 0.85f, 0.2f, 1.0f) },  // Gold
                    { ThemeColor.WindowUnpinned, new Vector4(0.35f, 0.55f, 0.4f, 1.0f) },  // Forest green
                    { ThemeColor.WindowCollapsed, new Vector4(0.2f, 0.7f, 0.4f, 1.0f) },  // Light green
                    { ThemeColor.WindowExpanded, new Vector4(0.3f, 0.5f, 0.35f, 1.0f) },  // Darker green

                    // Common UI colors
                    { ThemeColor.QualityBasic, new Vector4(0.4f, 0.7f, 0.5f, 1.0f) },  // Green-blue
                    { ThemeColor.QualityAdvanced, new Vector4(0.6f, 0.5f, 0.8f, 1.0f) },  // Purple
                    { ThemeColor.QualityExcellent, new Vector4(1.0f, 0.85f, 0.2f, 1.0f) },  // Gold

                    // Alpha helper colors
                    { ThemeColor.TransparentWhite, new Vector4(1.0f, 1.0f, 1.0f, 0.0f) },
                    { ThemeColor.TransparentBlack, new Vector4(0.0f, 0.0f, 0.0f, 0.0f) },

                    // Border/separator colors
                    { ThemeColor.BorderSubtle, new Vector4(35 / 255f, 60 / 255f, 40 / 255f, 0.5f) },
                    { ThemeColor.BorderStrong, new Vector4(50 / 255f, 90 / 255f, 55 / 255f, 1.0f) }
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
                ETheme.Purple => GetPurpleThemeColors(),
                ETheme.Cyberpunk => GetCyberpunkThemeColors(),
                ETheme.Solarized => GetSolarizedThemeColors(),
                ETheme.Forest => GetForestThemeColors(),
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

        /// <summary>
        /// Returns a theme color with modified alpha. Useful for hover states, transparency effects.
        /// </summary>
        public static Vector4 GetColor(ThemeColor colorKey, float alphaMultiplier)
        {
            var color = GetColor(colorKey);
            return new Vector4(color.X, color.Y, color.Z, color.W * alphaMultiplier);
        }

        /// <summary>
        /// Apply alpha to any Vector4 color - for manual alpha blending patterns
        /// </summary>
        public static Vector4 WithAlpha(Vector4 color, float alpha)
        {
            return new Vector4(color.X, color.Y, color.Z, alpha);
        }

        /// <summary>
        /// Multiply alpha of a color - for fade effects
        /// </summary>
        public static Vector4 MultiplyAlpha(Vector4 color, float alphaMultiplier)
        {
            return new Vector4(color.X, color.Y, color.Z, color.W * alphaMultiplier);
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
