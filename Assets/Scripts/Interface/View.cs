public interface IView
{
    void OnViewDisplayed() { GameManager.Instance.SoundManager.StopAmbient(); }

}