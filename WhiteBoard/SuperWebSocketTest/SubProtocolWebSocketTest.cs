﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperWebSocket;
using SuperWebSocket.SubProtocol;

namespace SuperWebSocketTest
{
    [TestFixture]
    public class SubProtocolWebSocketTest : WebSocketTest
    {
        [TestFixtureSetUp]
        public override void Setup()
        {
            LogUtil.Setup(new ConsoleLogger());

            m_WebSocketServer = new WebSocketServer(new BasicSubProtocol(this.GetType().Assembly));
            m_WebSocketServer.Setup(new RootConfig(), new ServerConfig
            {
                Port = 911,
                Ip = "Any",
                MaxConnectionNumber = 100,
                Mode = SocketMode.Sync,
                Name = "SuperWebSocket Server"
            }, SocketServerFactory.Instance);
        }

        [Test]
        public void TestEcho()
        {
            Socket socket;
            Stream stream;

            Handshake(out socket, out stream);

            StringBuilder sb = new StringBuilder();

            sb.Append("ECHO");

            var parameters = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                var p = Guid.NewGuid().ToString();
                sb.Append(" ");
                sb.Append(p);
                parameters.Add(p);
            }

            string messageSource = sb.ToString();

            stream.Write(new byte[] { WebSocketConstant.StartByte }, 0, 1);
            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            stream.Write(data, 0, data.Length);
            stream.Write(new byte[] { WebSocketConstant.EndByte }, 0, 1);
            stream.Flush();

            var receivedBuffer = new ArraySegmentList<byte>();

            foreach (var p in parameters)
            {
                ReceiveMessage(stream, receivedBuffer, Encoding.UTF8.GetBytes(p).Length + 2);
                string rp = Encoding.UTF8.GetString(receivedBuffer.ToArrayData(1, receivedBuffer.Count - 2));
                Console.WriteLine(rp);
                Assert.AreEqual(p, rp);
                receivedBuffer.ClearSegements();
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        [Test, Timeout(5000)]
        public override void MessageTransferTest()
        {
            Socket socket;
            Stream stream;

            Handshake(out socket, out stream);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                sb.Append(Guid.NewGuid().ToString());
            }

            string messageSource = sb.ToString();

            Random rd = new Random();

            ArraySegmentList<byte> receivedBuffer = new ArraySegmentList<byte>();

            for (int i = 0; i < 10; i++)
            {
                int startPos = rd.Next(0, messageSource.Length - 2);
                int endPos = rd.Next(startPos + 1, messageSource.Length - 1);

                string currentCommand = messageSource.Substring(startPos, endPos - startPos);

                Console.WriteLine("Client:" + currentCommand);

                stream.Write(new byte[] { WebSocketConstant.StartByte }, 0, 1);
                byte[] data = Encoding.UTF8.GetBytes("ECHO:" + currentCommand);
                stream.Write(data, 0, data.Length);
                stream.Write(new byte[] { WebSocketConstant.EndByte }, 0, 1);
                stream.Flush();

                ReceiveMessage(stream, receivedBuffer, data.Length + 2);
                Assert.AreEqual(data.Length + 2, receivedBuffer.Count);
                Assert.AreEqual(WebSocketConstant.StartByte, receivedBuffer[0]);
                Assert.AreEqual(WebSocketConstant.EndByte, receivedBuffer[receivedBuffer.Count - 1]);
                Assert.AreEqual(currentCommand, Encoding.UTF8.GetString(receivedBuffer.ToArrayData(1, receivedBuffer.Count - 2)));
                receivedBuffer.ClearSegements();
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
