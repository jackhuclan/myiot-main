using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace UnitTest.VgAutoDrill.Fundation.Iot
{
    public class CutterTrayTest
    {
        [Fact]
        public void TestCutterTrayInitialization()
        {
            var list = CutterTray.InitializeCutterSilo(6, 3, 2);

            Assert.Equal(36, list.Count);

            var trayLimit = 3 * 2;

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(i % trayLimit + 1, list[i].IndexOnLayer);
            }
        }
    }
}
