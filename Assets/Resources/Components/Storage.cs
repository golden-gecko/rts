public class Storage : MyComponent
{
    public override string GetInfo()
    {
        return string.Format("{0}\nResources: {1}", base.GetInfo(), Resources.GetInfo());
    }

    public ResourceContainer Resources { get; } = new ResourceContainer();
}
