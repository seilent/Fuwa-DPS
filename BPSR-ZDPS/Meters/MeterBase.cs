using BPSR_ZDPS.DataTypes;
using BPSR_ZDPS.Windows;
using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_ZDPS.Meters
{
    public class MeterBase
    {
        public string Name = "";

        protected Encounter? ActiveEncounter = null;

        public virtual void Draw(MainWindow mainWindow) { }

        /// <summary>
        /// Returns the number of entries that will be displayed in this meter.
        /// Used for calculating the exact height needed for auto-sizing the window.
        /// </summary>
        public virtual int GetEntryCount()
        {
            return 0;
        }

        /// <summary>
        /// Calculates the exact height needed for the meter's ListBox based on entry count.
        /// Uses a simple calculation that accounts for the selectable height with AlignTextToFramePadding.
        /// </summary>
        protected float GetListHeight(int entryCount)
        {
            if (entryCount <= 0)
                return 0;

            // Each selectable with AlignTextToFramePadding takes approximately GetFrameHeight() space
            // Add a small buffer to account for any rounding or theme differences
            float entryHeight = ImGui.GetFrameHeight() + 2.0f;
            float itemSpacing = ImGui.GetStyle().ItemSpacing.Y;

            // Total height: N entries + spacing between them
            return (entryCount * entryHeight) + (itemSpacing * Math.Max(0, entryCount - 1)) + 4.0f;
        }

        public static bool SelectableWithHintImage(string number, string name, string value, int profession)
        {
            var startPoint = ImGui.GetCursorPos();

            ImGui.AlignTextToFramePadding();

            float texSize = ImGui.GetItemRectSize().Y;
            float offset = ImGui.CalcTextSize(number).X;
            if (Settings.Instance.ShowClassIconsInMeters)
            {
                offset += (ImGui.GetStyle().ItemSpacing.X * 2) + (texSize + 2);
            }
            else
            {
                offset += (ImGui.GetStyle().ItemSpacing.X);
            }

            ImGui.SetCursorPosX(offset);

            // Add semi-transparent background for better text contrast against colored progress bar
            Vector4 bgColor = Settings.Instance.Theme == ETheme.Light
                ? new Vector4(1, 1, 1, 0.7f)  // Light theme: white with opacity
                : new Vector4(0, 0, 0, 0.5f); // Dark/Black theme: black with opacity
            ImGui.PushStyleColor(ImGuiCol.Header, bgColor);
            ImGui.PushStyleColor(ImGuiCol.HeaderHovered, bgColor);
            ImGui.PushStyleColor(ImGuiCol.HeaderActive, bgColor);

            bool ret = ImGui.Selectable(name, false, ImGuiSelectableFlags.SpanAllColumns);

            ImGui.PopStyleColor(3);
            ImGui.SameLine();

            ImGui.SetCursorPos(startPoint);

            ImGui.TextUnformatted(number);

            if (Settings.Instance.ShowClassIconsInMeters)
            {
                ImGui.SameLine();
                var tex = ImageHelper.GetTextureByKey($"Profession_{profession}_Slim");
                if (tex == null)
                {
                    ImGui.Dummy(new Vector2(texSize, texSize));
                }
                else
                {
                    var roleColor = DataTypes.Professions.RoleTypeColors(DataTypes.Professions.GetRoleFromBaseProfessionId(profession));

                    if (DataTypes.Settings.Instance.ColorClassIconsByRole)
                    {
                        ImGui.ImageWithBg((ImTextureRef)tex, new Vector2(texSize, texSize), new Vector2(0, 0), new Vector2(1, 1), new Vector4(0, 0, 0, 0), roleColor);
                    }
                    else
                    {
                        ImGui.Image((ImTextureRef)tex, new Vector2(texSize, texSize));
                    }
                }
            }

            ImGui.SameLine();
            ImGui.SetCursorPosX(ImGui.GetCursorPosX() + Math.Max(0.0f, ImGui.GetContentRegionAvail().X - ImGui.CalcTextSize(value).X));
            ImGui.TextUnformatted(value);

            return ret;
        }
    }
}
