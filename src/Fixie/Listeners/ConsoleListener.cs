﻿using System;
using System.Linq;
using System.Reflection;

namespace Fixie.Listeners
{
    public class ConsoleListener : Listener
    {
        public void AssemblyStarted(Assembly assembly)
        {
            Console.WriteLine("------ Testing Assembly {0} ------", assembly.FileName());
            Console.WriteLine();
        }

        public void CasePassed(PassResult result)
        {
        }

        public void CaseFailed(FailResult result)
        {
            var @case = result.Case;
            var exceptions = result.Exceptions;

            using (Foreground.Red)
                Console.WriteLine("Test '{0}' failed: {1}", @case.Name, exceptions.First().GetType().FullName);
            Console.Out.WriteCompoundStackTrace(exceptions);
            Console.WriteLine();
            Console.WriteLine();
        }

        public void AssemblyCompleted(Assembly assembly, AssemblyResult result)
        {
            var assemblyName = typeof(ConsoleListener).Assembly.GetName();
            var name = assemblyName.Name;
            var version = assemblyName.Version;

            Console.WriteLine("{0} passed, {1} failed ({2} {3}).", result.Passed, result.Failed, name, version);
            Console.WriteLine();
        }
    }
}