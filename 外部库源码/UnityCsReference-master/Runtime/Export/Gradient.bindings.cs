// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Bindings;

namespace UnityEngine
{
    // Color key used by Gradient
    [UsedByNativeCode]
    public struct GradientColorKey
    {
        // Gradient color key
        public GradientColorKey(Color col, float time)
        {
            color = col;
            this.time = time;
        }

        // color of key
        public Color color;

        // time of the key (0 - 1)
        public float time;
    }

    // Alpha key used by Gradient
    [UsedByNativeCode]
    public struct GradientAlphaKey
    {
        // Gradient alpha key
        public GradientAlphaKey(float alpha, float time)
        {
            this.alpha = alpha;
            this.time = time;
        }

        // alpha alpha of key
        public float alpha;

        // time of the key (0 - 1)
        public float time;
    }


    public enum GradientMode
    {
        Blend = 0,  // Keys will blend smoothly when the gradient is evaluated. (Default)
        Fixed = 1   // An exact key color will be returned when the gradient is evaluated.
    }

    // Gradient used for animating colors
    [StructLayout(LayoutKind.Sequential)]
    [ThreadAndSerializationSafe()]
    [RequiredByNativeCode]
    [NativeHeader("Runtime/Export/Gradient.bindings.h")]
    public class Gradient
    {
        internal IntPtr m_Ptr;

        [FreeFunction(Name = "Gradient_Bindings::Init", IsThreadSafe = true)]
        extern static private IntPtr Init();

        [FreeFunction(Name = "Gradient_Bindings::Cleanup", IsThreadSafe = true, HasExplicitThis = true)]
        extern private void Cleanup();

        [RequiredByNativeCode]
        public Gradient()
        {
            m_Ptr = Init();
        }

        ~Gradient()
        {
            Cleanup();
        }

        // Calculate color at a given time
        [FreeFunction(Name = "Gradient_Bindings::Evaluate", HasExplicitThis = true)]
        extern public Color Evaluate(float time);

        extern public GradientColorKey[] colorKeys
        {
            [FreeFunction("Gradient_Bindings::GetColorKeys", HasExplicitThis = true)] get;
            [FreeFunction("Gradient_Bindings::SetColorKeys", HasExplicitThis = true)] set;
        }

        extern public GradientAlphaKey[] alphaKeys
        {
            [FreeFunction("Gradient_Bindings::GetAlphaKeys", HasExplicitThis = true)] get;
            [FreeFunction("Gradient_Bindings::SetAlphaKeys", HasExplicitThis = true)] set;
        }


        extern public GradientMode mode { get; set; }

        extern internal Color constantColor { get; set; }

        // Setup Gradient with an array of color keys and alpha keys
        [FreeFunction(Name = "Gradient_Bindings::SetKeys", HasExplicitThis = true)]
        extern public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys);
    }
} // end of UnityEngine
