using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class IndexerShould
    {
        [TestMethod]
        public void GetElementsInOrder()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            for (var j = 0; j < size; j++)
            {
                Assert.AreEqual(size - 1 - j, sut[j]);
            }
        }

        [TestMethod]
        public void SetCorrectElements()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            for (var j = 0; j < size; j++)
            {
                sut[j] = j;
            }

            for (var k = 0; k < size; k++)
            {
                Assert.AreEqual(k, sut[k]);
            }
        }

        [TestMethod]
        public void NotIndexElementsThatHaveNotBeenAssigned()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < 5; i++)
            {
                sut.Add(i);
            }
            
            Assert.ThrowsException<IndexOutOfRangeException>(() => sut[5]);
        }
    }
}
