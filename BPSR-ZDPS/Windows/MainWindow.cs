using BPSR_ZDPSLib;
using BPSR_ZDPS.Meters;
using Hexa.NET.ImGui;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Zproto.WorldNtfCsharp.Types;
using Zproto;
using Newtonsoft.Json;
using BPSR_ZDPS.DataTypes;
using Hexa.NET.GLFW;

namespace BPSR_ZDPS.Windows
{
    public class MainWindow
    {
        Vector2 MainMenuBarSize;

        static bool RunOnce = true;
        static int RunOnceDelayed = 0;
        static int SettingsRunOnceDelayedPerOpen = 0;

        static int SelectedTabIndex = 0;

        static bool HasPromptedUpdateWindow = false;
        static bool HasPromptedOneTimeEnableUpdateChecks = false;

        static bool ProcessingDbWork = false;
        static bool ResumeFromDbWork = false;

        List<MeterBase> Meters = new();
        public EntityInspector entityInspector = new();
        static bool IsPinned = false;
        static int LastPinnedOpacity = 100;
        public Vector2 WindowPosition;
        public Vector2 NextWindowPosition = new();
        public Vector2 DefaultWindowSize = new Vector2(550, 600);
        public Vector2 WindowSize;
        public Vector2 NextWindowSize = new();

        // Cache for width calculation
        private string _cachedWidthHash = "";
        private float _cachedWidth = 0.0f;

        static ImGuiWindowClassPtr ContextMenuClass = ImGui.ImGuiWindowClass();

        public void Draw()
        {
            DrawContent();

            UpdateCheckPromptWindow.Draw(this);
            UpdateAvailableWindow.Draw(this);
            DatabaseMigrationWindow.Draw(this);
            SettingsWindow.Draw(this);
            EncounterHistoryWindow.Draw(this);
            entityInspector.Draw(this);
            NetDebug.Draw();
            DebugDungeonTracker.Draw(this);
            RaidManagerCooldownsWindow.Draw(this);
            DatabaseManagerWindow.Draw(this);
            SpawnTrackerWindow.Draw(this);
            ModuleSolver.Draw();
            EntityCacheViewerWindow.Draw(this);
            RaidManagerRaidWarningWindow.Draw(this);
            RaidManagerCountdownWindow.Draw(this);
            RaidManagerThreatWindow.Draw(this);
            ChatWindow.Draw(this);
        }

        static bool p_open = true;
        public void DrawContent()
        {
            var io = ImGui.GetIO();
            var main_viewport = ImGui.GetMainViewport();

            //ImGui.SetNextWindowPos(new Vector2(main_viewport.WorkPos.X + 200, main_viewport.WorkPos.Y + 120), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(DefaultWindowSize, ImGuiCond.FirstUseEver);

            var windowSettings = Settings.Instance.WindowSettings.MainWindow;

            // Calculate window width based on actual content
            float scale = windowSettings.MeterBarScale;
            float calculatedWidth = CalculateRequiredWidth(scale);
            float minWidth = (!Settings.Instance.AllowEncounterSavingPausingInOpenWorld ? 375.0f : 400.0f) * scale;

            // Use calculated width, or saved width if within range
            float targetWidth = calculatedWidth;
            if (windowSettings.WindowSize.X > 0)
            {
                targetWidth = Math.Max(minWidth, Math.Min(calculatedWidth, windowSettings.WindowSize.X));
            }
            targetWidth = Math.Max(minWidth, targetWidth);

            ImGui.SetNextWindowSizeConstraints(
                new Vector2(targetWidth, 0),
                new Vector2(targetWidth, 800.0f)
            );

            if (windowSettings.WindowPosition != new Vector2())
            {
                ImGui.SetNextWindowPos(windowSettings.WindowPosition, ImGuiCond.FirstUseEver);
            }

            if (windowSettings.WindowSize != new Vector2())
            {
                ImGui.SetNextWindowSize(windowSettings.WindowSize, ImGuiCond.FirstUseEver);
            }

            if (NextWindowPosition != new Vector2())
            {
                ImGui.SetNextWindowPos(NextWindowPosition * new Vector2(0.5f, 0.5f), ImGuiCond.Always, new Vector2(0.5f, 0.5f));
                NextWindowPosition = new Vector2();
            }

            if (NextWindowSize != new Vector2())
            {
                ImGui.SetNextWindowSize(NextWindowSize, ImGuiCond.Always);
                NextWindowSize = new Vector2();
            }

            ImGuiWindowFlags exWindowFlags = ImGuiWindowFlags.None;
            if (AppState.MousePassthrough && windowSettings.TopMost)
            {
                exWindowFlags |= ImGuiWindowFlags.NoInputs;
            }

            ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.AlwaysAutoResize | exWindowFlags;
            
            if (!p_open)
            {
                Hexa.NET.GLFW.GLFW.SetWindowShouldClose(HelperMethods.GLFWwindow, 1);
                return;
            }

            if (!ImGui.Begin("ZDPS", ref p_open, window_flags))
            {
                ImGui.End();
                return;
            }

            WindowPosition = ImGui.GetWindowPos();
            WindowSize = ImGui.GetWindowSize();

            DrawMenuBar();

            if (RunOnce)
            {
                RunOnce = false;

                // This includes string files and caches
                AppState.LoadDataTables();

                Settings.Instance.Apply();

                ModuleSolver.Init();

                if (string.IsNullOrEmpty(MessageManager.NetCaptureDeviceName))
                {
                    var bestDefaultDevice = MessageManager.TryFindBestNetworkDevice();
                    if (bestDefaultDevice != null)
                    {
                        MessageManager.NetCaptureDeviceName = bestDefaultDevice.Name;
                        Settings.Instance.NetCaptureDeviceName = bestDefaultDevice.Name;
                    }
                }

                if (DB.CheckIfMigrationsNeeded())
                {
                    ProcessingDbWork = true;
                }
                else
                {
                    if (MessageManager.NetCaptureDeviceName != "")
                    {
                        MessageManager.InitializeCapturing();
                    }
                }

                Meters.Add(new DpsMeter());
                Meters.Add(new HealingMeter());
                Meters.Add(new TankingMeter());
                Meters.Add(new TakenMeter());

                ContextMenuClass.ClassId = ImGuiP.ImHashStr("MainWindowContextMenuClass");
                ContextMenuClass.ViewportFlagsOverrideSet = ImGuiViewportFlags.TopMost;
            }
            if (RunOnceDelayed == 0)
            {
                RunOnceDelayed++;
            }
            else if (RunOnceDelayed == 1)
            {
                RunOnceDelayed++;
                unsafe
                {
                    HelperMethods.MainWindowPlatformHandleRaw = (IntPtr)ImGui.GetWindowViewport().PlatformHandleRaw;
                }

                HotKeyManager.SetWndProc();
                //HotKeyManager.SetHookProc();

                Settings.Instance.ApplyHotKeys(this);

                Utils.SetCurrentWindowIcon();
                Utils.BringWindowToFront();

                if (windowSettings.TopMost && !IsPinned)
                {
                    IsPinned = true;
                    Utils.SetWindowTopmost();
                    Utils.SetWindowOpacity(windowSettings.Opacity * 0.01f);
                    LastPinnedOpacity = windowSettings.Opacity;
                }

                if (Settings.Instance.External.BPTimerSettings.ExternalBPTimerEnabled)
                {
                    if (Settings.Instance.External.BPTimerSettings.ExternalBPTimerFieldBossHpReportsEnabled)
                    {
                        // Attempt to update our supported mob list with data from the BPTimer server
                        Managers.External.BPTimerManager.FetchSupportedMobList();
                    }
                }

                if (ProcessingDbWork)
                {
                    DatabaseMigrationWindow.Open();
                    Task.Run(() =>
                    {
                        DB.CheckAndRunMigrations();
                    });
                }
                else
                {
                    if (Settings.Instance.CheckForZDPSUpdatesOnStartup)
                    {
                        Web.WebManager.CheckForZDPSUpdates();
                    }

                    if (!Settings.Instance.HasPromptedEnableUpdateChecks && !HasPromptedOneTimeEnableUpdateChecks && !Settings.Instance.CheckForZDPSUpdatesOnStartup)
                    {
                        HasPromptedOneTimeEnableUpdateChecks = true;
                        UpdateCheckPromptWindow.Open();
                    }
                }
            }
            else if (RunOnceDelayed >= 2)
            {
                if (windowSettings.TopMost && LastPinnedOpacity != windowSettings.Opacity)
                {
                    Utils.SetWindowOpacity(windowSettings.Opacity * 0.01f);
                    LastPinnedOpacity = windowSettings.Opacity;
                }
            }

            if (ResumeFromDbWork)
            {
                ProcessingDbWork = false;
                ResumeFromDbWork = false;
                Utils.BringWindowToFront();
                if (MessageManager.NetCaptureDeviceName != "")
                {
                    MessageManager.InitializeCapturing();
                }

                if (Settings.Instance.CheckForZDPSUpdatesOnStartup)
                {
                    Web.WebManager.CheckForZDPSUpdates();
                }
            }

            if (AppState.IsUpdateAvailable && !HasPromptedUpdateWindow)
            {
                HasPromptedUpdateWindow = true;
                UpdateAvailableWindow.Open();
            }

            // Build visible meters list based on settings
            List<MeterBase> visibleMeters = new();
            visibleMeters.Add(Meters[0]); // DPS is always shown

            bool isMergeMode = Settings.Instance.MergeDpsAndHealTabs && Settings.Instance.ShowHealingTab;

            if (AppState.IsEncounterSavingPaused)
            {
                ImGui.PushStyleColor(ImGuiCol.ChildBg, Colors.DarkRed_Transparent);
                ImGui.BeginChild("##EncounterSavingPausedChild", ImGuiChildFlags.AutoResizeY);
                ImGui.TextAligned(0.5f, -1, "Encounter Saving Is Paused");
                ImGui.TextAligned(0.5f, -1, "Automatically resumes if you change maps.");
                ImGui.SetCursorPosX((ImGui.GetContentRegionAvail().X - 200) * 0.5f);
                ImGui.PushStyleColor(ImGuiCol.Button, Colors.DarkGreen);
                if (ImGui.Button("RESUME SAVING NOW##ResumeEncounterSavingBtn", new Vector2(200, 0)))
                {
                    AppState.IsEncounterSavingPaused = false;
                }
                ImGui.PopStyleColor();
                ImGui.EndChild();
                ImGui.PopStyleColor();
            }

            if (isMergeMode)
            {
                // In merge mode, just render sections directly without wrapper
                // The main window's AlwaysAutoResize will handle the sizing
                DrawMergedDpsHealingView();
            }
            else
            {
                // Normal tab mode
                if (Settings.Instance.ShowHealingTab)
                    visibleMeters.Add(Meters[1]); // Healing
                if (Settings.Instance.ShowTankingTab)
                    visibleMeters.Add(Meters[2]); // Tanking
                if (Settings.Instance.ShowNpcTakenTab)
                    visibleMeters.Add(Meters[3]); // NPC Taken

                // Only show tabs if more than one meter is visible
                if (visibleMeters.Count > 1)
                {
                    ImGuiTableFlags table_flags = ImGuiTableFlags.SizingStretchSame;
                    if (ImGui.BeginTable("##MetersTable", visibleMeters.Count, table_flags))
                    {
                        ImGui.TableSetupColumn("##TabBtn", ImGuiTableColumnFlags.WidthStretch |
                                              ImGuiTableColumnFlags.DefaultHide | ImGuiTableColumnFlags.NoResize, 1f, 0);

                        for (int i = 0; i < visibleMeters.Count; i++)
                        {
                            ImGui.TableNextColumn();

                            bool isSelected = (SelectedTabIndex == i);

                            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 5);

                            if (isSelected)
                            {
                                ImGui.PushStyleColor(ImGuiCol.Button, Colors.DimGray);
                            }

                            if (ImGui.Button($"{visibleMeters[i].Name}##TabBtn_{i}", new Vector2(-1, 0)))
                            {
                                SelectedTabIndex = i;
                            }

                            if (isSelected)
                            {
                                ImGui.PopStyleColor();
                            }

                            ImGui.PopStyleVar();
                        }

                        ImGui.EndTable();
                    }

                    // Ensure SelectedTabIndex is valid
                    if (SelectedTabIndex >= visibleMeters.Count)
                    {
                        SelectedTabIndex = 0;
                    }
                }
                else
                {
                    // Only one tab visible, reset to 0
                    SelectedTabIndex = 0;
                }

                // Meter content - draw directly, the meter will set its own ListBox height
                if (SelectedTabIndex > -1 && SelectedTabIndex < visibleMeters.Count)
                {
                    // Map visible index back to actual meter index
                    int actualIndex = Meters.IndexOf(visibleMeters[SelectedTabIndex]);
                    if (actualIndex >= 0)
                    {
                        Meters[actualIndex].Draw(this);
                    }
                }
            }

            ImGui.End();
        }

        static float settingsWidth = 0.0f;
        void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                var windowSettings = Settings.Instance.WindowSettings.MainWindow;

                MainMenuBarSize = ImGui.GetWindowSize();

                // Area info, time, and player stats at top left
                string duration = "00:00:00";
                if (EncounterManager.Current?.GetDuration().TotalSeconds > 0)
                {
                    duration = EncounterManager.Current.GetDuration().ToString("hh\\:mm\\:ss");
                }

                // Display: [Rank] Duration - AreaName - PlayerTotal (PlayerPerSecond)
                ImGui.Text($"[{AppState.PlayerMeterPlacement}] {duration}");

                if (!string.IsNullOrEmpty(EncounterManager.Current.SceneName))
                {
                    ImGui.SameLine();
                    string subName = "";
                    if (!string.IsNullOrEmpty(EncounterManager.Current.SceneSubName))
                    {
                        subName = $" ({EncounterManager.Current.SceneSubName})";
                    }
                    ImGui.TextUnformatted($"- {EncounterManager.Current.SceneName}{subName}");
                }

                // Player stats (Total DPS/HPS)
                ImGui.SameLine();
                string currentValuePerSecond = $"{Utils.NumberToShorthand(AppState.PlayerTotalMeterValue)} ({Utils.NumberToShorthand(AppState.PlayerMeterValuePerSecond)})";
                ImGui.Text(currentValuePerSecond);

                // Benchmark indicator
                if (AppState.IsBenchmarkMode)
                {
                    ImGui.SameLine();
                    ImGui.TextUnformatted($"[BENCHMARK ({AppState.BenchmarkTime}s)]");
                }

                if (Settings.Instance.AllowEncounterSavingPausingInOpenWorld && BattleStateMachine.DungeonStateHistory.Count > 0 && BattleStateMachine.DungeonStateHistory.LastOrDefault().Key == EDungeonState.DungeonStateNull)
                {
                    ImGui.BeginDisabled(AppState.IsBenchmarkMode);

                    ImGui.SetCursorPosX(MainMenuBarSize.X - (settingsWidth * 5));
                    ImGui.PushFont(HelperMethods.Fonts["FASIcons"], ImGui.GetFontSize());
                    ImGui.PushStyleColor(ImGuiCol.Text, (AppState.IsEncounterSavingPaused ? Colors.Red_Transparent : Colors.White));
                    if (ImGui.MenuItem($"{FASIcons.Pause}##PauseEncounterSavingBtn"))
                    {
                        AppState.IsEncounterSavingPaused = !AppState.IsEncounterSavingPaused;
                    }
                    ImGui.PopStyleColor();
                    ImGui.PopFont();
                    ImGui.SetItemTooltip("Pause Encounter Saving While In Open World.");

                    ImGui.EndDisabled();
                }

                ImGui.SetCursorPosX(MainMenuBarSize.X - (settingsWidth * 4));
                ImGui.PushFont(HelperMethods.Fonts["FASIcons"], ImGui.GetFontSize());
                if (ImGui.MenuItem($"{FASIcons.WindowMinimize}##MinimizeBtn"))
                {
                    Utils.MinimizeWindow();
                }
                ImGui.PopFont();

                ImGui.SetCursorPosX(MainMenuBarSize.X - (settingsWidth * 3));
                ImGui.PushFont(HelperMethods.Fonts["FASIcons"], ImGui.GetFontSize());
                ImGui.PushStyleColor(ImGuiCol.Text, new Vector4(1.0f, AppState.MousePassthrough ? 0.0f : 1.0f, AppState.MousePassthrough ? 0.0f : 1.0f, windowSettings.TopMost ? 1.0f : 0.5f));
                if (ImGui.MenuItem($"{FASIcons.Thumbtack}##TopMostBtn"))
                {
                    if (!windowSettings.TopMost)
                    {
                        Utils.SetWindowTopmost();
                        Utils.SetWindowOpacity(windowSettings.Opacity * 0.01f);
                        LastPinnedOpacity = windowSettings.Opacity;
                        windowSettings.TopMost = true;
                        IsPinned = true;
                    }
                    else
                    {
                        Utils.UnsetWindowTopmost();
                        Utils.SetWindowOpacity(1.0f);
                        windowSettings.TopMost = false;
                        IsPinned = false;
                    }
                }
                ImGui.PopStyleColor();
                ImGui.PopFont();
                ImGui.SetItemTooltip("Pin Window As Top Most");

                // Create new Encounter button
                ImGui.BeginDisabled(AppState.IsEncounterSavingPaused);

                ImGui.SetCursorPosX(MainMenuBarSize.X - (settingsWidth * 2));
                ImGui.PushFont(HelperMethods.Fonts["FASIcons"], ImGui.GetFontSize());
                if (ImGui.MenuItem($"{FASIcons.Rotate}##StartNewEncounterBtn"))
                {
                    CreateNewEncounter();
                }
                ImGui.PopFont();
                ImGui.SetItemTooltip("Start New Encounter");

                ImGui.EndDisabled();

                ImGui.SetCursorPosX(MainMenuBarSize.X - settingsWidth);
                ImGui.PushFont(HelperMethods.Fonts["FASIcons"], ImGui.GetFontSize());
                if (ImGui.BeginMenu($"{FASIcons.Gear}##OptionsMenu"))
                {
                    if (SettingsRunOnceDelayedPerOpen == 0)
                    {
                        SettingsRunOnceDelayedPerOpen++;
                    }
                    else if (SettingsRunOnceDelayedPerOpen == 2)
                    {
                        SettingsRunOnceDelayedPerOpen++;

                        if (windowSettings.TopMost)
                        {
                            Utils.SetWindowTopmost();
                        }
                        else
                        {
                            Utils.UnsetWindowTopmost();
                        }
                    }
                    else
                    {
                        SettingsRunOnceDelayedPerOpen++;
                    }

                    ImGui.PopFont();

                    if (ImGui.MenuItem("Encounter History"))
                    {
                        EncounterHistoryWindow.Open();
                    }

                    if (ImGui.MenuItem("Database Manager"))
                    {
                        DatabaseManagerWindow.Open();
                    }
                    ImGui.SetItemTooltip("Manage the ZDatabase.db contents.");

                    if (windowSettings.TopMost)
                    {
                        ImGui.SetNextWindowClass(ContextMenuClass);
                    }
                    if (ImGui.BeginMenu("Raid Manager"))
                    {
                        if (ImGui.MenuItem("Cooldown Priority Tracker"))
                        {
                            RaidManagerCooldownsWindow.Open();
                        }

                        if (ImGui.MenuItem("Raid Warnings"))
                        {
                            RaidManagerRaidWarningWindow.Open();
                        }

                        if (ImGui.MenuItem("Countdowns"))
                        {
                            RaidManagerCountdownWindow.Open();
                        }

                        if (ImGui.MenuItem("Threat Meter"))
                        {
                            RaidManagerThreatWindow.Open();
                        }

                        ImGui.EndMenu();
                    }

                    if (windowSettings.TopMost)
                    {
                        ImGui.SetNextWindowClass(ContextMenuClass);
                    }
                    if (ImGui.BeginMenu("Benchmark", !AppState.IsEncounterSavingPaused))
                    {
                        ImGui.TextUnformatted("Enter how many seconds you want to run a Benchmark session for:");
                        ImGui.SetNextItemWidth(-1);
                        int benchmarkTime = AppState.BenchmarkTime;
                        ImGui.BeginDisabled(AppState.IsBenchmarkMode);
                        if (ImGui.InputInt("##BenchmarkTimeInput", ref benchmarkTime, 1, 10))
                        {
                            if (benchmarkTime < 0)
                            {
                                benchmarkTime = 0;
                            }
                            else if (benchmarkTime > 1200)
                            {
                                // Limit to 1200 Seconds (20 minutes)
                                benchmarkTime = 1200;
                            }
                            AppState.BenchmarkTime = benchmarkTime;
                        }

                        bool benchmarkSingleTarget = AppState.BenchmarkSingleTarget;
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text("Only Track First Target Hit: ");
                        ImGui.SameLine();
                        ImGui.Checkbox("##BenchmarkSingleTarget", ref benchmarkSingleTarget);
                        AppState.BenchmarkSingleTarget = benchmarkSingleTarget;

                        ImGui.EndDisabled();
                        
                        ImGui.TextUnformatted("Note: The Benchmark time will start after the next attack.\nOnly data for your character will be processed.");
                        if (AppState.IsBenchmarkMode)
                        {
                            if (ImGui.Button("Stop Benchmark Early", new Vector2(-1, 0)))
                            {
                                AppState.HasBenchmarkBegun = false;
                                AppState.IsBenchmarkMode = false;
                                EncounterManager.StartEncounter(false, EncounterStartReason.BenchmarkEnd);
                            }
                            ImGui.SetItemTooltip("Stops the current Benchmark before the time limit is reached.");
                        }
                        else
                        {
                            ImGui.BeginDisabled(AppState.BenchmarkTime < 5);
                            if (ImGui.Button("Start Benchmark", new Vector2(-1, 0)))
                            {
                                AppState.BenchmarkSingleTargetUUID = 0;
                                AppState.IsBenchmarkMode = true;
                                CreateNewEncounter();
                            }
                            ImGui.EndDisabled();
                            if (AppState.BenchmarkTime < 5)
                            {
                                ImGui.SetItemTooltip("Benchmark Time must be at least 5 seconds.");
                            }
                        }
                        
                        ImGui.EndMenu();
                    }

                    if (windowSettings.TopMost)
                    {
                        ImGui.SetNextWindowClass(ContextMenuClass);
                    }
                    if (ImGui.BeginMenu("Integrations"))
                    {
                        bool isBPTimerEnabled = Settings.Instance.External.BPTimerSettings.ExternalBPTimerEnabled;
                        if (windowSettings.TopMost)
                        {
                            ImGui.SetNextWindowClass(ContextMenuClass);
                        }
                        if (ImGui.BeginMenu("BPTimer", isBPTimerEnabled))
                        {
                            if (windowSettings.TopMost)
                            {
                                ImGui.SetNextWindowClass(ContextMenuClass);
                            }
                            if (ImGui.MenuItem("Spawn Tracker"))
                            {
                                SpawnTrackerWindow.Open();
                            }
                            ImGui.SetItemTooltip("View Field Boss and Magical Creature spawns.\nUses the data from BPTimer.com website.");
                            ImGui.EndMenu();
                        }
                        if (!isBPTimerEnabled)
                        {
                            ImGui.SetItemTooltip("[BPTimer] must be Enabled in the [Settings > Integrations] menu.");
                        }
                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Module Optimizer"))
                    {
                        ModuleSolver.Open();
                    }
                    ImGui.SetItemTooltip("Find the best module combos for your build.");

                    if (ImGui.MenuItem("Chat"))
                    {
                        ChatWindow.Open();
                    }

                    ImGui.Separator();
                    if (ImGui.MenuItem("Settings"))
                    {
                        SettingsWindow.Open();
                    }
                    ImGui.Separator();
                    if (windowSettings.TopMost)
                    {
                        ImGui.SetNextWindowClass(ContextMenuClass);
                    }
                    if (ImGui.BeginMenu("Debug"))
                    {
                        if (ImGui.MenuItem("Net Debug"))
                        {
                            NetDebug.Open();
                        }
                        if (ImGui.MenuItem("Dungeon Tracker"))
                        {
                            DebugDungeonTracker.Open();
                        }
                        if (ImGui.MenuItem("Entity Cache Viewer"))
                        {
                            EntityCacheViewerWindow.Open();
                        }
                        ImGui.EndMenu();
                    }
                    ImGui.Separator();
                    if (ImGui.MenuItem("Exit"))
                    {
                        windowSettings.WindowPosition = WindowPosition;
                        windowSettings.WindowSize = WindowSize;
                        p_open = false;
                    }
                    ImGui.EndMenu();
                }
                else
                {
                    SettingsRunOnceDelayedPerOpen = 0;
                    ImGui.PopFont();
                }

                settingsWidth = ImGui.GetItemRectSize().X;

                ImGui.EndMenuBar();
            }
        }

        void DrawMergedDpsHealingView()
        {
            var currentEncounter = EncounterManager.Current;

            // Get top 20 DPS entities first
            var topDpsEntities = currentEncounter?.Entities.Values
                .Where(x => x.EntityType == Zproto.EEntityType.EntChar && (Settings.Instance.OnlyShowDamageContributorsInMeters ? x.TotalDamage > 0 : true))
                .OrderByDescending(x => x.TotalDamage)
                .Take(20)
                .ToHashSet();

            // Get healer list, filtered to only those in top 20 DPS
            var healerList = currentEncounter?.Entities.Values
                .Where(x => x.EntityType == Zproto.EEntityType.EntChar
                    && x.TotalHealing > 0
                    && (x.ProfessionId == 5 || x.ProfessionId == 13)
                    && topDpsEntities.Contains(x))
                .OrderByDescending(x => x.TotalHealing)
                .ToArray();

            // Check if we have healers
            bool hasHealers = healerList != null && healerList.Length > 0;

            // DPS section - just draw the meter directly
            Meters[0].Draw(this); // DpsMeter is always at index 0

            // Only show healing section if there are healers with healing
            if (hasHealers)
            {
                ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(2, ImGui.GetStyle().FramePadding.Y));

                // Add header for the healing section - use SeparatorText for better styling
                ImGui.SeparatorText(DataTypes.AppStrings.GetLocalized("Header_Healing"));

                // Calculate height for the healing section based on healer count
                float scale = Settings.Instance.WindowSettings.MainWindow.MeterBarScale;
                ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * scale);
                float frameHeight = ImGui.GetFrameHeight();
                ImGui.PopFont();
                float entryHeight = frameHeight;
                float listPadding = ImGui.GetStyle().WindowPadding.Y * 2;
                float itemSpacing = ImGui.GetStyle().ItemSpacing.Y * Math.Max(0, healerList.Length - 1);
                float healingHeight = (healerList.Length * entryHeight) + itemSpacing + listPadding;

                if (ImGui.BeginListBox("##HealingSection", new Vector2(-1, healingHeight)))
                {
                    ImGui.PopStyleVar();

                    ulong topTotalValue = 0;

                    for (int i = 0; i < healerList.Length; i++)
                    {
                    var entity = healerList[i];

                    if (i == 0 && Settings.Instance.NormalizeMeterContributions)
                    {
                        topTotalValue = entity.TotalHealing;
                    }

                    string name = "Unknown";
                    if (!string.IsNullOrEmpty(entity.Name))
                    {
                        name = entity.Name;
                    }
                    else
                    {
                        name = $"[U:{entity.UID}]";
                    }

                    string profession = "Unknown";
                    if (!string.IsNullOrEmpty(entity.SubProfession))
                    {
                        profession = entity.SubProfession;
                    }
                    else if (!string.IsNullOrEmpty(entity.Profession))
                    {
                        profession = entity.Profession;
                    }

                    double contribution = 0.0;
                    double contributionProgressBar = 0.0;
                    if (currentEncounter.TotalHealing != 0)
                    {
                        contribution = Math.Round(((double)entity.TotalHealing / (double)currentEncounter.TotalHealing) * 100, 4);

                        if (Settings.Instance.NormalizeMeterContributions)
                        {
                            contributionProgressBar = Math.Round(((double)entity.TotalHealing / (double)topTotalValue) * 100, 4);
                        }
                        else
                        {
                            contributionProgressBar = contribution;
                        }
                    }

                    string truePerSecond = "";
                    if (Settings.Instance.DisplayTruePerSecondValuesInMeters)
                    {
                        truePerSecond = $"[{Utils.NumberToShorthand(entity.HealingStats.TrueValuePerSecond)}] ";
                    }

                    string totalHealing = Utils.NumberToShorthand(entity.TotalHealing);
                    string totalHps = Utils.NumberToShorthand(entity.HealingStats.ValuePerSecond);
                    string hps_format = $"{totalHealing} {truePerSecond}({totalHps}) {contribution.ToString("F0").PadLeft(3, ' ')}%";
                    var startPoint = ImGui.GetCursorPos();

                    ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * Settings.Instance.WindowSettings.MainWindow.MeterBarScale);

                    ImGui.PushStyleColor(ImGuiCol.PlotHistogram, DataTypes.Professions.ProfessionColors(profession));
                    ImGui.ProgressBar((float)contributionProgressBar / 100.0f, new Vector2(-1, 0), $"##HpsEntryContribution_{i}");
                    ImGui.PopStyleColor();

                    string professionStr = $"-{profession}";
                    if (!Settings.Instance.ShowSubProfessionNameInMeters)
                    {
                        professionStr = "";
                    }

                    string abilityScoreStr = $" ({entity.AbilityScore})";
                    if (!Settings.Instance.ShowAbilityScoreInMeters)
                    {
                        abilityScoreStr = "";
                    }

                    ImGui.SetCursorPos(startPoint);
                    if (MeterBase.SelectableWithHintImage($" {(i + 1).ToString().PadLeft((healerList.Length < 101 ? 2 : 3), '0')}.", $"{name}{professionStr}{abilityScoreStr}##HpsEntry_{i}", hps_format, entity.ProfessionId))
                    {
                        entityInspector = new EntityInspector();
                        entityInspector.LoadEntity(entity, currentEncounter.StartTime);
                        entityInspector.Open();
                    }

                    ImGui.PopFont();
                }
                ImGui.EndListBox();
                }
                else
                {
                    ImGui.PopStyleVar();
                }
            }
        }

        public void CreateNewEncounter()
        {
            if (AppState.IsEncounterSavingPaused)
            {
                Log.Information("Tried to create a new manual Encounter but Encounter Saving is currently Paused.");
                return;
            }

            EncounterManager.StopEncounter();
            Log.Information($"Starting new manual encounter at {DateTime.Now}");
            EncounterManager.StartEncounter(true);
        }

        public void ToggleMouseClickthrough()
        {
            AppState.MousePassthrough = !AppState.MousePassthrough;
        }

        public void SetDbWorkComplete()
        {
            ResumeFromDbWork = true;
        }

        /// <summary>
        /// Invalidates both width and height calculation caches, forcing recalculation on next frame.
        /// Call this when settings change that affect the window size.
        /// </summary>
        public void InvalidateSizeCache()
        {
            _cachedWidthHash = "";
            // Invalidate height cache for all meters
            foreach (var meter in Meters)
            {
                meter.InvalidateHeightCache();
            }
        }

        float CalculateRequiredWidth(float scale)
        {
            float baseMinWidth = (!Settings.Instance.AllowEncounterSavingPausingInOpenWorld ? 375.0f : 400.0f) * scale;

            var currentEncounter = EncounterManager.Current;
            if (currentEncounter?.Entities == null || currentEncounter.Entities.Count == 0)
            {
                return baseMinWidth;
            }

            // Determine which meter(s) will be displayed and calculate for each
            bool isMergeMode = Settings.Instance.MergeDpsAndHealTabs && Settings.Instance.ShowHealingTab;
            bool showHealing = Settings.Instance.ShowHealingTab;
            bool showTanking = Settings.Instance.ShowTankingTab;
            bool showNpcTaken = Settings.Instance.ShowNpcTakenTab;

            // Build list of meter types to check (0=DPS, 1=Healing, 2=Tanking, 3=NPC Taken)
            List<int> meterTypesToCheck = new() { 0 }; // DPS is always shown
            if (isMergeMode)
            {
                meterTypesToCheck.Add(1); // Healing in merged mode
            }
            else if (showHealing)
            {
                meterTypesToCheck.Add(1); // Healing tab
            }
            if (showTanking)
            {
                meterTypesToCheck.Add(2); // Tanking tab
            }
            if (showNpcTaken)
            {
                meterTypesToCheck.Add(3); // NPC Taken tab
            }

            // Create a hash of properties that affect width (exclude damage values that change every frame)
            // Sort by UID to make hash order-independent (position swaps shouldn't recalculate width)
            StringBuilder hashBuilder = new();
            hashBuilder.Append($"merge:{isMergeMode};");
            hashBuilder.Append($"showScore:{Settings.Instance.ShowAbilityScoreInMeters};");
            hashBuilder.Append($"showProf:{Settings.Instance.ShowSubProfessionNameInMeters};");
            hashBuilder.Append($"showIcon:{Settings.Instance.ShowClassIconsInMeters};");
            hashBuilder.Append($"truePS:{Settings.Instance.DisplayTruePerSecondValuesInMeters};");
            hashBuilder.Append($"onlyContrib:{Settings.Instance.OnlyShowDamageContributorsInMeters};");

            // Only include entity properties that affect the displayed width
            // Sort by UID to ensure hash is stable regardless of DPS order changes
            foreach (var entity in currentEncounter.Entities.Values
                .Where(x => x.EntityType == Zproto.EEntityType.EntChar)
                .OrderBy(x => x.UID))
            {
                hashBuilder.Append($"{entity.UID}:{entity.Name}:{entity.AbilityScore}:{entity.SubProfession};");
            }
            string entityHash = hashBuilder.ToString();

            // Return cached width if nothing changed
            if (entityHash == _cachedWidthHash)
            {
                return _cachedWidth;
            }

            // Push font to calculate text size (same font used in SelectableWithHintImage)
            ImGui.PushFont(HelperMethods.Fonts["Cascadia-Mono"], 14.0f * scale);
            ImGui.AlignTextToFramePadding();
            float texSize = ImGui.GetItemRectSize().Y;

            float maxWidth = 0;

            // Calculate width for each meter type and use the maximum
            foreach (int meterType in meterTypesToCheck)
            {
                float meterWidth = CalculateWidthForMeterType(meterType, currentEncounter, scale, texSize);
                maxWidth = Math.Max(maxWidth, meterWidth);
            }

            ImGui.PopFont();

            // Ensure minimum width
            float finalWidth = Math.Max(baseMinWidth, maxWidth);

            Log.Debug("[CalculateRequiredWidth] meterTypes={MeterTypes}, scale={Scale:F2}, maxWidth={MaxWidth:F2}, finalWidth={FinalWidth:F2}",
                string.Join(",", meterTypesToCheck), scale, maxWidth, finalWidth);

            // Update cache
            _cachedWidthHash = entityHash;
            _cachedWidth = finalWidth;

            return finalWidth;
        }

        float CalculateWidthForMeterType(int meterType, Encounter currentEncounter, float scale, float texSize)
        {
            // Determine sort key and filter based on meter type
            Func<Entity, ulong> sortKey = meterType switch
            {
                0 => e => e.TotalDamage,
                1 => e => e.TotalHealing,
                2 => e => e.TotalTakenDamage,
                3 => e => e.TotalTakenDamage,
                _ => e => e.TotalDamage
            };

            Func<Entity, bool> filter = meterType switch
            {
                0 => e => Settings.Instance.OnlyShowDamageContributorsInMeters ? e.TotalDamage > 0 : true,
                1 => e => Settings.Instance.OnlyShowDamageContributorsInMeters ? e.TotalHealing > 0 : true,
                2 => e => true,
                3 => e => Settings.Instance.OnlyShowDamageContributorsInMeters ? e.TotalTakenDamage > 0 : true,
                _ => e => true
            };

            // For NPC Taken (meterType 3), we need to filter for monsters instead of chars
            Zproto.EEntityType entityTypeFilter = meterType == 3
                ? Zproto.EEntityType.EntMonster
                : Zproto.EEntityType.EntChar;

            bool filterHealersOnly = (meterType == 1 && Settings.Instance.MergeDpsAndHealTabs && Settings.Instance.ShowHealingTab);

            var displayedEntities = currentEncounter.Entities.Values
                .Where(x => x.EntityType == entityTypeFilter && filter(x))
                .Where(x => !filterHealersOnly || (x.ProfessionId == 5 || x.ProfessionId == 13))
                .OrderByDescending(sortKey)
                .Take(20)
                .ToList();

            if (displayedEntities.Count == 0)
                return 0.0f;

            float maxNameWidth = 0;
            float maxValueWidth = 0;
            float maxRankWidth = 0;

            for (int i = 0; i < displayedEntities.Count; i++)
            {
                var entity = displayedEntities[i];

                // Rank number
                string number = $" {(i + 1).ToString().PadLeft(displayedEntities.Count < 101 ? 2 : 3, '0')}.";
                Vector2 rankSize = ImGui.CalcTextSize(number);
                maxRankWidth = Math.Max(maxRankWidth, rankSize.X);

                // Name text
                string name = string.IsNullOrEmpty(entity.Name) ? $"[U:{entity.UID}]" : entity.Name;
                string abilityScore = Settings.Instance.ShowAbilityScoreInMeters ? $" ({entity.AbilityScore})" : "";
                string profession = Settings.Instance.ShowSubProfessionNameInMeters && !string.IsNullOrEmpty(entity.SubProfession)
                    ? $"-{entity.SubProfession}" : "";
                string nameText = $"{name}{profession}{abilityScore}";
                Vector2 nameSize = ImGui.CalcTextSize(nameText);
                maxNameWidth = Math.Max(maxNameWidth, nameSize.X);

                // Value text based on meter type
                ulong totalValue = sortKey(entity);
                double valuePerSecond = meterType switch
                {
                    0 => entity.DamageStats.ValuePerSecond,
                    1 => entity.HealingStats.ValuePerSecond,
                    2 => 0.0, // Tanking doesn't show per-second
                    3 => 0.0, // NPC Taken doesn't show per-second
                    _ => 0.0
                };
                double trueValuePerSecond = meterType switch
                {
                    0 => entity.DamageStats.TrueValuePerSecond,
                    1 => entity.HealingStats.TrueValuePerSecond,
                    _ => 0.0
                };

                ulong encounterTotal = meterType switch
                {
                    0 => currentEncounter.TotalDamage,
                    1 => currentEncounter.TotalHealing,
                    2 => 0,
                    3 => 0,
                    _ => 0
                };

                string truePerSecond = (meterType <= 1 && Settings.Instance.DisplayTruePerSecondValuesInMeters)
                    ? $"[{Utils.NumberToShorthand(trueValuePerSecond)}] " : "";
                string totalStr = Utils.NumberToShorthand(totalValue);
                string perSecondStr = valuePerSecond > 0 ? $"({Utils.NumberToShorthand(valuePerSecond)})" : "";

                double contribution = 0.0;
                if (encounterTotal != 0)
                {
                    contribution = Math.Round(((double)totalValue / (double)encounterTotal) * 100, 4);
                }

                string valueText = meterType <= 1
                    ? $"{totalStr} {truePerSecond}{perSecondStr} {contribution.ToString("F0").PadLeft(3, ' ')}%"
                    : totalStr;

                Vector2 valueSize = ImGui.CalcTextSize(valueText);
                maxValueWidth = Math.Max(maxValueWidth, valueSize.X);
            }

            float itemSpacing = ImGui.GetStyle().ItemSpacing.X;
            float windowPadding = ImGui.GetStyle().WindowPadding.X;

            float rankOffset = maxRankWidth;
            if (Settings.Instance.ShowClassIconsInMeters)
            {
                rankOffset += (itemSpacing * 2) + (texSize + 2);
            }
            else
            {
                rankOffset += itemSpacing;
            }

            float totalWidth = rankOffset + maxNameWidth + maxValueWidth + (windowPadding * 2);
            totalWidth += 10.0f * scale;

            return totalWidth;
        }
    }

    public class MainWindowWindowSettings : WindowSettingsBase
    {
        public float MeterBarScale = 1.0f;
    }
}
