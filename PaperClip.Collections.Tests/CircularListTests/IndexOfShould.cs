using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class IndexOfShould
    {
        [TestMethod]
        public void ReturnIndexOfElement()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.AreEqual(4, sut.IndexOf(5));
        }

        [TestMethod]
        public void ReturnNegativeOneIfElementIsNotInList()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.AreEqual(-1, sut.IndexOf(10));
        }
    }
}
