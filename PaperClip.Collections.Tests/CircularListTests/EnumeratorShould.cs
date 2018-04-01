using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class EnumeratorShould
    {
        [TestMethod]
        public void IterateElementsInOrder()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            var j = size - 1;
            foreach (var element in sut)
            {
                Assert.AreEqual(j, element);
                j--;
            }
        }

        [TestMethod]
        public void OnlyIterateElementsThatHaveBeenAsssignedTo()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < 5; i++)
            {
                sut.Add(i);
            }

            var j = sut.Count - 1;
            foreach (var element in sut)
            {
                Assert.AreEqual(j, element);
                j--;
            }
        }
    }
}
