// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine.Bindings;


namespace UnityEditor
{
    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
    public partial class PlayerSettings : UnityEngine.Object
    {
        public static extern bool vulkanEnableSetSRGBWrite { get; set; }
        public static extern bool vulkanUseSWCommandBuffers { get; set; }
    }
}
