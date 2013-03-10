﻿using System;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Fixie.Tests
{
    [TestFixture]
    public class MethodCaseTests
    {
        readonly Type fixtureClass;
        readonly MethodInfo passingMethod;
        readonly MethodInfo failingMethod;

        public MethodCaseTests()
        {
            fixtureClass = typeof(SampleFixture);
            passingMethod = fixtureClass.GetMethod("Pass", BindingFlags.Public | BindingFlags.Instance);
            failingMethod = fixtureClass.GetMethod("Fail", BindingFlags.Public | BindingFlags.Instance);
        }

        [Test]
        public void ShouldBeNamedAfterTheGivenMethod()
        {
            var passingCase = new MethodCase(fixtureClass, passingMethod);
            var failingCase = new MethodCase(fixtureClass, failingMethod);

            passingCase.Name.ShouldBe("Fixie.Tests.MethodCaseTests+SampleFixture.Pass");
            failingCase.Name.ShouldBe("Fixie.Tests.MethodCaseTests+SampleFixture.Fail");
        }

        [Test]
        public void ShouldInvokeTheGivenMethodWhenExecuted()
        {
            var passingCase = new MethodCase(fixtureClass, passingMethod);

            SampleFixture.MethodInvoked = false;
            passingCase.Execute();
            SampleFixture.MethodInvoked.ShouldBe(true);
        }

        [Test]
        public void ShouldReportPassingResultUponSuccessfulExecution()
        {
            var passingCase = new MethodCase(fixtureClass, passingMethod);

            var result = passingCase.Execute();

            result.Passed.ShouldBe(true);
            result.Exception.ShouldBe(null);
        }

        [Test]
        public void ShouldReportFailingResultWithOriginalExceptionUponUnsuccessfulExecution()
        {
            var failingCase = new MethodCase(fixtureClass, failingMethod);

            var result = failingCase.Execute();

            result.Passed.ShouldBe(false);
            result.Exception.ShouldBeTypeOf<MethodInvokedException>();
        }

        class MethodInvokedException : Exception { }

        class SampleFixture
        {
            public static bool MethodInvoked;

            public void Pass()
            {
                MethodInvoked = true;
            }

            public void Fail()
            {
                throw new MethodInvokedException();
            }
        }
    }
}