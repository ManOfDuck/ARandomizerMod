using Celeste.Mod.ARandomizerMod.CelesteNet;
using Celeste.Mod.ARandomizerMod.CelesteNet.Data;
using Monocle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.ARandomizerMod
{
    public class VariantManager
    {
        public static readonly string AllRoomsIdentifier = "ALL_ROOMS";

        public Dictionary<string, LinkedList<Variant>> variantsByRoomName = new();
        public LinkedList<Variant> activeVariants = new();
        private LevelData currentRoom;

        public void RoomLoaded(LevelData room)
        {
            currentRoom = room;
            Logger.Log(LogLevel.Error, "ARandomizerMod", room.Name);

            if (variantsByRoomName.ContainsKey(room.Name))
            {
                MatchVariantList(variantsByRoomName[room.Name]);
            }
            else
            {
                // Update active variants
                RandomizeNewVariants();

                // Update this room for all clients
                foreach (Variant variant in activeVariants)
                {
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "Sending variant " + variant.name + " with value " + variant.valueString);
                    CNetComm.Instance.SendVariantUpdate(room.Name, variant, VariantUpdateData.Operation.ADD);
                }
            }
        }

        public void ProcessVariantUpdate(VariantUpdateData data)
        {
            string roomName = data.roomName;
            Variant variant = data.variant;
            VariantUpdateData.Operation operation = data.operation;
            Logger.Log(LogLevel.Error, "ARandomizerMod", "Received variant " + variant.name + " with value " + variant.valueString);

            if (roomName.Equals(AllRoomsIdentifier))
            {
                // Peform opertaion on all existing rooms
                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        AddVariantToAllRooms(variant);
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        RemoveVariantFromAllRooms(variant);
                        break;
                    default:
                        Logger.Log(LogLevel.Error, "ARandomizerMod", "Unrecognized Variant Operation");
                        break;
                }

                MatchVariantList(variantsByRoomName[currentRoom.Name]);
            }
            else if (roomName is not null)
            {
                // Create a new dictionary entry, if neccessary
                if (!variantsByRoomName.ContainsKey(roomName))
                    variantsByRoomName.Add(roomName, new());

                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        Logger.Log(LogLevel.Error, "ARandomizerMod", "Adding variant");
                        variantsByRoomName[roomName].AddLast(variant);
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        variantsByRoomName[roomName].Remove(variant);
                        break;
                    default:
                        Logger.Log(LogLevel.Error, "ARandomizerMod", "Unrecognized Variant Operation");
                        break;
                }

                // If we're in this room, update our variants
                if (currentRoom?.Name.Equals(roomName) == true)
                {
                    MatchVariantList(variantsByRoomName[roomName]);
                }
            }
        }

        private void AddVariantToAllRooms(Variant variant)
        {
            foreach (string roomName in variantsByRoomName.Keys)
            {
                variantsByRoomName[roomName].AddLast(variant);
            }
        }

        private void RemoveVariantFromAllRooms(Variant variant)
        {
            foreach (string roomName in variantsByRoomName.Keys)
            {
                variantsByRoomName[roomName].Remove(variant);
            }

        }

        private void RandomizeNewVariants()
        {
            LinkedList<Variant> variantsToAdd = VariantRandomizer.RollNewVariants();
            int numVariantsToRemove = VariantRandomizer.RollRemovedVariants();

            foreach (Variant variant in variantsToAdd )
            {
                TriggerVariant(variant);
            }

            for (int i = 0; i < numVariantsToRemove; i++)
            {
                ResetRandomVariant();
            }
        }

        private void MatchVariantList(LinkedList<Variant> targetList)
        {
            Logger.Log(LogLevel.Error, "ARandomizerMod", "matching");
            foreach (Variant variant in activeVariants.ToArray()) // Convert to array to avoid concurrent modification exceptions
            {
                if (!targetList.Contains(variant))
                {
                    ResetVariant(variant);
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "reset");
                }
            }
            foreach (Variant variant in targetList)
            {
                if (!activeVariants.Contains(variant))
                {
                    TriggerVariant(variant);
                    Logger.Log(LogLevel.Error, "ARandomizerMod", "add");
                }
            }
        }

        public void TriggerVariant(Variant variant)
        {
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Activating variant " + variant.name + "...");
            variant.Trigger();

            foreach (Variant activeVariant in activeVariants)
            {
                if (activeVariant.name.Equals(variant.name))
                {
                    activeVariants.Remove(activeVariant);
                    break;
                }
            }

            activeVariants.AddLast(variant);
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Activated variant " + variant.name + " with value " + variant.valueString);
        }

        public Variant GetVariantWithName(string name)
        {
            foreach (Variant variant in activeVariants)
            {
                if (variant.name.Equals(name))
                {
                    return variant;
                }
            }
            return null;
        }

        public void ResetVariant(Variant variant)
        {
            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Resetting variant " + variant.name + "...");

            variant.Reset();
            activeVariants.Remove(variant);

            Logger.Log(LogLevel.Debug, "ARandomizerMod", "Reset variant " + variant.name + " to value " + variant.valueString);
        }

        public void ResetRandomVariant()
        {
            if (activeVariants.Count < 1) return;

            int variantToReset = new Random().Next(activeVariants.Count);
            LinkedListNode<Variant> node = activeVariants.First;
            for (int i = 0; i < activeVariants.Count; i++)
            {
                if (i >= variantToReset && node.Value.cost < 200)
                {
                    ResetVariant(node.Value);
                    return;
                }

                if (node.Next == null) return;
                node = node.Next;
            }
        }

        public void ResetAllVariants()
        {
            Variant[] variantsToReset = activeVariants.ToArray();

            foreach (Variant variant in variantsToReset)
            {
                ResetVariant(variant);
            }

            activeVariants.Clear();
        }
    }
}
