using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class ClearShould
    {
        [TestMethod]
        public void ResetCount()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.AreEqual(10, sut.Count);

            sut.Clear();

            Assert.AreEqual(0, sut.Count);
        }

        [TestMethod]
        public void ClearAllElements()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
                Assert.AreEqual(i, sut.First);
            }

            sut.Clear();

            for (var j = 0; j < size; j++)
            {
                Assert.ThrowsException<IndexOutOfRangeException>(() =>
                {
                    var test = sut[j];
                });
            }
        }
    }
}
