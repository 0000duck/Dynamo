﻿using System;
using System.IO;
using System.Reflection;

using Dynamo.Utilities;

using DynamoUtilities;

namespace RevitTestServices
{
    public static class AssemblyResolver
    {
        private static bool resolverSetup;
        public static void Setup()
        {
            if (resolverSetup) return;

            DynamoPathManager.Instance.InitializeCore(GetDynamoRootDirectory());
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyHelper.ResolveAssembly;

            resolverSetup = true;
        }

        internal static string GetDynamoRootDirectory()
        {
            var assemPath = Assembly.GetExecutingAssembly().Location;
            var assemDir = new DirectoryInfo(Path.GetDirectoryName(assemPath));
            return assemDir.Parent.FullName;
        }

    }
    
}
 