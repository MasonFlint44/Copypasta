using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class FirstShould
    {
        [TestMethod]
        public void ReturnPreviousElementAdded()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
                Assert.AreEqual(i, sut.First);
            }
        }
    }
}
