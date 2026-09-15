using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.UnitTest
{
    public class WatchablePropertiesTest
    {
        [Fact]
        [Trait("Category", "normal test")]
        public void ShouldReferenceEqual_WhenAddSameNameProperty()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            Assert.Same(watchableProperties.Property("a"), watchableProperties.Properties("a", "b").Property("a"));
        }

        [Fact]
        [Trait("Category", "normal test")]
        public void ShouldGetCorrectValues_WhenPropertyValueChanged()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a").SetValue("1");
            watchableProperties.Property("b").SetValue("2");
            watchableProperties.Property("c").SetValue("3");

            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Property("b").NewValue?.ToString());
            Assert.Equal("3", watchableProperties.Property("c").NewValue?.ToString());

            Assert.True(watchableProperties.Property("a").IsValueChanged);
            Assert.True(watchableProperties.Property("b").IsValueChanged);
            Assert.True(watchableProperties.Property("c").IsValueChanged);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerOnce_WhenPropertyValueChanged()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .When(p => p.IsValueChanged)
                .TriggerOnce(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue("1");
            watchableProperties.Property("a").SetValue("2");
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WhenPropertyValueChanged()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .When(p => p.IsValueChanged)
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue("1");
            watchableProperties.Property("a").SetValue("2");
            watchableProperties.Property("a").SetValue("3");
            Assert.Equal(3, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WhenObjectPropertyIsChanged()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", null)
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .When(p => p.IsValueChanged)
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue(new object());
            watchableProperties.Property("a").SetValue(new object());
            watchableProperties.Property("a").SetValue(new object());
            Assert.Equal(3, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerOnce_WhenPropertyValueChangedToSpecified()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "0")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .PreCondition(p => p.NewValue?.ToString() == "0")
                .PostCondition(p => p.NewValue?.ToString() == "1")
                .TriggerOnce(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue("1");
            watchableProperties.Property("a").SetValue("2");
            watchableProperties.Property("a").SetValue("3");
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldNotTriggerOnce_WhenPropertyValueChangedToSpecified()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "0")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .PreCondition(p => p.NewValue?.ToString() == "0")
                .PostCondition(p => p.NewValue?.ToString() == "1")
                .TriggerOnce(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue("2");
            Assert.Equal(0, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WhenPropertyValueChangedToSpecified()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "0")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .PreCondition(p => p.NewValue?.ToString() == "0")
                .PostCondition(p => p.NewValue?.ToString() == "1")
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.Property("a").SetValue("1");
            watchableProperties.Property("a").SetValue("2");
            watchableProperties.Property("a").SetValue("3");
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "normal test")]
        public void ShouldSame_WithMultiProperty_WhenWatchMultiProperties()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");
            var expected = watchableProperties.Properties("a", "b");
            var actual = watchableProperties.Properties("b", "a");
            Assert.Same(expected, actual);
        }

        [Fact]
        [Trait("Category", "normal test")]
        public void ShouldTriggerOnce_WithMultiProperty_WhenAnyPropertyValueChanged()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerOnce(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public async void ShouldTriggerEveryAction_WithMultiProperty_WhenAnyPropertyValueChanged()
        {
            int count1 = 0, count2 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    throw new NotImplementedException();
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(async () =>
                {
                    await Task.Delay(1000);
                    count1++;
                });

            watchableProperties.Properties("b", "a")
                .WhenAnyValueChanged()
                .TriggerAlways(async () =>
                {
                    await Task.Delay(2000);
                    count2++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            await Task.Delay(3000);//waiting for task completed!
            Assert.Equal(1, count1);
            Assert.Equal(1, count2);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WithMultiProperty_WhenAnyPropertyValueChanged()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });

            Assert.Equal(3, count);

            count = 0;

            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "1" }, { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "2" }, { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "3" }, { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WithMultiProperty_WhenAllPropertyValueChangedMultipleTime()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAllValueChanged()
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count);

            count = 0;

            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "1" }, { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "2" }, { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "3" }, { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WithMultiProperty_WhenAllPropertyValueChangedOnce()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAllValueChanged()
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerOnce_WithMultiProperty_WhenAnyPropertyValueChangedToSpecified()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "0")
                .AddProperty("b", "1")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .PreCondition(p => p.Property("a").NewValue?.ToString() == "0" && p.Property("b").NewValue?.ToString() == "1")
                .PostCondition(p => p.Property("a").NewValue?.ToString() == "10" && p.Property("b").NewValue?.ToString() == "11")
                .TriggerOnce(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "3" } });
            Assert.Equal(1, count);

            count = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "3" } });
            Assert.Equal(0, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WithMultiProperty_WhenAnyPropertyValueChangedToSpecified()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "0")
                .AddProperty("b", "1")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .PreCondition(p => p.Property("a").NewValue?.ToString() == "0" && p.Property("b").NewValue?.ToString() == "1")
                .PostCondition(p => p.Property("a").NewValue?.ToString() == "10" && p.Property("b").NewValue?.ToString() == "11")
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "0" }, { "b", "1" }, { "c", "0" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "3" } });
            Assert.Equal(3, count);

            count = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "110" }, { "b", "111" }, { "c", "11" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "10" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "110" }, { "b", "11" }, { "c", "12" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "10" }, { "b", "11" }, { "c", "10" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "110" }, { "b", "111" }, { "c", "13" } });
            Assert.Equal(0, count);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldGetCorrectValues()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });

            var actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count);
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");

            count = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "4" }, { "b", "4" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "5" }, { "b", "5" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "6" }, { "b", "6" } });

            actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count);
            Assert.Equal(actualDic["a"], "6");
            Assert.Equal(actualDic["b"], "6");
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlwaysBothWatchablePropertiesAndWatchableProperty_WhenSetValues()
        {
            int count1 = 0, count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count2++;
                });

            watchableProperties.Property("c")
                .WhenValueChanged()
                .TriggerAlways(() =>
                {
                    count3++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" }, { "c", "3" } });

            var actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
            Assert.Equal(3, count3);
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");
            Assert.Equal(actualDic["c"], "3");

            count1 = 0;
            count2 = 0;
            count3 = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "4" }, { "b", "4" }, { "c", "4" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "5" }, { "b", "5" }, { "c", "5" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "6" }, { "b", "6" }, { "c", "6" } });

            actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
            Assert.Equal(3, count3);
            Assert.Equal(actualDic["a"], "6");
            Assert.Equal(actualDic["b"], "6");
            Assert.Equal(actualDic["c"], "6");
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerOnceBothWatchablePropertiesAndWatchableProperty_WhenSetValues()
        {
            int count1 = 0, count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerOnce(() =>
                {
                    count1++;
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerOnce(() =>
                {
                    count2++;
                });

            watchableProperties.Property("c")
                .WhenValueChanged()
                .TriggerOnce(() =>
                {
                    count3++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" }, { "c", "3" } });

            var actualDic = watchableProperties.GetValues();
            Assert.Equal(1, count1);
            Assert.Equal(1, count2);
            Assert.Equal(1, count3);
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");
            Assert.Equal(actualDic["c"], "3");

            count1 = 0;
            count2 = 0;
            count3 = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "4" }, { "b", "4" }, { "c", "4" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "5" }, { "b", "5" }, { "c", "5" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "6" }, { "b", "6" }, { "c", "6" } });

            actualDic = watchableProperties.GetValues();
            Assert.Equal(0, count1);
            Assert.Equal(0, count2);
            Assert.Equal(0, count3);
            Assert.Equal(actualDic["a"], "6");
            Assert.Equal(actualDic["b"], "6");
            Assert.Equal(actualDic["c"], "6");
        }

        [Fact]
        [Trait("Category", "normal test")]
        public void ShouldNotSame_WhenAddDifferentWatchableProperties()
        {
            WatchableProperties watchablePropertiesA = new WatchableProperties();
            watchablePropertiesA.AddProperty("a", "").AddProperty("b", "");

            WatchableProperties watchablePropertiesB = new WatchableProperties();
            watchablePropertiesB.AddProperty("a", "").AddProperty("b", "");

            var a = watchablePropertiesA.Properties("a", "b");
            var b = watchablePropertiesB.Properties("a", "b");
            Assert.NotSame(a, b);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerOnlyOneWatchableProperties_WhenDifferentWatchablePropertiesHasSameProperties()
        {
            int countA = 0;
            WatchableProperties watchablePropertiesA = new WatchableProperties();
            watchablePropertiesA.AddProperty("a", "").AddProperty("b", "");
            watchablePropertiesA.Properties("a", "b")
                    .WhenAnyValueChanged()
                    .TriggerAlways(() =>
                    {
                        countA++;
                    });

            int countB = 0;
            WatchableProperties watchablePropertiesB = new WatchableProperties();
            watchablePropertiesB.AddProperty("a", "").AddProperty("b", "");
            watchablePropertiesB.Properties("a", "b")
                    .WhenAnyValueChanged()
                    .TriggerAlways(() =>
                    {
                        countB++;
                    });

            watchablePropertiesA.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "2" } });
            Assert.Equal(1, countA);
            Assert.Equal("1", watchablePropertiesA.Property("a").NewValue?.ToString());
            Assert.Equal("2", watchablePropertiesA.Property("b").NewValue?.ToString());
            Assert.Equal(0, countB);
            Assert.Equal("", watchablePropertiesB.Property("a").NewValue?.ToString());
            Assert.Equal("", watchablePropertiesB.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerMultiPropertiesAction_WhenMultipleWatchablePropertiesMatched()
        {
            int count1 = 0;
            int count2 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "1")
                .AddProperty("c", "1");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.Properties("b", "c")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count2++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "1" } });
            Assert.Equal(3, count1);
            Assert.Equal(0, count2);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "1" }, { "a", "3" }, { "b", "11" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "1" }, { "a", "3" }, { "b", "12" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "c", "1" }, { "a", "3" }, { "b", "13" } });
            Assert.Equal(6, count1);
            Assert.Equal(3, count2);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerSinglePropertyAction_WhenSetMultipleWatchablePropertiesValues()
        {
            int count1 = 0;
            int count2 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "1");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.Properties("a", "b")
                .WhenAllValueChanged()
                .TriggerAlways(() =>
                {
                    count2++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WhenWatchAllValueChanged_AfterCalledRemoveProperty()
        {
            int count1 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "");

            watchableProperties.Properties("a", "b")
                .WhenAllValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count1);

            count1 = 0;
            watchableProperties.RemoveProperty("a");
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "11" }, { "b", "3" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "22" }, { "b", "3" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "33" }, { "b", "3" } });
            //Property("a") will not be watched anymore after removing it, but it still exists in group Properties
            Assert.Throws<KeyNotFoundException>(() => watchableProperties.Property("a"));
            Assert.Throws<KeyNotFoundException>(() => watchableProperties.Properties("a", "b").Property("a"));
            Assert.Equal(0, count1);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "111" }, { "b", "13" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "222" }, { "b", "23" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "333" }, { "b", "33" } });
            Assert.Equal(3, count1);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldTriggerAlways_WhenWatchAnyValueChanged_AfterCalledRemoveProperty()
        {
            int count1 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count1);

            watchableProperties.RemoveProperty("a");
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "11" }, { "b", "11" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "22" }, { "b", "22" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "33" }, { "b", "33" } });
            Assert.Equal(6, count1);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldWorkCorrectly_WhenSetValuesFromChildWatchableProperties()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "");

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Properties("a", "b").Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Properties("a", "b").Property("b").NewValue?.ToString());

            watchableProperties.Properties("a", "b").SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            Assert.Equal("2", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Property("b").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Properties("a", "b").Property("a").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Properties("a", "b").Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldWorkCorrectly_WhenUsingDynamicProperty()
        {
            int count1 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" } });
            Assert.Equal(3, count1);

            watchableProperties.AddProperty("DynamicProperyA", "")
                .AddProperty("DynamicProperyB", "");
            Assert.Equal(4, watchableProperties.GetPropertyKeys().Count);

            watchableProperties.Properties("DynamicProperyA", "DynamicProperyB")
                .WhenAnyValueChanged()
                .TriggerAlways(() =>
                {
                    count1++;
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "11" }, { "DynamicProperyB", "11" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "22" }, { "DynamicProperyB", "22" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "33" }, { "DynamicProperyB", "33" } });
            Assert.Equal(6, count1);

            watchableProperties.RemoveProperty("DynamicProperyA");
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "111" }, { "DynamicProperyB", "33" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "222" }, { "DynamicProperyB", "33" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "333" }, { "DynamicProperyB", "33" } });
            Assert.Equal(6, count1);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "111" }, { "DynamicProperyB", "44" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "222" }, { "DynamicProperyB", "55" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "DynamicProperyA", "333" }, { "DynamicProperyB", "66" } });
            Assert.Equal(9, count1);
        }

        [Fact]
        [Trait("Category", "sychronized test")]
        public void ShouldWorkCorrectly_WhenHasHierarchyProperty()
        {
            WatchableProperties root = new WatchableProperties();
            root.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "")
                .AddProperty("d", "")
                .AddProperty("e", "")
                .AddProperty("f", "");

            Assert.Same(root, root.Properties("a", "b", "c", "d", "e", "f").Parent);
            Assert.Same(root, root.Properties("a", "b", "c", "d", "e").Root);

            var abcde1 = root.Properties("a", "b", "c", "d", "e");
            var abcd1 = abcde1.Properties("a", "b", "c", "d");
            var abc1 = abcd1.Properties("a", "b", "c");
            var ab1 = abc1.Properties("a", "b");

            Assert.Equal("_ROOT_", root.PropertiesName);
            Assert.Equal(string.Join('@', new[] { "a", "b", "c", "d", "e" }), abcde1.PropertiesName);

            Assert.Same(ab1, root[ab1.PropertiesName]);
            Assert.Same(abc1, root[abc1.PropertiesName]);
            Assert.Same(abcd1, root[abcd1.PropertiesName]);
            Assert.Same(abcde1, root[abcde1.PropertiesName]);

            Assert.Same(ab1, ab1[ab1.PropertiesName]);
            Assert.Same(abc1, abc1[abc1.PropertiesName]);
            Assert.Same(abcd1, abcd1[abcd1.PropertiesName]);
            Assert.Same(abcde1, abcde1[abcde1.PropertiesName]);

            Assert.Same(root, ab1.Parent);
            Assert.Same(root, abc1.Parent);
            Assert.Same(root, abcd1.Parent);
            Assert.Same(root, abcde1.Parent);

            Assert.Null(ab1.Parent.Parent);
            Assert.Null(abc1.Parent.Parent);
            Assert.Null(abcd1.Parent.Parent);
            Assert.Null(abcde1.Parent.Parent);

            Assert.Same(root, ab1.Root);
            Assert.Same(root, abc1.Root);
            Assert.Same(root, abcd1.Root);
            Assert.Same(root, abcde1.Root);
        }

        #region async test

        [Fact]
        [Trait("Category", "async test")]
        public void TriggerOnceAsync_ActionShouldBeExecutedOnce_WhenRunActionAsync()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
            .TriggerOnceAsync(() =>
            {
                Interlocked.Increment(ref count);
                System.Diagnostics.Debug.WriteLine("watchableProperties.Properties(\"a\", \"b\") called..." + count);
            });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.WaitAll();

            Assert.Equal(1, count);
        }

        [Fact]
        [Trait("Category", "async test")]
        public void ShouldGetCorrectValues_WhenRunActionAsync()
        {
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerOnceAsync(() =>
                {
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            Assert.Equal("2", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void TriggerAlwaysAsync_ActionShouldBeExecutedAlways_WhenRunActionAsync()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Properties(\"a\", \"b\") called...");
                    Interlocked.Increment(ref count);
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" } });
            watchableProperties.WaitAll();

            Assert.Equal(2, count);
            Assert.Equal("2", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("2", watchableProperties.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void ShouldTriggerOnceAsyncBothWatchablePropertiesAndWatchableProperty_WhenSetValues()
        {
            int count1 = 0, count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerOnceAsync(() =>
                {
                    Interlocked.Increment(ref count1);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Property(\"a\") called..." + count1);
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerOnceAsync(() =>
                {
                    Interlocked.Increment(ref count2);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Properties(\"a\", \"b\") called..." + count2);
                });

            watchableProperties.Property("c")
                .WhenValueChanged()
                .TriggerOnceAsync(() =>
                {
                    Interlocked.Increment(ref count3);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Property(\"c\") called..." + count3);
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" }, { "c", "3" } });

            watchableProperties.WaitAll();
            var actualDic = watchableProperties.GetValues();
            Assert.Equal(1, count1);
            Assert.Equal(1, count2);
            Assert.Equal(1, count3);
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");
            Assert.Equal(actualDic["c"], "3");

            count1 = 0;
            count2 = 0;
            count3 = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "4" }, { "b", "4" }, { "c", "4" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "5" }, { "b", "5" }, { "c", "5" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "6" }, { "b", "6" }, { "c", "6" } });

            watchableProperties.WaitAll();
            actualDic = watchableProperties.GetValues();
            Assert.Equal(0, count1);
            Assert.Equal(0, count2);
            Assert.Equal(0, count3);
            Assert.Equal(actualDic["a"], "6");
            Assert.Equal(actualDic["b"], "6");
            Assert.Equal(actualDic["c"], "6");
        }

        [Fact]
        [Trait("Category", "async test")]
        public void ShouldTriggerAlwaysAsyncBothWatchablePropertiesAndWatchableProperty_WhenSetValues()
        {
            int count1 = 0, count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Interlocked.Increment(ref count1);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Property(\"a\") called..." + count1);
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Interlocked.Increment(ref count2);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Properties(\"a\", \"b\") called..." + count2);
                });

            watchableProperties.Property("c")
                .WhenValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Interlocked.Increment(ref count3);
                    System.Diagnostics.Debug.WriteLine("watchableProperties.Property(\"c\") called..." + count3);
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" }, { "c", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" }, { "c", "3" } });

            watchableProperties.WaitAll();
            var actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
            Assert.Equal(3, count3);
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");
            Assert.Equal(actualDic["c"], "3");

            count1 = 0;
            count2 = 0;
            count3 = 0;
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "4" }, { "b", "4" }, { "c", "4" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "5" }, { "b", "5" }, { "c", "5" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "6" }, { "b", "6" }, { "c", "6" } });

            watchableProperties.WaitAll();
            actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
            Assert.Equal(3, count3);
            Assert.Equal(actualDic["a"], "6");
            Assert.Equal(actualDic["b"], "6");
            Assert.Equal(actualDic["c"], "6");
        }

        [Fact]
        [Trait("Category", "async test")]
        public void MultipleActionsShouldBeExecuted_WhenSameWatchablePropertiesHasMultipleActions()
        {
            int count = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            var abWatchableProperties = watchableProperties.Properties("a", "b").WhenAnyValueChanged();

            var action1 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 called...");
                Interlocked.Increment(ref count);
            };

            var action2 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 called...");
                Interlocked.Increment(ref count);
            };

            var action3 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 called...");
                Interlocked.Increment(ref count);
            };

            abWatchableProperties.TriggerAlwaysAsync(action1);
            abWatchableProperties.TriggerAlwaysAsync(action2);
            abWatchableProperties.TriggerAlwaysAsync(action3);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.WaitAll();

            Assert.Equal(3, count);
            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void MultipleActionsShouldBeExecuted_EvenIfOneActionThrowException_WhenSameWatchablePropertiesHasMultipleActions()
        {
            int count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            var abWatchableProperties = watchableProperties.Properties("a", "b").WhenAnyValueChanged();

            var action1 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 called...");
                throw new Exception("an exception on purpose!");
            };

            var action2 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 called...");
                Interlocked.Increment(ref count2);
            };

            var action3 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 called...");
                Interlocked.Increment(ref count3);
            };

            abWatchableProperties.TriggerAlwaysAsync(action1);
            abWatchableProperties.TriggerAlwaysAsync(action2);
            abWatchableProperties.TriggerAlwaysAsync(action3);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.WaitAll();

            Assert.Equal(1, count2);
            Assert.Equal(1, count3);
            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void MultipleActionsShouldBeExecuted_EvenIfOneLongActionRun_WhenSameWatchablePropertiesHasMultipleActions()
        {
            int count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "");

            var abWatchableProperties = watchableProperties.Properties("a", "b").WhenAnyValueChanged();

            var action1 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 called...");
                throw new Exception("an exception on purpose!");
            };

            var action2 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 called...");
                Thread.Sleep(1000);
                Interlocked.Increment(ref count2);
            };

            var action3 = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 called...");
                Interlocked.Increment(ref count3);
            };

            abWatchableProperties.TriggerAlwaysAsync(action1);
            abWatchableProperties.TriggerAlwaysAsync(action2);
            abWatchableProperties.TriggerAlwaysAsync(action3);

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" } });
            watchableProperties.WaitAll();

            Assert.Equal(1, count2);
            Assert.Equal(1, count3);
            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void MultipleWatchableProperties_MultipleActionsShouldBeExecuted_EvenIfOneLongActionRun_WhenSameWatchablePropertiesHasMultipleActions()
        {
            int count2_abGroup = 0, count3_abGroup = 0;
            int count2_cdGroup = 0, count3_cdGroup = 0;
            int count2_efGroup = 0, count3_efGroup = 0;

            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "")
                .AddProperty("d", "")
                .AddProperty("e", "")
                .AddProperty("f", "")
                .AddProperty("g", "");

            var abGroup = watchableProperties.Properties("a", "b").WhenAnyValueChanged();
            var cdGroup = watchableProperties.Properties("c", "d").WhenAnyValueChanged();
            var efGroup = watchableProperties.Properties("e", "f").WhenAnyValueChanged();

            #region abGroup

            var action1_abGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 of abGroup called...");
                throw new Exception("an exception on purpose!");
            };

            var action2_abGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 of abGroup called...");
                Thread.Sleep(10 * 1000);
                Interlocked.Increment(ref count2_abGroup);
            };

            var action3_abGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 of abGroup called...");
                Interlocked.Increment(ref count3_abGroup);
            };

            abGroup.TriggerAlwaysAsync(action1_abGroup);
            abGroup.TriggerAlwaysAsync(action2_abGroup);
            abGroup.TriggerAlwaysAsync(action3_abGroup);

            #endregion abGroup

            #region cdGroup

            var action1_cdGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 of cdGroup called...");
                throw new Exception("an exception on purpose!");
            };

            var action2_cdGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 of cdGroup called...");
                Thread.Sleep(10 * 1000);
                Interlocked.Increment(ref count2_cdGroup);
            };

            var action3_cdGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 of cdGroup called...");
                Interlocked.Increment(ref count3_cdGroup);
            };

            cdGroup.TriggerAlwaysAsync(action1_cdGroup);
            cdGroup.TriggerAlwaysAsync(action2_cdGroup);
            cdGroup.TriggerAlwaysAsync(action3_cdGroup);

            #endregion cdGroup

            #region efGroup

            var action1_efGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action1 of efGroup called...");
                throw new Exception("an exception on purpose!");
            };

            var action2_efGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action2 of efGroup called...");
                Thread.Sleep(10 * 1000);
                Interlocked.Increment(ref count2_efGroup);
            };

            var action3_efGroup = () =>
            {
                System.Diagnostics.Debug.WriteLine("action3 of efGroup called...");
                Interlocked.Increment(ref count3_efGroup);
            };

            efGroup.TriggerAlwaysAsync(action1_efGroup);
            efGroup.TriggerAlwaysAsync(action2_efGroup);
            efGroup.TriggerAlwaysAsync(action3_efGroup);

            #endregion efGroup

            #region outside of group

            watchableProperties.Property("g")
                .WhenValueChanged()
                .TriggerAlways(() =>
                {
                });

            #endregion outside of group

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" }, { "d", "1" }, { "e", "1" }, { "f", "1" } });
            watchableProperties.WaitAll();
            System.Diagnostics.Debug.WriteLine(watchableProperties.ToString());

            Assert.Equal(1, count3_abGroup);
            Assert.Equal(1, count3_cdGroup);
            Assert.Equal(1, count3_efGroup);

            Assert.Equal("1", watchableProperties.Property("a").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("b").NewValue?.ToString());

            Assert.Equal("1", watchableProperties.Property("c").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("d").NewValue?.ToString());

            Assert.Equal("1", watchableProperties.Property("e").NewValue?.ToString());
            Assert.Equal("1", watchableProperties.Property("f").NewValue?.ToString());
        }

        [Fact]
        [Trait("Category", "async test")]
        public void ChildWatchableProperties_WaitAll_ShouldNotAffectOtherWatchableProperties()
        {
            int count1 = 0, count2 = 0, count3 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "")
                .AddProperty("b", "")
                .AddProperty("c", "")
                .AddProperty("d", "");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Thread.Sleep(500);
                    Interlocked.Increment(ref count1);
                    System.Diagnostics.Debug.WriteLine("Property(\"a\") called..." + count1);
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Thread.Sleep(100);
                    Interlocked.Increment(ref count2);
                    System.Diagnostics.Debug.WriteLine("Properties(\"a\", \"b\") called..." + count2);
                });

            watchableProperties.Properties("c", "d")
                .WhenAnyValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Thread.Sleep(1000);
                    Interlocked.Increment(ref count3);
                });

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "1" }, { "b", "1" }, { "c", "1" }, { "d", "1" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "2" }, { "b", "2" }, { "c", "2" }, { "d", "2" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "3" }, { "b", "3" }, { "c", "3" }, { "d", "3" } });

            watchableProperties.Properties("a", "b").WaitAll();
            var actualDic = watchableProperties.GetValues();
            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
            Assert.Equal(0, count3);

            watchableProperties.Properties("c", "d").WaitAll();
            actualDic = watchableProperties.GetValues();
            Assert.Equal(actualDic["a"], "3");
            Assert.Equal(actualDic["b"], "3");
            Assert.Equal(actualDic["c"], "3");

            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "11" }, { "b", "11" }, { "c", "11" }, { "d", "11" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "22" }, { "b", "22" }, { "c", "22" }, { "d", "22" } });
            watchableProperties.SetValues(new Dictionary<string, object?> { { "a", "33" }, { "b", "33" }, { "c", "33" }, { "d", "33" } });
            watchableProperties.WaitAll();
            Assert.Equal(6, count1);
            Assert.Equal(6, count2);
            Assert.Equal(6, count3);

            actualDic = watchableProperties.GetValues();
            Assert.Equal(actualDic["a"], "33");
            Assert.Equal(actualDic["b"], "33");
            Assert.Equal(actualDic["c"], "33");
        }

        [Fact]
        [Trait("Category", "async test")]
        public async void RefreshShouldSetValues()
        {
            int count1 = 0, count2 = 0;
            WatchableProperties watchableProperties = new WatchableProperties();
            watchableProperties.AddProperty("a", "", async () => await Task.FromResult(new Random((int)DateTime.Now.Ticks).Next(1000).ToString()))
                .AddProperty("b", "", async () => await Task.FromResult(new Random((int)DateTime.Now.Ticks).Next(1000).ToString()))
                .AddProperty("c", "", async () => await Task.FromResult(new Random((int)DateTime.Now.Ticks).Next(1000).ToString()));

            var a = watchableProperties.Property("a");
            var b = watchableProperties.Property("b");

            watchableProperties.Property("a")
                .WhenValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Thread.Sleep(500);
                    Interlocked.Increment(ref count1);
                    System.Diagnostics.Debug.WriteLine($"Property(\"a\") called {count1}...");
                    System.Diagnostics.Debug.WriteLine($"[a.OldValue={a.OldValue},a.NewValue={a.NewValue}],[b.OldValue={b.OldValue},b.NewValue={b.NewValue}]");
                });

            watchableProperties.Properties("a", "b")
                .WhenAnyValueChanged()
                .TriggerAlwaysAsync(() =>
                {
                    Thread.Sleep(100);
                    Interlocked.Increment(ref count2);
                    System.Diagnostics.Debug.WriteLine($"Properties(\"a\", \"b\") called {count2}...]");
                    System.Diagnostics.Debug.WriteLine($"[a.OldValue={a.OldValue},a.NewValue={a.NewValue}],[b.OldValue={b.OldValue},b.NewValue={b.NewValue}]");
                });

            await watchableProperties.Refresh();
            watchableProperties.WaitAll();
            await watchableProperties.Refresh();
            watchableProperties.WaitAll();
            await watchableProperties.Refresh();
            watchableProperties.WaitAll();

            Assert.Equal(3, count1);
            Assert.Equal(3, count2);
        }

        #endregion async test
    }
}
