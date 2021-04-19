public class Destructor : Enemy {
    
    private void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
    }

    public void StartChasing() {
        stateMachine.ChangeState<DestructorChargeState>();
    }
    
}
