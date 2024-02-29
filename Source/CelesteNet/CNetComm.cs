using System;
using Celeste.Mod.CelesteNet.DataTypes;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Celeste.Mod.CelesteNet;
using Celeste.Mod.CelesteNet.Client;
using Celeste.Mod.CelesteNet.Client.Components;
using Celeste.Mod.ARandomizerMod.Data;
using System.Collections.Concurrent;

namespace Celeste.Mod.ARandomizerMod.CelesteNet
{
    // Thanks for corkr900 for the vast majority of this class

    public class CNetComm : GameComponent
    {
        public static CNetComm Instance { get; private set; }

        public CelesteNetClientContext CnetContext {  get { return CelesteNetClientModule.Instance?.Context; } }
        public CelesteNetClient CnetClient { get { return CelesteNetClientModule.Instance?.Client; } }
        public bool IsConnected { get { return CnetClient?.Con?.IsConnected ?? false; } }
        public uint? CnetID { get { return IsConnected ? (uint?)CnetClient?.PlayerInfo?.ID : null; } }

        public DataChannelList.Channel CurrentChannel
        {
            get
            {
                KeyValuePair<Type, CelesteNetGameComponent> listComp = CnetContext.Components.FirstOrDefault((KeyValuePair<Type, CelesteNetGameComponent> kvp) => {
                    return kvp.Key == typeof(CelesteNetPlayerListComponent);
                });
                if (listComp.Equals(default(KeyValuePair<Type, CelesteNetGameComponent>))) return null;
                CelesteNetPlayerListComponent comp = listComp.Value as CelesteNetPlayerListComponent;
                DataChannelList.Channel[] list = comp.Channels?.List;
                return list?.FirstOrDefault(c => c.Players.Contains(CnetClient.PlayerInfo.ID));
            }
        }

        public bool CurrentChannelIsMain
        {
            get
            {
                return CurrentChannel?.Name?.ToLower() == "main";
            }
        }

        public bool CanSendMessages
        {
            get
            {
                return IsConnected && !CurrentChannelIsMain;
            }
        }

        #region events

        public delegate void OnConnectedHandler(CelesteNetClientContext cxt);
        public static event OnConnectedHandler OnConnected;

        public delegate void OnDisonnectedHandler(CelesteNetConnection con);
        public static event OnDisonnectedHandler OnDisconnected;

        public delegate void OnReceiveTestHandler(TestData data);
        public static event OnReceiveTestHandler OnReceiveTest;

        #endregion

        #region handlers
        public void Handle(CelesteNetConnection con, TestData data)
        {
            data.player ??= CnetClient.PlayerInfo;  // It's null when handling our own messages
            updateQueue.Enqueue(() => OnReceiveTest?.Invoke(data));
        }
        #endregion

        // Put events in thread-safe queue, flush on update
        private ConcurrentQueue<Action> updateQueue = new();

        public CNetComm(Game game)
            : base(game)
        {
            Instance = this;
            CelesteNetClientContext.OnStart += OnCNetClientContextStart;
            Disposed += OnComponentDisposed;
            Logger.Log(LogLevel.Error, "ARandomizerMod", "Ctr called");
        }

        private void OnCNetClientContextStart(CelesteNetClientContext cxt)
        {
            CnetClient.Data.RegisterHandlersIn(this);
            CnetClient.Con.OnDisconnect += OnDisconnect;
            updateQueue.Enqueue(() => OnConnected?.Invoke(cxt));
        }

        private void OnComponentDisposed(object sender, EventArgs args)
        {
            CelesteNetClientContext.OnStart -= OnCNetClientContextStart;
        }

        private void OnDisconnect(CelesteNetConnection con)
        {
            updateQueue.Enqueue(() => OnDisconnected?.Invoke(con));
        }

        public override void Update(GameTime gameTime)
        {
            // Flush the queue
            ConcurrentQueue<Action> queue = updateQueue;
            updateQueue = new ConcurrentQueue<Action>();
            foreach (Action act in queue) act();

            base.Update(gameTime);
        }

        public void SendTestMessage(string message)
        {
            //if (!CanSendMessages) return;
            Logger.Log(LogLevel.Error, "ARandomizerMod", "Broadcasting hello");
            CnetClient.SendAndHandle(new TestData()
            {
                Message = message
            });
        }
    }
}

