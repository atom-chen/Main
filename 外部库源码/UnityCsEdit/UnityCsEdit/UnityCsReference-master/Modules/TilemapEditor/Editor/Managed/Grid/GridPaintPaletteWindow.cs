// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using Event = UnityEngine.Event;
using Object = UnityEngine.Object;

namespace UnityEditor
{
    internal class GridPaintPaletteWindow : EditorWindow
    {
        static class MouseStyles
        {
            // The following paths match the enums in OperatingSystemFamily
            public static readonly string[] mouseCursorOSPath =
            {
                "", // Other OS
                "Cursors/macOS",
                "Cursors/Windows",
                "Cursors/Linux",
            };
            // The following paths match the enums in OperatingSystemFamily
            public static readonly Vector2[] mouseCursorOSHotspot =
            {
                Vector2.zero, // Other OS
                new Vector2(6f, 4f),
                new Vector2(6f, 4f),
                new Vector2(6f, 4f),
            };
            // The following paths match the enums in sceneViewEditModes above
            public static readonly string[] mouseCursorTexturePaths =
            {
                "",
                "Grid.MoveTool.png",
                "Grid.PaintTool.png",
                "Grid.BoxTool.png",
                "Grid.PickingTool.png",
                "Grid.EraserTool.png",
                "Grid.FillTool.png",
            };
            public static readonly Texture2D[] mouseCursorTextures;
            static MouseStyles()
            {
                mouseCursorTextures = new Texture2D[mouseCursorTexturePaths.Length];
                int osIndex = (int)SystemInfo.operatingSystemFamily;
                for (int i = 0; i < mouseCursorTexturePaths.Length; ++i)
                {
                    if ((mouseCursorOSPath[osIndex] != null && mouseCursorOSPath[osIndex].Length > 0)
                        && (mouseCursorTexturePaths[i] != null && mouseCursorTexturePaths[i].Length > 0))
                    {
                        string cursorPath = Utils.Paths.Combine(mouseCursorOSPath[osIndex], mouseCursorTexturePaths[i]);
                        mouseCursorTextures[i] = EditorGUIUtility.LoadRequired(cursorPath) as Texture2D;
                    }
                    else
                        mouseCursorTextures[i] = null;
                }
            }
        }

        static class Styles
        {
            public static readonly GUIContent[] toolContents =
            {
                EditorGUIUtility.IconContent("Grid.Default", "|Select an area of the grid (S)"),
                EditorGUIUtility.IconContent("Grid.MoveTool", "|Move selection with active brush (M)"),
                EditorGUIUtility.IconContent("Grid.PaintTool", "|Paint with active brush (B)"),
                EditorGUIUtility.IconContent("Grid.BoxTool", "|Paint a filled box with active brush (U)"),
                EditorGUIUtility.IconContent("Grid.PickingTool", "|Pick or marquee select new brush (Ctrl)."),
                EditorGUIUtility.IconContent("Grid.EraserTool", "|Erase with active brush (Shift)"),
                EditorGUIUtility.IconContent("Grid.FillTool", "|Flood fill with active brush (G)")
            };


            public static readonly GUIContent emptyProjectInfo = EditorGUIUtility.TrTextContent("Create a new palette in the dropdown above.");
            public static readonly GUIContent emptyPaletteInfo = EditorGUIUtility.TrTextContent("Drag Tile, Sprite or Sprite Texture assets here.");
            public static readonly GUIContent invalidPaletteInfo = EditorGUIUtility.TrTextContent("This is an invalid palette. Did you delete the palette asset?");
            public static readonly GUIContent invalidGridInfo = EditorGUIUtility.TrTextContent("The palette has an invalid Grid. Did you add a Grid to the palette asset?");
            public static readonly GUIContent selectPaintTarget = EditorGUIUtility.TrTextContent("Select Paint Target");
            public static readonly GUIContent selectPalettePrefab = EditorGUIUtility.TrTextContent("Select Palette Prefab");
            public static readonly GUIContent selectTileAsset = EditorGUIUtility.TrTextContent("Select Tile Asset");
            public static readonly GUIContent unlockPaletteEditing = EditorGUIUtility.TrTextContent("Unlock Palette Editing");
            public static readonly GUIContent lockPaletteEditing = EditorGUIUtility.TrTextContent("Lock Palette Editing");
            public static readonly GUIContent createNewPalette = EditorGUIUtility.TrTextContent("Create New Palette");
            public static readonly GUIContent focusLabel = EditorGUIUtility.TrTextContent("Focus On");
            public static readonly GUIContent rendererOverlayTitleLabel = EditorGUIUtility.TrTextContent("Tilemap");
            public static readonly GUIContent activeTargetLabel = EditorGUIUtility.TrTextContent("Active Tilemap", "Specifies the currently active Tilemap used for painting in the Scene View.");

            public static readonly GUIContent edit = EditorGUIUtility.TrTextContent("Edit");
            public static readonly GUIContent editModified = EditorGUIUtility.TrTextContent("Edit*");
            public static readonly GUIStyle ToolbarStyle = "preToolbar";
            public static readonly GUIStyle ToolbarTitleStyle = "preToolbar";
            public static float toolbarWidth;

            static Styles()
            {
                GUIStyle toolbarStyle = "Command";
                toolbarWidth = toolContents.Sum(x => toolbarStyle.CalcSize(x).x);
            }
        }

        static readonly EditMode.SceneViewEditMode[] k_SceneViewEditModes =
        {
            EditMode.SceneViewEditMode.GridSelect,
            EditMode.SceneViewEditMode.GridMove,
            EditMode.SceneViewEditMode.GridPainting,
            EditMode.SceneViewEditMode.GridBox,
            EditMode.SceneViewEditMode.GridPicking,
            EditMode.SceneViewEditMode.GridEraser,
            EditMode.SceneViewEditMode.GridFloodFill
        };

        private const float k_DropdownWidth = 200f;
        private const float k_ActiveTargetLabelWidth = 90f;
        private const float k_ActiveTargetDropdownWidth = 130f;
        private const float k_TopAreaHeight = 95f;
        private const float k_MinBrushInspectorHeight = 50f;
        private const float k_MinClipboardHeight = 200f;
        private const float k_ToolbarHeight = 17f;
        private const float k_ResizerDragRectPadding = 10f;

        public static readonly GUIContent tilePalette = EditorGUIUtility.TrTextContent("Tile Palette");

        private PaintableSceneViewGrid m_PaintableSceneViewGrid;
        public PaintableGrid paintableSceneViewGrid { get { return m_PaintableSceneViewGrid; } }

        static void ToggleEditMode(EditMode.SceneViewEditMode mode)
        {
            if (EditMode.editMode != mode)
                EditMode.ChangeEditMode(mode, GridPaintingState.instance);
            else
                EditMode.QuitEditMode();
        }

        class ShortcutContext : IShortcutToolContext
        {
            public bool active { get; set; }
        }

        ShortcutContext m_ShortcutContext = new ShortcutContext { active = true };

        [FormerlyPrefKeyAs("Grid Painting/Select", "s")]
        [Shortcut("Grid Painting/Select", typeof(ShortcutContext), "s")]
        static void GridSelectKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridSelect);
        }

        [FormerlyPrefKeyAs("Grid Painting/Move", "m")]
        [Shortcut("Grid Painting/Move", typeof(ShortcutContext), "m")]
        static void GridMoveKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridMove);
        }

        [FormerlyPrefKeyAs("Grid Painting/Brush", "b")]
        [Shortcut("Grid Painting/Brush", typeof(ShortcutContext), "b")]
        static void GridBrushKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridPainting);
        }

        [FormerlyPrefKeyAs("Grid Painting/Rectangle", "u")]
        [Shortcut("Grid Painting/Rectangle", typeof(ShortcutContext), "u")]
        static void GridRectangleKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridBox);
        }

        [FormerlyPrefKeyAs("Grid Painting/Picker", "i")]
        [Shortcut("Grid Painting/Picker", typeof(ShortcutContext), "i")]
        static void GridPickerKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridPicking);
        }

        [FormerlyPrefKeyAs("Grid Painting/Erase", "d")]
        [Shortcut("Grid Painting/Erase", typeof(ShortcutContext), "d")]
        static void GridEraseKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridEraser);
        }

        [FormerlyPrefKeyAs("Grid Painting/Fill", "g")]
        [Shortcut("Grid Painting/Fill", typeof(ShortcutContext), "g")]
        static void GridFillKey()
        {
            ToggleEditMode(EditMode.SceneViewEditMode.GridFloodFill);
        }

        static void RotateBrush(GridBrush.RotationDirection direction)
        {
            GridPaintingState.gridBrush.Rotate(direction, GridPaintingState.activeGrid.cellLayout);
            GridPaintingState.activeGrid.Repaint();
        }

        [FormerlyPrefKeyAs("Grid Painting/Rotate Clockwise", "[")]
        [Shortcut("Grid Painting/Rotate Clockwise", typeof(ShortcutContext), "[")]
        static void RotateBrushClockwise()
        {
            if (GridPaintingState.gridBrush != null && GridPaintingState.activeGrid != null)
                RotateBrush(GridBrush.RotationDirection.Clockwise);
        }

        [FormerlyPrefKeyAs("Grid Painting/Rotate Anti-Clockwise", "]")]
        [Shortcut("Grid Painting/Rotate Anti-Clockwise", typeof(ShortcutContext), "]")]
        static void RotateBrushAntiClockwise()
        {
            if (GridPaintingState.gridBrush != null && GridPaintingState.activeGrid != null)
                RotateBrush(GridBrush.RotationDirection.CounterClockwise);
        }

        static void FlipBrush(GridBrush.FlipAxis axis)
        {
            GridPaintingState.gridBrush.Flip(axis, GridPaintingState.activeGrid.cellLayout);
            GridPaintingState.activeGrid.Repaint();
        }

        [FormerlyPrefKeyAs("Grid Painting/Flip X", "#[")]
        [Shortcut("Grid Painting/Flip X", typeof(ShortcutContext), "#[")]
        static void FlipBrushX()
        {
            if (GridPaintingState.gridBrush != null && GridPaintingState.activeGrid != null)
                FlipBrush(GridBrush.FlipAxis.X);
        }

        [FormerlyPrefKeyAs("Grid Painting/Flip Y", "#]")]
        [Shortcut("Grid Painting/Flip Y", typeof(ShortcutContext), "#]")]
        static void FlipBrushY()
        {
            if (GridPaintingState.gridBrush != null && GridPaintingState.activeGrid != null)
                FlipBrush(GridBrush.FlipAxis.Y);
        }

        static private List<GridPaintPaletteWindow> s_Instances;
        static public List<GridPaintPaletteWindow> instances
        {
            get
            {
                if (s_Instances == null)
                    s_Instances = new List<GridPaintPaletteWindow>();
                return s_Instances;
            }
        }

        [SerializeField]
        private PreviewResizer m_PreviewResizer;

        [SerializeField] private GameObject m_Palette;
        private GameObject m_PaletteInstance;

        private GridPalettesDropdown m_PaletteDropdown;
        public GameObject palette
        {
            get
            {
                return m_Palette;
            }
            set
            {
                if (m_Palette != value)
                {
                    clipboardView.OnBeforePaletteSelectionChanged();
                    m_Palette = value;
                    clipboardView.OnAfterPaletteSelectionChanged();
                    TilemapEditorUserSettings.lastUsedPalette = m_Palette;
                }
            }
        }

        public GameObject paletteInstance
        {
            get
            {
                return m_PaletteInstance;
            }
        }

        public GridPaintPaletteClipboard clipboardView { get; private set; }

        [SerializeField]
        public GameObject m_Target;

        private Vector2 m_BrushScroll;
        private GridBrushEditorBase m_PreviousToolActivatedEditor;
        private GridBrushBase.Tool m_PreviousToolActivated;

        private PreviewRenderUtility m_PreviewUtility;
        public PreviewRenderUtility previewUtility
        {
            get
            {
                if (m_PreviewUtility == null)
                    InitPreviewUtility();

                return m_PreviewUtility;
            }
        }

        private void OnSelectionChange()
        {
            // Update active palette if user has selected a palette prefab
            var selectedObject = Selection.activeGameObject;
            if (selectedObject != null)
            {
                bool isPrefab = EditorUtility.IsPersistent(selectedObject) || (selectedObject.hideFlags & HideFlags.NotEditable) != 0;
                if (isPrefab)
                {
                    var assetPath = AssetDatabase.GetAssetPath(selectedObject);
                    var allAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                    foreach (var asset in allAssets)
                    {
                        if (asset.GetType() == typeof(GridPalette))
                        {
                            var targetPalette = (GameObject)AssetDatabase.LoadMainAssetAtPath(assetPath);
                            if (targetPalette != palette)
                            {
                                palette = targetPalette;
                                Repaint();
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            HandleContextMenu();

            EditorGUILayout.BeginVertical();
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            float leftMargin = (Screen.width / EditorGUIUtility.pixelsPerPoint - Styles.toolbarWidth) * 0.5f;
            GUILayout.Space(leftMargin);
            EditMode.DoInspectorToolbar(k_SceneViewEditModes, Styles.toolContents, GridPaintingState.instance);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(leftMargin);
            DoActiveTargetsGUI();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(6f);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            Rect clipboardToolbarRect = EditorGUILayout.BeginHorizontal(GUIContent.none, "Toolbar");
            DoClipboardHeader();
            EditorGUILayout.EndHorizontal();
            Rect dragRect = new Rect(k_DropdownWidth + k_ResizerDragRectPadding, 0, position.width - k_DropdownWidth - k_ResizerDragRectPadding, k_ToolbarHeight);
            float brushInspectorSize = m_PreviewResizer.ResizeHandle(position, k_MinBrushInspectorHeight, k_MinClipboardHeight, k_ToolbarHeight, dragRect);
            float clipboardHeight = position.height - brushInspectorSize - k_TopAreaHeight;
            Rect clipboardRect = new Rect(0f, clipboardToolbarRect.yMax, position.width, clipboardHeight);
            OnClipboardGUI(clipboardRect);
            EditorGUILayout.EndVertical();

            GUILayout.Space(clipboardRect.height);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(GUIContent.none, "Toolbar");
            DoBrushesDropdown();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            m_BrushScroll = GUILayout.BeginScrollView(m_BrushScroll, false, false);
            GUILayout.Space(5f);
            OnBrushInspectorGUI();
            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            Color oldColor = Handles.color;
            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(0, clipboardRect.yMax + 0.5f, 0), new Vector3(Screen.width, clipboardRect.yMax + 0.5f, 0));
            Handles.color = Color.black.AlphaMultiplied(0.33f);
            Handles.DrawLine(new Vector3(0, GUILayoutUtility.GetLastRect().yMax + 0.5f, 0), new Vector3(Screen.width, GUILayoutUtility.GetLastRect().yMax + 0.5f, 0));
            Handles.color = oldColor;

            EditorGUILayout.BeginVertical();

            GUILayout.Space(2f);

            EditorGUILayout.EndVertical();

            // Keep repainting until all previews are loaded
            if (AssetPreview.IsLoadingAssetPreviews(GetInstanceID()))
                Repaint();

            // Release keyboard focus on click to empty space
            if (Event.current.type == EventType.MouseDown)
                GUIUtility.keyboardControl = 0;
        }

        public void ResetPreviewInstance()
        {
            if (m_PreviewUtility == null)
                InitPreviewUtility();

            DestroyPreviewInstance();
            if (palette != null)
            {
                m_PaletteInstance = previewUtility.InstantiatePrefabInScene(palette);

                // Prevent palette from overriding the prefab while it is active, unless user saves the palette
                PrefabUtility.DisconnectPrefabInstance(m_PaletteInstance);

                EditorUtility.InitInstantiatedPreviewRecursive(m_PaletteInstance);
                m_PaletteInstance.transform.position = new Vector3(0, 0, 0);
                m_PaletteInstance.transform.rotation = Quaternion.identity;
                m_PaletteInstance.transform.localScale = Vector3.one;

                string assetPath = AssetDatabase.GetAssetPath(palette);
                GridPalette paletteAsset = AssetDatabase.LoadAssetAtPath<GridPalette>(assetPath);
                if (paletteAsset != null)
                {
                    if (paletteAsset.cellSizing == GridPalette.CellSizing.Automatic)
                    {
                        Grid grid = m_PaletteInstance.GetComponent<Grid>();
                        if (grid != null)
                        {
                            grid.cellSize = GridPaletteUtility.CalculateAutoCellSize(grid, grid.cellSize);
                        }
                        else
                        {
                            Debug.LogWarning("Grid component not found from: " + assetPath);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("GridPalette subasset not found from: " + assetPath);
                }

                foreach (var renderer in m_PaletteInstance.GetComponentsInChildren<Renderer>())
                {
                    renderer.gameObject.layer = Camera.PreviewCullingLayer;
                    renderer.allowOcclusionWhenDynamic = false;
                }

                foreach (var transform in m_PaletteInstance.GetComponentsInChildren<Transform>())
                    transform.gameObject.hideFlags = HideFlags.HideAndDontSave;

                PreviewRenderUtility.SetEnabledRecursive(m_PaletteInstance, false);

                clipboardView.ResetPreviewMesh();
            }
        }

        public void DestroyPreviewInstance()
        {
            if (m_PaletteInstance != null)
                DestroyImmediate(m_PaletteInstance);
        }

        public void InitPreviewUtility()
        {
            m_PreviewUtility = new PreviewRenderUtility(true, true);
            m_PreviewUtility.camera.cullingMask = 1 << Camera.PreviewCullingLayer;
            m_PreviewUtility.camera.gameObject.layer = Camera.PreviewCullingLayer;
            m_PreviewUtility.lights[0].gameObject.layer = Camera.PreviewCullingLayer;
            m_PreviewUtility.camera.orthographic = true;
            m_PreviewUtility.camera.orthographicSize = 5f;
            m_PreviewUtility.camera.transform.position = new Vector3(0f, 0f, -10f);
            m_PreviewUtility.ambientColor = new Color(1f, 1f, 1f, 0);

            ResetPreviewInstance();
            clipboardView.SetupPreviewCameraOnInit();
        }

        private void HandleContextMenu()
        {
            if (Event.current.type == EventType.ContextClick)
            {
                DoContextMenu();
                Event.current.Use();
            }
        }

        public void SavePalette()
        {
            if (paletteInstance != null && palette != null)
            {
                GridPaintingState.savingPalette = true;
                SetHideFlagsRecursivelyIgnoringTilemapChildren(paletteInstance, HideFlags.HideInHierarchy);
                PrefabUtility.ReplacePrefab(paletteInstance, palette, ReplacePrefabOptions.ReplaceNameBased);
                SetHideFlagsRecursivelyIgnoringTilemapChildren(paletteInstance, HideFlags.HideAndDontSave);
                GridPaintingState.savingPalette = false;
            }
        }

        private void SetHideFlagsRecursivelyIgnoringTilemapChildren(GameObject root, HideFlags flags)
        {
            root.hideFlags = flags;
            // case 944661: Ignore all child game objects instantiated by a Tilemap component in the palette
            if (root.GetComponent<Tilemap>() == null)
            {
                for (int i = 0; i < root.transform.childCount; i++)
                    SetHideFlagsRecursivelyIgnoringTilemapChildren(root.transform.GetChild(i).gameObject, flags);
            }
        }

        private void DoContextMenu()
        {
            GenericMenu pm = new GenericMenu();
            if (GridPaintingState.scenePaintTarget != null)
                pm.AddItem(Styles.selectPaintTarget, false, SelectPaintTarget);
            else
                pm.AddDisabledItem(Styles.selectPaintTarget);

            if (palette != null)
                pm.AddItem(Styles.selectPalettePrefab, false, SelectPaletteAsset);
            else
                pm.AddDisabledItem(Styles.selectPalettePrefab);

            if (clipboardView.activeTile != null)
                pm.AddItem(Styles.selectTileAsset, false, SelectTileAsset);
            else
                pm.AddDisabledItem(Styles.selectTileAsset);

            pm.AddSeparator("");

            if (clipboardView.unlocked)
                pm.AddItem(Styles.lockPaletteEditing, false, FlipLocked);
            else
                pm.AddItem(Styles.unlockPaletteEditing, false, FlipLocked);

            pm.ShowAsContext();
        }

        private void FlipLocked()
        {
            clipboardView.unlocked = !clipboardView.unlocked;
        }

        private void SelectPaintTarget()
        {
            Selection.activeObject = GridPaintingState.scenePaintTarget;
        }

        private void SelectPaletteAsset()
        {
            Selection.activeObject = palette;
        }

        private void SelectTileAsset()
        {
            Selection.activeObject = clipboardView.activeTile;
        }

        private bool NotOverridingColor(GridBrush defaultGridBrush)
        {
            foreach (var cell in defaultGridBrush.cells)
            {
                TileBase tile = cell.tile;
                if (tile is Tile && ((tile as Tile).flags & TileFlags.LockColor) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void DoBrushesDropdown()
        {
            GUIContent content = GUIContent.Temp(GridPaintingState.gridBrush.name);
            if (EditorGUILayout.DropdownButton(content, FocusType.Passive, EditorStyles.toolbarPopup, GUILayout.Width(k_DropdownWidth)))
            {
                var menuData = new GridBrushesDropdown.MenuItemProvider();
                var flexibleMenu = new GridBrushesDropdown(menuData, GridPaletteBrushes.brushes.IndexOf(GridPaintingState.gridBrush), null, SelectBrush, k_DropdownWidth);
                PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), flexibleMenu);
            }
        }

        private void SelectBrush(int i, object o)
        {
            GridPaintingState.gridBrush = GridPaletteBrushes.brushes[i];
        }

        public void OnEnable()
        {
            instances.Add(this);
            if (clipboardView == null)
            {
                clipboardView = CreateInstance<GridPaintPaletteClipboard>();
                clipboardView.owner = this;
                clipboardView.hideFlags = HideFlags.HideAndDontSave;
                clipboardView.unlocked = false;
            }

            if (m_PaintableSceneViewGrid == null)
            {
                m_PaintableSceneViewGrid = CreateInstance<PaintableSceneViewGrid>();
                m_PaintableSceneViewGrid.hideFlags = HideFlags.HideAndDontSave;
            }

            GridPaletteBrushes.FlushCache();
            EditMode.editModeStarted += OnEditModeStart;
            EditMode.editModeEnded += OnEditModeEnd;
            GridSelection.gridSelectionChanged += OnGridSelectionChanged;
            GridPaintingState.RegisterPainterInterest(this);
            GridPaintingState.scenePaintTargetChanged += OnScenePaintTargetChanged;
            GridPaintingState.brushChanged += OnBrushChanged;
            SceneView.onSceneGUIDelegate += OnSceneViewGUI;
            PrefabUtility.prefabInstanceUpdated += PrefabInstanceUpdated;

            AssetPreview.SetPreviewTextureCacheSize(256, GetInstanceID());
            wantsMouseMove = true;
            wantsMouseEnterLeaveWindow = true;

            if (m_PreviewResizer == null)
            {
                m_PreviewResizer = new PreviewResizer();
                m_PreviewResizer.Init("TilemapBrushInspector");
            }

            minSize = new Vector2(240f, 200f);

            if (palette == null && TilemapEditorUserSettings.lastUsedPalette != null && GridPalettes.palettes.Contains(TilemapEditorUserSettings.lastUsedPalette))
            {
                palette = TilemapEditorUserSettings.lastUsedPalette;
            }

            Tools.onToolChanged += ToolChanged;

            ShortcutIntegration.instance.contextManager.RegisterToolContext(m_ShortcutContext);
        }

        private void PrefabInstanceUpdated(GameObject updatedPrefab)
        {
            // case 947462: Reset the palette instance after its prefab has been updated as it could have been changed
            if (m_PaletteInstance != null && PrefabUtility.GetCorrespondingObjectFromSource(updatedPrefab) == m_Palette && !GridPaintingState.savingPalette)
            {
                ResetPreviewInstance();
                Repaint();
            }
        }

        private void OnBrushChanged(GridBrushBase brush)
        {
            DisableFocus();
            if (brush is GridBrush)
                EnableFocus();
            SceneView.RepaintAll();
        }

        private void OnGridSelectionChanged()
        {
            Repaint();
        }

        private void ToolChanged(Tool from, Tool to)
        {
            if (to != Tool.None && PaintableGrid.InGridEditMode())
                EditMode.QuitEditMode();

            Repaint();
        }

        public void OnDisable()
        {
            CallOnToolDeactivated();
            instances.Remove(this);
            if (instances.Count <= 1)
                GridPaintingState.gridBrush = null;
            DestroyPreviewInstance();
            DestroyImmediate(clipboardView);
            DestroyImmediate(m_PaintableSceneViewGrid);

            if (m_PreviewUtility != null)
                m_PreviewUtility.Cleanup();
            m_PreviewUtility = null;

            if (PaintableGrid.InGridEditMode())
                EditMode.QuitEditMode();

            EditMode.editModeStarted -= OnEditModeStart;
            EditMode.editModeEnded -= OnEditModeEnd;
            Tools.onToolChanged -= ToolChanged;
            GridSelection.gridSelectionChanged -= OnGridSelectionChanged;
            SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
            GridPaintingState.scenePaintTargetChanged -= OnScenePaintTargetChanged;
            GridPaintingState.brushChanged -= OnBrushChanged;
            GridPaintingState.UnregisterPainterInterest(this);
            PrefabUtility.prefabInstanceUpdated -= PrefabInstanceUpdated;

            ShortcutIntegration.instance.contextManager.DeregisterToolContext(m_ShortcutContext);
        }

        private void OnScenePaintTargetChanged(GameObject scenePaintTarget)
        {
            DisableFocus();
            EnableFocus();
            Repaint();
        }

        public void ChangeToTool(GridBrushBase.Tool tool)
        {
            EditMode.ChangeEditMode(PaintableGrid.BrushToolToEditMode(tool), new Bounds(Vector3.zero, Vector3.positiveInfinity), GridPaintingState.instance);
            Repaint();
        }

        public void OnEditModeStart(IToolModeOwner owner, EditMode.SceneViewEditMode editMode)
        {
            if (GridPaintingState.gridBrush != null && PaintableGrid.InGridEditMode() && GridPaintingState.activeBrushEditor != null)
            {
                GridBrushBase.Tool tool = PaintableGrid.EditModeToBrushTool(editMode);
                GridPaintingState.activeBrushEditor.OnToolActivated(tool);
                m_PreviousToolActivatedEditor = GridPaintingState.activeBrushEditor;
                m_PreviousToolActivated = tool;

                for (int i = 0; i < k_SceneViewEditModes.Length; ++i)
                {
                    if (k_SceneViewEditModes[i] == editMode)
                    {
                        Cursor.SetCursor(MouseStyles.mouseCursorTextures[i],
                            MouseStyles.mouseCursorTextures[i] != null ? MouseStyles.mouseCursorOSHotspot[(int)SystemInfo.operatingSystemFamily] : Vector2.zero,
                            CursorMode.Auto);
                        break;
                    }
                }
            }

            Repaint();
        }

        public void OnEditModeEnd(IToolModeOwner owner)
        {
            if (EditMode.editMode != EditMode.SceneViewEditMode.GridMove && EditMode.editMode != EditMode.SceneViewEditMode.GridSelect)
            {
                GridSelection.Clear();
            }

            CallOnToolDeactivated();
            Repaint();
        }

        private void CallOnToolDeactivated()
        {
            if (GridPaintingState.gridBrush != null && m_PreviousToolActivatedEditor != null)
            {
                m_PreviousToolActivatedEditor.OnToolDeactivated(m_PreviousToolActivated);
                m_PreviousToolActivatedEditor = null;

                if (!PaintableGrid.InGridEditMode())
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        private void OnBrushInspectorGUI()
        {
            var brush = GridPaintingState.gridBrush;
            if (brush == null)
                return;

            EditorGUI.BeginChangeCheck();
            if (GridPaintingState.activeBrushEditor != null)
                GridPaintingState.activeBrushEditor.OnPaintInspectorGUI();
            else if (GridPaintingState.fallbackEditor != null)
                GridPaintingState.fallbackEditor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                GridPaletteBrushes.ActiveGridBrushAssetChanged();
            }
        }

        private void DoActiveTargetsGUI()
        {
            bool hasPaintTarget = GridPaintingState.scenePaintTarget != null;
            using (new EditorGUI.DisabledScope(!hasPaintTarget || GridPaintingState.validTargets == null))
            {
                GUILayout.Label(Styles.activeTargetLabel, GUILayout.Width(k_ActiveTargetLabelWidth));
                GUIContent content = GUIContent.Temp(hasPaintTarget ? GridPaintingState.scenePaintTarget.name : "Nothing");
                if (EditorGUILayout.DropdownButton(content, FocusType.Passive, EditorStyles.popup, GUILayout.Width(k_ActiveTargetDropdownWidth)))
                {
                    int index = hasPaintTarget ? Array.IndexOf(GridPaintingState.validTargets, GridPaintingState.scenePaintTarget) : 0;
                    var menuData = new GridPaintTargetsDropdown.MenuItemProvider();
                    var flexibleMenu = new GridPaintTargetsDropdown(menuData, index, null, SelectTarget, k_ActiveTargetDropdownWidth);
                    PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), flexibleMenu);
                }
            }
        }

        private void SelectTarget(int i, object o)
        {
            GridPaintingState.scenePaintTarget = (o as GameObject);
            if (GridPaintingState.scenePaintTarget != null)
                EditorGUIUtility.PingObject(GridPaintingState.scenePaintTarget);
        }

        private void DoClipboardHeader()
        {
            if (!GridPalettes.palettes.Contains(palette) || palette == null) // Palette not in list means it was deleted
            {
                GridPalettes.CleanCache();
                if (GridPalettes.palettes.Count > 0)
                {
                    palette = GridPalettes.palettes.LastOrDefault();
                }
            }

            EditorGUILayout.BeginHorizontal();
            DoPalettesDropdown();
            using (new EditorGUI.DisabledScope(palette == null))
            {
                clipboardView.unlocked = GUILayout.Toggle(clipboardView.unlocked,
                        clipboardView.isModified ? Styles.editModified : Styles.edit,
                        EditorStyles.toolbarButton);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DoPalettesDropdown()
        {
            string name = palette != null ? palette.name : Styles.createNewPalette.text;
            Rect rect = GUILayoutUtility.GetRect(GUIContent.Temp(name), EditorStyles.toolbarDropDown, GUILayout.Width(k_DropdownWidth));
            if (GridPalettes.palettes.Count == 0)
            {
                if (EditorGUI.DropdownButton(rect, GUIContent.Temp(name), FocusType.Passive, EditorStyles.toolbarDropDown))
                {
                    OpenAddPalettePopup(rect);
                }
            }
            else
            {
                GUIContent content = GUIContent.Temp(GridPalettes.palettes.Count > 0 && palette != null ? palette.name : Styles.createNewPalette.text);
                if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, EditorStyles.toolbarPopup))
                {
                    var menuData = new GridPalettesDropdown.MenuItemProvider();
                    m_PaletteDropdown = new GridPalettesDropdown(menuData, GridPalettes.palettes.IndexOf(palette), null, SelectPalette, k_DropdownWidth);
                    PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), m_PaletteDropdown);
                }
            }
        }

        private void SelectPalette(int i, object o)
        {
            if (i < GridPalettes.palettes.Count)
            {
                palette = GridPalettes.palettes[i];
            }
            else
            {
                m_PaletteDropdown.editorWindow.Close();
                OpenAddPalettePopup(new Rect(0, 0, 0, 0));
            }
        }

        private void OpenAddPalettePopup(Rect rect)
        {
            bool popupOpened = GridPaletteAddPopup.ShowAtPosition(rect, this);
            if (popupOpened)
                GUIUtility.ExitGUI();
        }

        private void OnClipboardGUI(Rect position)
        {
            if (Event.current.type != EventType.Layout && position.Contains(Event.current.mousePosition) && GridPaintingState.activeGrid != clipboardView)
            {
                GridPaintingState.activeGrid = clipboardView;
                SceneView.RepaintAll();
            }

            // Validate palette (case 1017965)
            GUIContent paletteError = null;
            if (palette == null)
            {
                if (GridPalettes.palettes.Count == 0)
                    paletteError = Styles.emptyProjectInfo;
                else
                    paletteError = Styles.invalidPaletteInfo;
            }
            else if (palette.GetComponent<Grid>() == null)
            {
                paletteError = Styles.invalidGridInfo;
            }

            if (paletteError != null)
            {
                Color old = GUI.color;
                GUI.color = Color.gray;
                GUI.Label(new Rect(position.center.x - GUI.skin.label.CalcSize(paletteError).x * .5f, position.center.y, 500, 100), paletteError);
                GUI.color = old;
                return;
            }

            bool oldEnabled = GUI.enabled;
            GUI.enabled = !clipboardView.showNewEmptyClipboardInfo || DragAndDrop.objectReferences.Length > 0;

            if (Event.current.type == EventType.Repaint)
                clipboardView.guiRect = position;

            EditorGUI.BeginChangeCheck();
            clipboardView.OnGUI();
            if (EditorGUI.EndChangeCheck())
                Repaint();

            GUI.enabled = oldEnabled;

            if (clipboardView.showNewEmptyClipboardInfo)
            {
                Color old = GUI.color;
                GUI.color = Color.gray;
                Rect rect = new Rect(position.center.x - GUI.skin.label.CalcSize(Styles.emptyPaletteInfo).x * .5f, position.center.y, 500, 100);
                GUI.Label(rect, Styles.emptyPaletteInfo);
                GUI.color = old;
            }
        }

        private void OnSceneViewGUI(SceneView sceneView)
        {
            if (GridPaintingState.defaultBrush != null && GridPaintingState.scenePaintTarget != null)
                SceneViewOverlay.Window(Styles.rendererOverlayTitleLabel, DisplayFocusMode, (int)SceneViewOverlay.Ordering.TilemapRenderer, SceneViewOverlay.WindowDisplayOption.OneWindowPerTitle);
            else if (TilemapEditorUserSettings.focusMode != TilemapEditorUserSettings.FocusMode.None)
            {
                // case 946284: Disable Focus if focus mode is set but there is nothing to focus on
                DisableFocus();
                TilemapEditorUserSettings.focusMode = TilemapEditorUserSettings.FocusMode.None;
            }
        }

        private void DisplayFocusMode(Object displayTarget, SceneView sceneView)
        {
            var oldFocus = TilemapEditorUserSettings.focusMode;
            var focus = (TilemapEditorUserSettings.FocusMode)EditorGUILayout.EnumPopup(Styles.focusLabel, oldFocus);
            if (focus != oldFocus)
            {
                DisableFocus();
                TilemapEditorUserSettings.focusMode = focus;
                EnableFocus();
            }
        }

        private void EnableFocus()
        {
            switch (TilemapEditorUserSettings.focusMode)
            {
                case TilemapEditorUserSettings.FocusMode.Tilemap:
                {
                    if (SceneView.lastActiveSceneView != null)
                        SceneView.lastActiveSceneView.SetSceneViewFiltering(true);
                    HierarchyProperty.FilterSingleSceneObject(GridPaintingState.scenePaintTarget.GetInstanceID(), false);
                    break;
                }
                case TilemapEditorUserSettings.FocusMode.Grid:
                {
                    Tilemap tilemap = GridPaintingState.scenePaintTarget.GetComponent<Tilemap>();
                    if (tilemap != null && tilemap.layoutGrid != null)
                    {
                        if (SceneView.lastActiveSceneView != null)
                            SceneView.lastActiveSceneView.SetSceneViewFiltering(true);
                        HierarchyProperty.FilterSingleSceneObject(tilemap.layoutGrid.gameObject.GetInstanceID(), false);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private void DisableFocus()
        {
            if (TilemapEditorUserSettings.focusMode == TilemapEditorUserSettings.FocusMode.None)
                return;

            HierarchyProperty.ClearSceneObjectsFilter();

            if (SceneView.lastActiveSceneView != null)
                SceneView.lastActiveSceneView.SetSceneViewFiltering(false);
        }

        [MenuItem("Window/2D/Tile Palette", false, 2)]
        public static void OpenTilemapPalette()
        {
            GridPaintPaletteWindow w = GetWindow<GridPaintPaletteWindow>();
            w.titleContent = tilePalette;
        }

        // TODO: Better way of clearing caches than AssetPostprocessor
        public class AssetProcessor : AssetPostprocessor
        {
            public override int GetPostprocessOrder()
            {
                return int.MaxValue;
            }

            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
            {
                if (!GridPaintingState.savingPalette)
                {
                    foreach (var window in GridPaintPaletteWindow.instances)
                    {
                        window.ResetPreviewInstance();
                    }
                }
            }
        }

        public class PaletteAssetModificationProcessor : AssetModificationProcessor
        {
            static string[] OnWillSaveAssets(string[] paths)
            {
                if (!GridPaintingState.savingPalette)
                {
                    foreach (var window in GridPaintPaletteWindow.instances)
                    {
                        if (window.clipboardView.isModified)
                        {
                            window.clipboardView.SavePaletteIfNecessary();
                            window.Repaint();
                        }
                    }
                }
                return paths;
            }
        }
    }
}
