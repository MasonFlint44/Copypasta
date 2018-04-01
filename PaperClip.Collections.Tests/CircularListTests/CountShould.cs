using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class CountShould
    {
        [TestMethod]
        public void BeNoGreaterThanSize()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size * 2; i++)
            {
                sut.Add(i);
                Assert.IsTrue(sut.Count <= sut.Size);
            }
        }

        [TestMethod]
        public void BeEqualToNumberOfElementsAssignedTo()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
                Assert.AreEqual(i + 1, sut.Count);
            }
        }
    }
}
