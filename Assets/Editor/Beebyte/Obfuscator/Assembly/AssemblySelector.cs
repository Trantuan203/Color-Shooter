﻿/*
 * Copyright (c) 2018-2023 Beebyte Limited. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
#if UNITY_2017_3_OR_NEWER
using UnityEditor.Compilation;
#endif
using UnityEngine;

namespace Beebyte.Obfuscator.Assembly
{
    public class AssemblySelector
    {
        private readonly HashSet<string> _compiledAssemblyPaths = new HashSet<string>();
        private readonly HashSet<string> _assemblyPaths = new HashSet<string>();

        public AssemblySelector(Options options)
        {
            if (options == null) throw new ArgumentException("options must not be null", "options");
            if (options.compiledAssemblies == null)
                throw new ArgumentException(
                    "options.compiledAssemblies must not be null", "options");
            if (options.assemblies == null)
                throw new ArgumentException(
                    "options.assemblies must not be null", "options");
            if (Application.dataPath == null) throw new ArgumentException("Application.dataPath must not be null");

            foreach (string assemblyName in options.compiledAssemblies)
            {
                string location = FindDllLocation(assemblyName);
                if (location != null)
                {
                    _compiledAssemblyPaths.Add(location);
                }
            }

            foreach (string assemblyName in options.assemblies)
            {
                string location = FindDllLocation(assemblyName);
                if (location != null)
                {
                    _assemblyPaths.Add(location);
                }
            }

#if UNITY_2017_3_OR_NEWER
            if (!options.includeCompilationPipelineAssemblies) return;

            string projectDir = Path.GetDirectoryName(Application.dataPath);
            
#if UNITY_2021_3_OR_NEWER
            string[] nativeCompiledFileNames = GetNativeCompiledFileNames();
            string[] packageDllFileNames = GetPackageDllFileNames();
            string[] additionalAssembliesToObfuscate = Directory.GetFiles(
#if UNITY_2022_1_OR_NEWER
                    projectDir + "/Library/Bee/PlayerScriptAssemblies/")
#else
                    projectDir + "/Library/PlayerScriptAssemblies/")
#endif
                .Where(path => path.EndsWith(".dll"))
                .Where(path => !Path.GetFileName(path).Contains("-firstpass"))
                .Where(path => !Path.GetFileName(path).StartsWith("Unity."))
                .Where(path => !Path.GetFileName(path).StartsWith("UnityEngine."))
                .Where(path => !nativeCompiledFileNames.Contains(Path.GetFileName(path)))
                .Where(path => !packageDllFileNames.Contains(Path.GetFileName(path)))
                .Select(Path.GetFullPath)
                .ToArray();
            _assemblyPaths = Enumerable.ToHashSet(_assemblyPaths.Select(Path.GetFullPath));
            _assemblyPaths.UnionWith(additionalAssembliesToObfuscate);
#else
#if UNITY_2018_1_OR_NEWER
#if UNITY_2019_3_OR_NEWER
            foreach (UnityEditor.Compilation.Assembly assembly in CompilationPipeline.GetAssemblies(AssembliesType
                .PlayerWithoutTestAssemblies))
#else
            foreach (UnityEditor.Compilation.Assembly assembly in CompilationPipeline.GetAssemblies(AssembliesType.Player))
#endif
            {
#else
            foreach (UnityEditor.Compilation.Assembly assembly in CompilationPipeline.GetAssemblies())
            {
                if ((assembly.flags & AssemblyFlags.EditorAssembly) != 0)
                {
                    continue;
                }
#endif
                if (assembly.name.Contains("-firstpass"))
                {
                    continue;
                }

                if (assembly.sourceFiles.Length == 0)
                {
                    continue;
                }

                if (assembly.sourceFiles[0].StartsWith("Packages"))
                {
                    continue;
                }

		string scriptDllLocation = Path.Combine(projectDir, assembly.outputPath).Replace('\\', '/');
#if !UNITY_2019_2_OR_NEWER || UNITY_2019_2_0 || UNITY_2019_2_1 || UNITY_2019_2_2 || UNITY_2019_2_3 || UNITY_2019_2_4 || UNITY_2019_2_5 || UNITY_2019_2_6 || UNITY_2019_2_7
                string dllLocation = scriptDllLocation;
#else
                string dllLocation = ConvertToPlayerAssemblyLocationIfPresentOrElseNull(scriptDllLocation) ?? scriptDllLocation;
#endif

                // If the assembly is for a different build target platform, oddly it will still be in the compilation
                // pipeline, however the file won't actually exist.
                if (dllLocation != null && File.Exists(dllLocation))
                {
                    _assemblyPaths.Add(dllLocation);
                }
            }
#endif
#endif
        }

        private string[] GetNativeCompiledFileNames()
        {
            return CompilationPipeline.GetAssemblies()
                .Where(assembly => (assembly.sourceFiles.Length == 0))
                .Select(assembly => assembly.outputPath)
                .Select(Path.GetFileName)
                .ToArray();
        }

        private string[] GetPackageDllFileNames()
        {
            return CompilationPipeline.GetAssemblies()
                .Where(assembly => assembly.sourceFiles.Length != 0)
                .Where(assembly => assembly.sourceFiles[0].StartsWith("Packages"))
                .Select(assembly => assembly.outputPath)
                .Select(Path.GetFileName)
                .ToArray();
        }

        public ICollection<string> GetCompiledAssemblyPaths()
        {
            return _compiledAssemblyPaths;
        }

        public ICollection<string> GetAssemblyPaths()
        {
            return _assemblyPaths;
        }

        private static string FindDllLocation(string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                throw new ArgumentException(
                    "Empty or null DLL names are forbidden (check Obfuscator Options assemblies / compiled assemblies list)");
            }

            string compiledAssemblyLocation = GetCompiledAssemblyLocation(suffix);

#if !UNITY_2019_2_OR_NEWER || UNITY_2019_2_0 || UNITY_2019_2_1 || UNITY_2019_2_2 || UNITY_2019_2_3 || UNITY_2019_2_4 || UNITY_2019_2_5 || UNITY_2019_2_6 || UNITY_2019_2_7
            return compiledAssemblyLocation;
#else
            return ConvertToPlayerAssemblyLocationIfPresentOrElseNull(compiledAssemblyLocation) ?? compiledAssemblyLocation;
#endif
        }

        private static string GetCompiledAssemblyLocation(string suffix)
        {
            if (IsAPath(suffix))
            {
                var path = GetFileOnKnownPath(suffix);
                if (path != null)
                {
                    return path;
                }
            }

            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (assembly.Location.Equals(string.Empty))
                    {
                        DisplayFailedAssemblyParseWarning(assembly);
                    }
                    else if (assembly.Location.EndsWith(suffix))
                    {
                        return assembly.Location.Replace('\\', '/');
                    }
                }
                catch (NotSupportedException)
                {
                    DisplayFailedAssemblyParseWarning(assembly);
                }
            }

            Debug.LogWarning(
                suffix + " was not found (check Obfuscator Options assemblies / compiled assemblies list)");
            return null;
        }

        private static bool IsAPath(string s)
        {
            return s.Contains("/") || s.Contains("\\");
        }

        private static string GetFileOnKnownPath(string path)
        {
            var fromAssetFolder = Application.dataPath + Path.DirectorySeparatorChar.ToString() + path;
            if (File.Exists(fromAssetFolder))
            {
                return fromAssetFolder;
            }

            if (File.Exists(path))
            {
                return path;
            }

            return null;
        }

        private static string ConvertToPlayerAssemblyLocationIfPresentOrElseNull(string location)
        {
            if (location == null)
            {
                return null;
            }

            string beeScriptAssemblyLocation =
                Regex.Replace(location, "/ScriptAssemblies/", "/Bee/PlayerScriptAssemblies/");
            string playerScriptAssemblyLocation =
                Regex.Replace(location, "/ScriptAssemblies/", "/PlayerScriptAssemblies/");
            return File.Exists(beeScriptAssemblyLocation) ? beeScriptAssemblyLocation :
                File.Exists(playerScriptAssemblyLocation) ? playerScriptAssemblyLocation : null;
        }

        private static void DisplayFailedAssemblyParseWarning(System.Reflection.Assembly assembly)
        {
            Debug.LogWarning("Could not parse dynamically created assembly (string.Empty location) " +
                             assembly.FullName +
                             ". If you extend classes from within this assembly that in turn extend from " +
                             "MonoBehaviour you will need to manually annotate these classes with [Skip]");
        }
    }
}
