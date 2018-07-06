// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;
using UnityEditorInternal;
using System;
using UnityEditor.Callbacks;

namespace UnityEditor
{
    public enum GameViewSizeGroupType
    {
        Standalone,
        [System.Obsolete("WebPlayer has been removed in 5.4")]
        WebPlayer,
        iOS,
        Android,
        [System.Obsolete("PS3 has been removed in 5.5", false)]
        PS3,
        [System.Obsolete("Wii U support was removed in 2018.1", false)]
        WiiU,
        [System.Obsolete("Tizen has been removed in 2017.3", false)]
        Tizen,
        [System.Obsolete("Windows Phone 8 was removed in 5.3", false)]
        WP8,
        N3DS,
        HMD
    }

    [FilePathAttribute("GameViewSizes.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal class GameViewSizes : ScriptableSingleton<GameViewSizes>
    {
        // Written out to make it easy to find in text file (instead of an array)
        [SerializeField] GameViewSizeGroup m_Standalone = new GameViewSizeGroup();
        [SerializeField] GameViewSizeGroup m_iOS = new GameViewSizeGroup();
        [SerializeField] GameViewSizeGroup m_Android = new GameViewSizeGroup();
        [SerializeField] GameViewSizeGroup m_N3DS = new GameViewSizeGroup();
        [SerializeField] GameViewSizeGroup m_HMD = new GameViewSizeGroup();

        [NonSerialized] GameViewSize m_Remote = null;
        [NonSerialized] Vector2 m_LastStandaloneScreenSize = new Vector2(-1, -1);
        [NonSerialized] Vector2 m_LastRemoteScreenSize = new Vector2(-1, -1);
        [NonSerialized] int m_ChangeID = 0;
        [NonSerialized] static GameViewSizeGroupType s_GameViewSizeGroupType;

        public GameViewSizeGroupType currentGroupType
        {
            get { return s_GameViewSizeGroupType; }
        }

        public GameViewSizeGroup currentGroup
        {
            get {return GetGroup(s_GameViewSizeGroupType); }
        }

        private void OnEnable()
        {
            RefreshGameViewSizeGroupType(BuildTarget.NoTarget, EditorUserBuildSettings.activeBuildTarget);
        }

        public GameViewSizeGroup GetGroup(GameViewSizeGroupType gameViewSizeGroupType)
        {
            InitBuiltinGroups();
            switch (gameViewSizeGroupType)
            {
#pragma warning disable 618
                case GameViewSizeGroupType.WebPlayer:
                case GameViewSizeGroupType.WP8:
                case GameViewSizeGroupType.PS3:
                case GameViewSizeGroupType.Tizen:
                case GameViewSizeGroupType.WiiU:
#pragma warning restore 618
                case GameViewSizeGroupType.Standalone:
                    return m_Standalone;
                case GameViewSizeGroupType.iOS:
                    return m_iOS;
                case GameViewSizeGroupType.Android:
                    return m_Android;
                case GameViewSizeGroupType.N3DS:
                    return m_N3DS;
                case GameViewSizeGroupType.HMD:
                    return m_HMD;
                default:
                    Debug.LogError("Unhandled group enum! " + gameViewSizeGroupType);
                    break;
            }
            return m_Standalone;
        }

        public void SaveToHDD()
        {
            bool saveAsText = true;
            Save(saveAsText);
        }

        public bool IsDefaultStandaloneScreenSize(GameViewSizeGroupType gameViewSizeGroupType, int index)
        {
            return gameViewSizeGroupType == GameViewSizeGroupType.Standalone && GetDefaultStandaloneIndex() == index;
        }

        public bool IsRemoteScreenSize(GameViewSizeGroupType gameViewSizeGroupType, int index)
        {
            return GetGroup(gameViewSizeGroupType).IndexOf(m_Remote) == index;
        }

        public int GetDefaultStandaloneIndex()
        {
            return m_Standalone.GetBuiltinCount() - 1;
        }

        // returns true if screen size was changed
        public void RefreshStandaloneAndRemoteDefaultSizes()
        {
            if (InternalEditorUtility.defaultScreenWidth != m_LastStandaloneScreenSize.x ||
                InternalEditorUtility.defaultScreenHeight != m_LastStandaloneScreenSize.y)
            {
                m_LastStandaloneScreenSize = new Vector2(InternalEditorUtility.defaultScreenWidth,
                        InternalEditorUtility.defaultScreenHeight);
                RefreshStandaloneDefaultScreenSize((int)m_LastStandaloneScreenSize.x, (int)m_LastStandaloneScreenSize.y);
            }

            if (InternalEditorUtility.remoteScreenWidth != m_LastRemoteScreenSize.x ||
                InternalEditorUtility.remoteScreenHeight != m_LastRemoteScreenSize.y)
            {
                m_LastRemoteScreenSize = new Vector2(InternalEditorUtility.remoteScreenWidth,
                        InternalEditorUtility.remoteScreenHeight);
                RefreshRemoteScreenSize((int)m_LastRemoteScreenSize.x, (int)m_LastRemoteScreenSize.y);
            }

            if (UnityEngine.XR.XRSettings.isDeviceActive &&
                m_Remote.width != UnityEngine.XR.XRSettings.eyeTextureWidth &&
                m_Remote.height != UnityEngine.XR.XRSettings.eyeTextureHeight)
            {
                RefreshRemoteScreenSize(UnityEngine.XR.XRSettings.eyeTextureWidth, UnityEngine.XR.XRSettings.eyeTextureHeight);
            }
        }

        public void RefreshStandaloneDefaultScreenSize(int width, int height)
        {
            GameViewSize gvs = m_Standalone.GetGameViewSize(GetDefaultStandaloneIndex());
            gvs.height = height;
            gvs.width = width;
            Changed();
        }

        public void RefreshRemoteScreenSize(int width, int height)
        {
            m_Remote.width = width;
            m_Remote.height = height;
            if (width > 0 && height > 0)
                m_Remote.baseText = "Remote";
            else
                m_Remote.baseText = "Remote (Not Connected)";
            Changed();
        }

        public void Changed()
        {
            m_ChangeID++;
        }

        public int GetChangeID()
        {
            return m_ChangeID;
        }

        private void InitBuiltinGroups()
        {
            bool isInitialized = m_Standalone.GetBuiltinCount() > 0;
            if (isInitialized)
                return;

            m_Remote = new GameViewSize(GameViewSizeType.FixedResolution, 0, 0, "Remote (Not Connected)");

            // Shared
            GameViewSize kFree = new GameViewSize(GameViewSizeType.AspectRatio, 0, 0, "Free Aspect");
            GameViewSize k5_4 = new GameViewSize(GameViewSizeType.AspectRatio, 5, 4, "");
            GameViewSize k4_3 = new GameViewSize(GameViewSizeType.AspectRatio, 4, 3, "");
            GameViewSize k3_2 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "");
            GameViewSize k16_10 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 10, "");
            GameViewSize k16_9 = new GameViewSize(GameViewSizeType.AspectRatio, 16, 9, "");
            GameViewSize kStandalone = new GameViewSize(GameViewSizeType.FixedResolution, 0, 0, "Standalone");

            // iOS
            GameViewSize k_iPhoneTall = new GameViewSize(GameViewSizeType.FixedResolution, 320, 480, "iPhone Tall");
            GameViewSize k_iPhoneWide = new GameViewSize(GameViewSizeType.FixedResolution, 480, 320, "iPhone Wide");
            GameViewSize k_iPhone4GTall = new GameViewSize(GameViewSizeType.FixedResolution, 640, 960, "iPhone 4 Tall");
            GameViewSize k_iPhone4GWide = new GameViewSize(GameViewSizeType.FixedResolution, 960, 640, "iPhone 4 Wide");
            GameViewSize k_iPadTall = new GameViewSize(GameViewSizeType.FixedResolution, 768, 1024, "iPad Tall");
            GameViewSize k_iPadWide = new GameViewSize(GameViewSizeType.FixedResolution, 1024, 768, "iPad Wide");
            GameViewSize k_iPhone5Tall = new GameViewSize(GameViewSizeType.AspectRatio, 9, 16, "iPhone 5 Tall");
            GameViewSize k_iPhone5Wide = new GameViewSize(GameViewSizeType.AspectRatio, 16, 9, "iPhone 5 Wide");
            GameViewSize k_iPhoneTall2_3 = new GameViewSize(GameViewSizeType.AspectRatio, 2, 3, "iPhone Tall");
            GameViewSize k_iPhoneWide3_2 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "iPhone Wide");
            GameViewSize k_iPadTall3_4 = new GameViewSize(GameViewSizeType.AspectRatio, 3, 4, "iPad Tall");
            GameViewSize k_iPadWide4_3 = new GameViewSize(GameViewSizeType.AspectRatio, 4, 3, "iPad Wide");

            // Android
            GameViewSize k_HVGA_Portrait = new GameViewSize(GameViewSizeType.FixedResolution, 320, 480, "HVGA Portrait");
            GameViewSize k_HVGA_Landscape = new GameViewSize(GameViewSizeType.FixedResolution, 480, 320, "HVGA Landscape");
            GameViewSize k_WVGA_Portrait = new GameViewSize(GameViewSizeType.FixedResolution, 480, 800, "WVGA Portrait");
            GameViewSize k_WVGA_Landscape = new GameViewSize(GameViewSizeType.FixedResolution, 800, 480, "WVGA Landscape");
            GameViewSize k_FWVGA_Portrait = new GameViewSize(GameViewSizeType.FixedResolution, 480, 854, "FWVGA Portrait");
            GameViewSize k_FWVGA_Landscape = new GameViewSize(GameViewSizeType.FixedResolution, 854, 480, "FWVGA Landscape");
            GameViewSize k_WSVGA_Portrait = new GameViewSize(GameViewSizeType.FixedResolution, 600, 1024, "WSVGA Portrait");
            GameViewSize k_WSVGA_Landscape = new GameViewSize(GameViewSizeType.FixedResolution, 1024, 600, "WSVGA Landscape");
            GameViewSize k_WXGA_Portrait = new GameViewSize(GameViewSizeType.FixedResolution, 800, 1280, "WXGA Portrait");
            GameViewSize k_WXGA_Landscape = new GameViewSize(GameViewSizeType.FixedResolution, 1280, 800, "WXGA Landscape");
            GameViewSize k_3_2_Portrait = new GameViewSize(GameViewSizeType.AspectRatio, 2, 3, "3:2 Portrait");
            GameViewSize k_3_2_Landscape = new GameViewSize(GameViewSizeType.AspectRatio, 3, 2, "3:2 Landscape");
            GameViewSize k_16_10_Portrait = new GameViewSize(GameViewSizeType.AspectRatio, 10, 16, "16:10 Portrait");
            GameViewSize k_16_10_Landscape = new GameViewSize(GameViewSizeType.AspectRatio, 16, 10, "16:10 Landscape");

            // Nintendo 3DS
            GameViewSize kN3DSTopScreen = new GameViewSize(GameViewSizeType.FixedResolution, 400, 240, "Top Screen");
            GameViewSize kN3DSBottomScreen = new GameViewSize(GameViewSizeType.FixedResolution, 320, 240, "Bottom Screen");

            m_Standalone.AddBuiltinSizes(kFree, k5_4, k4_3, k3_2, k16_10, k16_9, kStandalone);

            m_iOS.AddBuiltinSizes(kFree,
                k_iPhoneTall, k_iPhoneWide,
                k_iPhone4GTall, k_iPhone4GWide,
                k_iPadTall, k_iPadWide,
                k_iPhone5Tall, k_iPhone5Wide,
                k_iPhoneTall2_3, k_iPhoneWide3_2,
                k_iPadTall3_4, k_iPadWide4_3);
            m_Android.AddBuiltinSizes(kFree, m_Remote,
                k_HVGA_Portrait, k_HVGA_Landscape,
                k_WVGA_Portrait, k_WVGA_Landscape,
                k_FWVGA_Portrait, k_FWVGA_Landscape,
                k_WSVGA_Portrait, k_WSVGA_Landscape,
                k_WXGA_Portrait, k_WXGA_Landscape,
                k_3_2_Portrait, k_3_2_Landscape,
                k_16_10_Portrait, k_16_10_Landscape);

            m_N3DS.AddBuiltinSizes(kFree, kN3DSTopScreen, kN3DSBottomScreen);

            m_HMD.AddBuiltinSizes(kFree, m_Remote);
        }

        internal static bool DefaultLowResolutionSettingForStandalone()
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    return !PlayerSettings.macRetinaSupport; // if retina support enabled -> expecting LowRes setting disabled by default
                default:
                    return true;
            }
        }

        internal static bool DefaultLowResolutionSettingForSizeGroupType(GameViewSizeGroupType sizeGroupType)
        {
            switch (sizeGroupType)
            {
                case GameViewSizeGroupType.Standalone:
                    return DefaultLowResolutionSettingForStandalone();
                case GameViewSizeGroupType.N3DS:
                    return true;
                case GameViewSizeGroupType.iOS:
                case GameViewSizeGroupType.Android:
                default:
                    return false;
            }
        }

        private static void RefreshDerivedGameViewSize(GameViewSizeGroupType groupType, int gameViewSizeIndex, GameViewSize gameViewSize)
        {
            if (GameViewSizes.instance.IsDefaultStandaloneScreenSize(groupType, gameViewSizeIndex))
            {
                gameViewSize.width = (int)InternalEditorUtility.defaultScreenWidth;
                gameViewSize.height = (int)InternalEditorUtility.defaultScreenHeight;
            }
            else if (GameViewSizes.instance.IsRemoteScreenSize(groupType, gameViewSizeIndex))
            {
                int width = 0;
                int height = 0;
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    width = UnityEngine.XR.XRSettings.eyeTextureWidth;
                    height = UnityEngine.XR.XRSettings.eyeTextureHeight;
                }
                else
                {
                    width = (int)InternalEditorUtility.remoteScreenWidth;
                    height = (int)InternalEditorUtility.remoteScreenHeight;
                }

                if (width > 0 && height > 0)
                {
                    gameViewSize.sizeType = GameViewSizeType.FixedResolution;
                    gameViewSize.width = width;
                    gameViewSize.height = height;
                }
                else
                {
                    // Free aspect if invalid remote width or height
                    gameViewSize.sizeType = GameViewSizeType.AspectRatio;
                    gameViewSize.width = gameViewSize.height = 0;
                }
            }
        }

        public static Rect GetConstrainedRect(Rect startRect, GameViewSizeGroupType groupType, int gameViewSizeIndex, out bool fitsInsideRect)
        {
            fitsInsideRect = true;
            Rect constrainedRect = startRect;
            GameViewSize gameViewSize = GameViewSizes.instance.GetGroup(groupType).GetGameViewSize(gameViewSizeIndex);
            RefreshDerivedGameViewSize(groupType, gameViewSizeIndex, gameViewSize);

            if (gameViewSize.isFreeAspectRatio)
            {
                return startRect;
            }

            float newRatio = 0;
            bool useRatio;
            switch (gameViewSize.sizeType)
            {
                case GameViewSizeType.AspectRatio:
                {
                    newRatio = gameViewSize.aspectRatio;
                    useRatio = true;
                }
                break;
                case GameViewSizeType.FixedResolution:
                {
                    if (gameViewSize.height > startRect.height || gameViewSize.width > startRect.width)
                    {
                        newRatio = gameViewSize.aspectRatio;
                        useRatio = true;
                        fitsInsideRect = false;
                    }
                    else
                    {
                        constrainedRect.height = gameViewSize.height;
                        constrainedRect.width = gameViewSize.width;
                        useRatio = false;
                    }
                }
                break;
                default:
                    throw new ArgumentException("Unrecognized size type");
            }

            if (useRatio)
            {
                constrainedRect.height = (constrainedRect.width / newRatio) > startRect.height
                    ? (startRect.height)
                    : (constrainedRect.width / newRatio);
                constrainedRect.width = (constrainedRect.height * newRatio);
            }

            // clamp
            constrainedRect.height = Mathf.Clamp(constrainedRect.height, 0f, startRect.height);
            constrainedRect.width = Mathf.Clamp(constrainedRect.width, 0f, startRect.width);

            // center
            constrainedRect.y = (startRect.height * 0.5f - constrainedRect.height * 0.5f) + startRect.y;
            constrainedRect.x = (startRect.width * 0.5f - constrainedRect.width * 0.5f) + startRect.x;

            // Round to whole pixels - actually is important for correct rendering of game view!
            constrainedRect.width = Mathf.Floor(constrainedRect.width + 0.5f);
            constrainedRect.height = Mathf.Floor(constrainedRect.height + 0.5f);
            constrainedRect.x = Mathf.Floor(constrainedRect.x + 0.5f);
            constrainedRect.y = Mathf.Floor(constrainedRect.y + 0.5f);

            return constrainedRect;
        }

        public static Vector2 GetRenderTargetSize(Rect startRect, GameViewSizeGroupType groupType, int gameViewSizeIndex, out bool clamped)
        {
            GameViewSize gameViewSize = GameViewSizes.instance.GetGroup(groupType).GetGameViewSize(gameViewSizeIndex);
            RefreshDerivedGameViewSize(groupType, gameViewSizeIndex, gameViewSize);
            Vector2 targetSize;
            clamped = false;

            // Free aspect takes up all available pixels by default
            if (gameViewSize.isFreeAspectRatio)
            {
                targetSize = startRect.size;
            }
            else
            {
                switch (gameViewSize.sizeType)
                {
                    // Aspect ratio is enforced, but fills up as much game view as it can
                    case GameViewSizeType.AspectRatio:
                    {
                        if (startRect.height == 0f || gameViewSize.aspectRatio == 0f)
                        {
                            targetSize = Vector2.zero;
                            break;
                        }
                        var startRatio = startRect.width / startRect.height;
                        if (startRatio < gameViewSize.aspectRatio)
                        {
                            targetSize = new Vector2(startRect.width, Mathf.Round(startRect.width / gameViewSize.aspectRatio));
                        }
                        else
                        {
                            targetSize = new Vector2(Mathf.Round(startRect.height * gameViewSize.aspectRatio), startRect.height);
                        }
                    }
                    break;
                    // Fixed resolution is fixed, but scaled down to fit, or scaled up to largest possible integer
                    case GameViewSizeType.FixedResolution:
                    {
                        targetSize = new Vector2(gameViewSize.width, gameViewSize.height);
                    }
                    break;
                    default:
                        throw new ArgumentException("Unrecognized size type");
                }
            }

            // Prevent ludicrous render target sizes. Heuristics based on:
            // - GPU supported max. texture size
            // - "should be enough for anyone" (i.e. more than 8K resolution)
            // - Available VRAM
            //
            // The reason is that while GPUs support large textures (e.g. 16k x 16k), trying to
            // actually create one will just make you run out of memory. VRAM size estimate that we
            // have is also only very approximate.
            // Let's assume we can use 20% of VRAM for game view render target;
            // and that we need 12 bytes/pixel (4 for color, double buffered, 4 for depth).
            // Figure out what's the max texture area that fits there.
            var maxVRAMArea = SystemInfo.graphicsMemorySize * 0.20f / 12f * 1024f * 1024f;

            var targetArea = targetSize.x * targetSize.y;
            if (targetArea > maxVRAMArea)
            {
                var aspect = targetSize.y / targetSize.x;
                targetSize.x = Mathf.Sqrt(maxVRAMArea * aspect);
                targetSize.y = aspect * targetSize.x;
                clamped = true;
            }

            // Over 8K resolution (7680x4320) should be enough for anyone (tm)
            var maxResolutionSize = 8192f;
            var maxSize = Mathf.Min(SystemInfo.maxRenderTextureSize, maxResolutionSize);

            if (targetSize.x > maxSize || targetSize.y > maxSize)
            {
                if (targetSize.x > targetSize.y)
                    targetSize *= maxSize / targetSize.x;
                else
                    targetSize *= maxSize / targetSize.y;
                clamped = true;
            }

            return targetSize;
        }

        class BuildTargetChangedHandler : Build.IActiveBuildTargetChanged
        {
            public int callbackOrder { get { return 0; } }

            public void OnActiveBuildTargetChanged(BuildTarget oldTarget, BuildTarget newTarget)
            {
                RefreshGameViewSizeGroupType(oldTarget, newTarget);
            }
        }

        static void RefreshGameViewSizeGroupType(BuildTarget oldTarget, BuildTarget newTarget)
        {
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(newTarget);
            s_GameViewSizeGroupType = BuildTargetGroupToGameViewSizeGroup(buildTargetGroup);
        }

        public static GameViewSizeGroupType BuildTargetGroupToGameViewSizeGroup(BuildTargetGroup buildTargetGroup)
        {
            if (UnityEngine.XR.XRSettings.enabled && UnityEngine.XR.XRSettings.showDeviceView)
                return GameViewSizeGroupType.HMD;

            switch (buildTargetGroup)
            {
                case BuildTargetGroup.Standalone:
                    return GameViewSizeGroupType.Standalone;

                case BuildTargetGroup.iOS:
                    return GameViewSizeGroupType.iOS;

                case BuildTargetGroup.Android:
                    return GameViewSizeGroupType.Android;

                case BuildTargetGroup.N3DS:
                    return GameViewSizeGroupType.N3DS;

                default:
                    return GameViewSizeGroupType.Standalone;
            }
        }
    }
}

// namespace
