using Shouldly;

namespace DataUtils.Tests;

public class BufferManagerTests
{
    [SetUp]
    public void PrepareBufferManager()
    {
        this.BufferManager = new BufferManager<int>(20);
    }

    public BufferManager<int> BufferManager { get; set; }

    [Test]
    public void Test1()
    {
        var reference = BufferManager.InputData(new []{1,2,3}).First();
        
        reference.BufferNumber.ShouldBe(0);
        reference.Length.ShouldBe(3);
        reference.Start.ShouldBe(0);
    }
    
    [Test]
    public void Test2()
    {
        var _ = BufferManager.InputData(new []{1,2,3});
        var reference = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}).First();
        
        reference.BufferNumber.ShouldBe(0);
        reference.Length.ShouldBe(15);
        reference.Start.ShouldBe(3);
    }
    
    [Test]
    public void Test3()
    {
        var _ = BufferManager.InputData(new []{1,2,3});
        var __ = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        var reference = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15}).First();
        
        reference.BufferNumber.ShouldBe(1);
        reference.Length.ShouldBe(15);
        reference.Start.ShouldBe(0);
    }
    
    [Test]
    public void Test4()
    {
        var _ = BufferManager.InputData(new []{1,2,3});
        var __ = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        var ___ = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        var reference = BufferManager.InputData(new []{1,2}).First();
        
        reference.BufferNumber.ShouldBe(0);
        reference.Length.ShouldBe(2);
        reference.Start.ShouldBe(18);
    }
    
    [Test]
    public void Test5()
    {
        var _ = BufferManager.InputData(new []{1,2,3});
        var __ = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        var references = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        
        references.Count().ShouldBe(2);
        references.First().BufferNumber.ShouldBe(1);
        references.First().Length.ShouldBe(20);
        references.First().Start.ShouldBe(0);
        
        references.Last().BufferNumber.ShouldBe(2);
        references.Last().Length.ShouldBe(10);
        references.Last().Start.ShouldBe(0);
    }
    
    [Test]
    public void Test6()
    {
        var references = BufferManager.InputData(new []{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15});
        
        references.Count().ShouldBe(3);
        references.All(r => r.Length == 20).ShouldBeTrue();
        references.All(r => r.Start == 0).ShouldBeTrue();
    }
}