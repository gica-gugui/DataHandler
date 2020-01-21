using DataHandler.Data.Enums;
using DataHandler.Data.Models;
using DataHandler.Handler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataHandler.Tests
{
    [TestClass]
    public class FilterParserTest
    {
        private readonly Output output;

        public FilterParserTest()
        {
            output = new Output
            {
                Headers = new Dictionary<string, Header>
                {
                    ["Description"] = new Header { Index = 0, Description = "Description" },
                    ["Project"] = new Header { Index = 1, Description = "Project" },
                    ["StartDate"] = new Header { Index = 2, Description = "Start date" },
                    ["Category"] = new Header { Index = 3, Description = "Category" },
                    ["Responsible"] = new Header { Index = 4, Description = "Responsible" },
                    ["SavingsAmount"] = new Header { Index = 5, Description = "Savings amount" },
                    ["Currency"] = new Header { Index = 6, Description = "Currency" },
                    ["Complexity"] = new Header { Index = 7, Description = "Complexity" }
                },
                Lines = new List<Input>
                {
                    new Input { Project = 2, Description = "Harmonize Lactobacillus acidophilus sourcing", StartDate = new DateTime(2014, 01, 01), Category = "Dairy", Responsible = "Daisy Milks", Complexity = Complexity.Simple },
                    new Input { Project = 3, Description = "Substitute Crème fraîche with evaporated milk in ice-cream products", StartDate = new DateTime(2013, 01, 01), Category = "Dairy", Responsible = "Daisy Milks", SavingsAmount = (decimal)141415.942696, Currency = "EUR", Complexity = Complexity.Moderate }
                }
            };
        }

        [TestMethod]
        public void FilterHandler_Filter_HeadersImmutable()
        {
            var args = new CmdArguments { FilePath = @"..\..\..\Files\Data.tsv", SortByDate = true, Project = 2 };

            var result = FilterHandler.Filter(output, args);

            Assert.AreSame(output.Headers, result.Headers);
        }

        [TestMethod]
        public void FilterHandler_Filter_SortSuccess()
        {
            var args = new CmdArguments { FilePath = @"..\..\..\Files\Data.tsv", SortByDate = true };

            var result = FilterHandler.Filter(output, args);

            Assert.IsTrue(result.Lines.Count == 2);
            Assert.IsTrue(result.Lines.First().Project == 3);
            Assert.IsTrue(result.Lines.Last().Project == 2);
        }

        [TestMethod]
        public void FilterHandler_Filter_FilterSuccess()
        {
            var args = new CmdArguments { FilePath = @"..\..\..\Files\Data.tsv", SortByDate = false, Project = 2 };

            var result = FilterHandler.Filter(output, args);

            Assert.IsTrue(result.Lines.Count == 1);
            Assert.IsTrue(result.Lines.First().Project == 2);
        }

        [TestMethod]
        public void FilterHandler_Filter_FilterAndSortNoEffect()
        {
            var args = new CmdArguments { FilePath = @"..\..\..\Files\Data.tsv", SortByDate = false, Project = null };

            var result = FilterHandler.Filter(output, args);

            Assert.IsTrue(result.Lines.Count == output.Lines.Count);
            Assert.IsTrue(result.Lines.First().Project == output.Lines.First().Project);
            Assert.IsTrue(result.Lines.Last().Project == output.Lines.Last().Project);
        }
    }
}
