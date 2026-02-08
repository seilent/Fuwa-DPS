using BPSR_ZDPSLib;
using BPSR_ZDPS.DataTypes;
using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BPSR_ZDPS.Windows
{
    public static class SettingsWindow
    {
        public const string LAYER = "SettingsWindowLayer";
        public static string TITLE_ID = "###SettingsWindow";

        static int PreviousSelectedNetworkDeviceIdx = -1;
        static int SelectedNetworkDeviceIdx = -1;
        static bool normalizeMeterContributions;
        static bool useShortWidthNumberFormatting;
        static bool showClassIconsInMeters;
        static bool colorClassIconsByRole;
        static bool showSkillIconsInDetails;
        static bool onlyShowDamageContributorsInMeters;
        static bool onlyShowPartyMembersInMeters;
        static bool showAbilityScoreInMeters;
        static bool showSeasonStrengthInMeters;
        static bool showSubProfessionNameInMeters;
        static bool useAutomaticWipeDetection;
        static bool skipTeleportStateCheckInAutomaticWipeDetection;
        static bool disableWipeRecalculationOverwriting;
        static bool splitEncountersOnNewPhases;
        static bool excludeDragonsFromPhaseSplit;
        static bool displayTruePerSecondValuesInMeters;
        static bool allowGamepadNavigationInputInZDPS;
        static bool keepPastEncounterInMeterUntilNextDamage;
        static bool useDatabaseForEncounterHistory;
        static int databaseRetentionPolicyDays;
        static bool limitEncounterBuffTrackingWithoutDatabase;
        static bool allowEncounterSavingPausingInOpenWorld;

        static bool meterSettingsTankingShowDeaths;
        static bool meterSettingsNpcTakenShowHpData;
        static bool meterSettingsNpcTakenHideMaxHp;
        static bool meterSettingsNpcTakenUseHpMeter;

        // Tab visibility settings
        static bool showHealingTab;
        static bool showTankingTab;
        static bool showNpcTakenTab;
        static bool mergeDpsAndHealTabs;
        static bool disableTopMostWhenNoEntities;

        static bool playNotificationSoundOnMatchmake;
        static string matchmakeNotificationSoundPath;
        static bool loopNotificationSoundOnMatchmake;
        static float matchmakeNotificationVolume;
        static bool playNotificationSoundOnReadyCheck;
        static string readyCheckNotificationSoundPath;
        static bool loopNotificationSoundOnReadyCheck;
        static float readyCheckNotificationVolume;

        static bool logToFile;

        static bool IsBindingEncounterResetKey = false;
        static uint EncounterResetKey;
        static string EncounterResetKeyName = "";
        static bool IsBindingPinnedWindowClickthroughKey = false;
        static uint PinnedWindowClickthroughKey;
        static string PinnedWindowClickthroughKeyName = "";

        static SharpPcap.LibPcap.LibPcapLiveDeviceList? NetworkDevices;
        static EGameCapturePreference GameCapturePreference;
        static string gameCaptureCustomExeName;

        static bool saveEncounterReportToFile;
        static int reportFileRetentionPolicyDays;
        static int minimumPlayerCountToCreateReport;
        static bool alwaysCreateReportAtDungeonEnd;

        static bool webhookReportsEnabled;
        static EWebhookReportsMode webhookReportsMode;
        static string webhookReportsDeduplicationServerUrl;
        static string webhookReportsDiscordUrl;
        static string webhookReportsCustomUrl;

        static bool checkForZDPSUpdatesOnStartup;
        static string latestZDPSVersionCheckURL;

        static bool lowPerformanceMode;

        // External Settings
        static bool externalBPTimerEnabled;
        static bool externalBPTimerIncludeCharacterId;
        static bool externalBPTimerFieldBossHpReportsEnabled;

        static WindowSettings windowSettings;

        static ETheme theme;
        static string locale;

        static bool IsDiscordWebhookUrlValid = true;

        static int RunOnceDelayed = 0;

        static bool IsElevated = false;

        public static void Open()
        {
            RunOnceDelayed = 0;

            ImGuiP.PushOverrideID(ImGuiP.ImHashStr(LAYER));
            ImGui.OpenPopup(TITLE_ID);

            NetworkDevices = SharpPcap.LibPcap.LibPcapLiveDeviceList.Instance;

            Load();

            LoadHotkeys();

            // Set selection to matching device name (the index could have changed since last time we were here)
            if (!string.IsNullOrEmpty(Settings.Instance.NetCaptureDeviceName))
            {
                for (int i = 0; i < NetworkDevices.Count; i++)
                {
                    if (NetworkDevices[i].Name == Settings.Instance.NetCaptureDeviceName)
                    {
                        SelectedNetworkDeviceIdx = i;
                        if (PreviousSelectedNetworkDeviceIdx == -1)
                        {
                            // This is the first time we're opening the menu, so let's set the default previous value as well
                            // Doing so prevents the capture from being restarted on first save
                            PreviousSelectedNetworkDeviceIdx = i;
                        }
                    }
                }
            }

            // Default to first device in list as fallback, if there are any
            if (SelectedNetworkDeviceIdx == -1 && NetworkDevices?.Count > 0)
            {
                SelectedNetworkDeviceIdx = 0;
            }

            // Disable all HotKeys while we're in the Settings menu to prevent unexpected behavior when rebinding
            HotKeyManager.UnregisterAllHotKeys();

            ImGui.PopID();
        }

        public static void Draw(MainWindow mainWindow)
        {
            var io = ImGui.GetIO();
            var main_viewport = ImGui.GetMainViewport();

            // TODO: Open window at center of current active monitor
            // Will need to use GLFW to figure out monitors/sizes/positions/etc

            //ImGui.SetNextWindowPos(new Vector2(main_viewport.WorkPos.X + 200, main_viewport.WorkPos.Y + 120), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(550, 350), new Vector2(ImGui.GETFLTMAX()));
            //ImGui.SetNextWindowPos(new Vector2(io.DisplaySize.X, io.DisplaySize.Y), ImGuiCond.Appearing);

            ImGui.SetNextWindowSize(new Vector2(650, 680), ImGuiCond.FirstUseEver);
            ImGuiP.PushOverrideID(ImGuiP.ImHashStr(LAYER));

            if (ImGui.BeginPopupModal($"{AppStrings.GetLocalized("Settings_Title")}{TITLE_ID}"))
            {
                if (RunOnceDelayed == 0)
                {
                    RunOnceDelayed++;
                    using (var identity = System.Security.Principal.WindowsIdentity.GetCurrent())
                    {
                        IsElevated = new System.Security.Principal.WindowsPrincipal(identity).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                    }
                }
                else if (RunOnceDelayed == 2)
                {
                    RunOnceDelayed++;
                    Utils.SetCurrentWindowIcon();
                    Utils.BringWindowToFront();
                }
                else if (RunOnceDelayed < 3)
                {
                    RunOnceDelayed++;
                }

                ImGuiTabBarFlags tabBarFlags = ImGuiTabBarFlags.FittingPolicyScroll | ImGuiTabBarFlags.NoTooltip | ImGuiTabBarFlags.NoCloseWithMiddleMouseButton;
                if (ImGui.BeginTabBar("##SettingsTabs", tabBarFlags))
                {
                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_General")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##GeneralTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Network_Device"));
                        ImGui.Text(AppStrings.GetLocalized("Settings_Network_SelectDevice"));

                        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);

                        string network_device_preview = "";
                        if (SelectedNetworkDeviceIdx > -1 && NetworkDevices?.Count > 0)
                        {
                            network_device_preview = NetworkDevices[SelectedNetworkDeviceIdx].Description;
                        }

                        if (ImGui.BeginCombo("##NetworkDeviceCombo", network_device_preview, ImGuiComboFlags.HeightLarge))
                        {
                            for (int i = 0; i < NetworkDevices?.Count; i++)
                            {
                                bool isSelected = (SelectedNetworkDeviceIdx == i);
                                var device = NetworkDevices[i];

                                string friendlyName = "";
                                if (!string.IsNullOrEmpty(device.Interface?.FriendlyName))
                                {
                                    friendlyName = $"{device.Interface?.FriendlyName}\n";
                                }

                                if (ImGui.Selectable($"{friendlyName}{device.Description}\n{device.Name}", isSelected))
                                {
                                    SelectedNetworkDeviceIdx = i;
                                }

                                if (isSelected)
                                {
                                    ImGui.SetItemDefaultFocus();
                                }

                                ImGui.Separator();
                            }

                            if (NetworkDevices == null || NetworkDevices?.Count == 0)
                            {
                                ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_NoDeviceFound"));
                            }

                            ImGui.EndCombo();
                        }

                        ImGui.AlignTextToFramePadding();
                        ImGui.TextUnformatted(AppStrings.GetLocalized("Settings_Network_GameCapturePreference"));
                        ImGui.SameLine();

                        var gamePrefName = Utils.GameCapturePreferenceToName(GameCapturePreference);
                        ImGui.SetNextItemWidth(150);
                        if (ImGui.BeginCombo("##EGameCapturePreference", gamePrefName))
                        {
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_Auto")))
                            {
                                GameCapturePreference = EGameCapturePreference.Auto;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_Standalone")))
                            {
                                GameCapturePreference = EGameCapturePreference.Standalone;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_Steam")))
                            {
                                GameCapturePreference = EGameCapturePreference.Steam;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_Epic")))
                            {
                                GameCapturePreference = EGameCapturePreference.Epic;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_HaoPlaySEA")))
                            {
                                GameCapturePreference = EGameCapturePreference.HaoPlaySea;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_XDG")))
                            {
                                GameCapturePreference = EGameCapturePreference.XDG;
                            }
                            else if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Network_Custom")))
                            {
                                GameCapturePreference = EGameCapturePreference.Custom;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_Network_GameCapturePreferenceTooltip"));

                            ImGui.EndCombo();
                        }

                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Network_Tooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (GameCapturePreference == EGameCapturePreference.Custom)
                        {
                            ImGui.Indent();
                            
                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_Network_CustomExeName"));
                            ImGui.SameLine();
                            ImGui.SetNextItemWidth(-1);
                            if (ImGui.InputText("##GameCaptureCustomExeName", ref gameCaptureCustomExeName, 512))
                            {
                                gameCaptureCustomExeName = Path.GetFileNameWithoutExtension(gameCaptureCustomExeName);
                            }
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Network_CustomExeNameTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.Unindent();
                        }

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Keybinds_Title"));

                        if (IsElevated == false)
                        {
                            ImGui.PushStyleColor(ImGuiCol.ChildBg, Theme.GetColor(ThemeColor.WarningText, 0.3f));
                            ImGui.BeginChild("##KeybindsNotice", new Vector2(0, 0), ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.Borders);
                            ImGui.PushFont(HelperMethods.Fonts["Segoe-Bold"], ImGui.GetFontSize());
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_ImportantNote"));
                            ImGui.PopFont();
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_AdminNote"));
                            ImGui.EndChild();
                            ImGui.PopStyleColor();
                        }

                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_Description"));
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_Cancel"));

                        ImGui.Indent();

                        RebindKeyButton(AppStrings.GetLocalized("Settings_Keybinds_EncounterReset"), ref EncounterResetKey, ref EncounterResetKeyName, ref IsBindingEncounterResetKey);
                        if (splitEncountersOnNewPhases)
                        {
                            ImGui.Indent();
                            ImGui.PushStyleColor(ImGuiCol.Text, Theme.GetColor(ThemeColor.WarningText, 0.7f));
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_EncounterResetWarning"));
                            ImGui.PopStyleColor();
                            ImGui.Unindent();
                        }
                        RebindKeyButton(AppStrings.GetLocalized("Settings_Keybinds_PinnedWindowClickthrough"), ref PinnedWindowClickthroughKey, ref PinnedWindowClickthroughKeyName, ref IsBindingPinnedWindowClickthroughKey);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Keybinds_ClickthroughTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.Unindent();

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UpdateChecking_Title"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Update_CheckOnStartup"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##CheckForZDPSUpdatesOnStartup", ref checkForZDPSUpdatesOnStartup);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Update_CheckOnStartupTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Update_VersionCheckURL"));
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(-1);
                        if (ImGui.InputText("##LatestZDPSVersionCheckURL", ref latestZDPSVersionCheckURL, 512))
                        {
                            // If the value was empty, revert back to the default URL
                            if (string.IsNullOrEmpty(latestZDPSVersionCheckURL))
                            {
                                latestZDPSVersionCheckURL = "https://raw.githubusercontent.com/Blue-Protocol-Source/BPSR-ZDPS-Metadata/master/LatestVersion.txt";
                            }
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Update_VersionCheckURLTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Database_Title"));

                        ShowRestartRequiredNotice(Settings.Instance.UseDatabaseForEncounterHistory != useDatabaseForEncounterHistory, "Use Database For Encounter History");

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Database_UseDatabase"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##UseDatabaseForEncounterHistory", ref useDatabaseForEncounterHistory);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Database_UseDatabaseTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.BeginDisabled(!useDatabaseForEncounterHistory);
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Database_RetentionPolicy"));
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                        ImGui.SetNextItemWidth(-1);
                        ImGui.SliderInt("##DatabaseRetentionPolicyDays", ref databaseRetentionPolicyDays, 0, 30, databaseRetentionPolicyDays == 0 ? AppStrings.GetLocalized("Settings_Database_KeepForever") : string.Format(AppStrings.GetLocalized("Settings_Database_Days"), databaseRetentionPolicyDays));
                        ImGui.PopStyleColor(2);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Database_RetentionPolicyTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        ImGui.BeginDisabled(useDatabaseForEncounterHistory);
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Database_LimitBuffTracking"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##LimitEncounterBuffTrackingWithoutDatabase", ref limitEncounterBuffTrackingWithoutDatabase);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Database_LimitBuffTrackingTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Database_AllowPausing"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##AllowEncounterSavingPausingInOpenWorld", ref allowEncounterSavingPausingInOpenWorld);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Database_AllowPausingTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_Combat")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##CombatTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Combat_Title"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_NormalizeBars"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##NormalizeMeterContributions", ref normalizeMeterContributions);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_NormalizeBarsTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_ShortWidthNumber"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##UseShortWidthNumberFormatting", ref useShortWidthNumberFormatting);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_ShortWidthNumberTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_AutoWipeDetection"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##UseAutomaticWipeDetection", ref useAutomaticWipeDetection);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_AutoWipeDetectionTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_SkipTeleportCheck"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##SkipTeleportStateCheckInAutomaticWipeDetection", ref skipTeleportStateCheckInAutomaticWipeDetection);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_SkipTeleportCheckTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_DisableWipeRecalc"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##DisableWipeRecalculationOverwriting", ref disableWipeRecalculationOverwriting);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        //ImGui.TextWrapped("When enabled, the internal process of checking the Dead status of all players in the Encounter is allowed to overwrite the detected wipe status from the normal automatic detector.\nAllowing this to overturn results is experimental so only enable it if you run into incorrect wipe reporting.");
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_DisableWipeRecalcTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_SplitPhases"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##SplitEncountersOnNewPhases", ref splitEncountersOnNewPhases);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_SplitPhasesTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_ExcludeDragonsFromSplit"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ExcludeDragonsFromPhaseSplit", ref excludeDragonsFromPhaseSplit);

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Combat_TruePerSecond"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##DisplayTruePerSecondValuesInMeters", ref displayTruePerSecondValuesInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Combat_TruePerSecondTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_UserInterface")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##UserInterfaceTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_Title"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ShowClassIcons"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ShowClassIconsInMeters", ref showClassIconsInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ShowClassIconsTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ColorIconsByRole"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ColorClassIconsByRole", ref colorClassIconsByRole);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ColorIconsByRoleTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ShowSkillIcons"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ShowSkillIconsInDetails", ref showSkillIconsInDetails);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ShowSkillIconsTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_OnlyShowContributors"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##OnlyShowContributorsInMeters", ref onlyShowDamageContributorsInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OnlyShowContributorsTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_OnlyShowPartyMembers"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##OnlyShowPartyMembersInMeters", ref onlyShowPartyMembersInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OnlyShowPartyMembersTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ShowAbilityScore"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ShowAbilityScoreInMeters", ref showAbilityScoreInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ShowAbilityScoreTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ShowSeasonStrength"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ShowSeasonStrengthInMeters", ref showSeasonStrengthInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ShowSeasonStrengthTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_ShowSubProfession"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##ShowSubProfessionNameInMeters", ref showSubProfessionNameInMeters);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ShowSubProfessionTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_GamepadNavigation"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##AllowGamepadNavigationInputInZDPS", ref allowGamepadNavigationInputInZDPS);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_GamepadNavigationTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_KeepPastEncounter"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##KeepPastEncounterInMeterUntilNextDamage", ref keepPastEncounterInMeterUntilNextDamage);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_KeepPastEncounterTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.CollapsingHeader(AppStrings.GetLocalized("Settings_UI_OpacityHeader")))
                        {
                            ImGui.Indent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_OpacityMainWindow"));
                            ImGui.SetNextItemWidth(-1);
                            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                            if (ImGui.SliderInt("##MainWindowOpacity", ref windowSettings.MainWindow.Opacity, 20, 100, $"{windowSettings.MainWindow.Opacity}%%", ImGuiSliderFlags.ClampOnInput))
                            {
                                windowSettings.MainWindow.Opacity = windowSettings.MainWindow.Opacity;
                            }
                            ImGui.PopStyleColor(2);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OpacityMainWindowTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.Separator();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_OpacityDisableTopMost"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##DisableTopMostWhenNoEntities", ref disableTopMostWhenNoEntities);
                            if (ImGui.IsItemHovered())
                            {
                                ImGui.SetTooltip(AppStrings.GetLocalized("Settings_UI_OpacityDisableTopMostTooltip"));
                            }
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OpacityDisableTopMostTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_OpacityCooldownTracker"));
                            ImGui.SetNextItemWidth(-1);
                            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                            if (ImGui.SliderInt("##CooldownPriorityTrackerWindowOpacity", ref windowSettings.RaidManagerCooldowns.Opacity, 20, 100, $"{windowSettings.RaidManagerCooldowns.Opacity}%%", ImGuiSliderFlags.ClampOnInput))
                            {
                                windowSettings.RaidManagerCooldowns.Opacity = windowSettings.RaidManagerCooldowns.Opacity;
                            }
                            ImGui.PopStyleColor(2);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OpacityCooldownTrackerTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_OpacityEntityCacheViewer"));
                            ImGui.SetNextItemWidth(-1);
                            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                            if (ImGui.SliderInt("##EntityCacheViewerWindowOpacity", ref windowSettings.EntityCacheViewer.Opacity, 20, 100, $"{windowSettings.EntityCacheViewer.Opacity}%%", ImGuiSliderFlags.ClampOnInput))
                            {
                                windowSettings.EntityCacheViewer.Opacity = windowSettings.EntityCacheViewer.Opacity;
                            }
                            ImGui.PopStyleColor(2);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OpacityEntityCacheViewerTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_OpacityIntegrations"));

                            if (ImGui.CollapsingHeader(AppStrings.GetLocalized("Settings_UI_OpacityBPTimerHeader") + "##BPTimerOpacitySection", ImGuiTreeNodeFlags.DefaultOpen))
                            {
                                ImGui.Indent();

                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_UI_OpacitySpawnTracker"));
                                ImGui.SetNextItemWidth(-1);
                                ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                                ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                                if (ImGui.SliderInt("##BPTimerSpawnTrackerWindowOpacity", ref windowSettings.SpawnTracker.Opacity, 20, 100, $"{windowSettings.SpawnTracker.Opacity}%%"))
                                {
                                    windowSettings.SpawnTracker.Opacity = windowSettings.SpawnTracker.Opacity;
                                }
                                ImGui.PopStyleColor(2);
                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_OpacitySpawnTrackerTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();

                                ImGui.Unindent();
                            }

                            ImGui.Unindent();
                        }

                        if (ImGui.CollapsingHeader(AppStrings.GetLocalized("Settings_UI_ScaleHeader")))
                        {
                            ImGui.Indent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_ScaleMeterBar"));
                            ImGui.SetNextItemWidth(-1);
                            ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                            ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                            if (ImGui.SliderFloat("##MeterBarScale", ref windowSettings.MainWindow.MeterBarScale, 0.80f, 2.0f, $"{(int)(windowSettings.MainWindow.MeterBarScale * 100)}%%"))
                            {
                                windowSettings.MainWindow.MeterBarScale = MathF.Round(windowSettings.MainWindow.MeterBarScale, 2);
                            }
                            ImGui.PopStyleColor(2);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ScaleMeterBarTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_OpacityIntegrations"));

                            if (ImGui.CollapsingHeader(AppStrings.GetLocalized("Settings_UI_OpacityBPTimerHeader") + "##BPTimerScaleSection", ImGuiTreeNodeFlags.DefaultOpen))
                            {
                                ImGui.Indent();

                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_UI_ScaleSpawnTrackerText"));
                                ImGui.SetNextItemWidth(-1);
                                ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                                ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                                if (ImGui.SliderFloat("##BPTimerSpawnTrackerTextScale", ref windowSettings.SpawnTracker.TextScale, 0.80f, 3.0f, $"{(int)(windowSettings.SpawnTracker.TextScale * 100)}%%"))
                                {
                                    windowSettings.SpawnTracker.TextScale = MathF.Round(windowSettings.SpawnTracker.TextScale, 2);
                                }
                                ImGui.PopStyleColor(2);
                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ScaleSpawnTrackerTextTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();

                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_UI_ScaleSpawnTrackerLine"));
                                ImGui.SetNextItemWidth(-1);
                                ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                                ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                                if (ImGui.SliderFloat("##BPTimerSpawnTrackerLineScale", ref windowSettings.SpawnTracker.LineScale, 0.80f, 3.0f, $"{(int)(windowSettings.SpawnTracker.LineScale * 100)}%%"))
                                {
                                    windowSettings.SpawnTracker.LineScale = MathF.Round(windowSettings.SpawnTracker.LineScale, 2);
                                }
                                ImGui.PopStyleColor(2);
                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ScaleSpawnTrackerLineTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();

                                ImGui.Unindent();
                            }

                            ImGui.Unindent();
                        }

                        if(ImGui.CollapsingHeader(AppStrings.GetLocalized("Settings_UI_MeterHeader")))
                        {
                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_MeterTabVisibility"));

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowHealing"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##ShowHealingTab", ref showHealingTab);

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowTanking"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##ShowTankingTab", ref showTankingTab);

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowNpcTaken"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##ShowNpcTakenTab", ref showNpcTakenTab);

                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_MeterLayout"));

                            ImGui.BeginDisabled(!showHealingTab);
                            if (showHealingTab)
                            {
                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterMergeDpsHeal"));
                                ImGui.SameLine();
                                ImGui.Checkbox("##MergeDpsAndHealTabs", ref mergeDpsAndHealTabs);
                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_MeterMergeDpsHealTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();
                            }
                            else
                            {
                                // Force disable merge mode when healing tab is hidden
                                mergeDpsAndHealTabs = false;
                            }
                            ImGui.EndDisabled();

                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_MeterTanking"));

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowDeaths"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##MeterSettingsTankingShowDeaths", ref meterSettingsTankingShowDeaths);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_MeterShowDeathsTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_MeterNpcTaken"));

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowHpData"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##MeterSettingsNpcTakenShowHpData", ref meterSettingsNpcTakenShowHpData);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_MeterShowHpDataTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterHideMaxHp"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##MeterSettingsNpcTakenHideMaxHp", ref meterSettingsNpcTakenHideMaxHp);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_MeterHideMaxHpTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text(AppStrings.GetLocalized("Settings_UI_MeterShowHpPercentBar"));
                            ImGui.SameLine();
                            ImGui.Checkbox("##MeterSettingsNpcTakenUseHpMeter", ref meterSettingsNpcTakenUseHpMeter);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_MeterShowHpPercentBarTooltip"));
                            ImGui.EndDisabled();
                            ImGui.Unindent();
                        }

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_ResetHeader"));

                        if (ImGui.Button(AppStrings.GetLocalized("Settings_UI_ResetMainWindowPos")))
                        {
                            var glfwMonitor = Hexa.NET.GLFW.GLFW.GetPrimaryMonitor();
                            var glfwVidMode = Hexa.NET.GLFW.GLFW.GetVideoMode(glfwMonitor);
                            mainWindow.NextWindowPosition = new Vector2(glfwVidMode.Width, glfwVidMode.Height);
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ResetMainWindowPosTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button(AppStrings.GetLocalized("Settings_UI_ResetMainWindowSize")))
                        {
                            mainWindow.NextWindowSize = mainWindow.DefaultWindowSize;
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ResetMainWindowSizeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button(AppStrings.GetLocalized("Settings_UI_ResetCooldownTrackerSize")))
                        {
                            RaidManagerCooldownsWindow.ResetWindowSize = true;
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ResetCooldownTrackerSizeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button(AppStrings.GetLocalized("Settings_UI_ResetEntityCacheViewerSize")))
                        {
                            EntityCacheViewerWindow.ResetWindowSize = true;
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ResetEntityCacheViewerSizeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button(AppStrings.GetLocalized("Settings_UI_ResetSpawnTrackerSize")))
                        {
                            SpawnTrackerWindow.ResetWindowSize = true;
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ResetSpawnTrackerSizeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_LowPerfHeader"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_LowPerfLabel"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##LowPerformanceMode", ref lowPerformanceMode);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_LowPerfTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_UI_Appearance"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_Theme"));
                        ImGui.SameLine();

                        string currentThemeName = theme switch
                        {
                            ETheme.Dark => AppStrings.GetLocalized("Settings_UI_ThemeDark"),
                            ETheme.Black => AppStrings.GetLocalized("Settings_UI_ThemeBlack"),
                            ETheme.Light => AppStrings.GetLocalized("Settings_UI_ThemeLight"),
                            ETheme.Purple => AppStrings.GetLocalized("Settings_UI_ThemePurple"),
                            ETheme.Cyberpunk => AppStrings.GetLocalized("Settings_UI_ThemeCyberpunk"),
                            ETheme.Solarized => AppStrings.GetLocalized("Settings_UI_ThemeSolarized"),
                            ETheme.Forest => AppStrings.GetLocalized("Settings_UI_ThemeForest"),
                            _ => AppStrings.GetLocalized("Settings_UI_ThemeDark")
                        };

                        ImGui.SetNextItemWidth(200);
                        if (ImGui.BeginCombo("##ThemeCombo", currentThemeName))
                        {
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeDark")))
                            {
                                theme = ETheme.Dark;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeDarkTooltip"));

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeBlack")))
                            {
                                theme = ETheme.Black;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeBlackTooltip"));

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeLight")))
                            {
                                theme = ETheme.Light;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeLightTooltip"));

                            ImGui.Separator();

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemePurple")))
                            {
                                theme = ETheme.Purple;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemePurpleTooltip"));

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeCyberpunk")))
                            {
                                theme = ETheme.Cyberpunk;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeCyberpunkTooltip"));

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeSolarized")))
                            {
                                theme = ETheme.Solarized;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeSolarizedTooltip"));

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_ThemeForest")))
                            {
                                theme = ETheme.Forest;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_UI_ThemeForestTooltip"));

                            ImGui.EndCombo();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_ThemeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_UI_Language"));
                        ImGui.SameLine();

                        string currentLocaleName = locale switch
                        {
                            "en" => AppStrings.GetLocalized("Settings_UI_English"),
                            "ja" => AppStrings.GetLocalized("Settings_UI_Japanese"),
                            _ => AppStrings.GetLocalized("Settings_UI_English")
                        };

                        ImGui.SetNextItemWidth(200);
                        if (ImGui.BeginCombo("##LocaleCombo", currentLocaleName))
                        {
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_English")))
                            {
                                locale = "en";
                            }

                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_UI_Japanese")))
                            {
                                locale = "ja";
                            }

                            ImGui.EndCombo();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_UI_LanguageTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_Matchmaking")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##MatchmakingTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Matchmaking_Header"));
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_PlaySound"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##PlayNotificationSoundOnMatchmake", ref playNotificationSoundOnMatchmake);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_PlaySoundTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.BeginDisabled(!playNotificationSoundOnMatchmake);
                        ImGui.Indent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_SoundPath"));
                        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 140 - ImGui.GetStyle().ItemSpacing.X);
                        ImGui.InputText("##MatchmakeNotificationSoundPath", ref matchmakeNotificationSoundPath, 1024);
                        ImGui.SameLine();
                        if (ImGui.Button(AppStrings.GetLocalized("Common_Browse") + "##MatchmakeSoundPathBrowseBtn", new Vector2(140, 0)))
                        {
                            string defaultDir = File.Exists(matchmakeNotificationSoundPath) ? Path.GetDirectoryName(matchmakeNotificationSoundPath) : "";

                            ImFileBrowser.OpenFile((selectedFilePath)=>
                            {
                                System.Diagnostics.Debug.WriteLine($"MatchmakeNotificationSoundPath = {selectedFilePath}");
                                matchmakeNotificationSoundPath = selectedFilePath;
                            },
                            "Select a sound file...", defaultDir, "MP3 (*.mp3)|*.mp3|WAV (*.wav)|*.wav|All Files (*.*)|*.*", 0);
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_SoundPathTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_LoopSound"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##loopNotificationSoundOnMatchmake", ref loopNotificationSoundOnMatchmake);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_LoopSoundTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_Volume"));
                        ImGui.SetNextItemWidth(-1);
                        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                        if (ImGui.SliderFloat("##MatchmakeNotificationVolume", ref matchmakeNotificationVolume, 0.10f, 3.0f, $"{(int)(matchmakeNotificationVolume * 100)}%%"))
                        {
                            matchmakeNotificationVolume = MathF.Round(matchmakeNotificationVolume, 2);
                        }
                        ImGui.PopStyleColor(2);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_VolumeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckPlaySound"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##PlayNotificationSoundOnReadyCheck", ref playNotificationSoundOnReadyCheck);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckPlaySoundTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.BeginDisabled(!playNotificationSoundOnReadyCheck);
                        ImGui.Indent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckSoundPath"));
                        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 140 - ImGui.GetStyle().ItemSpacing.X);
                        ImGui.InputText("##ReadyCheckNotificationSoundPath", ref readyCheckNotificationSoundPath, 1024);
                        ImGui.SameLine();
                        if (ImGui.Button(AppStrings.GetLocalized("Common_Browse") + "##ReadyCheckSoundPathBrowseBtn", new Vector2(140, 0)))
                        {
                            string defaultDir = File.Exists(readyCheckNotificationSoundPath) ? Path.GetDirectoryName(readyCheckNotificationSoundPath) : "";

                            ImFileBrowser.OpenFile((selectedFilePath) =>
                            {
                                System.Diagnostics.Debug.WriteLine($"ReadyCheckNotificationSoundPath = {selectedFilePath}");
                                readyCheckNotificationSoundPath = selectedFilePath;
                            },
                            "Select a sound file...", defaultDir, "MP3 (*.mp3)|*.mp3|WAV (*.wav)|*.wav|All Files (*.*)|*.*", 0);
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckSoundPathTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckLoopSound"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##loopNotificationSoundOnReadyCheck", ref loopNotificationSoundOnReadyCheck);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckLoopSoundTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckVolume"));
                        ImGui.SetNextItemWidth(-1);
                        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                        if (ImGui.SliderFloat("##ReadyCheckNotificationVolume", ref readyCheckNotificationVolume, 0.10f, 3.0f, $"{(int)(readyCheckNotificationVolume * 100)}%%"))
                        {
                            readyCheckNotificationVolume = MathF.Round(readyCheckNotificationVolume, 2);
                        }
                        ImGui.PopStyleColor(2);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Matchmaking_ReadyCheckVolumeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_Integrations")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##IntegrationsTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Integrations_Title"));

                        ShowGenericImportantNotice(!useAutomaticWipeDetection, "AutoWipeDetectionDisabled", AppStrings.GetLocalized("Settings_Integrations_AutoWipeDisabledWarning"));
                        ShowGenericImportantNotice(skipTeleportStateCheckInAutomaticWipeDetection, "SkipTeleportStateCheckInAutomaticWipeDetectionEnabled", AppStrings.GetLocalized("Settings_Integrations_SkipTeleportWarning"));
                        ShowGenericImportantNotice(!splitEncountersOnNewPhases, "SplitEncountersOnNewPhasesDisabled", AppStrings.GetLocalized("Settings_Integrations_SplitPhasesWarning"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_SaveReport"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##SaveEncounterReportToFile", ref saveEncounterReportToFile);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_SaveReportTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.BeginDisabled(!saveEncounterReportToFile);
                        ImGui.Indent();
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_ReportRetentionPolicy"));
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                        ImGui.SetNextItemWidth(-1);
                        ImGui.SliderInt("##ReportFileRetentionPolicyDays", ref reportFileRetentionPolicyDays, 0, 30, reportFileRetentionPolicyDays == 0 ? AppStrings.GetLocalized("Settings_Integrations_KeepForever") : string.Format(AppStrings.GetLocalized("Settings_Integrations_Players"), reportFileRetentionPolicyDays));
                        ImGui.PopStyleColor(2);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_ReportRetentionPolicyTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();
                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_MinPlayers"));
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, ImGui.GetColorU32(ImGuiCol.FrameBgHovered, 0.55f));
                        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, ImGui.GetColorU32(ImGuiCol.FrameBgActive, 0.55f));
                        ImGui.SetNextItemWidth(-1);
                        ImGui.SliderInt("##MinimumPlayerCountToCreateReport", ref minimumPlayerCountToCreateReport, 0, 20, minimumPlayerCountToCreateReport == 0 ? AppStrings.GetLocalized("Settings_Integrations_Any") : string.Format(AppStrings.GetLocalized("Settings_Integrations_Players"), minimumPlayerCountToCreateReport));
                        ImGui.PopStyleColor(2);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_MinPlayersTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_AlwaysCreateReport"));
                        ImGui.SameLine();
                        ImGui.Checkbox("##AlwaysCreateReportAtDungeonEnd", ref alwaysCreateReportAtDungeonEnd);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_AlwaysCreateReportTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.SeparatorText(AppStrings.GetLocalized("Settings_Integrations_WebhookHeader"));

                        ImGui.AlignTextToFramePadding();
                        ImGui.TextUnformatted(AppStrings.GetLocalized("Settings_Integrations_WebhookMode"));
                        ImGui.SameLine();
                        ImGui.SetNextItemWidth(-1);

                        string reportsModeName = "";
                        switch (webhookReportsMode)
                        {
                            case EWebhookReportsMode.DiscordDeduplication:
                                reportsModeName = AppStrings.GetLocalized("Settings_Integrations_DiscordDedup");
                                break;
                            case EWebhookReportsMode.Discord:
                                reportsModeName = AppStrings.GetLocalized("Settings_Integrations_DiscordWebhook");
                                break;
                            case EWebhookReportsMode.Custom:
                                reportsModeName = AppStrings.GetLocalized("Settings_Integrations_CustomUrl");
                                break;
                            case EWebhookReportsMode.FallbackDiscordDeduplication:
                                reportsModeName = AppStrings.GetLocalized("Settings_Integrations_FallbackDedup");
                                break;
                        }

                        if (ImGui.BeginCombo("##WebhookMode", $"{reportsModeName}", ImGuiComboFlags.None))
                        {
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Integrations_DiscordDedup")))
                            {
                                webhookReportsMode = EWebhookReportsMode.DiscordDeduplication;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_Integrations_DiscordDedupTooltip"));
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Integrations_DiscordWebhook")))
                            {
                                webhookReportsMode = EWebhookReportsMode.Discord;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_Integrations_DiscordWebhookTooltip"));
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Integrations_CustomUrl")))
                            {
                                webhookReportsMode = EWebhookReportsMode.Custom;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_Integrations_CustomUrlTooltip"));
                            if (ImGui.Selectable(AppStrings.GetLocalized("Settings_Integrations_FallbackDedup")))
                            {
                                webhookReportsMode = EWebhookReportsMode.FallbackDiscordDeduplication;
                            }
                            ImGui.SetItemTooltip(AppStrings.GetLocalized("Settings_Integrations_FallbackDedupTooltip"));
                            ImGui.EndCombo();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_WebhookModeTooltip"));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        // TODO: Maybe allow adding multiple Webhooks and toggling the enabled state of each one (should allow entering a friendly name next to them too)

                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(string.Format(AppStrings.GetLocalized("Settings_Integrations_WebhookSendTo"), reportsModeName));
                        ImGui.SameLine();
                        ImGui.Checkbox("##WebhookReportsEnabled", ref webhookReportsEnabled);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped(string.Format(AppStrings.GetLocalized("Settings_Integrations_WebhookSendToTooltip"), reportsModeName));
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.BeginDisabled(!webhookReportsEnabled);
                        ImGui.Indent();

                        switch (webhookReportsMode)
                        {
                            case EWebhookReportsMode.DiscordDeduplication:
                            case EWebhookReportsMode.Discord:
                            case EWebhookReportsMode.FallbackDiscordDeduplication:
                                if (webhookReportsMode == EWebhookReportsMode.DiscordDeduplication || webhookReportsMode == EWebhookReportsMode.FallbackDiscordDeduplication)
                                {
                                    ImGui.AlignTextToFramePadding();
                                    ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_DedupServerUrl"));
                                    ImGui.SameLine();
                                    ImGui.SetNextItemWidth(-1);
                                    ImGui.InputText("##WebhookReportsDeduplicationServerHost", ref webhookReportsDeduplicationServerUrl, 512);
                                    ImGui.Indent();
                                    ImGui.BeginDisabled(true);
                                    ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_DedupServerUrlTooltip"));
                                    if (webhookReportsMode == EWebhookReportsMode.FallbackDiscordDeduplication)
                                    {
                                        ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_FallbackNote"));
                                    }
                                    ImGui.EndDisabled();
                                    ImGui.Unindent();
                                }

                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_WebhookUrl"));
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(-1);
                                if (ImGui.InputText("##WebhookReportsDiscordUrl", ref webhookReportsDiscordUrl, 512))
                                {
                                    if (Utils.SplitAndValidateDiscordWebhook(webhookReportsDiscordUrl) != null)
                                    {
                                        IsDiscordWebhookUrlValid = true;
                                    }
                                    else
                                    {
                                        IsDiscordWebhookUrlValid = false;
                                    }
                                }

                                if (!IsDiscordWebhookUrlValid)
                                {
                                    ImGui.Indent();
                                    ImGui.BeginDisabled(true);
                                    ImGui.PushStyleColor(ImGuiCol.Text, Theme.GetColor(ThemeColor.WarningText));
                                    ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_InvalidUrl"));
                                    ImGui.PopStyleColor();
                                    ImGui.EndDisabled();
                                    ImGui.Unindent();
                                }

                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_WebhookUrlTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();
                                break;
                            case EWebhookReportsMode.Custom:
                                ImGui.AlignTextToFramePadding();
                                ImGui.Text(AppStrings.GetLocalized("Settings_Integrations_WebhookUrl"));
                                ImGui.SameLine();
                                ImGui.SetNextItemWidth(-1);
                                ImGui.InputText("##WebhookReportsCustomUrl", ref webhookReportsCustomUrl, 512);
                                ImGui.Indent();
                                ImGui.BeginDisabled(true);
                                ImGui.TextWrapped(AppStrings.GetLocalized("Settings_Integrations_CustomUrlInputTooltip"));
                                ImGui.EndDisabled();
                                ImGui.Unindent();
                                break;
                        }

                        ImGui.Unindent();
                        ImGui.EndDisabled();

                        if (ImGui.CollapsingHeader("BPTimer", ImGuiTreeNodeFlags.DefaultOpen))
                        {
                            ImGui.AlignTextToFramePadding();
                            ImGui.Text("BPTimer Enabled: ");
                            ImGui.SameLine();
                            ImGui.Checkbox("##ExternalBPTimerEnabled", ref externalBPTimerEnabled);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped("When enabled, allows sending reports back to BPTimer.com.");
                            bool hasBPTimerReports = externalBPTimerFieldBossHpReportsEnabled;
                            if (!hasBPTimerReports)
                            {
                                ImGui.PushStyleColor(ImGuiCol.Text, Theme.GetColor(ThemeColor.WarningText));
                            }
                            else
                            {
                                ImGui.PushStyleColor(ImGuiCol.Text, Theme.GetColor(ThemeColor.SuccessText));
                            }
                            ImGui.TextWrapped("Note: This setting alone does not enable reports. They must be enabled individually below.");
                            ImGui.PopStyleColor();

                            ImGui.EndDisabled();
                            if (ImGui.CollapsingHeader("Data Collection##BPTimerDataCollectionSection"))
                            {
                                ImGui.Indent();
                                ImGui.TextUnformatted("BPTimer collects the following data:");
                                ImGui.BulletText("Boss ID/HP/Position");
                                ImGui.BulletText("Character Line Number");
                                ImGui.BulletText("Account ID");
                                ImGui.SetItemTooltip("This is being used to determine what game region is being played on.");
                                ImGui.BulletText("Character UID (if you opt-in below)");
                                ImGui.BulletText("Your IP Address");
                                ImGui.Unindent();
                            }
                            ImGui.Unindent();

                            ImGui.BeginDisabled(!externalBPTimerEnabled);
                            ImGui.Indent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text("Include Own Character Data In Report: ");
                            ImGui.SameLine();
                            ImGui.Checkbox("##ExternalBPTimerIncludeCharacterId", ref externalBPTimerIncludeCharacterId);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped("When enabled, your Character UID will be included in the reported data.");
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.AlignTextToFramePadding();
                            ImGui.Text("BPTimer Field Boss HP Reports: ");
                            ImGui.SameLine();
                            ImGui.Checkbox("##ExternalBPTimerFieldBossHpReportsEnabled", ref externalBPTimerFieldBossHpReportsEnabled);
                            ImGui.Indent();
                            ImGui.BeginDisabled(true);
                            ImGui.TextWrapped("When enabled, reports Field Boss (and Magical Creature) HP data back to BPTimer.com.");
                            ImGui.EndDisabled();
                            ImGui.Unindent();

                            ImGui.Unindent();
                            ImGui.EndDisabled();
                        }

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem(AppStrings.GetLocalized("Settings_Tab_Development")))
                    {
                        var contentRegionAvail = ImGui.GetContentRegionAvail();
                        ImGui.BeginChild("##DevelopmentTabContent", new Vector2(contentRegionAvail.X, contentRegionAvail.Y - 56), ImGuiChildFlags.Borders);

                        ImGui.TextUnformatted("NOTE: This version of ZDPS is designed only for BPSR Season 1.");

                        ImGui.SeparatorText("Development");
                        if (ImGui.Button("Reload DataTables"))
                        {
                            AppState.LoadDataTables();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped("Does not update most existing values - mainly works for data set in new Encounters.");
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button("Restart Capture"))
                        {
                            MessageManager.StopCapturing();
                            MessageManager.InitializeCapturing();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped("Turns the MessageManager off and on to resolve issues of stalled data.");
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button("Reload Module Save"))
                        {
                            ModuleSolver.Init();
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped("Reloads your module inventory from the 'ModulesSaveData.json' file.");
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ShowRestartRequiredNotice(Settings.Instance.LogToFile != logToFile, "Write Debug Log To File");
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Write Debug Log To File: ");
                        ImGui.SameLine();
                        ImGui.Checkbox("##LogToFile", ref logToFile);
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped("When enabled, writes a debug log for ZDPS (ZDPS_log.txt). Applies after restarting ZDPS.");
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        if (ImGui.Button("Open GitHub Project Page"))
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = Settings.Instance.ZDPSWebsiteURL,
                                UseShellExecute = true,
                            });
                        }
                        ImGui.Indent();
                        ImGui.BeginDisabled(true);
                        ImGui.TextWrapped($"Open a web page to the GitHub Project located at\n{Settings.Instance.ZDPSWebsiteURL}");
                        ImGui.EndDisabled();
                        ImGui.Unindent();

                        ImGui.EndChild();
                        ImGui.EndTabItem();
                    }
                    
                    ImGui.EndTabBar();
                }

                ImGui.NewLine();
                float buttonWidth = 120;
                if (ImGui.Button(AppStrings.GetLocalized("Settings_Button_Save"), new Vector2(buttonWidth, 0)))
                {
                    Save(mainWindow);

                    ImGui.CloseCurrentPopup();
                }

                ImGui.SameLine();
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X - buttonWidth);
                if (ImGui.Button(AppStrings.GetLocalized("Settings_Button_Close"), new Vector2(buttonWidth, 0)))
                {
                    SelectedNetworkDeviceIdx = PreviousSelectedNetworkDeviceIdx;

                    Load();

                    EncounterResetKey = Settings.Instance.HotkeysEncounterReset;
                    if (EncounterResetKey == 0)
                    {
                        EncounterResetKeyName = "[UNBOUND]";
                    }
                    else
                    {
                        EncounterResetKeyName = ImGui.GetKeyNameS(HotKeyManager.VirtualKeyToImGuiKey((int)EncounterResetKey));
                    }

                    PinnedWindowClickthroughKey = Settings.Instance.HotkeysPinnedWindowClickthrough;
                    if (PinnedWindowClickthroughKey == 0)
                    {
                        PinnedWindowClickthroughKeyName = "[UNBOUND]";
                    }
                    else
                    {
                        PinnedWindowClickthroughKeyName = ImGui.GetKeyNameS(HotKeyManager.VirtualKeyToImGuiKey((int)PinnedWindowClickthroughKey));
                    }

                    RegisterAllHotkeys(mainWindow);

                    ImGui.CloseCurrentPopup();
                }

                ImFileBrowser.Draw();

                ImGui.EndPopup();
            }

            ImGui.PopID();
        }

        private static void Load()
        {
            normalizeMeterContributions = Settings.Instance.NormalizeMeterContributions;
            useShortWidthNumberFormatting = Settings.Instance.UseShortWidthNumberFormatting;
            showClassIconsInMeters = Settings.Instance.ShowClassIconsInMeters;
            colorClassIconsByRole = Settings.Instance.ColorClassIconsByRole;
            showSkillIconsInDetails = Settings.Instance.ShowSkillIconsInDetails;
            onlyShowDamageContributorsInMeters = Settings.Instance.OnlyShowDamageContributorsInMeters;
            onlyShowPartyMembersInMeters = Settings.Instance.OnlyShowPartyMembersInMeters;
            showAbilityScoreInMeters = Settings.Instance.ShowAbilityScoreInMeters;
            showSeasonStrengthInMeters = Settings.Instance.ShowSeasonStrengthInMeters;
            showSubProfessionNameInMeters = Settings.Instance.ShowSubProfessionNameInMeters;
            useAutomaticWipeDetection = Settings.Instance.UseAutomaticWipeDetection;
            skipTeleportStateCheckInAutomaticWipeDetection = Settings.Instance.SkipTeleportStateCheckInAutomaticWipeDetection;
            disableWipeRecalculationOverwriting = Settings.Instance.DisableWipeRecalculationOverwriting;
            splitEncountersOnNewPhases = Settings.Instance.SplitEncountersOnNewPhases;
            excludeDragonsFromPhaseSplit = Settings.Instance.ExcludeDragonsFromPhaseSplit;
            displayTruePerSecondValuesInMeters = Settings.Instance.DisplayTruePerSecondValuesInMeters;
            allowGamepadNavigationInputInZDPS = Settings.Instance.AllowGamepadNavigationInputInZDPS;
            keepPastEncounterInMeterUntilNextDamage = Settings.Instance.KeepPastEncounterInMeterUntilNextDamage;

            useDatabaseForEncounterHistory = Settings.Instance.UseDatabaseForEncounterHistory;
            databaseRetentionPolicyDays = Settings.Instance.DatabaseRetentionPolicyDays;
            limitEncounterBuffTrackingWithoutDatabase = Settings.Instance.LimitEncounterBuffTrackingWithoutDatabase;
            allowEncounterSavingPausingInOpenWorld = Settings.Instance.AllowEncounterSavingPausingInOpenWorld;

            meterSettingsTankingShowDeaths = Settings.Instance.MeterSettingsTankingShowDeaths;
            meterSettingsNpcTakenShowHpData = Settings.Instance.MeterSettingsNpcTakenShowHpData;
            meterSettingsNpcTakenHideMaxHp = Settings.Instance.MeterSettingsNpcTakenHideMaxHp;
            meterSettingsNpcTakenUseHpMeter = Settings.Instance.MeterSettingsNpcTakenUseHpMeter;

            // Tab visibility settings
            showHealingTab = Settings.Instance.ShowHealingTab;
            showTankingTab = Settings.Instance.ShowTankingTab;
            showNpcTakenTab = Settings.Instance.ShowNpcTakenTab;
            mergeDpsAndHealTabs = Settings.Instance.MergeDpsAndHealTabs;
            disableTopMostWhenNoEntities = Settings.Instance.DisableTopMostWhenNoEntities;

            GameCapturePreference = Settings.Instance.GameCapturePreference;
            gameCaptureCustomExeName = Settings.Instance.GameCaptureCustomExeName;

            playNotificationSoundOnMatchmake = Settings.Instance.PlayNotificationSoundOnMatchmake;
            matchmakeNotificationSoundPath = Settings.Instance.MatchmakeNotificationSoundPath;
            loopNotificationSoundOnMatchmake = Settings.Instance.LoopNotificationSoundOnMatchmake;
            matchmakeNotificationVolume = Settings.Instance.MatchmakeNotificationVolume;

            playNotificationSoundOnReadyCheck = Settings.Instance.PlayNotificationSoundOnReadyCheck;
            readyCheckNotificationSoundPath = Settings.Instance.ReadyCheckNotificationSoundPath;
            loopNotificationSoundOnReadyCheck = Settings.Instance.LoopNotificationSoundOnReadyCheck;
            readyCheckNotificationVolume = Settings.Instance.ReadyCheckNotificationVolume;

            saveEncounterReportToFile = Settings.Instance.SaveEncounterReportToFile;
            reportFileRetentionPolicyDays = Settings.Instance.ReportFileRetentionPolicyDays;
            minimumPlayerCountToCreateReport = Settings.Instance.MinimumPlayerCountToCreateReport;
            alwaysCreateReportAtDungeonEnd = Settings.Instance.AlwaysCreateReportAtDungeonEnd;
            webhookReportsEnabled = Settings.Instance.WebhookReportsEnabled;
            webhookReportsMode = Settings.Instance.WebhookReportsMode;
            webhookReportsDeduplicationServerUrl = Settings.Instance.WebhookReportsDeduplicationServerHost;
            webhookReportsDiscordUrl = Settings.Instance.WebhookReportsDiscordUrl;
            webhookReportsCustomUrl = Settings.Instance.WebhookReportsCustomUrl;

            checkForZDPSUpdatesOnStartup = Settings.Instance.CheckForZDPSUpdatesOnStartup;
            latestZDPSVersionCheckURL = Settings.Instance.LatestZDPSVersionCheckURL;

            windowSettings = (WindowSettings)Settings.Instance.WindowSettings.Clone();

            logToFile = Settings.Instance.LogToFile;

            lowPerformanceMode = Settings.Instance.LowPerformanceMode;

            theme = Settings.Instance.Theme;
            locale = Settings.Instance.Locale;

            // External
            externalBPTimerEnabled = Settings.Instance.External.BPTimerSettings.ExternalBPTimerEnabled;
            externalBPTimerIncludeCharacterId = Settings.Instance.External.BPTimerSettings.ExternalBPTimerIncludeCharacterId;
            externalBPTimerFieldBossHpReportsEnabled = Settings.Instance.External.BPTimerSettings.ExternalBPTimerFieldBossHpReportsEnabled;
        }

        private static void Save(MainWindow mainWindow)
        {
            var io = ImGui.GetIO();
            if (SelectedNetworkDeviceIdx != PreviousSelectedNetworkDeviceIdx || GameCapturePreference != Settings.Instance.GameCapturePreference)
            {
                PreviousSelectedNetworkDeviceIdx = SelectedNetworkDeviceIdx;

                MessageManager.StopCapturing();

                Settings.Instance.NetCaptureDeviceName = NetworkDevices[SelectedNetworkDeviceIdx].Name;
                MessageManager.NetCaptureDeviceName = NetworkDevices[SelectedNetworkDeviceIdx].Name;

                Settings.Instance.GameCapturePreference = GameCapturePreference;
                Settings.Instance.GameCaptureCustomExeName = gameCaptureCustomExeName;

                MessageManager.InitializeCapturing();
            }
            if (allowGamepadNavigationInputInZDPS)
            {
                io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            }
            else
            {
                io.ConfigFlags &= ~ImGuiConfigFlags.NavEnableGamepad;
            }

            Settings.Instance.AllowEncounterSavingPausingInOpenWorld = allowEncounterSavingPausingInOpenWorld;
            if (!allowEncounterSavingPausingInOpenWorld)
            {
                AppState.IsEncounterSavingPaused = false;
            }

            Settings.Instance.NormalizeMeterContributions = normalizeMeterContributions;
            Settings.Instance.UseShortWidthNumberFormatting = useShortWidthNumberFormatting;
            Settings.Instance.ShowClassIconsInMeters = showClassIconsInMeters;
            Settings.Instance.ColorClassIconsByRole = colorClassIconsByRole;
            Settings.Instance.ShowSkillIconsInDetails = showSkillIconsInDetails;
            Settings.Instance.OnlyShowDamageContributorsInMeters = onlyShowDamageContributorsInMeters;
            Settings.Instance.OnlyShowPartyMembersInMeters = onlyShowPartyMembersInMeters;
            Settings.Instance.ShowAbilityScoreInMeters = showAbilityScoreInMeters;
            Settings.Instance.ShowSeasonStrengthInMeters = showSeasonStrengthInMeters;
            Settings.Instance.ShowSubProfessionNameInMeters = showSubProfessionNameInMeters;
            Settings.Instance.UseAutomaticWipeDetection = useAutomaticWipeDetection;
            Settings.Instance.SkipTeleportStateCheckInAutomaticWipeDetection = skipTeleportStateCheckInAutomaticWipeDetection;
            Settings.Instance.DisableWipeRecalculationOverwriting = disableWipeRecalculationOverwriting;
            Settings.Instance.SplitEncountersOnNewPhases = splitEncountersOnNewPhases;
            Settings.Instance.ExcludeDragonsFromPhaseSplit = excludeDragonsFromPhaseSplit;
            Settings.Instance.DisplayTruePerSecondValuesInMeters = displayTruePerSecondValuesInMeters;
            Settings.Instance.AllowGamepadNavigationInputInZDPS = allowGamepadNavigationInputInZDPS;
            Settings.Instance.KeepPastEncounterInMeterUntilNextDamage = keepPastEncounterInMeterUntilNextDamage;

            Settings.Instance.UseDatabaseForEncounterHistory = useDatabaseForEncounterHistory;
            Settings.Instance.DatabaseRetentionPolicyDays = databaseRetentionPolicyDays;
            Settings.Instance.LimitEncounterBuffTrackingWithoutDatabase = limitEncounterBuffTrackingWithoutDatabase;

            Settings.Instance.MeterSettingsTankingShowDeaths = meterSettingsTankingShowDeaths;
            Settings.Instance.MeterSettingsNpcTakenShowHpData = meterSettingsNpcTakenShowHpData;
            Settings.Instance.MeterSettingsNpcTakenHideMaxHp = meterSettingsNpcTakenHideMaxHp;
            Settings.Instance.MeterSettingsNpcTakenUseHpMeter = meterSettingsNpcTakenUseHpMeter;

            // Tab visibility settings
            Settings.Instance.ShowHealingTab = showHealingTab;
            Settings.Instance.ShowTankingTab = showTankingTab;
            Settings.Instance.ShowNpcTakenTab = showNpcTakenTab;
            Settings.Instance.MergeDpsAndHealTabs = mergeDpsAndHealTabs;
            Settings.Instance.DisableTopMostWhenNoEntities = disableTopMostWhenNoEntities;

            Settings.Instance.PlayNotificationSoundOnMatchmake = playNotificationSoundOnMatchmake;
            Settings.Instance.MatchmakeNotificationSoundPath = matchmakeNotificationSoundPath;
            Settings.Instance.LoopNotificationSoundOnMatchmake = loopNotificationSoundOnMatchmake;
            Settings.Instance.MatchmakeNotificationVolume = matchmakeNotificationVolume;

            Settings.Instance.PlayNotificationSoundOnReadyCheck = playNotificationSoundOnReadyCheck;
            Settings.Instance.ReadyCheckNotificationSoundPath = readyCheckNotificationSoundPath;
            Settings.Instance.LoopNotificationSoundOnReadyCheck = loopNotificationSoundOnReadyCheck;
            Settings.Instance.ReadyCheckNotificationVolume = readyCheckNotificationVolume;

            Settings.Instance.SaveEncounterReportToFile = saveEncounterReportToFile;
            Settings.Instance.ReportFileRetentionPolicyDays = reportFileRetentionPolicyDays;
            Settings.Instance.MinimumPlayerCountToCreateReport = minimumPlayerCountToCreateReport;
            Settings.Instance.AlwaysCreateReportAtDungeonEnd = alwaysCreateReportAtDungeonEnd;
            Settings.Instance.WebhookReportsEnabled = webhookReportsEnabled;
            Settings.Instance.WebhookReportsMode = webhookReportsMode;
            Settings.Instance.WebhookReportsDeduplicationServerHost = webhookReportsDeduplicationServerUrl;
            Settings.Instance.WebhookReportsDiscordUrl = webhookReportsDiscordUrl;
            Settings.Instance.WebhookReportsCustomUrl = webhookReportsCustomUrl;

            Settings.Instance.CheckForZDPSUpdatesOnStartup = checkForZDPSUpdatesOnStartup;
            Settings.Instance.LatestZDPSVersionCheckURL = latestZDPSVersionCheckURL;

            Settings.Instance.WindowSettings = (WindowSettings)windowSettings.Clone();

            Settings.Instance.LogToFile = logToFile;

            Settings.Instance.LowPerformanceMode = lowPerformanceMode;

            Settings.Instance.Theme = theme;
            Settings.Instance.Locale = locale;

            // External
            Settings.Instance.External.BPTimerSettings.ExternalBPTimerEnabled = externalBPTimerEnabled;
            Settings.Instance.External.BPTimerSettings.ExternalBPTimerIncludeCharacterId = externalBPTimerIncludeCharacterId;
            Settings.Instance.External.BPTimerSettings.ExternalBPTimerFieldBossHpReportsEnabled = externalBPTimerFieldBossHpReportsEnabled;

            RegisterAllHotkeys(mainWindow);

            DB.Init();

            // Write out the new settings to file now that they've been applied
            Settings.Save();

            // Reset meters to visually rebuild as if freshly launched (recalculates size naturally)
            mainWindow?.ResetMeters();

            // Apply the new theme immediately
            Theme.ApplyTheme(Settings.Instance.Theme);

            if (externalBPTimerEnabled && externalBPTimerFieldBossHpReportsEnabled)
            {
                // Attempt to update our supported mob list with data from the BPTimer server
                Managers.External.BPTimerManager.FetchSupportedMobList();
            }
        }

        static void ShowRestartRequiredNotice(bool showCondition, string settingName)
        {
            if (showCondition)
            {
                ImGui.PushStyleColor(ImGuiCol.ChildBg, Theme.GetColor(ThemeColor.WarningText, 0.3f));
                ImGui.BeginChild($"##RestartRequiredNotice_{settingName}", new Vector2(0, 0), ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.Borders);
                ImGui.PushFont(HelperMethods.Fonts["Segoe-Bold"], ImGui.GetFontSize());
                ImGui.TextUnformatted("Important Note:");
                ImGui.PopFont();
                ImGui.TextWrapped($"Changing the [{settingName}] setting requires restarting ZDPS to take effect.");
                ImGui.EndChild();
                ImGui.PopStyleColor();
            }
        }

        static void ShowGenericImportantNotice(bool showCondition, string uniqueName, string text)
        {
            if (showCondition)
            {
                ImGui.PushStyleColor(ImGuiCol.ChildBg, Theme.GetColor(ThemeColor.WarningText, 0.3f));
                ImGui.BeginChild($"##GenericImportantNotice_{uniqueName}", new Vector2(0, 0), ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.Borders);
                ImGui.PushFont(HelperMethods.Fonts["Segoe-Bold"], ImGui.GetFontSize());
                ImGui.TextUnformatted("Important Note:");
                ImGui.PopFont();
                ImGui.TextWrapped($"{text}");
                ImGui.EndChild();
                ImGui.PopStyleColor();
            }
        }

        static void LoadHotkeys()
        {
            EncounterResetKey = Settings.Instance.HotkeysEncounterReset;
            if (EncounterResetKey == 0)
            {
                EncounterResetKeyName = "[UNBOUND]";
            }
            else
            {
                EncounterResetKeyName = ImGui.GetKeyNameS(HotKeyManager.VirtualKeyToImGuiKey((int)EncounterResetKey));
            }

            PinnedWindowClickthroughKey = Settings.Instance.HotkeysPinnedWindowClickthrough;
            if (PinnedWindowClickthroughKey == 0)
            {
                PinnedWindowClickthroughKeyName = "[UNBOUND]";
            }
            else
            {
                PinnedWindowClickthroughKeyName = ImGui.GetKeyNameS(HotKeyManager.VirtualKeyToImGuiKey((int)PinnedWindowClickthroughKey));
            }
        }

        static void RegisterAllHotkeys(MainWindow mainWindow)
        {
            if (EncounterResetKey != 0)// && EncounterResetKey != Settings.Instance.HotkeysEncounterReset)
            {
                HotKeyManager.RegisterKey("EncounterReset", mainWindow.CreateNewEncounter, EncounterResetKey);
            }
            Settings.Instance.HotkeysEncounterReset = EncounterResetKey;

            if (PinnedWindowClickthroughKey != 0)
            {
                HotKeyManager.RegisterKey("PinnedWindowClickthrough", mainWindow.ToggleMouseClickthrough, PinnedWindowClickthroughKey);
            }
            Settings.Instance.HotkeysPinnedWindowClickthrough = PinnedWindowClickthroughKey;
        }

        public static void RebindKeyButton(string bindingName, ref uint bindingVariable, ref string bindingVariableName, ref bool bindingState)
        {
            ImGui.AlignTextToFramePadding();
            ImGui.Text($"{bindingName}:");

            string bindDisplay = "[UNBOUND]";

            if (bindingState == true)
            {
                for (uint key = (uint)ImGuiKey.NamedKeyBegin; key < (uint)ImGuiKey.NamedKeyEnd; key++)
                {
                    if (ImGui.IsKeyPressed(ImGuiKey.Escape))
                    {
                        bindingState = false;
                    }
                    else if (ImGui.IsKeyPressed((ImGuiKey)key))
                    {
                        ImGuiKey[] blacklistedKeys =
                            [
                            ImGuiKey.ModAlt, ImGuiKey.LeftAlt, ImGuiKey.RightAlt, ImGuiKey.ReservedForModAlt,
                            ImGuiKey.ModCtrl, ImGuiKey.LeftCtrl, ImGuiKey.RightCtrl, ImGuiKey.ReservedForModCtrl,
                            ImGuiKey.ModShift, ImGuiKey.LeftShift, ImGuiKey.RightShift, ImGuiKey.ReservedForModShift,
                            ImGuiKey.ModMask, ImGuiKey.ModSuper, ImGuiKey.LeftSuper, ImGuiKey.RightSuper, ImGuiKey.ReservedForModSuper,
                            ImGuiKey.MouseLeft, ImGuiKey.MouseMiddle, ImGuiKey.MouseRight, ImGuiKey.MouseWheelX, ImGuiKey.MouseWheelY,
                            ImGuiKey.Escape, ImGuiKey.F12
                            ];
                        
                        if (!blacklistedKeys.Contains((ImGuiKey)key))
                        {
                            string keyName = ImGui.GetKeyNameS((ImGuiKey)key);
                            bindingVariable = (uint)HotKeyManager.ImGuiKeyToVirtualKey((ImGuiKey)key);
                            bindingVariableName = keyName;
                            bindingState = false;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(bindingVariableName))
            {
                bindDisplay = bindingVariableName;
            }
            ImGui.SameLine();
            bool isInBindingState = bindingState;

            if (isInBindingState)
            {
                ImGui.PushStyleColor(ImGuiCol.Button, ImGui.GetStyle().Colors[(int)ImGuiCol.ButtonHovered]);
            }
            if (ImGui.Button($"{bindDisplay}##BindBtn_{bindingName}", new Vector2(120, 0)))
            {
                bindingState = true;
            }
            if (isInBindingState)
            {
                ImGui.PopStyleColor();
            }
            ImGui.SameLine();
            ImGui.BeginDisabled(bindingVariable == 0);
            if (ImGui.Button($"X##ClearBindingBtn_{bindingName}"))
            {
                bindingVariable = 0;
                bindingVariableName = "";
                bindingState = false;
            }
            ImGui.EndDisabled();
            ImGui.SetItemTooltip("Clear Keybinding.");
        }
    }
}
