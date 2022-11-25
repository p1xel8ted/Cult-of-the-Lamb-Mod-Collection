using System.Linq;
using UnityEngine;

namespace GoatOuthouses;

public class FollowerTaskOuthouse : FollowerTask
{
    private Follower? _follower;
    private bool _holdingPoop;
    private readonly int _siloFertiliserId;
    private readonly Structures_SiloFertiliser _siloFertiliser;

    public FollowerTaskOuthouse(int siloFertiliser)
    {
        _siloFertiliserId = siloFertiliser;
        _siloFertiliser = StructureManager.GetStructureByID<Structures_SiloFertiliser>(_siloFertiliserId);
    }

    public override int GetSubTaskCode()
    {
        return _siloFertiliserId;
    }

    public override void OnStart()
    {
        SetState(FollowerTaskState.GoingTo);
    }

    private void DoingBegin(Follower? follower)
    {
        if (follower)
        {
            follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
        }

        var outhouseFullExists = StructureManager.GetAllStructuresOfType<Structures_Outhouse>(FollowerLocation.Base).Any(a => a.IsFull);
        if (outhouseFullExists)
        {
            var structureByID = StructureManager.GetAllStructuresOfType<Structures_Outhouse>().Find(a => a.IsFull);
            if (structureByID.Data.Inventory.Count <= 0)
            {
                End();
                return;
            }

            _holdingPoop = true;
            structureByID.Data.Inventory[0].quantity--;
            if (structureByID.Data.Inventory[0].quantity <= 0)
            {
                structureByID.Data.Inventory.RemoveAt(0);
            }

            foreach (var interactionSiloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
            {
                if (interactionSiloFertilizer.StructureBrain.Data.ID == _siloFertizilerId)
                {
                    interactionSiloFertilizer.UpdateCapacityIndicators();
                    break;
                }
            }

            if (follower)
            {
                follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Farming/run-poop");
            }

            ClearDestination();
            SetState(FollowerTaskState.GoingTo);
        }
    }
    
    public override PriorityCategory GetPriorityCategory(FollowerRole FollowerRole, WorkerPriority WorkerPriority, FollowerBrain brain)
    {
        return PriorityCategory.ExtremelyUrgent;
    }
    
    public override void Cleanup(Follower follower)
    {
        follower.SetHat(HatType.None);
        follower.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");

        if (_holdingPoop)
        {
            RefundPoop();
        }
        base.Cleanup(follower);
    }

    public override void OnDoingBegin(Follower? follower)
    {
        _follower = follower;
        DoingBegin(follower);
    }

    public override void OnGoingToBegin(Follower? follower)
    {
        base.OnGoingToBegin(follower);
        _follower = follower;
    }

    public override void TaskTick(float deltaGameTime)
    {
        if (State == FollowerTaskState.Doing)
        {
            //carry poop to place?
        }
    }

    public override void OnAbort()
    {
        base.OnAbort();
        if (_holdingPoop)
        {
            RefundPoop();
        }
    }
    
    public override void SimDoingBegin(SimFollower simFollower)
    {
        DoingBegin(null);
    }
    
    public override void Setup(Follower? follower)
    {
        base.Setup(follower);
        _follower = follower;
    }
    
    public override void SimCleanup(SimFollower simFollower)
    {
        base.SimCleanup(simFollower);
        if (_holdingPoop)
        {
            RefundPoop();
        }
    }

    private void RefundPoop()
    {
        var structureByID = StructureManager.GetAllStructuresOfType<Structures_Outhouse>().Find(a => !a.IsFull);
        if (structureByID != null)
        {
            structureByID.Data.Inventory.Add(new InventoryItem
            {
                type = 39,
                quantity = 1
            });
            foreach (var interactionSiloFertilizer in Interaction_SiloFertilizer.SiloFertilizers)
            {
                interactionSiloFertilizer.UpdateCapacityIndicators();
            }
            foreach (var interactionOuthouse in Interaction_Outhouse.Outhouses)
            {
                interactionOuthouse.Update();
            }

            _holdingPoop = false;
        }
    }

    public override Vector3 UpdateDestination(Follower follower)
    {
        var vector = default(Vector3);

        var pickupPoint = StructureManager.GetAllStructuresOfType<Structures_Outhouse>(FollowerLocation.Base).Find(a => a.IsFull);
        var dropOffPoint = StructureManager.GetStructureByID<Structures_SiloFertiliser>(_siloFertizilerId);

        if (dropOffPoint != null)
        {
            vector = dropOffPoint.Data.Position;
        }
        else
        {
            if (pickupPoint != null)
            {
                vector = pickupPoint.Data.Position;
            }
            else
            {
                Abort();
            }
        }

        return vector;
    }

    public override FollowerTaskType Type => (FollowerTaskType) 55555;
    public override FollowerLocation Location => _siloFertiziler.Data.Location;
}