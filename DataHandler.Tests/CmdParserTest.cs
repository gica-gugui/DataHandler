using DataHandler.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataHandler.Tests
{
    [TestClass]
    public class CmdParserTest
    {
        [DataTestMethod]
        [DataRow("-File", @"..\..\..\Files\Data.tsv", "-SortByStartDate", "-Project", "1")]
        [DataRow("-File", @"..\..\..\Files\Data.tsv", "-SortByStartDate", null, null)]
        [DataRow("-File", @"..\..\..\Files\Data.tsv", null, "-Project", "1")]
        [DataRow("-File", @"..\..\..\Files\Data.tsv", null, null, null)]
        public void CmdParser_Parse_Success(string fileFlag, string fileValue, string sortByDateFlag, string projectFlag, string projectValue)
        {
            var args = new string[] { fileFlag, fileValue, sortByDateFlag, projectFlag, projectValue };

            var result = CmdParser.Parse(args);

            Assert.AreEqual(fileValue, result.FilePath);
            Assert.AreEqual(sortByDateFlag != null, result.SortByDate);
            Assert.AreEqual(projectValue?.ToString(), result.Project?.ToString());
        }

        [DataTestMethod]
        [DataRow(new string[0])]
        [DataRow(new string[] { @"..\..\..\Files\Data.tsv" })]
        [DataRow(new string[] { "-SortByStartDate", "-Project" })]
        public void CmdParser_Parse_FileFlagRequired(string[] args)
        {
            var exception = Assert.ThrowsException<Exception>(() => {
                CmdParser.Parse(args);
            });

            Assert.AreEqual("The flag -File is required.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_FileValueRequired()
        {
            var args = new string[] { "-File" };

            var exception = Assert.ThrowsException<Exception>(() => {
                CmdParser.Parse(args);
            });

            Assert.AreEqual("No value was provided for the argument flag -File.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_InvalidProjectValueType()
        {
            var args = new string[] { "-File", @"..\..\..\Files\Data.tsv", "-Project", "a" };

            var exception = Assert.ThrowsException<Exception>(() => {
                CmdParser.Parse(args);
            });

            Assert.AreEqual("The value a is not valid for flag -Project.", exception.Message);
        }
    }
}
