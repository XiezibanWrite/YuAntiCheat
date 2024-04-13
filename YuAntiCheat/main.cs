﻿using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static CloudGenerator;
using UnityEngine.Playables;
using Il2CppSystem.IO;

[assembly: AssemblyFileVersion(YuAntiCheat.Main.PluginVersion)]
[assembly: AssemblyInformationalVersion(YuAntiCheat.Main.PluginVersion)]
[assembly: AssemblyVersion(YuAntiCheat.Main.PluginVersion)]
namespace YuAntiCheat;

[BepInPlugin(PluginGuid, "YuAntiCheat", PluginVersion)]
[BepInProcess("Among Us.exe")]
public class Main : BasePlugin
{

    public static readonly string ModName = "YuAntiCheat"; // 咱们的模组名字
    public static readonly string ModColor = "#fffcbe"; // 咱们的模组颜色
    public static readonly string MainMenuText = "邪恶存在不了一点~"; // 咱们模组的首页标语
    public const string PluginGuid = "com.Yu.YuAntiCheat"; //咱们模组的Guid
    public const string PluginVersion = "1.0.1"; //咱们模组的版本号
    public const string CanUseInAmongUsVer = "2024.3.5"; //智齿的AU版本

    public Harmony Harmony { get; } = new Harmony(PluginGuid);

    public static BepInEx.Logging.ManualLogSource Logger;
    
    public static IEnumerable<PlayerControl> AllPlayerControls => PlayerControl.AllPlayerControls.ToArray().Where(p => p != null);
    
    public static Main Instance; //设置Main实例

    public static bool safemode = true;//设置安全模式
    
    public override void Load()//加载 启动！
    {
        Instance = this; //Main实例

        Logger = BepInEx.Logging.Logger.CreateLogSource("YuAntiCheat"); //输出前缀 设置！

        if (Application.version == CanUseInAmongUsVer)
            Logger.LogInfo($"AmongUs Version: {Application.version}"); //牢底居然有智齿的版本？！
        else
            Logger.LogInfo($"游戏本体版本过低或过高,AmongUs Version: {Application.version}"); //牢底你的版本也不行啊
        //各组件初始化
        Harmony.PatchAll();
        //模组加载好了标语
        Logger.LogInfo($"模组加载完成-YuAC is starting now!");
    }
}