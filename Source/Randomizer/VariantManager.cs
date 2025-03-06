using Celeste.Mod.ARandomizerMod.CelesteNet;
using Celeste.Mod.ARandomizerMod.CelesteNet.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Celeste.Mod.ARandomizerMod
{
    public static class VariantManager
    {
        public static readonly string AllRoomsIdentifier = "ALL_ROOMS";

        public static Dictionary<string, LinkedList<Variant>> VariantsByRoomName { get; private set; } = new();
        public static LinkedList<Variant> ActiveVariants { get; private set; } = new();
        private static LevelData currentRoom;

        public static void RoomLoaded(LevelData room)
        {
            currentRoom = room;


            if (VariantsByRoomName.ContainsKey(room.Name))
            {
                MatchVariantList(VariantsByRoomName[room.Name]);
            }
            else
            {
                // Update active variants
                RandomizeNewVariants();

                // Update this room for all clients
                foreach (Variant variant in ActiveVariants)
                {
                    
                }
            }
        }

        public static void ProcessVariantUpdate(VariantUpdateData data)
        {
            string roomName = data.roomName;
            Variant variant = data.variant;
            VariantUpdateData.Operation operation = data.operation;
            Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), "Received variant " + variant.name + " with value " + variant.valueString);

            if (roomName.Equals(AllRoomsIdentifier, StringComparison.Ordinal))
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
                        Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unrecognized Variant Operation");
                        break;
                }
            }
            else if (roomName is not null)
            {
                // Create a new dictionary entry, if neccessary
                if (!VariantsByRoomName.ContainsKey(roomName))
                    VariantsByRoomName.Add(roomName, new());

                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        VariantsByRoomName[roomName].AddLast(variant);
                        // If we're in this room, trigger this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            TriggerVariant(variant);
                        }
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        VariantsByRoomName[roomName].Remove(variant);
                        // If we're in this room, reset this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            ResetVariant(variant);
                        }
                        break;
                    default:
                        Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unrecognized Variant Operation");
                        break;
                }
            }
        }

        private static void AddVariantToAllRooms(Variant variant)
        {
            foreach (string roomName in VariantsByRoomName.Keys)
            {
                VariantsByRoomName[roomName].AddLast(variant);
            }
        }

        private static void RemoveVariantFromAllRooms(Variant variant)
        {
            foreach (string roomName in VariantsByRoomName.Keys)
            {
                VariantsByRoomName[roomName].Remove(variant);
            }

        }

        private static void AddVariantToRoom(Variant variant, string roomName)
        {
            VariantsByRoomName[roomName].AddLast(variant);
            Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), "Sending variant " + variant.name + " with value " + variant.valueString);
            CNetComm.Instance.SendVariantUpdate(roomName, variant, VariantUpdateData.Operation.ADD);

            if (currentRoom.Name == roomName)
            {
                TriggerVariant(variant);
            }
        }

        private static void RemoveVariantFromRoom(Variant variant, string roomName)
        {
           // if (VariantsByRoomName[roomName].Contains(r)
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
                if (!ActiveVariants.Contains(variant))
                {
                    TriggerVariant(variant);
                }
            }
            foreach (Variant variant in ActiveVariants.ToArray()) // Convert to array to avoid concurrent modification exceptions
            {
                if (!targetList.Contains(variant))
                {
                    ResetVariant(variant);
                }
            }
        }

        public static void TriggerVariant(Variant variant)
        {
            Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), "Activating variant " + variant.name + "...");
            if (!variant.Trigger())
                return;

            foreach (Variant activeVariant in ActiveVariants)
            {
                if (activeVariant.name.Equals(variant.name))
                {
                    ActiveVariants.Remove(activeVariant);
                    break;
                }
            }

            ActiveVariants.AddLast(variant);
        }

        public static Variant GetVariantWithName(string name)
        {
            foreach (Variant variant in ActiveVariants)
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
            Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), "Resetting variant " + variant.name + "...");

            if (!variant.Reset())
                return;

            ActiveVariants.Remove(variant);
        }

        public static void ResetRandomVariant()
        {
            if (ActiveVariants.Count < 1) return;

            int variantToReset = new Random().Next(ActiveVariants.Count);
            LinkedListNode<Variant> node = ActiveVariants.First;
            for (int i = 0; i < ActiveVariants.Count; i++)
            {
                if (i >= variantToReset && node.Value.Cost < 200)
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
            Variant[] variantsToReset = ActiveVariants.ToArray();

            foreach (Variant variant in variantsToReset)
            {
                ResetVariant(variant);
            }

            ActiveVariants.Clear();
        }

        private static void TestAllVariants()
        {
            TestVariantList(VariantLists.FUCKED_UP);
            TestVariantList(VariantLists.nasty);
            TestVariantList(VariantLists.tame);
            TestVariantList(VariantLists.dubious);
            TestVariantList(VariantLists.silly);
            TestVariantList(VariantLists.nice);
            TestVariantList(VariantLists.good);
            TestVariantList(VariantLists.great);
        }

        private static void TestVariantList(Variant[] list)
        {
            foreach (Variant v in list)
            {
                Logger.Log(LogLevel.Info, nameof(ARandomizerModModule), "Testing variant " + v.name + " with value " + v.valueString);
                v.SetValue();
                TriggerVariant(v);
                CNetComm.Instance.SendVariantUpdate(currentRoom.Name, v, VariantUpdateData.Operation.ADD);
            }
        }
    }
}
