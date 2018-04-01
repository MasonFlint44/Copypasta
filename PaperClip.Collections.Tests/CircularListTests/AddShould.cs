using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class AddShould
    {
        [TestMethod]
        public void IncreaseCountIfCountIsLessThanSize()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
                Assert.AreEqual(i + 1, sut.Count);
            }
        }

        [TestMethod]
        public void NotIncreaseCountIfCountIsEqualToSize()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }
            Assert.AreEqual(sut.Size, sut.Count);

            sut.Add(11);
            Assert.AreEqual(sut.Size, sut.Count);
        }

        [TestMethod]
        public void AddElementToFrontOfList()
        {
            var size = 10;
            var sut = new CircularList<int>(size) {1, 2};

            Assert.AreEqual(sut[0], 2);
        }

        [TestMethod]
        public void KeepElementsInOrder()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            sut.Add(size);

            for (var i = 0; i < size; i++)
            {
                Assert.AreEqual(size - i, sut[i]);
            }
        }

        [TestMethod]
        public void OverwriteOldestElement()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.IsTrue(sut.Contains(0));
            sut.Add(size);
            Assert.IsFalse(sut.Contains(0));
        }

        [TestMethod]
        public void FireListUpdatedEvent()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            var eventsFired = 0;
            sut.ListUpdated += (sender, args) => eventsFired++;

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            Assert.AreEqual(size, eventsFired);
        }
    }
}
