using Emeraldwalk.Emeraldwalk_VsFileMirror.Model.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emeraldwalk.VsFileMirror_UnitTests
{
    [TestClass]
    public class FilePathServiceTester
    {
        private IFilePathService Instance { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Instance = new FilePathService(null);
        }

        [TestMethod]
        public void GetProperCasePath_Returns_Proper_Case_Path()
        {
            //arrange
            string expected = "Test.txt";
            File.Delete(expected);
            File.CreateText(expected);

            string lowerCase = Path.GetFullPath(expected).ToLower();
            string upperCase = lowerCase.ToUpper();
            string expectedFullPath = Path.GetFullPath(expected);
            
            //act
            string actualFromLower = this.Instance.FixFilePathCasing(lowerCase);
            string actualFromUpper = this.Instance.FixFilePathCasing(upperCase);

            //assert
            Assert.AreEqual(expectedFullPath, actualFromLower);
            Assert.AreEqual(expectedFullPath, actualFromUpper);
        }

        //[TestMethod]
        //public void FixFilePathCasing_UncPath_Returns_Proper_Casee()
        //{
        //    string uncPath = @"\\SomeServer\SomeShare\SomePath\test.txt";
        //    string upperCase = uncPath.ToUpper();

        //    string actual = this.Instance.FixFilePathCasing(upperCase);

        //    Assert.AreEqual(uncPath, actual);
        //}
    }
}
