using HarmonyLib;
using System.Collections.Generic;
using System.Text;
using TMPro;
using YuAntiCheat;
using UnityEngine;
using System.IO;
using System.Reflection;
using YuAntiCheat.Updater;

namespace YuAntiCheat.UI;

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPriority(Priority.First)]
internal class TitleLogoPatch
{
    public static GameObject ModStamp;
    public static GameObject YuAC_Background;
    public static GameObject Ambience;
    public static GameObject Starfield;
    public static GameObject Sizer;
    public static GameObject AULogo;
    public static GameObject BottomButtonBounds;
    public static GameObject LeftPanel;
    public static GameObject RightPanel;
    public static GameObject CloseRightButton;
    public static GameObject Tint;

    public static Vector3 RightPanelOp;
    
    public static void showPopup(string text){
        var popup = GameObject.Instantiate(DiscordManager.Instance.discordPopup, Camera.main!.transform);
        
        var background = popup.transform.Find("Background").GetComponent<SpriteRenderer>();
        //var button = popup.transform.Find("ExitGame").GetComponent<SpriteRenderer>();
        var size = background.size;
        size.x *= 2.5f;
        size.y *= 2f;
        background.size = size;

        //button.GetComponent<AspectPosition>().anchorPoint = new(0.2f, 0.38f);

        popup.TextAreaTMP.fontSizeMin = 2;
        popup.Show(text);
    }
    
    private static void Postfix(MainMenuManager __instance)
    {
        GameObject.Find("BackgroundTexture")?.SetActive(!MainMenuManagerPatch.ShowedBak);
        
        if (!(ModStamp = GameObject.Find("ModStamp"))) return;
        ModStamp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        /*YuAC_Background = new GameObject("YuAC Background");
        YuAC_Background.transform.position = new Vector3(2.1f, 0.2f, 520f);
        var bgRenderer = YuAC_Background.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = LoadSprite("YuAntiCheat.Resources.Yu-Logo-tm.png", 179f);//Bg*/
        
        if (!(Ambience = GameObject.Find("Ambience"))) return;
        // if (!(Starfield = Ambience.transform.FindChild("starfield").gameObject)) return;
        // StarGen starGen = Starfield.GetComponent<StarGen>();
        // starGen.SetDirection(new Vector2(0, -2));
        // //Starfield.transform.SetParent(YuAC_Background.transform);
        // GameObject.Destroy(Ambience);
        Ambience.SetActive(false);
        
        if (!(LeftPanel = GameObject.Find("LeftPanel"))) return;
        LeftPanel.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        static void ResetParent(GameObject obj) => obj.transform.SetParent(LeftPanel.transform.parent);
        LeftPanel.ForEachChild((Il2CppSystem.Action<GameObject>)ResetParent);
        LeftPanel.SetActive(false);
        
                Color shade = new(0f, 0f, 0f, 0f);
        var standardActiveSprite = __instance.newsButton.activeSprites.GetComponent<SpriteRenderer>().sprite;
        var minorActiveSprite = __instance.quitButton.activeSprites.GetComponent<SpriteRenderer>().sprite;

        Dictionary<List<PassiveButton>, (Sprite, Color, Color, Color, Color)> mainButtons = new()
        {
            {new List<PassiveButton>() {__instance.playButton, __instance.inventoryButton, __instance.shopButton},
                (standardActiveSprite, new(1f, 0.524f, 0.549f, 0.8f), shade, Color.white, Color.white) },
            {new List<PassiveButton>() {__instance.newsButton, __instance.myAccountButton, __instance.settingsButton},
                (minorActiveSprite, new(1f, 0.825f, 0.686f, 0.8f), shade, Color.white, Color.white) },
            {new List<PassiveButton>() {__instance.creditsButton, __instance.quitButton},
                (minorActiveSprite, new(0.526f, 1f, 0.792f, 0.8f), shade, Color.white, Color.white) },
        };

        void FormatButtonColor(PassiveButton button, Sprite borderType, Color inActiveColor, Color activeColor, Color inActiveTextColor, Color activeTextColor)
        {
            button.activeSprites.transform.FindChild("Shine")?.gameObject?.SetActive(false);
            button.inactiveSprites.transform.FindChild("Shine")?.gameObject?.SetActive(false);
            var activeRenderer = button.activeSprites.GetComponent<SpriteRenderer>();
            var inActiveRenderer = button.inactiveSprites.GetComponent<SpriteRenderer>();
            activeRenderer.sprite = minorActiveSprite;
            inActiveRenderer.sprite = minorActiveSprite;
            activeRenderer.color = activeColor.a == 0f ? new Color(inActiveColor.r, inActiveColor.g, inActiveColor.b, 1f) : activeColor;
            inActiveRenderer.color = inActiveColor;
            button.activeTextColor = activeTextColor;
            button.inactiveTextColor = inActiveTextColor;
        }

        foreach (var kvp in mainButtons)
            kvp.Key.Do(button => FormatButtonColor(button, kvp.Value.Item1, kvp.Value.Item2, kvp.Value.Item3, kvp.Value.Item4, kvp.Value.Item5));

        GameObject.Find("Divider")?.SetActive(false);
        
        if (!(RightPanel = GameObject.Find("RightPanel"))) return;
        var rpap = RightPanel.GetComponent<AspectPosition>();
        if (rpap) Object.Destroy(rpap);
        RightPanelOp = RightPanel.transform.localPosition;
        RightPanel.transform.localPosition = RightPanelOp + new Vector3(10f, 0f, 0f);
        RightPanel.GetComponent<SpriteRenderer>().color = new(1f, 0.78f, 0.9f, 1f);
        
        if (!(Sizer = GameObject.Find("Sizer"))) return;
        if (!(AULogo = GameObject.Find("LOGO-AU"))) return;
        Sizer.transform.localPosition += new Vector3(0f, 0.12f, 0f);
        AULogo.transform.localScale = new Vector3(0.66f, 0.67f, 1f);
        AULogo.transform.position += new Vector3(0f, 0.1f, 0f);
        var logoRenderer = AULogo.GetComponent<SpriteRenderer>();
        logoRenderer.sprite = LoadSprite("YuAntiCheat.Resources.YuAC-Logo-tm.png",100f);//Yu的Logo

        if (!(BottomButtonBounds = GameObject.Find("BottomButtonBounds"))) return;
        BottomButtonBounds.transform.localPosition -= new Vector3(0f, 0.1f, 0f);

        
        CloseRightButton = new GameObject("CloseRightPanelButton");
        CloseRightButton.transform.SetParent(RightPanel.transform);
        CloseRightButton.transform.localPosition = new Vector3(-4.78f, 1.3f, 1f);
        CloseRightButton.transform.localScale = new(1f, 1f, 1f);
        CloseRightButton.AddComponent<BoxCollider2D>().size = new(0.6f, 1.5f);
        var closeRightSpriteRenderer = CloseRightButton.AddComponent<SpriteRenderer>();
        //closeRightSpriteRenderer.sprite = LoadSprite("TONEX.Resources.Images.RightPanelCloseButton.png", 100f);
        closeRightSpriteRenderer.color = new(1f, 0.78f, 0.9f, 1f);
        var closeRightPassiveButton = CloseRightButton.AddComponent<PassiveButton>();
        closeRightPassiveButton.OnClick = new();
        //closeRightPassiveButton.OnClick.AddListener((System.Action)MainMenuManagerPatch.HideRightPanel);
        closeRightPassiveButton.OnMouseOut = new();
        closeRightPassiveButton.OnMouseOut.AddListener((System.Action)(() => closeRightSpriteRenderer.color = new(1f, 0.78f, 0.9f, 1f)));
        closeRightPassiveButton.OnMouseOver = new();
        closeRightPassiveButton.OnMouseOver.AddListener((System.Action)(() => closeRightSpriteRenderer.color = new(1f, 0.68f, 0.99f, 1f)));

        Tint = __instance.screenTint.gameObject;
        var ttap = Tint.GetComponent<AspectPosition>();
        if (ttap) Object.Destroy(ttap);
        Tint.transform.SetParent(RightPanel.transform);
        Tint.transform.localPosition = new Vector3(-0.0824f, 0.0513f, Tint.transform.localPosition.z);
        Tint.transform.localScale = new Vector3(1f, 1f, 1f);
        
        var creditsScreen = __instance.creditsScreen;
        if (creditsScreen)
        {
            var csto = creditsScreen.GetComponent<TransitionOpen>();
            if (csto) Object.Destroy(csto);
            var closeButton = creditsScreen.transform.FindChild("CloseButton");
            closeButton?.gameObject.SetActive(false);
        }
        
        if (Main.IfFirstStartGame)
        {
            showPopup(TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese || TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese ? $"欢迎使用YuAntiCheat v{Main.PluginVersion}！\n祝你不被外挂打扰！\n本模组可能被树懒的反作弊踢出\n本模组无任何外挂行为\n请勿装载该模组进入其他模组房间\n请勿装载另一个模组" : $"Welcome to use YuAntiCheat v{Main.PluginVersion}!\nHope you have a good game!");
            Main.IfFirstStartGame = false;
        }
    }
    
    public static Dictionary<string, Sprite> CachedSprites = new();
    public static Sprite LoadSprite(string path, float pixelsPerUnit = 1f)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
            Texture2D texture = LoadTextureFromResources(path);
            sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch
        {
            
        }
        return null;
    }
    
    public static Texture2D LoadTextureFromResources(string path)
    {
        try
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            using MemoryStream ms = new();
            stream.CopyTo(ms);
            ImageConversion.LoadImage(texture, ms.ToArray(), false);
            return texture;
        }
        catch
        {
        }
        return null;
    }
}
[HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))] 
public static class VersionShower_Start
{
    public static void Postfix(VersionShower __instance)
    {
        if(Main.ModMode == 1) __instance.text.text = TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese || TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese ? $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>您正在使用 v{Main.PluginVersion} Canary测试版！</color>)" : $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>You are using  v{Main.PluginVersion} Canary Version</color>)";
        else if(Main.ModMode == 0)__instance.text.text = TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese || TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese ? $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>您正在使用 v{Main.PluginVersion} Debug开发者版！</color>)" : $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>You are using  v{Main.PluginVersion} Debug Version</color>)";
        else{ 
            if(ModUpdater.hasUpdate)
                __instance.text.text = TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese || TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese ? $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>YuAC有v{ModUpdater.latestVersion}更新!</color>)" : $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#DC143C>YuAC has v{ModUpdater.latestVersion} version update!</color>)";
            else
                __instance.text.text = TranslationController.Instance.currentLanguage.languageID == SupportedLangs.SChinese || TranslationController.Instance.currentLanguage.languageID == SupportedLangs.TChinese ? $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#00FF00>YuAC无更新</color>)" : $"<color={Main.ModColor}>{Main.ModName}</color> (<color=#00FF00>Your YuAC version is up to date/color>)";
        } 
    }
}
