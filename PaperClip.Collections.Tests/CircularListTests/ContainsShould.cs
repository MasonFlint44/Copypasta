using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class ContainsShould
    {
        [TestMethod]
        public void ReturnTrueIfElementIsInList()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.IsTrue(sut.Contains(5));
        }

        [TestMethod]
        public void ReturnFalseIfElementIsNotInList()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.IsFalse(sut.Contains(size));
        }
    }
}
