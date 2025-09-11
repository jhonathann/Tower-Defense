using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class that handles the wave behaviour
/// </summary>
public class Wave
{
    public bool waveFinished = true;
    int numberOfEnemies = 9;
    int destroyedEnemies = 0;
    public readonly List<WaveMember> members = new();
    /// <summary>
    /// Action to keep track of when the wave is over
    /// </summary>
    private Action MemberDestroyedCallback;
    public Wave()
    {
        MemberDestroyedCallback = OnMemberDestroyed;
        Populate(numberOfEnemies);
    }
    /// <summary>
    /// Adds the initial enemies
    /// </summary>
    /// <param name="numberOfEnemies">The number of initial enemies</param>
    private void Populate(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            members.Add(new WaveMember(MemberDestroyedCallback));
        }
    }
    public void Mutate()
    {
        float average = members.Average(member => member.DistanceReached); //Calculate the average of the reach point of all the members (since the goal is at tile 0, the less the average the better)
        List<WaveMember> topPerformers = members.OrderBy(member => member.DistanceReached).Take(members.Count / 2).ToList(); //Selects the one who performed the best
        foreach (WaveMember member in members)
        {
            if (member.DistanceReached <= average)
            {
                member.Mutate();
            }
            else
            {
                int randomIndex = UnityEngine.Random.Range(0, topPerformers.Count);
                WaveMember parent = topPerformers[randomIndex];
                member.Inherit(parent);
            }
        }
        numberOfEnemies++;
        WaveMember newMember = new(MemberDestroyedCallback);
        newMember.Inherit(topPerformers[0]);//The new member inherits from the top one
        members.Add(newMember);
    }
    private void OnMemberDestroyed()
    {
        destroyedEnemies++;
        if (destroyedEnemies == numberOfEnemies)
        {
            waveFinished = true;
            PortalController.WaveState = WaveState.Waiting;
            destroyedEnemies = 0;
        }
    }
}
