using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaperClip.Collections.Interfaces;

namespace PaperClip.Collections.Tests.CircularListTests
{
    [TestClass]
    public class ListUpdatedShould
    {
        private int ExpectedAddedElement { get; set; }
        public bool ExpectedWasElementRemoved { get; set; }
        public int ExpectedRemovedElement { get; set; }

        [TestMethod]
        public void SetAddedElement()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            sut.ListUpdated += CheckAddedElementValue;

            for (var i = 0; i < size; i++)
            {
                ExpectedAddedElement = i;
                sut.Add(i);
            }
        }

        [TestMethod]
        public void SetWasElementRemovedToFalseIfNoElementRemoved()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            ExpectedWasElementRemoved = false;
            sut.ListUpdated += CheckWasElementRemovedValue;

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }
        }

        [TestMethod]
        public void SetWasElementRemovedToTrueIfElementRemoved()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            ExpectedWasElementRemoved = false;
            sut.ListUpdated += CheckWasElementRemovedValue;

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            ExpectedWasElementRemoved = true;
            sut.Add(size);
        }

        [TestMethod]
        public void SetRemovedElement()
        {
            var size = 10;
            var sut = new CircularList<int>(size);

            sut.ListUpdated += CheckRemovedElementValue;

            for (var i = 0; i < size; i++)
            {
                sut.Add(i);
            }

            ExpectedRemovedElement = 0;
            sut.Add(size);
        }

        private void CheckAddedElementValue(object sender, ICircularListUpdatedEventArgs<int> e)
        {
            Assert.AreEqual(ExpectedAddedElement, e.AddedElement);
        }

        private void CheckWasElementRemovedValue(object sender, ICircularListUpdatedEventArgs<int> e)
        {
            Assert.AreEqual(ExpectedWasElementRemoved, e.WasElementRemoved);
        }

        private void CheckRemovedElementValue(object sender, ICircularListUpdatedEventArgs<int> e)
        {
            Assert.AreEqual(ExpectedRemovedElement, e.RemovedElement);
        }
    }
}
