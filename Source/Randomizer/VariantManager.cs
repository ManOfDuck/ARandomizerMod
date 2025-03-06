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
                // Update active variants
                MatchVariantList(VariantsByRoomName[room.Name]);
            }
            else
            {
                // Add current variants to this room on all clients
                foreach (Variant variant in ActiveVariants)
                {
                    SendNewVariantInCurrentRoom(variant);
                }

                LinkedList<Variant> variantsToAdd = VariantRandomizer.RollNewVariants();

                // Add new variants to this room
                foreach (Variant variant in variantsToAdd)
                {
                    variant.SetValue();
                    SendNewVariantInCurrentRoom(variant);
                }
            }
        }

        public static void ProcessVariantUpdate(VariantUpdateData data)
        {
            string roomName = data.roomName;
            Variant variant = data.variant;
            VariantUpdateData.Operation operation = data.operation;

            if (roomName.Equals(AllRoomsIdentifier, StringComparison.Ordinal))
            {
                // Peform opertaion on all existing rooms
                switch (operation)
                {
                    case VariantUpdateData.Operation.ADD:
                        Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Received variant " + variant.name + " with value " + variant.valueString + " in room " + roomName);
                        foreach (string name in VariantsByRoomName.Keys)
                        {
                            VariantsByRoomName[name].AddLast(variant);
                        }
                        TriggerVariant(variant);
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Received reset of variant " + variant.name + " in all rooms");
                        foreach (string name in VariantsByRoomName.Keys)
                        {
                            VariantsByRoomName[name].Remove(variant);
                        }
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
                        Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Received variant " + variant.name + " with value " + variant.valueString + " in room " + roomName);
                        VariantsByRoomName[roomName].AddLast(variant);
                        // If we're in this room, trigger this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Variant update is in current room");
                            TriggerVariant(variant);
                        }
                        break;
                    case VariantUpdateData.Operation.REMOVE:
                        Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Received reset of variant " + variant.name + " in room " + roomName);
                        VariantsByRoomName[roomName].Remove(variant);
                        // If we're in this room, reset this variant
                        if (currentRoom?.Name.Equals(roomName) == true)
                        {
                            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Variant update is in current room");
                            ResetVariant(variant);
                        }
                        break;
                    default:
                        Logger.Log(LogLevel.Error, nameof(ARandomizerModModule), "Unrecognized Variant Operation");
                        break;
                }
            }
        }

        public static void SendNewVariantInCurrentRoom(Variant variant)
        {
            SendNewVariant(variant, currentRoom.Name);
        }

        public static void SendVariantRemovalInCurrentRoom(Variant variant) 
        { 
            SendVariantRemoval(variant, currentRoom.Name);
        }

        public static void SendNewVariantInAllRooms(Variant variant)
        {
            SendNewVariant(variant, AllRoomsIdentifier);
        }

        public static void SendVariantRemovalInAllRooms(Variant variant)
        {
            SendVariantRemoval(variant, AllRoomsIdentifier);
        }

        public static void SendNewVariant(Variant variant, string roomName)
        {
            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Sending variant " + variant.name + " with value " + variant.valueString + " to room " + roomName);
            CNetComm.Instance.SendVariantUpdate(roomName, variant, VariantUpdateData.Operation.ADD);
        }

        public static void SendVariantRemoval(Variant variant, string roomName)
        {
            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Sending reset on variant " + variant.name + " to room " + roomName);
            CNetComm.Instance.SendVariantUpdate(roomName, variant, VariantUpdateData.Operation.REMOVE);
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

        /// <summary>
        /// Locally triggers a variant. To be used only when matching variant list, not triggering new variants
        /// </summary>
        /// <param name="variant"></param>
        private static void TriggerVariant(Variant variant)
        {
            if (ActiveVariants.Contains(variant)) return;
            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Activating variant " + variant.name + "...");
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

        /// <summary>
        /// Locally resets a variant. To be used only when matching variant list, not removing variants
        /// </summary>
        /// <param name="variant"></param>
        private static void ResetVariant(Variant variant)
        {
            if (!ActiveVariants.Contains(variant)) return;
            Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Resetting variant " + variant.name + "...");

            if (!variant.Reset())
                return;

            ActiveVariants.Remove(variant);
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
                Logger.Log(LogLevel.Debug, nameof(ARandomizerModModule), "Testing variant " + v.name + " with value " + v.valueString);
                v.SetValue();
                TriggerVariant(v);
                CNetComm.Instance.SendVariantUpdate(currentRoom.Name, v, VariantUpdateData.Operation.ADD);
            }
        }
    }
}
