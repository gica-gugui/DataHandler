using DataHandler.Data.Models;
using DataHandler.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;

namespace DataHandler.Tests
{
    [TestClass]
    public class InputParserTest
    {
        [TestMethod]
        public void CmdParser_Parse_Success()
        {
            var path = @"..\..\..\Files\Data.tsv";

            var output = InputParser.Parse(path);

            var properties = typeof(Input)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            Assert.IsTrue(output.Headers.Values.Count == properties.Length);
            Assert.IsTrue(output.Headers.Select(h => h.Key).Except(properties.Select(p => p.Name)).Count() == 0);
            Assert.IsTrue(output.Lines.Count == 8);
            Assert.AreEqual(null, output.Lines[0].SavingsAmount);
            Assert.AreEqual(null, output.Lines[0].Currency);
            Assert.AreEqual("Black and white logo paper", output.Lines[7].Description);
            Assert.AreEqual((decimal)4880.199567, output.Lines[7].SavingsAmount);
        }

        [DataTestMethod]
        [DataRow(@"..\Data.tsv")]
        [DataRow("")]
        public void CmdParser_Parse_NoFileAtPath(string path)
        {
            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("No file was found at the provided input file path.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_NoHeaderInDataFile()
        {
            var path = @"..\..\..\Files\Data.Empty.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The input file is empty.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_NoProjectInHeader()
        {
            var path = @"..\..\..\Files\Data.Header.NoProject.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The header must have a column named Project.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_DuplicateProjectInHeader()
        {
            var path = @"..\..\..\Files\Data.Header.DuplicateProject.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The header has a duplicate column named Project.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_LessValuesThanInHeader()
        {
            var path = @"..\..\..\Files\Data.Values.LessThanHeader.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The input line has a different number of values than the header.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_InvalidValueForInt()
        {
            var path = @"..\..\..\Files\Data.Values.InvalidProject.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The value aaa is not valid for column Project.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_InvalidValueForDateTime()
        {
            var path = @"..\..\..\Files\Data.Values.InvalidStartDate.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The value aaa is not valid for column Start date.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_InvalidValueForDecimal()
        {
            var path = @"..\..\..\Files\Data.Values.InvalidSavingsAmount.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The value aaa is not valid for column Savings amount.", exception.Message);
        }

        [TestMethod]
        public void CmdParser_Parse_InvalidValueForEnum()
        {
            var path = @"..\..\..\Files\Data.Values.InvalidComplexity.tsv";

            var exception = Assert.ThrowsException<Exception>(() => {
                InputParser.Parse(path);
            });

            Assert.AreEqual("The value NoComplexity is not valid for column Complexity.", exception.Message);
        }
    }
}
