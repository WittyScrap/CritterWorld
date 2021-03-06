// <copyright file="ArenaTest.cs">Copyright ©  2019</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _100492443.Critters;

namespace _100492443.Critters.Tests
{
    /// <summary>This class contains parameterized unit tests for Arena</summary>
    [PexClass(typeof(Arena))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ArenaTest
    {

    }
}
