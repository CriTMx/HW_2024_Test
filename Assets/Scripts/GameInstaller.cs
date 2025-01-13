using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PulpitBehavior pulpitPrefab;
    [SerializeField] private int maxSimultaneousPulpits = 2;

    public override void InstallBindings()
    {
        Container.Bind<GameObjectPool<PulpitBehavior>>()
            .ToSelf()
            .AsSingle()
            .WithArguments(pulpitPrefab, maxSimultaneousPulpits);
    }
}