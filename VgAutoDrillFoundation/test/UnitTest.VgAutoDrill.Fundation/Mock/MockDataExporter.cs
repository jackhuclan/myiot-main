using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;

namespace UnitTest.VgAutoDrill.Fundation.Mock
{
    internal class MockDataExporter : Mock<IMessageChannel>
    {
        private Mock<IMessageChannel> _mock;

        public MockDataExporter()
        {
            _mock = new Mock<IMessageChannel>();
        }

        public Mock<IMessageChannel> Mock()
        {
            return _mock;
        }
    }
}
