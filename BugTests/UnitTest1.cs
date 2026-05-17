using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void Test1_InitialState_ShouldBeOpen()
    {
        // Arrange
        var bug = new Bug();
        
        // Act & Assert
        Assert.AreEqual(BugState.Open, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test2_Assign_ShouldChangeStateToAssigned()
    {
        // Arrange
        var bug = new Bug();
        
        // Act
        bug.Fire(BugTrigger.Assign);
        
        // Assert
        Assert.AreEqual(BugState.Assigned, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test3_StartProgress_ShouldChangeStateToInProgress()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        
        // Act
        bug.Fire(BugTrigger.StartProgress);
        
        // Assert
        Assert.AreEqual(BugState.InProgress, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test4_Fix_ShouldChangeStateToFixed()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        
        // Act
        bug.Fire(BugTrigger.Fix);
        
        // Assert
        Assert.AreEqual(BugState.Fixed, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test5_Verify_ShouldChangeStateToVerified()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        
        // Act
        bug.Fire(BugTrigger.Verify);
        
        // Assert
        Assert.AreEqual(BugState.Verified, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test6_Close_ShouldChangeStateToClosed()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        
        // Act
        bug.Fire(BugTrigger.Close);
        
        // Assert
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test7_Reopen_ShouldChangeStateToReopened()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        
        // Act
        bug.Fire(BugTrigger.Reopen);
        
        // Assert
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test8_Reject_ShouldChangeStateToRejected()
    {
        // Arrange
        var bug = new Bug();
        
        // Act
        bug.Fire(BugTrigger.Reject);
        
        // Assert
        Assert.AreEqual(BugState.Rejected, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test9_InvalidTransition_ShouldNotChangeState()
    {
        // Arrange
        var bug = new Bug();
        var initialState = bug.CurrentState;
        
        // Act - нельзя фиксить баг с самого начала
        bug.Fire(BugTrigger.Fix);
        
        // Assert
        Assert.AreEqual(initialState, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test10_CanFire_ShouldReturnTrueForValidTransitions()
    {
        // Arrange
        var bug = new Bug();
        
        // Act & Assert
        Assert.IsTrue(bug.CanFire(BugTrigger.Assign));
        Assert.IsTrue(bug.CanFire(BugTrigger.Reject));
        Assert.IsFalse(bug.CanFire(BugTrigger.Fix));
    }
    
    [TestMethod]
    public void Test11_ReopenAfterClosed_ShouldWork()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        bug.Fire(BugTrigger.Close);
        
        // Act
        bug.Fire(BugTrigger.Reopen);
        
        // Assert
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test12_ReassignAfterReopen_ShouldWork()
    {
        // Arrange
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Reopen);
        
        // Act
        bug.Fire(BugTrigger.Assign);
        
        // Assert
        Assert.AreEqual(BugState.Assigned, bug.CurrentState);
    }
}
