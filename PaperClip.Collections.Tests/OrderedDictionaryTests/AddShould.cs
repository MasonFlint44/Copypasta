using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaperClip.Collections.Tests.OrderedDictionaryTests
{
    [TestClass]
    public class AddShould
    {
        [TestMethod]
        public void KeepElementsInOrder()
        {
            var sut = new OrderedDictionary<int, int>();
            for (var i = 0; i < 5; i++)
            {
                sut.Add(i, i);
            }

            var j = 0;
            foreach (var element in sut)
            {
                Assert.AreEqual(element.Key, j);
                Assert.AreEqual(element.Value, j++);
            }
        }
    }
}
