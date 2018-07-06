// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
    internal class UIElementsUtility
    {
        private static Stack<IMGUIContainer> s_ContainerStack = new Stack<IMGUIContainer>();
        private static Dictionary<int, Panel> s_UIElementsCache = new Dictionary<int, Panel>();

        private static Event s_EventInstance = new Event(); // event instance reused for ProcessEvent()

        private static EventDispatcher s_EventDispatcher;

        internal static Action<IMGUIContainer> s_BeginContainerCallback;
        internal static Action<IMGUIContainer> s_EndContainerCallback;

        internal static IEventDispatcher eventDispatcher
        {
            get
            {
                if (s_EventDispatcher == null)
                    s_EventDispatcher = new EventDispatcher();

                return s_EventDispatcher;
            }
        }

        // For testing purposes
        internal static void ClearDispatcher()
        {
            s_EventDispatcher = null;
        }

        static UIElementsUtility()
        {
            GUIUtility.takeCapture += TakeCapture;
            GUIUtility.releaseCapture += ReleaseCapture;
            GUIUtility.processEvent += ProcessEvent;
            GUIUtility.cleanupRoots += CleanupRoots;
            GUIUtility.endContainerGUIFromException += EndContainerGUIFromException;
        }

        private static void TakeCapture()
        {
            if (s_ContainerStack.Count > 0)
            {
                var topmostContainer = s_ContainerStack.Peek();

                if (MouseCaptureController.IsMouseCaptureTaken() && !topmostContainer.HasMouseCapture())
                {
                    Debug.Log("Should not grab hot control with an active capture");
                }
                topmostContainer.TakeMouseCapture();
            }
        }

        private static void ReleaseCapture()
        {
            MouseCaptureController.ReleaseMouseCapture();
        }

        private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
        {
            Panel panel;
            if (nativeEventPtr != IntPtr.Zero && s_UIElementsCache.TryGetValue(instanceID, out panel))
            {
                // Instead of allocating a new Event object every time
                // we reuse this instance and copy event data into it
                s_EventInstance.CopyFromPtr(nativeEventPtr);

                return DoDispatch(panel);
            }
            return false;
        }

        public static void RemoveCachedPanel(int instanceID)
        {
            s_UIElementsCache.Remove(instanceID);
        }

        private static void CleanupRoots()
        {
            // see GUI.CleanupRoots
            s_EventInstance = null;
            s_EventDispatcher = null;
            s_UIElementsCache = null;
            s_ContainerStack = null;
            s_BeginContainerCallback = null;
            s_EndContainerCallback = null;
        }

        private static bool EndContainerGUIFromException(Exception exception)
        {
            // only End if we have a current container
            if (s_ContainerStack.Count > 0)
            {
                GUIUtility.EndContainer();
                s_ContainerStack.Pop();
            }

            return GUIUtility.ShouldRethrowException(exception);
        }

        internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
        {
            if (container.useOwnerObjectGUIState)
            {
                GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
            }
            else
            {
                GUIUtility.BeginContainer(container.guiState);
            }

            s_ContainerStack.Push(container);
            GUIUtility.s_SkinMode = (int)container.contextType;
            GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();

            Event.current = evt;

            // call AFTER setting current event
            if (s_BeginContainerCallback != null)
                s_BeginContainerCallback(container);

            GUI.enabled = container.enabledInHierarchy;
            GUILayoutUtility.BeginContainer(cache);
            GUIUtility.ResetGlobalState();
        }

        // End the 2D GUI.
        internal static void EndContainerGUI()
        {
            if (Event.current.type == EventType.Layout
                && s_ContainerStack.Count > 0)
            {
                var r = s_ContainerStack.Peek().layout;
                GUILayoutUtility.LayoutFromContainer(r.width, r.height);
            }
            // restore cache
            GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
            GUIContent.ClearStaticCache();

            if (s_ContainerStack.Count > 0)
            {
                IMGUIContainer container = s_ContainerStack.Peek();
                if (s_EndContainerCallback != null)
                    s_EndContainerCallback(container);
                GUIUtility.EndContainer();
                s_ContainerStack.Pop();
            }
        }

        // TODO rename skinMode to context type and make that explicit everywhere
        internal static ContextType GetGUIContextType()
        {
            return GUIUtility.s_SkinMode == 0 ? ContextType.Player : ContextType.Editor;
        }

        internal static EventBase CreateEvent(Event systemEvent)
        {
            return CreateEvent(systemEvent, systemEvent.type);
        }

        // In order for tests to run without an EditorWindow but still be able to send
        // events, we sometimes need to force the event type. IMGUI::GetEventType() (native) will
        // return the event type as Ignore if the proper views haven't yet been
        // initialized. This (falsely) breaks tests that rely on the event type. So for tests, we
        // just ensure the event type is what we originally set it to when we sent it.
        internal static EventBase CreateEvent(Event systemEvent, EventType eventType)
        {
            switch (eventType)
            {
                case EventType.MouseMove:
                    return MouseMoveEvent.GetPooled(systemEvent);
                case EventType.MouseDrag:
                    return MouseMoveEvent.GetPooled(systemEvent);
                case EventType.MouseDown:
                    return MouseDownEvent.GetPooled(systemEvent);
                case EventType.MouseUp:
                    return MouseUpEvent.GetPooled(systemEvent);
                case EventType.ContextClick:
                    return ContextClickEvent.GetPooled(systemEvent);
                case EventType.MouseEnterWindow:
                    return MouseEnterWindowEvent.GetPooled(systemEvent);
                case EventType.MouseLeaveWindow:
                    return MouseLeaveWindowEvent.GetPooled(systemEvent);
                case EventType.ScrollWheel:
                    return WheelEvent.GetPooled(systemEvent);
                case EventType.KeyDown:
                    return KeyDownEvent.GetPooled(systemEvent);
                case EventType.KeyUp:
                    return KeyUpEvent.GetPooled(systemEvent);
                case EventType.DragUpdated:
                    return DragUpdatedEvent.GetPooled(systemEvent);
                case EventType.DragPerform:
                    return DragPerformEvent.GetPooled(systemEvent);
                case EventType.DragExited:
                    return DragExitedEvent.GetPooled(systemEvent);
                case EventType.ValidateCommand:
                    return ValidateCommandEvent.GetPooled(systemEvent);
                case EventType.ExecuteCommand:
                    return ExecuteCommandEvent.GetPooled(systemEvent);
                default:// Layout, Ignore, Used
                    return IMGUIEvent.GetPooled(systemEvent);
            }
        }

        static bool DoDispatch(BaseVisualElementPanel panel)
        {
            bool usesEvent;

            if (s_EventInstance.type == EventType.Repaint)
            {
                // If this is an individual repaint event (not part of a RepaintAll then we have to handle sRGBWrite ourselves)
                bool oldSRGBWrite = GL.sRGBWrite;
                if (oldSRGBWrite)
                    GL.sRGBWrite = false;
                panel.Repaint(s_EventInstance);
                if (oldSRGBWrite)
                    GL.sRGBWrite = true;
                // TODO get rid of this when we wrap every GUIView inside IMGUIContainers
                // here we pretend to use the repaint event
                // in order to suspend to suspend OnGUI() processing on the native side
                // since we've already run it if we have an IMGUIContainer
                usesEvent = panel.IMGUIContainersCount > 0;
            }
            else
            {
                panel.ValidateLayout();

                using (EventBase evt = CreateEvent(s_EventInstance))
                {
                    // DispatchEvent changes s_EventInstance.mousePosition.
                    s_EventDispatcher.DispatchEvent(evt, panel);
                    s_EventInstance.mousePosition = evt.originalMousePosition;

                    // FIXME: we dont always have to repaint if evt.isPropagationStopped.
                    if (evt.isPropagationStopped)
                    {
                        panel.visualTree.Dirty(ChangeType.Repaint);
                    }
                    usesEvent = evt.isPropagationStopped;
                }
            }

            return usesEvent;
        }

        internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
        {
            return s_UIElementsCache.GetEnumerator();
        }

        internal static Panel FindOrCreatePanel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null)
        {
            Panel panel;
            if (!s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out panel))
            {
                panel = new Panel(ownerObject, contextType, dataWatch, eventDispatcher);
                s_UIElementsCache.Add(ownerObject.GetInstanceID(), panel);
            }
            else
            {
                Debug.Assert(contextType == panel.contextType, "Context type mismatch");
            }

            return panel;
        }

        internal static Panel FindOrCreatePanel(ScriptableObject ownerObject)
        {
            return FindOrCreatePanel(ownerObject, GetGUIContextType());
        }
    }
}
