public class Destructor : Enemy {
    
    private void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
    }
    
}
