using VgAutoDrill.Infrastructure;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class ConcurrentListTest
{
    [Fact]
    public void TestAddObjectsWithDifferentKey()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        for (int i = 0; i < 100; i++)
        {
            list.Add(new Person { Id = i, Name = "zhangsan" + i, Address = "xinghu street" + i });
        }

        var all = list.GetAll();
        Assert.Equal(100, all.Count);
    }

    [Fact]
    public void TestAddObjectsWithSameKey()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        for (int i = 0; i < 100; i++)
        {
            list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        }

        var all = list.GetAll();
        Assert.Single(all);
    }

    [Fact]
    public void TestRemoveAll()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        list.RemoveAll();
        var all = list.GetAll();
        Assert.Empty(all);
    }

    [Fact]
    public void TestTryRemove()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        bool removed = list.TryRemove(x => x.Id == 1, out var person);
        Assert.True(removed);
        Assert.NotNull(person);

        var all = list.GetAll();
        Assert.Equal(3, all.Count);
    }

    [Fact]
    public void TestContainsKey()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        Assert.True(list.ContainsKey(1));
    }

    [Fact]
    public void TestTryGetValue()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        var got = list.TryGetValue(2, out var p);
        Assert.True(got);
        Assert.NotNull(p);
    }

    [Fact]
    public void TestFirstOrDefault()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        var p = list.FirstOrDefault(x => x.Id == 2);
        Assert.NotNull(p);
        Assert.Equal("lisi", p.Name);
    }

    [Fact]
    public void TestFirst()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        var p = list.First(x => x.Id == 2);
        Assert.NotNull(p);
        Assert.Equal("lisi", p.Name);
    }

    [Fact]
    public void TestGetAll()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        list.Add(new Person { Id = 1, Name = "zhangsan", Address = "xinghu street 1" });
        list.Add(new Person { Id = 2, Name = "lisi", Address = "xinghu street 2" });
        list.Add(new Person { Id = 3, Name = "wangwu", Address = "xinghu street 3" });
        list.Add(new Person { Id = 4, Name = "zhaosi", Address = "xinghu street 4" });

        var p = list.GetAll(x => x.Id > 2);
        Assert.Equal(2, p.Count);
    }

    [Fact]
    public void TestAddAndRemove_ShouldNotThrowException()
    {
        var list = new ConcurrentList<int, Person>(p => p.Id);
        var tasks = new List<Task>();
        for (int i = 0; i < 100; i++)
        {
            var t = i;
            var task1 = Task.Run(() =>
            {
                list.Add(new Person { Id = t, Name = "zhangsan" + t, Address = "xinghu street" + t });
            });
            tasks.Add(task1);

            var task2 = Task.Run(() =>
            {
                list.TryRemove(x => x.Id == 1, out var _);
            });
            tasks.Add(task2);
        }

        Task.WaitAll(tasks.ToArray());
    }
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
