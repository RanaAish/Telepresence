/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
namespace M2MqttUnity.Examples
{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {
        public GameObject Ball;
        public bool EnteredTrigger = false;

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;

        public void TestPublish()
        {
            client.Publish("test/", System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");

        }
        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                EnteredTrigger = true;
                TestPublish();
                Debug.Log("coolison");



            }

        }

        public void SetBrokerAddress(string brokerAddress)
        {
            brokerAddress = "localhost";
        }

        public void SetBrokerPort(int brokerPort)
        {
            brokerPort = 1883;
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
        }

        protected override void OnConnected()
        {
            base.OnConnected();
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "test/" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "on/" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }
        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "test/" });
        }

      

        protected override void Start()
        {
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            string[] coordinates = msg.Split(',');
            Debug.Log(coordinates[0]);
            Debug.Log("Received: " + coordinates[0] + "   " + coordinates[1] + "   " + coordinates[2] + "   ");
            StoreMessage(coordinates);
        }

        private void StoreMessage(string [] eventMsg)
        {
            Ball.transform.position = new Vector3(int.Parse(eventMsg[0]), int.Parse(eventMsg[1]), int.Parse(eventMsg[2]));
        }


        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            
        }
    }
}
