using System.Linq;
using UnityEngine;

namespace GoatOuthouses;

public class FollowerTaskOuthouse : FollowerTask
{
    private Follower _follower;
    private bool holdingPoop;
    private readonly int _siloFertizilerId;
    private readonly Structures_SiloFertiliser _siloFertiziler;

    public FollowerTaskOuthouse(int siloFertiziler)
    {
        _siloFertizilerId = siloFertiziler;
        _siloFertiziler = StructureManager.GetStructureByID<Structures_SiloFertiliser>(_siloFertizilerId);
    }

    public override int GetSubTaskCode()
    {
        return _siloFertizilerId;
    }

    public override void OnStart()
    {
        SetState(FollowerTaskState.GoingTo);
    }

    private void DoingBegin(Follower follower)
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

            holdingPoop = true;
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

        if (this.holdingPoop)
        {
            this.RefundPoop();
        }
        base.Cleanup(follower);
    }

    public override void OnDoingBegin(Follower follower)
    {
        _follower = follower;
        DoingBegin(follower);
    }

    public override void OnGoingToBegin(Follower follower)
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
        if (holdingPoop)
        {
            RefundPoop();
        }
    }
    
    public override void SimDoingBegin(SimFollower simFollower)
    {
        this.DoingBegin(null);
    }
    
    public override void Setup(Follower follower)
    {
        base.Setup(follower);
        this._follower = follower;
    }
    
    public override void SimCleanup(SimFollower simFollower)
    {
        base.SimCleanup(simFollower);
        if (this.holdingPoop)
        {
            this.RefundPoop();
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

            holdingPoop = false;
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