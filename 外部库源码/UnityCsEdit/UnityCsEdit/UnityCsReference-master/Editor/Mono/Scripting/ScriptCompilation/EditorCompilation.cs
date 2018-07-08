// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEditor.Modules;
using UnityEditor.Compilation;
using UnityEditor.Scripting.Compilers;
using UnityEditorInternal;
using CompilerMessage = UnityEditor.Scripting.Compilers.CompilerMessage;
using CompilerMessageType = UnityEditor.Scripting.Compilers.CompilerMessageType;
using Directory = System.IO.Directory;
using File = System.IO.File;
using IOException = System.IO.IOException;

namespace UnityEditor.Scripting.ScriptCompilation
{
    class EditorCompilation
    {
        public enum CompileStatus
        {
            Idle,
            Compiling,
            CompilationStarted,
            CompilationFailed,
            CompilationComplete
        }

        public enum DeleteFileOptions
        {
            NoLogError = 0,
            LogError = 1,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TargetAssemblyInfo
        {
            public string Name;
            public AssemblyFlags Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AssemblyCompilerMessages
        {
            public string assemblyFilename;
            public CompilerMessage[] messages;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PackageAssembly
        {
            public string DirectoryPath;
            public string Name;
            public bool IncludeTestAssemblies;
        }

        [Flags]
        public enum CompilationSetupErrorFlags
        {
            none = 0,
            cyclicReferences = (1 << 0),
            loadError = (1 << 0)
        }

        abstract class UnitySpecificCompilerMessageProcessor
        {
            public abstract bool IsInterestedInMessage(CompilerMessage m);
            public abstract void PostprocessMessage(ref CompilerMessage m);
        }

        class UnsafeErrorProcessor : UnitySpecificCompilerMessageProcessor
        {
            string unityUnsafeMessage;

            public UnsafeErrorProcessor(ScriptAssembly assembly, EditorCompilation editorCompilation)
            {
                var assemblyName = AssetPath.GetAssemblyNameWithoutExtension(assembly.Filename);

                try
                {
                    var customScriptAssembly = editorCompilation.FindCustomScriptAssemblyFromAssemblyName(assemblyName);
                    unityUnsafeMessage = string.Format("Enable \"Allow 'unsafe' code\" in the inspector for '{0}' to fix this error.", customScriptAssembly.FilePath);
                }
                catch
                {
                    unityUnsafeMessage = "Enable \"Allow 'unsafe' code\" in Player Settings to fix this error.";
                }
            }

            public override bool IsInterestedInMessage(CompilerMessage m)
            {
                return m.type == CompilerMessageType.Error && m.message.Contains("CS0227");
            }

            public override void PostprocessMessage(ref CompilerMessage m)
            {
                m.message += ". " + unityUnsafeMessage;
            }
        }

        class ModuleReferenceErrorProcessor : UnitySpecificCompilerMessageProcessor
        {
            Regex messageRegex;

            public ModuleReferenceErrorProcessor()
            {
                messageRegex = new Regex("`UnityEngine.(\\w*)Module,");
            }

            public override bool IsInterestedInMessage(CompilerMessage m)
            {
                return m.type == CompilerMessageType.Error && (m.message.Contains("CS1069") || m.message.Contains("CS1070"));
            }

            private static string GetNiceDisplayNameForModule(string name)
            {
                for (int i = 1; i < name.Length; i++)
                    if (char.IsLower(name[i - 1]) && !char.IsLower(name[i]))
                    {
                        name = name.Insert(i, " ");
                        i++;
                    }
                return name;
            }

            public override void PostprocessMessage(ref CompilerMessage message)
            {
                var match = messageRegex.Match(message.message);
                if (match.Success)
                {
                    var index = message.message.IndexOf("Consider adding a reference to assembly");
                    if (index != -1)
                        message.message = message.message.Substring(0, index);
                    var moduleName = GetNiceDisplayNameForModule(match.Groups[1].Value);

                    message.message += string.Format("Enable the built in package '{0}' in the Package Manager window to fix this error.", moduleName);
                }
            }
        }

        bool areAllScriptsDirty;
        string projectDirectory = string.Empty;
        string assemblySuffix = string.Empty;
        private HashSet<string> allScripts = new HashSet<string>();
        HashSet<string> dirtyScripts = new HashSet<string>();
        HashSet<string> runScriptUpdaterAssemblies = new HashSet<string>();
        PrecompiledAssembly[] precompiledAssemblies;
        CustomScriptAssembly[] customScriptAssemblies;
        CustomScriptAssembly[] packageCustomScriptAssemblies;
        EditorBuildRules.TargetAssembly[] customTargetAssemblies; // TargetAssemblies for customScriptAssemblies and packageCustomScriptAssemblies.
        PrecompiledAssembly[] unityAssemblies;
        CompilationTask compilationTask;
        string outputDirectory;
        CompilationSetupErrorFlags setupErrorFlags = CompilationSetupErrorFlags.none;
        List<Compilation.AssemblyBuilder> assemblyBuilders = new List<Compilation.AssemblyBuilder>();

        static readonly string EditorTempPath = "Temp";

        public Action<CompilationSetupErrorFlags> setupErrorFlagsChanged;
        private PackageAssembly[] m_PackageAssemblies;
        public event Action<string> assemblyCompilationStarted;
        public event Action<string, UnityEditor.Compilation.CompilerMessage[]> assemblyCompilationFinished;

        static EditorCompilation()
        {
        }

        internal string GetAssemblyTimestampPath(string editorAssemblyPath)
        {
            return AssetPath.Combine(editorAssemblyPath, "BuiltinAssemblies.stamp");
        }

        internal void SetProjectDirectory(string projectDirectory)
        {
            this.projectDirectory = projectDirectory;
        }

        internal void SetAssemblySuffix(string assemblySuffix)
        {
            this.assemblySuffix = assemblySuffix;
        }

        public void SetAllScripts(string[] allScripts)
        {
            this.allScripts = new HashSet<string>(allScripts);

            foreach (var dirtyScript in dirtyScripts)
                this.allScripts.Add(dirtyScript);
        }

        public bool IsExtensionSupportedByCompiler(string extension)
        {
            var languages = ScriptCompilers.SupportedLanguages;
            return languages.Count(l => l.GetExtensionICanCompile() == extension) > 0;
        }

        public string[] GetExtensionsSupportedByCompiler()
        {
            var languages = ScriptCompilers.SupportedLanguages;
            return languages.Select(language => language.GetExtensionICanCompile()).ToArray();
        }

        public void DirtyPredefinedAssemblyScripts(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
        {
            var scriptAssemblySettings = CreateScriptAssemblySettings(platformGroup, platform, options);
            var scriptAssemblies = GetAllScriptAssembliesOfType(scriptAssemblySettings, EditorBuildRules.TargetAssemblyType.Predefined);

            foreach (var assembly in scriptAssemblies)
            {
                foreach (var script in assembly.Files)
                {
                    dirtyScripts.Add(script);
                }
            }
        }

        public void DirtyAllScripts()
        {
            areAllScriptsDirty = true;
        }

        public void DirtyScript(string path)
        {
            allScripts.Add(path);
            dirtyScripts.Add(path);
        }

        public void ClearDirtyScripts()
        {
            dirtyScripts.Clear();
            areAllScriptsDirty = false;
        }

        public void RunScriptUpdaterOnAssembly(string assemblyFilename)
        {
            runScriptUpdaterAssemblies.Add(assemblyFilename);
        }

        public void SetAllUnityAssemblies(PrecompiledAssembly[] unityAssemblies)
        {
            this.unityAssemblies = unityAssemblies;
        }

        public void SetCompileScriptsOutputDirectory(string directory)
        {
            this.outputDirectory = directory;
        }

        public string GetCompileScriptsOutputDirectory()
        {
            if (string.IsNullOrEmpty(outputDirectory))
                throw new Exception("Must set an output directory through SetCompileScriptsOutputDirectory before compiling");
            return outputDirectory;
        }

        public void SetCompilationSetupErrorFlags(CompilationSetupErrorFlags flags)
        {
            var newFlags = setupErrorFlags | flags;

            if (newFlags != setupErrorFlags)
            {
                setupErrorFlags = newFlags;

                if (setupErrorFlagsChanged != null)
                    setupErrorFlagsChanged(setupErrorFlags);
            }
        }

        public void ClearCompilationSetupErrorFlags(CompilationSetupErrorFlags flags)
        {
            var newFlags = setupErrorFlags & ~flags;

            if (newFlags != setupErrorFlags)
            {
                setupErrorFlags = newFlags;

                if (setupErrorFlagsChanged != null)
                    setupErrorFlagsChanged(setupErrorFlags);
            }
        }

        public bool HaveSetupErrors()
        {
            return setupErrorFlags != CompilationSetupErrorFlags.none;
        }

        public void SetAllPrecompiledAssemblies(PrecompiledAssembly[] precompiledAssemblies)
        {
            this.precompiledAssemblies = precompiledAssemblies;
        }

        public PrecompiledAssembly[] GetAllPrecompiledAssemblies()
        {
            return this.precompiledAssemblies;
        }

        public TargetAssemblyInfo[] GetAllCompiledAndResolvedCustomTargetAssemblies()
        {
            if (customTargetAssemblies == null)
                return new TargetAssemblyInfo[0];

            var customTargetAssemblyCompiledPaths = new Dictionary<EditorBuildRules.TargetAssembly, string>();

            foreach (var assembly in customTargetAssemblies)
            {
                var path = assembly.FullPath(outputDirectory, assemblySuffix);

                // Collect all assemblies that have been compiled (exist on file system)
                if (File.Exists(path))
                    customTargetAssemblyCompiledPaths.Add(assembly, path);
            }

            bool removed;

            do
            {
                removed = false;

                if (customTargetAssemblyCompiledPaths.Count > 0)
                {
                    foreach (var assembly in customTargetAssemblies)
                    {
                        if (!customTargetAssemblyCompiledPaths.ContainsKey(assembly))
                            continue;

                        // Check for each compiled assembly that all it's references
                        // have also been compiled. If not, remove it from the list
                        // of compiled assemblies.
                        foreach (var reference in assembly.References)
                        {
                            if (!customTargetAssemblyCompiledPaths.ContainsKey(reference))
                            {
                                customTargetAssemblyCompiledPaths.Remove(assembly);
                                removed = true;
                                break;
                            }
                        }
                    }
                }
            }
            while (removed);

            var count = customTargetAssemblyCompiledPaths.Count;
            var targetAssemblies = new TargetAssemblyInfo[customTargetAssemblyCompiledPaths.Count];
            int index = 0;

            foreach (var entry in customTargetAssemblyCompiledPaths)
            {
                var assembly = entry.Key;
                targetAssemblies[index++] = ToTargetAssemblyInfo(assembly);
            }

            return targetAssemblies;
        }

        static CustomScriptAssembly LoadCustomScriptAssemblyFromJson(string path)
        {
            var json = File.ReadAllText(path);

            try
            {
                var customScriptAssemblyData = CustomScriptAssemblyData.FromJson(json);
                return CustomScriptAssembly.FromCustomScriptAssemblyData(path, customScriptAssemblyData);
            }
            catch (Exception e)
            {
                throw new Compilation.AssemblyDefinitionException(e.Message, path);
            }
        }

        string[] CustomTargetAssembliesToFilePaths(IEnumerable<EditorBuildRules.TargetAssembly> targetAssemblies)
        {
            var customAssemblies = targetAssemblies.Select(a => FindCustomTargetAssemblyFromTargetAssembly(a));
            var filePaths = customAssemblies.Select(a => a.FilePath).ToArray();
            return filePaths;
        }

        string CustomTargetAssembliesToFilePaths(EditorBuildRules.TargetAssembly targetAssembly)
        {
            return FindCustomTargetAssemblyFromTargetAssembly(targetAssembly).FilePath;
        }

        void CheckCyclicAssemblyReferencesDFS(EditorBuildRules.TargetAssembly visitAssembly, HashSet<EditorBuildRules.TargetAssembly> visited)
        {
            if (visited.Contains(visitAssembly))
                throw new Compilation.AssemblyDefinitionException("Assembly with cyclic references detected", CustomTargetAssembliesToFilePaths(visited));

            visited.Add(visitAssembly);

            foreach (var reference in visitAssembly.References)
            {
                if (reference.Filename == visitAssembly.Filename)
                    throw new Compilation.AssemblyDefinitionException("Assembly contains a references to itself", CustomTargetAssembliesToFilePaths(visitAssembly));

                CheckCyclicAssemblyReferencesDFS(reference, visited);
            }

            visited.Remove(visitAssembly);
        }

        void CheckCyclicAssemblyReferences()
        {
            if (customTargetAssemblies == null || customTargetAssemblies.Length < 1)
                return;

            var visited = new HashSet<EditorBuildRules.TargetAssembly>();

            try
            {
                foreach (var assembly in customTargetAssemblies)
                    CheckCyclicAssemblyReferencesDFS(assembly, visited);
            }
            catch (Exception e)
            {
                SetCompilationSetupErrorFlags(CompilationSetupErrorFlags.cyclicReferences);
                throw e;
            }
        }

        void UpdateCustomTargetAssemblies()
        {
            var allCustomScriptAssemblies = new List<CustomScriptAssembly>();

            if (customScriptAssemblies != null)
                allCustomScriptAssemblies.AddRange(customScriptAssemblies);

            if (packageCustomScriptAssemblies != null)
            {
                if (customScriptAssemblies == null)
                {
                    // There are no other custom script assemblies, add default customs script assemblies for all packages.
                    allCustomScriptAssemblies.AddRange(packageCustomScriptAssemblies.Select(a => CustomScriptAssembly.Create(a.Name, a.FilePath)));
                }
                else
                {
                    foreach (var packageCustomScriptAssembly in packageCustomScriptAssemblies)
                    {
                        var packageAssembly = this.m_PackageAssemblies.Single(x => x.Name == packageCustomScriptAssembly.Name);
                        var pathPrefix = packageCustomScriptAssembly.PathPrefix.ToLowerInvariant();

                        // We have found an assembly definition file in the package directory, do not
                        // add a default custom script assembly for the package.
                        var customAssemblyInPackageRoot = customScriptAssemblies.SingleOrDefault(a => a.PathPrefix.ToLowerInvariant() == pathPrefix);
                        if (customAssemblyInPackageRoot != null)
                        {
                            continue;
                        }

                        allCustomScriptAssemblies.Add(CreatePackageCustomScriptAssembly(packageAssembly));
                    }
                }
            }

            foreach (var assembly in allCustomScriptAssemblies)
            {
                try
                {
                    if (m_PackageAssemblies != null && !assembly.PackageAssembly.HasValue)
                    {
                        var pathPrefix = assembly.PathPrefix.ToLowerInvariant();

                        foreach (var packageAssembly in m_PackageAssemblies)
                        {
                            var lower = AssetPath.ReplaceSeparators(packageAssembly.DirectoryPath).ToLowerInvariant();
                            if (pathPrefix.StartsWith(lower))
                            {
                                assembly.PackageAssembly = packageAssembly;
                                break;
                            }
                        }
                    }

                    if (assembly.References != null)
                    {
                        foreach (var reference in assembly.References)
                        {
                            if (!allCustomScriptAssemblies.Any(a => a.Name == reference))
                                throw new Compilation.AssemblyDefinitionException(string.Format("Assembly has reference to non-existent assembly '{0}'", reference), assembly.FilePath);
                        }
                    }
                }
                catch (Exception e)
                {
                    SetCompilationSetupErrorFlags(CompilationSetupErrorFlags.loadError);
                    throw e;
                }
            }

            customTargetAssemblies = EditorBuildRules.CreateTargetAssemblies(allCustomScriptAssemblies);
            ClearCompilationSetupErrorFlags(CompilationSetupErrorFlags.cyclicReferences);
        }

        public Exception[] SetAllCustomScriptAssemblyJsons(string[] paths)
        {
            var assemblies = new List<CustomScriptAssembly>();
            var exceptions = new List<Exception>();

            ClearCompilationSetupErrorFlags(CompilationSetupErrorFlags.loadError);

            foreach (var path in paths)
            {
                var fullPath = AssetPath.IsPathRooted(path) ? AssetPath.GetFullPath(path) : AssetPath.Combine(projectDirectory, path);
                CustomScriptAssembly loadedCustomScriptAssembly = null;

                try
                {
                    loadedCustomScriptAssembly = LoadCustomScriptAssemblyFromJson(fullPath);

                    var duplicates = assemblies.Where(a => string.Equals(a.Name, loadedCustomScriptAssembly.Name, System.StringComparison.OrdinalIgnoreCase));

                    if (duplicates.Any())
                    {
                        var filePaths = new List<string>();
                        filePaths.Add(loadedCustomScriptAssembly.FilePath);
                        filePaths.AddRange(duplicates.Select(a => a.FilePath));
                        throw new Compilation.AssemblyDefinitionException(string.Format("Assembly with name '{0}' already exists", loadedCustomScriptAssembly.Name), filePaths.ToArray());
                    }

                    var samePrefixes = assemblies.Where(a => a.PathPrefix == loadedCustomScriptAssembly.PathPrefix);

                    if (samePrefixes.Any())
                    {
                        var filePaths = new List<string>();
                        filePaths.Add(loadedCustomScriptAssembly.FilePath);
                        filePaths.AddRange(samePrefixes.Select(a => a.FilePath));
                        throw new Compilation.AssemblyDefinitionException(string.Format("Folder '{0}' contains multiple assembly definition files", loadedCustomScriptAssembly.PathPrefix), filePaths.ToArray());
                    }

                    if (loadedCustomScriptAssembly.References == null)
                        loadedCustomScriptAssembly.References = new string[0];

                    if (loadedCustomScriptAssembly.References.Length != loadedCustomScriptAssembly.References.Distinct().Count())
                    {
                        var duplicateRefs = loadedCustomScriptAssembly.References.GroupBy(r => r).Where(g => g.Count() > 0).Select(g => g.Key).ToArray();
                        var duplicateRefsString = string.Join(",", duplicateRefs);

                        throw new Compilation.AssemblyDefinitionException(string.Format("Assembly has duplicate references: {0}",
                                duplicateRefsString),
                            loadedCustomScriptAssembly.FilePath);
                    }
                }
                catch (Exception e)
                {
                    SetCompilationSetupErrorFlags(CompilationSetupErrorFlags.loadError);
                    exceptions.Add(e);
                }

                if (loadedCustomScriptAssembly != null && !assemblies.Any(a => a.Name.Equals(loadedCustomScriptAssembly.Name, StringComparison.OrdinalIgnoreCase)))
                    assemblies.Add(loadedCustomScriptAssembly);
            }

            customScriptAssemblies = assemblies.ToArray();

            try
            {
                UpdateCustomTargetAssemblies();
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }

            return exceptions.ToArray();
        }

        public bool IsPathInPackageDirectory(string path)
        {
            if (m_PackageAssemblies == null)
                return false;
            return m_PackageAssemblies.Any(p => path.StartsWith(p.DirectoryPath, StringComparison.OrdinalIgnoreCase));
        }

        public void SetAllPackageAssemblies(PackageAssembly[] packageAssemblies)
        {
            m_PackageAssemblies = packageAssemblies;
            this.packageCustomScriptAssemblies = m_PackageAssemblies.Select(CreatePackageCustomScriptAssembly).ToArray();
            UpdateCustomTargetAssemblies();
        }

        private static CustomScriptAssembly CreatePackageCustomScriptAssembly(PackageAssembly packageAssembly)
        {
            var customScriptAssembly = CustomScriptAssembly.Create(packageAssembly.Name, AssetPath.ReplaceSeparators(packageAssembly.DirectoryPath));
            customScriptAssembly.PackageAssembly = packageAssembly;
            return customScriptAssembly;
        }

        // Delete all .dll's that aren't used anymore
        public void DeleteUnusedAssemblies()
        {
            string fullEditorAssemblyPath = AssetPath.Combine(projectDirectory, GetCompileScriptsOutputDirectory());

            if (!Directory.Exists(fullEditorAssemblyPath))
                return;

            var deleteFiles = Directory.GetFiles(fullEditorAssemblyPath).Select(f => AssetPath.ReplaceSeparators(f)).ToList();
            string timestampPath = GetAssemblyTimestampPath(GetCompileScriptsOutputDirectory());
            deleteFiles.Remove(AssetPath.Combine(projectDirectory, timestampPath));

            var scriptAssemblies = GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor);

            foreach (var assembly in scriptAssemblies)
            {
                if (assembly.Files.Length > 0)
                {
                    string path = AssetPath.Combine(fullEditorAssemblyPath, assembly.Filename);
                    deleteFiles.Remove(path);
                    deleteFiles.Remove(MDBPath(path));
                    deleteFiles.Remove(PDBPath(path));
                }
            }

            foreach (var path in deleteFiles)
                DeleteFile(path);
        }

        public void CleanScriptAssemblies()
        {
            string fullEditorAssemblyPath = AssetPath.Combine(projectDirectory, GetCompileScriptsOutputDirectory());

            if (!Directory.Exists(fullEditorAssemblyPath))
                return;

            foreach (var path in Directory.GetFiles(fullEditorAssemblyPath))
                DeleteFile(path);
        }

        static void DeleteFile(string path, DeleteFileOptions fileOptions = DeleteFileOptions.LogError)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                if (fileOptions == DeleteFileOptions.LogError)
                    UnityEngine.Debug.LogErrorFormat("Could not delete file '{0}'\n", path);
            }
        }

        static bool MoveOrReplaceFile(string sourcePath, string destinationPath)
        {
            bool fileMoved = true;
            try
            {
                File.Move(sourcePath, destinationPath);
            }
            catch (IOException)
            {
                fileMoved = false;
            }
            if (!fileMoved)
            {
                fileMoved = true;
                var backupFile = destinationPath + ".bak";
                DeleteFile(backupFile, DeleteFileOptions.NoLogError); // Delete any previous backup files.

                try
                {
                    File.Replace(sourcePath, destinationPath, backupFile, true);
                }
                catch (IOException)
                {
                    fileMoved = false;
                }

                // Try to delete backup file. Does not need to exist
                // We will eventually delete the file in DeleteUnusedAssemblies.
                DeleteFile(backupFile, DeleteFileOptions.NoLogError);
            }
            return fileMoved;
        }

        static string PDBPath(string dllPath)
        {
            return dllPath.Replace(".dll", ".pdb");
        }

        static string MDBPath(string dllPath)
        {
            return dllPath + ".mdb";
        }

        static bool CopyAssembly(string sourcePath, string destinationPath)
        {
            if (!MoveOrReplaceFile(sourcePath, destinationPath))
                return false;

            string sourceMdb = MDBPath(sourcePath);
            string destinationMdb = MDBPath(destinationPath);

            if (File.Exists(sourceMdb))
                MoveOrReplaceFile(sourceMdb, destinationMdb);
            else if (File.Exists(destinationMdb))
                DeleteFile(destinationMdb);

            string sourcePdb = PDBPath(sourcePath);
            string destinationPdb = PDBPath(destinationPath);

            if (File.Exists(sourcePdb))
                MoveOrReplaceFile(sourcePdb, destinationPdb);
            else if (File.Exists(destinationPdb))
                DeleteFile(destinationPdb);

            return true;
        }

        public CustomScriptAssembly FindCustomScriptAssemblyFromAssemblyName(string assemblyName)
        {
            assemblyName = AssetPath.GetAssemblyNameWithoutExtension(assemblyName);

            if (customScriptAssemblies != null)
            {
                var result = customScriptAssemblies.FirstOrDefault(a => AssemblyNameWithSuffix(a.Name) == assemblyName);
                if (result != null)
                    return result;
            }

            if (packageCustomScriptAssemblies != null)
                return packageCustomScriptAssemblies.Single(a => AssemblyNameWithSuffix(a.Name) == assemblyName);

            throw new InvalidOperationException();
        }

        internal CustomScriptAssembly FindCustomScriptAssemblyFromScriptPath(string scriptPath)
        {
            var customTargetAssembly = EditorBuildRules.GetCustomTargetAssembly(scriptPath, projectDirectory, customTargetAssemblies);
            var customScriptAssembly = customTargetAssembly != null ? FindCustomScriptAssemblyFromAssemblyName(customTargetAssembly.Filename) : null;

            return customScriptAssembly;
        }

        internal CustomScriptAssembly FindCustomTargetAssemblyFromTargetAssembly(EditorBuildRules.TargetAssembly assembly)
        {
            var assemblyName = AssetPath.GetAssemblyNameWithoutExtension(assembly.Filename);
            return FindCustomScriptAssemblyFromAssemblyName(assemblyName);
        }

        public bool CompileScripts(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
        {
            var scriptAssemblySettings = CreateScriptAssemblySettings(platformGroup, platform, options);

            EditorBuildRules.TargetAssembly[] notCompiledTargetAssemblies = null;
            bool result = CompileScripts(scriptAssemblySettings, EditorTempPath, options, ref notCompiledTargetAssemblies);

            if (notCompiledTargetAssemblies != null)
                foreach (var targetAssembly in notCompiledTargetAssemblies)
                {
                    var customScriptAssembly = customScriptAssemblies.Single(a => a.Name == AssetPath.GetAssemblyNameWithoutExtension(targetAssembly.Filename));

                    var filePath = customScriptAssembly.FilePath;

                    if (filePath.StartsWith(projectDirectory))
                        filePath = filePath.Substring(projectDirectory.Length);

                    UnityEngine.Debug.LogWarning(string.Format("Script assembly '{0}' has not been compiled. Folder containing assembly definition file '{1}' contains script files for different script languages. Folder must only contain script files for one script language.", targetAssembly.Filename, filePath));
                }

            return result;
        }

        private static EditorBuildRules.TargetAssembly[] GetCustomAssembliesNotContainingTests(EditorBuildRules.TargetAssembly[] targetAssemblies)
        {
            return (targetAssemblies ?? Enumerable.Empty<EditorBuildRules.TargetAssembly>()).Where(x => (x.OptionalUnityReferences & OptionalUnityReferences.TestAssemblies) != OptionalUnityReferences.TestAssemblies).ToArray();
        }

        internal bool CompileScripts(ScriptAssemblySettings scriptAssemblySettings, string tempBuildDirectory, EditorScriptCompilationOptions options, ref EditorBuildRules.TargetAssembly[] notCompiledTargetAssemblies)
        {
            DeleteUnusedAssemblies();

            IEnumerable<string> allDirtyScripts = areAllScriptsDirty ? allScripts.ToArray() : dirtyScripts.ToArray();

            areAllScriptsDirty = false;
            dirtyScripts.Clear();

            if (!allDirtyScripts.Any() && runScriptUpdaterAssemblies.Count == 0)
                return false;

            var assemblies = new EditorBuildRules.CompilationAssemblies
            {
                UnityAssemblies = unityAssemblies,
                PrecompiledAssemblies = precompiledAssemblies,
                CustomTargetAssemblies = customTargetAssemblies,
                PredefinedAssembliesCustomTargetReferences = GetCustomAssembliesNotContainingTests(customTargetAssemblies),
                EditorAssemblyReferences = ModuleUtils.GetAdditionalReferencesForUserScripts()
            };

            var args = new EditorBuildRules.GenerateChangedScriptAssembliesArgs
            {
                AllSourceFiles = allScripts,
                DirtySourceFiles = allDirtyScripts,
                ProjectDirectory = projectDirectory,
                Settings = scriptAssemblySettings,
                Assemblies = assemblies,
                RunUpdaterAssemblies = runScriptUpdaterAssemblies
            };

            var scriptAssemblies = EditorBuildRules.GenerateChangedScriptAssemblies(args);

            notCompiledTargetAssemblies = args.NotCompiledTargetAssemblies.ToArray();

            if (!scriptAssemblies.Any())
                return false;

            return CompileScriptAssemblies(scriptAssemblies, scriptAssemblySettings, tempBuildDirectory, options, CompilationTaskOptions.StopOnFirstError);
        }

        internal bool CompileCustomScriptAssemblies(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
        {
            var scriptAssemblySettings = CreateScriptAssemblySettings(platformGroup, platform, options);
            return CompileCustomScriptAssemblies(scriptAssemblySettings, EditorTempPath, options, platformGroup, platform);
        }

        internal bool CompileCustomScriptAssemblies(ScriptAssemblySettings scriptAssemblySettings, string tempBuildDirectory, EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
        {
            var scriptAssemblies = GetAllScriptAssembliesOfType(scriptAssemblySettings, EditorBuildRules.TargetAssemblyType.Custom);
            return CompileScriptAssemblies(scriptAssemblies, scriptAssemblySettings, tempBuildDirectory, options, CompilationTaskOptions.None);
        }

        internal bool CompileScriptAssemblies(ScriptAssembly[] scriptAssemblies, ScriptAssemblySettings scriptAssemblySettings, string tempBuildDirectory, EditorScriptCompilationOptions options, CompilationTaskOptions compilationTaskOptions)
        {
            StopAllCompilation();

            // Do no start compilation if there is an setup error.
            if (setupErrorFlags != CompilationSetupErrorFlags.none)
                return false;

            CheckCyclicAssemblyReferences();

            DeleteUnusedAssemblies();

            if (!Directory.Exists(scriptAssemblySettings.OutputDirectory))
                Directory.CreateDirectory(scriptAssemblySettings.OutputDirectory);

            if (!Directory.Exists(tempBuildDirectory))
                Directory.CreateDirectory(tempBuildDirectory);

            // Compile to tempBuildDirectory
            compilationTask = new CompilationTask(scriptAssemblies, tempBuildDirectory, options, compilationTaskOptions, UnityEngine.SystemInfo.processorCount);

            compilationTask.OnCompilationStarted += (assembly, phase) =>
                {
                    var assemblyOutputPath = AssetPath.Combine(scriptAssemblySettings.OutputDirectory, assembly.Filename);
                    Console.WriteLine("- Starting compile {0}", assemblyOutputPath);
                    InvokeAssemblyCompilationStarted(assemblyOutputPath);
                };

            compilationTask.OnCompilationFinished += (assembly, messages) =>
                {
                    var assemblyOutputPath = AssetPath.Combine(scriptAssemblySettings.OutputDirectory, assembly.Filename);
                    Console.WriteLine("- Finished compile {0}", assemblyOutputPath);

                    if (runScriptUpdaterAssemblies.Contains(assembly.Filename))
                        runScriptUpdaterAssemblies.Remove(assembly.Filename);

                    if (messages.Any(m => m.type == CompilerMessageType.Error))
                    {
                        AddUnitySpecificErrorMessages(assembly, messages);

                        InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
                        return;
                    }

                    var buildingForEditor = scriptAssemblySettings.BuildingForEditor;
                    string enginePath = InternalEditorUtility.GetEngineCoreModuleAssemblyPath();

                    string unetPath = UnityEditor.EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/Networking/UnityEngine.Networking.dll";
                    if (!Serialization.Weaver.WeaveUnetFromEditor(assembly, tempBuildDirectory, tempBuildDirectory, enginePath, unetPath, buildingForEditor))
                    {
                        messages.Add(new CompilerMessage { message = "UNet Weaver failed", type = CompilerMessageType.Error, file = assembly.FullPath, line = -1, column = -1 });
                        StopAllCompilation();
                        InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
                        return;
                    }

                    // Copy from tempBuildDirectory to assembly output directory
                    if (!CopyAssembly(AssetPath.Combine(tempBuildDirectory, assembly.Filename), assembly.FullPath))
                    {
                        messages.Add(new CompilerMessage { message = string.Format("Copying assembly from directory {0} to {1} failed", tempBuildDirectory, assembly.OutputDirectory), type = CompilerMessageType.Error, file = assembly.FullPath, line = -1, column = -1 });
                        StopCompilationTask();
                        InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
                        return;
                    }

                    InvokeAssemblyCompilationFinished(assemblyOutputPath, messages);
                };

            compilationTask.Poll();
            return true;
        }

        void AddUnitySpecificErrorMessages(ScriptAssembly assembly, List<CompilerMessage> messages)
        {
            var processors = new List<UnitySpecificCompilerMessageProcessor>()
            {
                new UnsafeErrorProcessor(assembly, this),
                new ModuleReferenceErrorProcessor()
            };

            if (!messages.Any(m => processors.Any(p => p.IsInterestedInMessage(m))))
                return;

            List<CompilerMessage> newMessages = new List<CompilerMessage>();

            foreach (var message in messages)
            {
                var newMessage = new CompilerMessage(message);
                foreach (var processor in processors)
                {
                    if (processor.IsInterestedInMessage(message))
                        processor.PostprocessMessage(ref newMessage);
                }
                newMessages.Add(newMessage);
            }

            messages.Clear();
            messages.AddRange(newMessages);
        }

        public void InvokeAssemblyCompilationStarted(string assemblyOutputPath)
        {
            if (assemblyCompilationStarted != null)
                assemblyCompilationStarted(assemblyOutputPath);
        }

        public void InvokeAssemblyCompilationFinished(string assemblyOutputPath, List<CompilerMessage> messages)
        {
            if (assemblyCompilationFinished != null)
            {
                var convertedMessages = ConvertCompilerMessages(messages);
                assemblyCompilationFinished(assemblyOutputPath, convertedMessages);
            }
        }

        public bool AreAllScriptsDirty()
        {
            return areAllScriptsDirty;
        }

        public bool DoesProjectFolderHaveAnyDirtyScripts()
        {
            return (areAllScriptsDirty && allScripts.Count > 0) || dirtyScripts.Count > 0;
        }

        public bool DoesProjectFolderHaveAnyScripts()
        {
            return allScripts != null && allScripts.Count > 0;
        }

        public bool DoesProjectHaveAnyCustomScriptAssemblies()
        {
            foreach (var script in allScripts)
            {
                var targetAssembly = EditorBuildRules.GetTargetAssembly(script, projectDirectory, customTargetAssemblies);

                if (targetAssembly.Type == EditorBuildRules.TargetAssemblyType.Custom)
                    return true;
            }

            return false;
        }

        ScriptAssemblySettings CreateScriptAssemblySettings(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, EditorScriptCompilationOptions options)
        {
            var defines = InternalEditorUtility.GetCompilationDefines(options, buildTargetGroup, buildTarget);

            var predefinedAssembliesCompilerOptions = new ScriptCompilerOptions();

            if ((options & EditorScriptCompilationOptions.BuildingPredefinedAssembliesAllowUnsafeCode) == EditorScriptCompilationOptions.BuildingPredefinedAssembliesAllowUnsafeCode)
                predefinedAssembliesCompilerOptions.AllowUnsafeCode = true;

            var settings = new ScriptAssemblySettings
            {
                BuildTarget = buildTarget,
                BuildTargetGroup = buildTargetGroup,
                OutputDirectory = GetCompileScriptsOutputDirectory(),
                Defines = defines,
                ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup),
                CompilationOptions = options,
                PredefinedAssembliesCompilerOptions = predefinedAssembliesCompilerOptions,
                FilenameSuffix = assemblySuffix,
                OptionalUnityReferences = ToOptionalUnityReferences(options),
            };

            return settings;
        }

        ScriptAssemblySettings CreateEditorScriptAssemblySettings(EditorScriptCompilationOptions options)
        {
            return CreateScriptAssemblySettings(EditorUserBuildSettings.activeBuildTargetGroup, EditorUserBuildSettings.activeBuildTarget, options);
        }

        public AssemblyCompilerMessages[] GetCompileMessages()
        {
            if (compilationTask == null)
                return null;

            var result = new AssemblyCompilerMessages[compilationTask.CompilerMessages.Count];

            int index = 0;
            foreach (var entry in compilationTask.CompilerMessages)
            {
                var assembly = entry.Key;
                var messages = entry.Value;

                result[index++] = new AssemblyCompilerMessages { assemblyFilename = assembly.Filename, messages = messages };
            }

            // Sort compiler messages by assemby filename to make the order deterministic.
            Array.Sort(result, (m1, m2) => String.Compare(m1.assemblyFilename, m2.assemblyFilename));

            return result;
        }

        public bool IsCompilationPending()
        {
            // If there were any errors in setting up the compilation, then return false.
            if (setupErrorFlags != CompilationSetupErrorFlags.none)
                return false;

            // If we have dirty scripts or script updater has marked assemblies for updated,
            // then compilation will trigger on next TickCompilationPipeline.
            return DoesProjectFolderHaveAnyDirtyScripts() || runScriptUpdaterAssemblies.Count() > 0;
        }

        public bool IsAnyAssemblyBuilderCompiling()
        {
            if (assemblyBuilders.Count > 0)
            {
                bool isCompiling = false;

                var removeAssemblyBuilders = new List<Compilation.AssemblyBuilder>();

                // Check status of compile tasks
                foreach (var assemblyBuilder in assemblyBuilders)
                {
                    var status = assemblyBuilder.status;

                    if (status == Compilation.AssemblyBuilderStatus.IsCompiling)
                        isCompiling = true;
                    else if (status == Compilation.AssemblyBuilderStatus.Finished)
                        removeAssemblyBuilders.Add(assemblyBuilder);
                }

                // Remove all compile tasks that finished compiling.
                if (removeAssemblyBuilders.Count > 0)
                    assemblyBuilders.RemoveAll(t => removeAssemblyBuilders.Contains(t));

                return isCompiling;
            }

            return false;
        }

        public bool IsCompiling()
        {
            // Native code expects IsCompiling to be true after marking scripts as dirty,
            // therefore return true if the compilation is pending
            return IsCompilationTaskCompiling() || IsCompilationPending() || IsAnyAssemblyBuilderCompiling();
        }

        public bool IsCompilationTaskCompiling()
        {
            return compilationTask != null && compilationTask.IsCompiling;
        }

        public void StopAllCompilation()
        {
            StopCompilationTask();
            compilationTask = null;
        }

        public void StopCompilationTask()
        {
            if (compilationTask == null)
                return;

            compilationTask.Stop();
        }

        internal static OptionalUnityReferences ToOptionalUnityReferences(EditorScriptCompilationOptions editorScriptCompilationOptions)
        {
            var optinalUnityReferences = OptionalUnityReferences.None;

            var buildingIncludingTestAssemblies = (editorScriptCompilationOptions & EditorScriptCompilationOptions.BuildingIncludingTestAssemblies) == EditorScriptCompilationOptions.BuildingIncludingTestAssemblies;
            if (buildingIncludingTestAssemblies)
            {
                optinalUnityReferences |= OptionalUnityReferences.TestAssemblies;
            }
            return optinalUnityReferences;
        }

        public CompileStatus TickCompilationPipeline(EditorScriptCompilationOptions options, BuildTargetGroup platformGroup, BuildTarget platform)
        {
            // Return CompileStatus.Compiling if any compile task is still compiling.
            // This ensures that the compile tasks finish compiling before any
            // scripts in the Assets folder are compiled and a domain reload
            // is triggered.
            if (IsAnyAssemblyBuilderCompiling())
                return CompileStatus.Compiling;

            // If we are not currently compiling and there are dirty scripts, start compilation.
            if (!IsCompilationTaskCompiling() && IsCompilationPending())
            {
                if (CompileScripts(options, platformGroup, platform))
                    return CompileStatus.CompilationStarted;
            }

            return PollCompilation();
        }

        public CompileStatus PollCompilation()
        {
            if (IsCompilationTaskCompiling())
            {
                if (compilationTask.Poll()) // Returns true when compilation finished.
                    return (compilationTask == null || compilationTask.CompileErrors) ? CompileStatus.CompilationFailed : CompileStatus.CompilationComplete;

                return CompileStatus.Compiling;
            }

            return CompileStatus.Idle;
        }

        string AssemblyFilenameWithSuffix(string assemblyFilename)
        {
            if (!string.IsNullOrEmpty(assemblySuffix))
            {
                var basename = AssetPath.GetAssemblyNameWithoutExtension(assemblyFilename);
                return string.Concat(basename, assemblySuffix, ".dll");
            }

            return assemblyFilename;
        }

        string AssemblyNameWithSuffix(string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblySuffix))
            {
                var basename = AssetPath.GetAssemblyNameWithoutExtension(assemblyName);
                return string.Concat(basename, assemblySuffix);
            }

            return assemblyName;
        }

        public TargetAssemblyInfo[] GetTargetAssemblies()
        {
            EditorBuildRules.TargetAssembly[] predefindTargetAssemblies = EditorBuildRules.GetPredefinedTargetAssemblies();

            TargetAssemblyInfo[] targetAssemblyInfo = new TargetAssemblyInfo[predefindTargetAssemblies.Length + (customTargetAssemblies != null ? customTargetAssemblies.Count() : 0)];

            for (int i = 0; i < predefindTargetAssemblies.Length; ++i)
                targetAssemblyInfo[i] = ToTargetAssemblyInfo(predefindTargetAssemblies[i]);

            if (customTargetAssemblies != null)
                for (int i = 0; i < customTargetAssemblies.Count(); ++i)
                {
                    int j = predefindTargetAssemblies.Length + i;
                    targetAssemblyInfo[j] = ToTargetAssemblyInfo(customTargetAssemblies[i]);
                }

            return targetAssemblyInfo;
        }

        public TargetAssemblyInfo[] GetTargetAssembliesWithScripts(EditorScriptCompilationOptions options)
        {
            ScriptAssemblySettings settings = CreateEditorScriptAssemblySettings(EditorScriptCompilationOptions.BuildingForEditor | options);
            var targetAssemblies = EditorBuildRules.GetTargetAssembliesWithScripts(allScripts, projectDirectory, customTargetAssemblies, settings);

            var targetAssemblyInfos = new TargetAssemblyInfo[targetAssemblies.Length];

            for (int i = 0; i < targetAssemblies.Length; ++i)
                targetAssemblyInfos[i] = ToTargetAssemblyInfo(targetAssemblies[i]);

            return targetAssemblyInfos;
        }

        public ScriptAssembly[] GetAllScriptAssembliesForLanguage<T>(EditorScriptCompilationOptions additionalOptions) where T : SupportedLanguage
        {
            var assemblies = GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor).Where(a => a.Language.GetType() == typeof(T)).ToArray();
            return assemblies;
        }

        public ScriptAssembly GetScriptAssemblyForLanguage<T>(string assemblyNameOrPath, EditorScriptCompilationOptions additionalOptions) where T : SupportedLanguage
        {
            var assemblyName = AssetPath.GetAssemblyNameWithoutExtension(assemblyNameOrPath);
            var scriptAssemblies = GetAllScriptAssembliesForLanguage<T>(additionalOptions);
            return scriptAssemblies.SingleOrDefault(a => String.Compare(assemblyName, AssetPath.GetAssemblyNameWithoutExtension(a.Filename), StringComparison.OrdinalIgnoreCase) == 0);
        }

        public EditorBuildRules.TargetAssembly[] GetCustomTargetAssemblies()
        {
            return customTargetAssemblies;
        }

        public PrecompiledAssembly[] GetUnityAssemblies()
        {
            return unityAssemblies;
        }

        public TargetAssemblyInfo GetTargetAssembly(string scriptPath)
        {
            EditorBuildRules.TargetAssembly targetAssembly = EditorBuildRules.GetTargetAssembly(scriptPath, projectDirectory, customTargetAssemblies);
            TargetAssemblyInfo targetAssemblyInfo = ToTargetAssemblyInfo(targetAssembly);
            return targetAssemblyInfo;
        }

        public EditorBuildRules.TargetAssembly GetTargetAssemblyDetails(string scriptPath)
        {
            return EditorBuildRules.GetTargetAssembly(scriptPath, projectDirectory, customTargetAssemblies);
        }

        public ScriptAssembly[] GetAllEditorScriptAssemblies(EditorScriptCompilationOptions additionalOptions)
        {
            return GetAllScriptAssemblies(EditorScriptCompilationOptions.BuildingForEditor | EditorScriptCompilationOptions.BuildingIncludingTestAssemblies | additionalOptions);
        }

        ScriptAssembly[] GetAllScriptAssemblies(EditorScriptCompilationOptions options)
        {
            return GetAllScriptAssemblies(options, unityAssemblies, precompiledAssemblies);
        }

        public ScriptAssembly[] GetAllScriptAssemblies(EditorScriptCompilationOptions options, PrecompiledAssembly[] unityAssembliesArg, PrecompiledAssembly[] precompiledAssembliesArg)
        {
            ScriptAssemblySettings settings = CreateEditorScriptAssemblySettings(options);

            var assemblies = new EditorBuildRules.CompilationAssemblies
            {
                UnityAssemblies = unityAssembliesArg,
                PrecompiledAssemblies = precompiledAssembliesArg,
                CustomTargetAssemblies = customTargetAssemblies,
                PredefinedAssembliesCustomTargetReferences = GetCustomAssembliesNotContainingTests(customTargetAssemblies),
                EditorAssemblyReferences = ModuleUtils.GetAdditionalReferencesForUserScripts()
            };

            return EditorBuildRules.GetAllScriptAssemblies(allScripts, projectDirectory, settings, assemblies);
        }

        ScriptAssembly[] GetAllScriptAssembliesOfType(ScriptAssemblySettings settings, EditorBuildRules.TargetAssemblyType type)
        {
            var assemblies = new EditorBuildRules.CompilationAssemblies
            {
                UnityAssemblies = unityAssemblies,
                PrecompiledAssemblies = precompiledAssemblies,
                CustomTargetAssemblies = customTargetAssemblies,
                PredefinedAssembliesCustomTargetReferences = GetCustomAssembliesNotContainingTests(customTargetAssemblies),
                EditorAssemblyReferences = ModuleUtils.GetAdditionalReferencesForUserScripts()
            };

            return EditorBuildRules.GetAllScriptAssemblies(allScripts, projectDirectory, settings, assemblies, type);
        }

        public MonoIsland[] GetAllMonoIslands(EditorScriptCompilationOptions additionalOptions)
        {
            return GetAllMonoIslands(unityAssemblies, precompiledAssemblies, EditorScriptCompilationOptions.BuildingForEditor | EditorScriptCompilationOptions.BuildingIncludingTestAssemblies | additionalOptions);
        }

        public MonoIsland[] GetAllMonoIslands(PrecompiledAssembly[] unityAssembliesArg, PrecompiledAssembly[] precompiledAssembliesArg, EditorScriptCompilationOptions options)
        {
            var scriptAssemblies = GetAllScriptAssemblies(options, unityAssembliesArg, precompiledAssembliesArg);
            var monoIslands = new MonoIsland[scriptAssemblies.Length];

            for (int i = 0; i < scriptAssemblies.Length; ++i)
                monoIslands[i] = scriptAssemblies[i].ToMonoIsland(EditorScriptCompilationOptions.BuildingForEditor, EditorTempPath);

            return monoIslands;
        }

        public bool IsRuntimeScriptAssembly(string assemblyNameOrPath)
        {
            var assemblyFilename = AssetPath.GetFileName(assemblyNameOrPath);

            if (!assemblyFilename.EndsWith(".dll"))
                assemblyFilename += ".dll";

            var predefinedAssemblyTargets = EditorBuildRules.GetPredefinedTargetAssemblies();

            if (predefinedAssemblyTargets.Any(a => ((a.Flags & AssemblyFlags.EditorOnly) != AssemblyFlags.EditorOnly) && a.Filename == assemblyFilename))
                return true;

            if (customTargetAssemblies != null && customTargetAssemblies.Any(a => ((a.Flags & AssemblyFlags.EditorOnly) != AssemblyFlags.EditorOnly) && a.Filename == assemblyFilename))
                return true;

            return false;
        }

        TargetAssemblyInfo ToTargetAssemblyInfo(EditorBuildRules.TargetAssembly targetAssembly)
        {
            TargetAssemblyInfo targetAssemblyInfo = new TargetAssemblyInfo();

            targetAssemblyInfo.Name = AssemblyFilenameWithSuffix(targetAssembly.Filename);
            targetAssemblyInfo.Flags = targetAssembly.Flags;

            return targetAssemblyInfo;
        }

        static EditorScriptCompilationOptions ToEditorScriptCompilationOptions(Compilation.AssemblyBuilderFlags flags)
        {
            EditorScriptCompilationOptions options = EditorScriptCompilationOptions.BuildingEmpty;

            if ((flags & Compilation.AssemblyBuilderFlags.DevelopmentBuild) == Compilation.AssemblyBuilderFlags.DevelopmentBuild)
                options |= EditorScriptCompilationOptions.BuildingDevelopmentBuild;

            if ((flags & Compilation.AssemblyBuilderFlags.EditorAssembly) == Compilation.AssemblyBuilderFlags.EditorAssembly)
                options |= EditorScriptCompilationOptions.BuildingForEditor;

            return options;
        }

        static AssemblyFlags ToAssemblyFlags(Compilation.AssemblyBuilderFlags assemblyBuilderFlags)
        {
            AssemblyFlags assemblyFlags = AssemblyFlags.None;

            if ((assemblyBuilderFlags & Compilation.AssemblyBuilderFlags.EditorAssembly) == Compilation.AssemblyBuilderFlags.EditorAssembly)
                assemblyFlags |= AssemblyFlags.EditorOnly;

            return assemblyFlags;
        }

        ScriptAssembly InitializeScriptAssemblyWithoutReferencesAndDefines(Compilation.AssemblyBuilder assemblyBuilder)
        {
            var scriptFiles = assemblyBuilder.scriptPaths.Select(p => AssetPath.Combine(projectDirectory, p)).ToArray();
            var assemblyPath = AssetPath.Combine(projectDirectory, assemblyBuilder.assemblyPath);

            var scriptAssembly = new ScriptAssembly();
            scriptAssembly.Flags = ToAssemblyFlags(assemblyBuilder.flags);
            scriptAssembly.BuildTarget = assemblyBuilder.buildTarget;
            scriptAssembly.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(assemblyBuilder.buildTargetGroup);
            scriptAssembly.Language = ScriptCompilers.GetLanguageFromExtension(ScriptCompilers.GetExtensionOfSourceFile(assemblyBuilder.scriptPaths[0]));
            scriptAssembly.Files = scriptFiles;
            scriptAssembly.Filename = AssetPath.GetFileName(assemblyPath);
            scriptAssembly.OutputDirectory = AssetPath.GetDirectoryName(assemblyPath);
            scriptAssembly.CompilerOptions = assemblyBuilder.compilerOptions;
            scriptAssembly.ScriptAssemblyReferences = new ScriptAssembly[0];

            return scriptAssembly;
        }

        public ScriptAssembly CreateScriptAssembly(Compilation.AssemblyBuilder assemblyBuilder)
        {
            var scriptAssembly = InitializeScriptAssemblyWithoutReferencesAndDefines(assemblyBuilder);

            var options = ToEditorScriptCompilationOptions(assemblyBuilder.flags);

            var references = GetDefaultReferences(scriptAssembly, options);

            if (assemblyBuilder.additionalReferences != null && assemblyBuilder.additionalReferences.Length > 0)
                references = references.Concat(assemblyBuilder.additionalReferences).ToArray();

            if (assemblyBuilder.excludeReferences != null && assemblyBuilder.excludeReferences.Length > 0)
                references = references.Where(r => !assemblyBuilder.excludeReferences.Contains(r)).ToArray();

            var defines = GetDefaultDefines(assemblyBuilder);

            if (assemblyBuilder.additionalDefines != null)
                defines = defines.Concat(assemblyBuilder.additionalDefines).ToArray();

            scriptAssembly.References = references.ToArray();
            scriptAssembly.Defines = defines.ToArray();

            return scriptAssembly;
        }

        string[] GetDefaultReferences(ScriptAssembly scriptAssembly, EditorScriptCompilationOptions options)
        {
            bool buildingForEditor = (scriptAssembly.Flags & AssemblyFlags.EditorOnly) == AssemblyFlags.EditorOnly;

            var unityReferences = EditorBuildRules.GetUnityReferences(scriptAssembly, unityAssemblies, options);
            var customReferences = EditorBuildRules.GetCompiledCustomAssembliesReferences(scriptAssembly, customTargetAssemblies, GetCompileScriptsOutputDirectory(), assemblySuffix);
            var precompiledReferences = EditorBuildRules.GetPrecompiledReferences(scriptAssembly, EditorBuildRules.TargetAssemblyType.Custom, options, EditorBuildRules.EditorCompatibility.CompatibleWithEditor, precompiledAssemblies);
            var additionalReferences = MonoLibraryHelpers.GetSystemLibraryReferences(scriptAssembly.ApiCompatibilityLevel, scriptAssembly.BuildTarget, scriptAssembly.Language, buildingForEditor, scriptAssembly.Filename);
            string[] editorReferences = buildingForEditor ? ModuleUtils.GetAdditionalReferencesForUserScripts() : new string[0];

            var references = unityReferences.Concat(customReferences).Concat(precompiledReferences).Concat(editorReferences).Concat(additionalReferences);

            return references.ToArray();
        }

        public string[] GetDefaultReferences(AssemblyBuilder assemblyBuilder)
        {
            var scriptAssembly = InitializeScriptAssemblyWithoutReferencesAndDefines(assemblyBuilder);
            var options = ToEditorScriptCompilationOptions(assemblyBuilder.flags);

            var references = GetDefaultReferences(scriptAssembly, options);

            return references;
        }

        public string[] GetDefaultDefines(AssemblyBuilder assemblyBuilder)
        {
            var options = ToEditorScriptCompilationOptions(assemblyBuilder.flags);
            var defines = InternalEditorUtility.GetCompilationDefines(options, assemblyBuilder.buildTargetGroup, assemblyBuilder.buildTarget);
            return defines;
        }

        public void AddAssemblyBuilder(UnityEditor.Compilation.AssemblyBuilder assemblyBuilder)
        {
            assemblyBuilders.Add(assemblyBuilder);
        }

        public static UnityEditor.Compilation.CompilerMessage[] ConvertCompilerMessages(List<CompilerMessage> messages)
        {
            var newMessages = new UnityEditor.Compilation.CompilerMessage[messages.Count];

            int index = 0;
            foreach (var message in messages)
            {
                var newMessage = new UnityEditor.Compilation.CompilerMessage();

                newMessage.message = message.message;
                newMessage.file = message.file;
                newMessage.line = message.line;
                newMessage.column = message.column;

                switch (message.type)
                {
                    case CompilerMessageType.Error:
                        newMessage.type = UnityEditor.Compilation.CompilerMessageType.Error;
                        break;

                    case CompilerMessageType.Warning:
                        newMessage.type = UnityEditor.Compilation.CompilerMessageType.Warning;
                        break;
                }

                newMessages[index++] = newMessage;
            }

            return newMessages;
        }
    }
}
