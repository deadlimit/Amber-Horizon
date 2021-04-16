public class Destructor : Enemy {

    private void Awake() {
        base.Awake();
    }

    private void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
    }
    
}
