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
    public static class VariantManager
    {
        public static readonly string AllRoomsIdentifier = "ALL_ROOMS";

        public static Dictionary<string, LinkedList<Variant>> variantsByRoomName = new();
        public static LinkedList<Variant> activeVariants = new();
        private static LevelData currentRoom;

        public static void RoomLoaded(LevelData room)
        {
            currentRoom = room;

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
                    Logger.Log(LogLevel.Info, "ARandomizerMod", "Sending variant " + variant.name + " with value " + variant.valueString);
                    CNetComm.Instance.SendVariantUpdate(room.Name, variant, VariantUpdateData.Operation.ADD);
                }
            }
        }

        public static void ProcessVariantUpdate(VariantUpdateData data)
        {
            string roomName = data.roomName;
            Variant variant = data.variant;
            VariantUpdateData.Operation operation = data.operation;
            Logger.Log(LogLevel.Info, "ARandomizerMod", "Received variant " + variant.name + " with value " + variant.valueString);

            if (roomName.Equals(AllRoomsIdentifier))
            {
                // Peform opertaion on all existing rooms
                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        AddVariantToAllRooms(variant);
                        TriggerVariant(variant);
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        RemoveVariantFromAllRooms(variant);
                        ResetVariant(variant);
                        break;
                    default:
                        Logger.Log(LogLevel.Error, "ARandomizerMod", "Unrecognized Variant Operation");
                        break;
                }
            }
            else if (roomName is not null)
            {
                // Create a new dictionary entry, if neccessary
                if (!variantsByRoomName.ContainsKey(roomName))
                    variantsByRoomName.Add(roomName, new());

                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        variantsByRoomName[roomName].AddLast(variant);
                        // If we're in this room, trigger this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            TriggerVariant(variant);
                        }
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        variantsByRoomName[roomName].Remove(variant);
                        // If we're in this room, reset this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            ResetVariant(variant);
                        }
                        break;
                    default:
                        Logger.Log(LogLevel.Error, "ARandomizerMod", "Unrecognized Variant Operation");
                        break;
                }
            }
        }

        private static void AddVariantToAllRooms(Variant variant)
        {
            foreach (string roomName in variantsByRoomName.Keys)
            {
                variantsByRoomName[roomName].AddLast(variant);
            }
        }

        private static void RemoveVariantFromAllRooms(Variant variant)
        {
            foreach (string roomName in variantsByRoomName.Keys)
            {
                variantsByRoomName[roomName].Remove(variant);
            }

        }

        private static void RandomizeNewVariants()
        {
            LinkedList<Variant> variantsToAdd = VariantRandomizer.RollNewVariants();
            int numVariantsToRemove = VariantRandomizer.RollRemovedVariants();

            foreach (Variant variant in variantsToAdd )
            {
                variant.SetValue();
                TriggerVariant(variant);
            }

            for (int i = 0; i < numVariantsToRemove; i++)
            {
                //ResetRandomVariant();
            }
        }

        private static void MatchVariantList(LinkedList<Variant> targetList)
        {
            foreach (Variant variant in targetList)
            {
                if (!activeVariants.Contains(variant))
                {
                    TriggerVariant(variant);
                }
            }
            foreach (Variant variant in activeVariants.ToArray()) // Convert to array to avoid concurrent modification exceptions
            {
                if (!targetList.Contains(variant))
                {
                    ResetVariant(variant);
                }
            }
        }

        public static void TriggerVariant(Variant variant)
        {
            Logger.Log(LogLevel.Info, "ARandomizerMod", "Activating variant " + variant.name + "...");
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
        }

        public static Variant GetVariantWithName(string name)
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

        public static void ResetVariant(Variant variant)
        {
            Logger.Log(LogLevel.Info, "ARandomizerMod", "Resetting variant " + variant.name + "...");

            variant.Reset();
            activeVariants.Remove(variant);
        }

        public static void ResetRandomVariant()
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

        public static void ResetAllVariants()
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
