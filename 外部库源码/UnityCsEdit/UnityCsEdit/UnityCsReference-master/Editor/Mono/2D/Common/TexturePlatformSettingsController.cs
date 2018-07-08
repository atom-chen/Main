// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System.Collections.Generic;
using UnityEditor.U2D.Interface;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityEditor.U2D.Common
{
    internal class TexturePlatformSettingsViewController : ITexturePlatformSettingsController
    {
        public bool HandleDefaultSettings(List<TextureImporterPlatformSettings> platformSettings, ITexturePlatformSettingsView view)
        {
            Assert.IsTrue(platformSettings.Count > 0, "At least 1 platform setting is needed to display the texture platform setting UI.");

            var allSize = platformSettings[0].maxTextureSize;
            var allCompression = platformSettings[0].textureCompression;
            var allUseCrunchedCompression = platformSettings[0].crunchedCompression;
            var allCompressionQuality = platformSettings[0].compressionQuality;

            var newSize = allSize;
            var newCompression = allCompression;
            var newUseCrunchedCompression = allUseCrunchedCompression;
            var newCompressionQuality = allCompressionQuality;

            var mixedSize = false;
            var mixedCompression = false;
            var mixedUseCrunchedCompression = false;
            var mixedCompressionQuality = false;

            var sizeChanged = false;
            var compressionChanged = false;
            var useCrunchedCompressionChanged = false;
            var compressionQualityChanged = false;

            for (var i = 1; i < platformSettings.Count; ++i)
            {
                var settings = platformSettings[i];
                if (settings.maxTextureSize != allSize)
                    mixedSize = true;
                if (settings.textureCompression != allCompression)
                    mixedCompression = true;
                if (settings.crunchedCompression != allUseCrunchedCompression)
                    mixedUseCrunchedCompression = true;
                if (settings.compressionQuality != allCompressionQuality)
                    mixedCompressionQuality = true;
            }

            newSize = view.DrawMaxSize(allSize, mixedSize, out sizeChanged);
            newCompression = view.DrawCompression(allCompression, mixedCompression, out compressionChanged);
            if (!mixedCompression && allCompression != TextureImporterCompression.Uncompressed)
            {
                newUseCrunchedCompression = view.DrawUseCrunchedCompression(allUseCrunchedCompression, mixedUseCrunchedCompression, out useCrunchedCompressionChanged);

                if (!mixedUseCrunchedCompression && allUseCrunchedCompression)
                {
                    newCompressionQuality = view.DrawCompressionQualitySlider(allCompressionQuality, mixedCompressionQuality, out compressionQualityChanged);
                }
            }

            if (sizeChanged || compressionChanged || useCrunchedCompressionChanged || compressionQualityChanged)
            {
                for (var i = 0; i < platformSettings.Count; ++i)
                {
                    if (sizeChanged)
                        platformSettings[i].maxTextureSize = newSize;
                    if (compressionChanged)
                        platformSettings[i].textureCompression = newCompression;
                    if (useCrunchedCompressionChanged)
                        platformSettings[i].crunchedCompression = newUseCrunchedCompression;
                    if (compressionQualityChanged)
                        platformSettings[i].compressionQuality = newCompressionQuality;
                }
                return true;
            }
            else
                return false;
        }

        public bool HandlePlatformSettings(BuildTarget buildTarget, List<TextureImporterPlatformSettings> platformSettings, ITexturePlatformSettingsView view, ITexturePlatformSettingsFormatHelper formatHelper)
        {
            Assert.IsTrue(platformSettings.Count > 0, "At least 1 platform setting is needed to display the texture platform setting UI.");

            var allOverride = platformSettings[0].overridden;
            var allSize = platformSettings[0].maxTextureSize;
            var allFormat = platformSettings[0].format;
            var allCompressionQuality = platformSettings[0].compressionQuality;

            var newOverride = allOverride;
            var newSize = allSize;
            var newFormat = allFormat;
            var newCompressionQuality = allCompressionQuality;

            var mixedOverride = false;
            var mixedSize = false;
            var mixedFormat = false;
            var mixedCompression = false;

            var overrideChanged = false;
            var sizeChanged = false;
            var formatChanged = false;
            var compressionChanged = false;

            for (var i = 1; i < platformSettings.Count; ++i)
            {
                var settings = platformSettings[i];
                if (settings.overridden != allOverride)
                    mixedOverride = true;
                if (settings.maxTextureSize != allSize)
                    mixedSize = true;
                if (settings.format != allFormat)
                    mixedFormat = true;
                if (settings.compressionQuality != allCompressionQuality)
                    mixedCompression = true;
            }

            newOverride = view.DrawOverride(allOverride, mixedOverride, out overrideChanged);

            if (!mixedOverride && allOverride)
            {
                newSize = view.DrawMaxSize(allSize, mixedSize, out sizeChanged);
            }

            int[] formatValues = null;
            string[] formatStrings = null;
            formatHelper.AcquireTextureFormatValuesAndStrings(buildTarget, out formatValues, out formatStrings);

            newFormat = view.DrawFormat(allFormat, formatValues, formatStrings, mixedFormat, mixedOverride || !allOverride, out formatChanged);

            if (!mixedFormat && !mixedOverride && allOverride && formatHelper.TextureFormatRequireCompressionQualityInput(allFormat))
            {
                bool showAsEnum =
                    buildTarget == BuildTarget.iOS ||
                    buildTarget == BuildTarget.tvOS ||
                    buildTarget == BuildTarget.Android
                ;

                if (showAsEnum)
                {
                    var compressionMode = 1;
                    if (allCompressionQuality == (int)TextureCompressionQuality.Fast)
                        compressionMode = 0;
                    else if (allCompressionQuality == (int)TextureCompressionQuality.Best)
                        compressionMode = 2;

                    var returnValue = view.DrawCompressionQualityPopup(compressionMode, mixedCompression, out compressionChanged);

                    if (compressionChanged)
                    {
                        switch (returnValue)
                        {
                            case 0: newCompressionQuality = (int)TextureCompressionQuality.Fast; break;
                            case 1: newCompressionQuality = (int)TextureCompressionQuality.Normal; break;
                            case 2: newCompressionQuality = (int)TextureCompressionQuality.Best; break;

                            default:
                                Assert.IsTrue(false, "ITexturePlatformSettingsView.DrawCompressionQualityPopup should never return compression option value that's not 0, 1 or 2.");
                                break;
                        }
                    }
                }
                else
                {
                    newCompressionQuality = view.DrawCompressionQualitySlider(allCompressionQuality, mixedCompression, out compressionChanged);
                }
            }

            if (overrideChanged || sizeChanged || formatChanged || compressionChanged)
            {
                for (var i = 0; i < platformSettings.Count; ++i)
                {
                    if (overrideChanged)
                        platformSettings[i].overridden = newOverride;
                    if (sizeChanged)
                        platformSettings[i].maxTextureSize = newSize;
                    if (formatChanged)
                        platformSettings[i].format = newFormat;
                    if (compressionChanged)
                        platformSettings[i].compressionQuality = newCompressionQuality;
                }

                return true;
            }
            else
                return false;
        }
    }
}
