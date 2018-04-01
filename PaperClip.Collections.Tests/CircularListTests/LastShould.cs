using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class LastShould
    {
        [TestMethod]
        public void ReturnOldestElementAdded()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.AreEqual(0, sut.Last);

            sut.Add(size);

            Assert.AreEqual(1, sut.Last);
        }
    }
}
