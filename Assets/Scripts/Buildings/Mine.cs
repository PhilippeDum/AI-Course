using UnityEngine;

public class Mine : Building
{
    public override void OnEnable()
    {
        base.OnEnable();

        Something();
    }

    private void Something()
    {

    }
}